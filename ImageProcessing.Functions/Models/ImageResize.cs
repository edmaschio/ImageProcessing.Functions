namespace ImageProcessing.Functions.Models
{
    public class ImageResize
    {
        public string FileName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public int Width { get; set; }
        public int Length { get; set; }
        public string ImageContainer { get; set; } = null!;
        public string Target { get; set; } = null!;
    }
}
