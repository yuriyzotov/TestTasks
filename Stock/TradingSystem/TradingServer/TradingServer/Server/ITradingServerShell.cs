using System;
using System.Threading.Tasks;
namespace TradingServer.Server
{
    public interface ITradingServerShell
    {
        Task Run();
        void Stop();
    }
}
