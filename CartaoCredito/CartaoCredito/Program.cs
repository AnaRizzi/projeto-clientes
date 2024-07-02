using CartaoCredito;
using CartaoCredito.Config;
using CartaoCredito.Infra;
using CartaoCredito.Interfaces;
using CartaoCredito.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var RABBIT_CONSUMER = "RabbitConsumerConfig";
builder.Services.AddSingleton<IMensageriaService, MensageriaService>();
builder.Services.AddSingleton<IConsumer, Consumer>();
builder.Services.AddSingleton(builder.Configuration.GetSection(RABBIT_CONSUMER).Get<RabbitConfig>());

var host = builder.Build();
host.Run();
