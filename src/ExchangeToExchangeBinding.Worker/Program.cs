using ExchangeToExchangeBinding;
using ExchangeToExchangeBinding.Worker;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
  configurator.SetKebabCaseEndpointNameFormatter();

  configurator.UsingRabbitMq((context, rabbitMq) =>
  {
    rabbitMq.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMq")!));

    rabbitMq.ConfigureEndpoints(context);
  });

  configurator.AddConsumer<Step1Consumer, DurableQuorumConsumerDefinition<Step1Consumer>>()
    .Endpoint(x => x.Name = "queue:step1");

  configurator.AddConsumer<Step2Consumer, DurableQuorumConsumerDefinition<Step2Consumer>>()
    .Endpoint(x => x.Name = "queue:step2");

  configurator.AddConsumer<Step3Consumer, DurableQuorumConsumerDefinition<Step3Consumer>>()
    .Endpoint(x => x.Name = "queue:step3");
});

builder.Services.AddOptions<MassTransitHostOptions>()
  .Configure(options =>
  {
    options.WaitUntilStarted = true;
  });

var app = builder.Build();

app.Run();