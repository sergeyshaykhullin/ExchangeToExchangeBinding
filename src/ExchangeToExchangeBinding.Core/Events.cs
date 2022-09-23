using MassTransit;

namespace ExchangeToExchangeBinding.Core;

[EntityName("event:step1-created")]
public class Step1Created
{
  public required Guid Id { get; init; }
}

[EntityName("event:step2-created")]
public class Step2Created
{
  public required Guid Id { get; init; }
}

[EntityName("event:step3-created")]
public class Step3Created
{
  public required Guid Id { get; init; }
}