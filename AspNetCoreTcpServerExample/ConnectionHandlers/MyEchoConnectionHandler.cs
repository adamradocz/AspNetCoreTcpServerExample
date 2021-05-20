using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AspNetCoreTcpServerExample.ConnectionHandlers
{
    public class MyEchoConnectionHandler : ConnectionHandler
    {
        private readonly ILogger<MyEchoConnectionHandler> _logger;

        public MyEchoConnectionHandler(ILogger<MyEchoConnectionHandler> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync(ConnectionContext connectionContext)
        {
            _logger.LogInformation($"{connectionContext.ConnectionId} - {connectionContext.RemoteEndPoint} connected");

            while (true)
            {
                var result = await connectionContext.Transport.Input.ReadAsync();
                var buffer = result.Buffer;

                foreach (var segment in buffer)
                {
                    await connectionContext.Transport.Output.WriteAsync(segment);
                }

                if (result.IsCompleted)
                {
                    break;
                }

                connectionContext.Transport.Input.AdvanceTo(buffer.End);
            }

            _logger.LogInformation($"{connectionContext.ConnectionId} - {connectionContext.RemoteEndPoint} disconnected");
        }
    }
}
