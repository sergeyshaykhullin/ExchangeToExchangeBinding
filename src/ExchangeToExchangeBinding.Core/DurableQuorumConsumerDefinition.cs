using MassTransit;

namespace ExchangeToExchangeBinding;

public class DurableQuorumConsumerDefinition<T> : ConsumerDefinition<T>
  where T : class, IConsumer
{
  protected override void ConfigureConsumer(
    IReceiveEndpointConfigurator endpointConfigurator,
    IConsumerConfigurator<T> consumerConfigurator)
  {
    if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbitMqEndpointConfigurator)
    {
      rabbitMqEndpointConfigurator.Durable = true;
      rabbitMqEndpointConfigurator.AutoDelete = false;
      rabbitMqEndpointConfigurator.SetQuorumQueue();
    }
  }
}