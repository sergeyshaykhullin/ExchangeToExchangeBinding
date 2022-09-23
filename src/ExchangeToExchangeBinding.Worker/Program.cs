using ExchangeToExchangeBinding;
using ExchangeToExchangeBinding.Worker;
using MassTransit;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
  configurator.SetKebabCaseEndpointNameFormatter();

  configurator.AddConsumer<Step1Consumer, DurableQuorumConsumerDefinition<Step1Consumer>>()
    .Endpoint(x => x.Name = "queuestep1");

  configurator.AddConsumer<Step2Consumer, DurableQuorumConsumerDefinition<Step2Consumer>>()
    .Endpoint(x => x.Name = "queuestep2");

  configurator.AddConsumer<Step3Consumer, DurableQuorumConsumerDefinition<Step3Consumer>>()
    .Endpoint(x => x.Name = "queuestep3");
  
  configurator.UsingRabbitMq((context, rabbitMq) =>
  {
    rabbitMq.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMq")!));

    rabbitMq.DeployPublishTopology = false;
    
    rabbitMq.ConfigureEndpoints(context);
  });
  
});

builder.Services.AddOptions<MassTransitHostOptions>()
  .Configure(options =>
  {
    options.WaitUntilStarted = true;
  });

builder.Services.AddOpenTelemetryTracing(tracing =>
{
  tracing.AddSource("MassTransit")
    .AddOtlpExporter()
    .AddAspNetCoreInstrumentation(options =>
    {
      options.RecordException = true;
    })
    .AddMassTransitInstrumentation();
});

var app = builder.Build();

app.Run();