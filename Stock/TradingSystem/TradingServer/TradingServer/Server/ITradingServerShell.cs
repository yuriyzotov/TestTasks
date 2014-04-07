using System;
namespace TradingServer.Server
{
    public interface ITradingServerShell
    {
        void Run();
        void Stop();
    }
}
