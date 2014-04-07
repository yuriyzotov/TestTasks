using System;
namespace TradingServer.Server
{
    public interface IClientSynchronisation
    {
        System.Threading.Tasks.Task PushNotification { get; }
    }
}
