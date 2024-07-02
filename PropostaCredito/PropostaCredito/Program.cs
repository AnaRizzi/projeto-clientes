using PropostaCredito;
using PropostaCredito.Config;
using PropostaCredito.Infra;
using PropostaCredito.Interfaces;
using PropostaCredito.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var RABBIT_CONSUMER = "RabbitConsumerConfig";
builder.Services.AddSingleton<IMensageriaService, MensageriaService>();
builder.Services.AddSingleton<IConsumer, Consumer>();
builder.Services.AddSingleton(builder.Configuration.GetSection(RABBIT_CONSUMER).Get<RabbitConfig>());

var host = builder.Build();
host.Run();
