using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingServer.Configuration
{
    /// <summary>
    /// store server configuration
    /// </summary>
    public interface IServerConfiguration
    {
        /// <summary>
        /// posrt on which server is listening
        /// </summary>
        int ServerPort { get; }
        
        /// <summary>
        /// in millisecinds
        /// </summary>
        int QuotesSourceFrequency { get; }
        
        /// <summary>
        /// in milliseconds
        /// </summary>
        int ClientPushingFrequency { get; }
    }
}
