namespace ImageProcessing.Functions.Services.Interfaces;

public interface IBlobsManagement
{
    Task<string> UploadFile(string containerName, string fileName, byte[] file);
}
