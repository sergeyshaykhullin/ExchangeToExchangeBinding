using MassTransit;

namespace ExchangeToExchangeBinding.Core;

[EntityName("eventstep1-created")]
public class Step1Created
{
  public Guid Id { get; init; }
}

[EntityName("eventstep2-created")]
public class Step2Created
{
  public Guid Id { get; init; }
}

[EntityName("eventstep3-created")]
public class Step3Created
{
  public Guid Id { get; init; }
}