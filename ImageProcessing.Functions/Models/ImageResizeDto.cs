namespace ImageProcessing.Functions.Models
{
    public class ImageResizeDto
    {
        public string FileName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
        public string ImageContainer { get; set; } = null!;
        public string Target { get; set; } = null!;
    }
}
