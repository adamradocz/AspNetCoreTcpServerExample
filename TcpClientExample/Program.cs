using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpClientExample
{
    class Program
    {
        private const int _portNum = 8007;
        private const string _hostName = "127.0.0.1";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any key when the TCP server has started...");
            Console.ReadKey();

            var tcpClient = new TcpClient();

            try
            {
                await tcpClient.ConnectAsync(_hostName, _portNum);
                await using var networkStream = tcpClient.GetStream();

                // Translate the Message into ASCII.
                string message = "A message from the TCP client.";
                byte[] data = Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                await networkStream.WriteAsync(data.AsMemory(0, data.Length));
                Console.WriteLine($"Sent: {message}");

                // Read the Tcp Server Response Bytes.
                byte[] receivedData = new byte[tcpClient.ReceiveBufferSize];
                int receivedBytes = await networkStream.ReadAsync(receivedData.AsMemory(0, receivedData.Length));
                string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, receivedBytes);
                Console.WriteLine($"Received: {receivedMessage}");

                networkStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                tcpClient?.Close();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
