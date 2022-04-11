using ImageProcessing.Functions.Services;
using ImageProcessing.Functions.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBlobsManagement>(_ =>
{
    var connBlobs = builder.Configuration.GetConnectionString("StorageImages");
    if (string.IsNullOrWhiteSpace(connBlobs))
        throw new ArgumentException(nameof(connBlobs));

    return new BlobsManagement(connBlobs);
});

builder.Services.AddScoped<IQueuesManagement>(_ =>
{
    var connQueues = builder.Configuration.GetConnectionString("ServiceBusImages");
    if (string.IsNullOrWhiteSpace(connQueues))
        throw new ArgumentException(nameof(connQueues));

    return new QueuesManagement(connQueues);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
