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

        public ImagesController(
            IBlobsManagement blobsManagement,
            IQueuesManagement queuesManagement)
        {
            _blobsManagement = blobsManagement ?? throw new ArgumentNullException(nameof(blobsManagement));
            _queuesManagement = queuesManagement ?? throw new ArgumentNullException(nameof(queuesManagement));
        }

        [HttpPost]
        [Route("Profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(IFormFile? img)
        {
            if (img != null)
                await UploadFile(img, 300, 300);

            return Ok();
        }

        [NonAction]
        private async Task UploadFile(IFormFile file, int width, int height)
        {
            if (file is not { Length: > 0 }) return;
        }
    }
}
