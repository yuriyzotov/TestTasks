using System;
using System.Threading.Tasks;
namespace TradingServer.Server
{
    public interface IClentListener
    {
        Task HandleConnectionAsync(System.Net.Sockets.TcpClient tcpClient);
    }
}
