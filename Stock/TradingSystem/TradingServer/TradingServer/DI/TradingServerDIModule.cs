﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using TradingServer.Formatters;

using TradingServer.Model;
using TradingServer.Server;

namespace TradingServer.DI
{
    class TradingServerDIModule: NinjectModule
    {
        public override void Load()
        {
            this.Bind<IQuoteValidator>().To<QuoteValidator>();
            this.Bind<IQuotesModel>().To<QuotesTable>().InSingletonScope();
            this.Bind<IQuotesSource>().To<QuotesSource>().InSingletonScope();
            this.Bind<ITradingServerShell>().To<TradingServerShell>().InSingletonScope();
            this.Bind<IClientSynchronisation>().To<ClientSynchronisation>().InSingletonScope();
            this.Bind<IClentListener>().To<ClentListener>();
        }
    }
}
