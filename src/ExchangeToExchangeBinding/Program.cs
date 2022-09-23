using ExchangeToExchangeBinding.Core;
using MassTransit;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
  configurator.SetKebabCaseEndpointNameFormatter();

  configurator.UsingRabbitMq((context, rabbitMq) =>
  {
    rabbitMq.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMq")!));

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

app.MapGet("/publish", async (IPublishEndpoint publishEndpoint) =>
{
  var id = Guid.NewGuid();

  await publishEndpoint.Publish(new Step1Created { Id = id });

  return Results.Ok(id);
});

app.Run();