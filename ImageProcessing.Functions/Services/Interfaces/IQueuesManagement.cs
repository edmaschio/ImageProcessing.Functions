namespace ImageProcessing.Functions.Services.Interfaces;

public interface IQueuesManagement
{
    Task<bool> SendMessageAsync<T>(T serviceMessage, string queueName, string connectionString);
}
