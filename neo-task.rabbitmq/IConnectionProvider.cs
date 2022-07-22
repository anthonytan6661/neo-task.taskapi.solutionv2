using RabbitMQ.Client;
using System;

namespace neo_task.rabbitmq
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}