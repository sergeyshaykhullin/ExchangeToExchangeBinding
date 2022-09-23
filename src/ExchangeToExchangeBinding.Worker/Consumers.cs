using ExchangeToExchangeBinding.Core;
using MassTransit;

namespace ExchangeToExchangeBinding.Worker;

public class Step1Consumer : IConsumer<Step1Created>
{
  private readonly ILogger<Step1Consumer> logger;

  public Step1Consumer(ILogger<Step1Consumer> logger)
  {
    this.logger = logger;
  }

  public async Task Consume(ConsumeContext<Step1Created> context)
  {
    logger.LogInformation($"Step1: {context.Message.Id}");

    await context.Publish(new Step2Created
    {
      Id = context.Message.Id
    }, context.CancellationToken);
  }
}

public class Step2Consumer : IConsumer<Step2Created>
{
  private readonly ILogger<Step2Consumer> logger;

  public Step2Consumer(ILogger<Step2Consumer> logger)
  {
    this.logger = logger;
  }

  public async Task Consume(ConsumeContext<Step2Created> context)
  {
    logger.LogInformation($"Step2: {context.Message.Id}");

    await context.Publish(new Step3Created
    {
      Id = context.Message.Id
    }, context.CancellationToken);
  }
}

public class Step3Consumer : IConsumer<Step3Created>
{
  private readonly ILogger<Step3Consumer> logger;

  public Step3Consumer(ILogger<Step3Consumer> logger)
  {
    this.logger = logger;
  }

  public Task Consume(ConsumeContext<Step3Created> context)
  {
    logger.LogInformation($"Step3: {context.Message.Id}");

    return Task.CompletedTask;
  }
}