using System;
namespace TradingServer.Server
{
    public interface IClentListener
    {
        void HandleConnectionAsync(System.Net.Sockets.TcpClient tcpClient);
    }
}
