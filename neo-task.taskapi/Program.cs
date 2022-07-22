using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text.Json.Serialization;
using neo_task.rabbitmq;
using neo_task.taskapi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionProvider, ConnectionProvider>();
builder.Services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
    "neo-task", ExchangeType.Topic));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins().AllowAnyHeader().AllowAnyOrigin();
                      });
});

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.
           Add(new JsonStringEnumConverter());

    options.JsonSerializerOptions.DefaultIgnoreCondition =
             JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddDbContext<NeoTaskModelContext>(opt =>
    opt.UseInMemoryDatabase("NeoTask"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
