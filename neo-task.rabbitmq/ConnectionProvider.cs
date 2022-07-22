using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace neo_task.rabbitmq
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IConfiguration _config;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private bool _disposed;

        public ConnectionProvider(IConfiguration config)
        {
            this._config = config;
            this._factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"), //_config["RabbitMQ:HostName"],
                Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBITMQ_PORT")), //Convert.ToInt16(_config["RabbitMQ:Port"]),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER"), //_config["RabbitMQ:UserName"],
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"), //_config["RabbitMQ:Password"],
                VirtualHost = Environment.GetEnvironmentVariable("RABBITMQ_VHOST"), //_config["RabbitMQ:VirtualHost"],
              //  RequestedHeartbeat = TimeSpan.FromSeconds(30)
            };
            this._factory.AutomaticRecoveryEnabled = true;
            this._connection = _factory.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _connection?.Close();

            _disposed = true;
        }
    }
}
