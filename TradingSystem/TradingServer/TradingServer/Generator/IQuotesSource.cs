using System;
namespace TradingServer
{
    public interface IQuotesSource
    {
        void Start(int timeout);
        void Stop();
    }
}
