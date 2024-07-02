using Clientes;
using Clientes.Domain.Interfaces;
using Clientes.Domain.Requests;
using Clientes.Infra.RabbitMQ.Configuration;
using Clientes.Infra.RabbitMQ.Infra;
using Clientes.Infra.RabbitMQ.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ClienteRequest).Assembly));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var RABBIT_PRODUCER = "ProducerConfig";
builder.Services.AddSingleton<IMensageriaService, MensageriaService>();
builder.Services.AddSingleton(typeof(IProducer<>), typeof(Producer<>));
builder.Services.AddSingleton(builder.Configuration.GetSection(RABBIT_PRODUCER).Get<ProducerConfig>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();

