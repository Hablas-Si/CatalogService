using Models;
using CatalogService.Repository;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Serilog;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Initializing Serilog...");
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Konfigurér Serilog fra appsettings.json
    .CreateLogger();
Console.WriteLine("Serilog initialized.");
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


// BsonSeralizer... fortæller at hver gang den ser en Guid i alle entiteter skal den serializeres til en string. 
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));


builder.Services.Configure<MongoDBSettings>(options =>
{
    options.ConnectionURI = Environment.GetEnvironmentVariable("ConnectionURI");
});

builder.Services.AddSingleton<ICatalogRepository, CatalogRepository>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


    app.UseSerilogRequestLogging();

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
