using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingServer.Configuration;

namespace TradingServer.Server
{
    public class ClientSynchronisation : TradingServer.Server.IClientSynchronisation
    {
        public ClientSynchronisation(IServerConfiguration config)
        {
            this.myConfig = config;
            myPushTask = Task.Delay(config.ClientPushingFrequency);
        }

        
        public Task PushNotification
        {
            get
            {
                if(myPushTask.IsCompleted)
                {
                    var tempTask = Task.Delay(myConfig.ClientPushingFrequency);
                    return Interlocked.Exchange<Task>(ref myPushTask, tempTask);
                }
                return myPushTask;
            }
        }

        private IServerConfiguration myConfig;
        private Task myPushTask;
    }
}
