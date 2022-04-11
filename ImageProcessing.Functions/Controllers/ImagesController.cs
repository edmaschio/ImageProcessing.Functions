using ImageProcessing.Functions.Models;
using ImageProcessing.Functions.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessing.Functions.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IBlobsManagement _blobsManagement;
        private readonly IQueuesManagement _queuesManagement;

        private const string DefaultContainerName = "images";
        private const string DefaultQueueName = "imagequeue";

        public ImagesController(
            IBlobsManagement blobsManagement,
            IQueuesManagement queuesManagement)
        {
            _blobsManagement = blobsManagement ?? throw new ArgumentNullException(nameof(blobsManagement));
            _queuesManagement = queuesManagement ?? throw new ArgumentNullException(nameof(queuesManagement));
        }

        [HttpPost]
        [Route("ImageUpload")]
        public async Task<IActionResult> ImageUpload(IFormFile? img)
        {
            if (img != null)
                await UploadFile(img, 400, 400);

            return Ok();
        }

        [NonAction]
        private async Task UploadFile(IFormFile file, int width, int height)
        {
            if (file is not { Length: > 0 }) return;

            byte[]? fileBytes = null;
            MemoryStream? stream;
            await using (stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fileBytes = stream.ToArray();
            }

            if (fileBytes == null) return;

            var fileExtension = Path.GetExtension(file.FileName);

            string? name = Path.GetRandomFileName()
                + "_"
                + DateTime.UtcNow.ToString("dd/MM/yyyy").Replace("/", "-")
                + fileExtension;

            var url = await _blobsManagement.UploadFile(DefaultContainerName, name, fileBytes);

            await SendMessageToTheQueue(url, name, width, height, DefaultContainerName);
        }

        [NonAction]
        private async Task SendMessageToTheQueue(string url, string name, int width, int height, string defaultContainerName)
        {
            ImageResizeDto imgDto = new()
            {
                FileName = name,
                Url = url,
                Width = width,
                Height = height,
                ImageContainer = defaultContainerName
            };

            await _queuesManagement.SendMessageAsync(imgDto, DefaultQueueName);
        }
    }
}
