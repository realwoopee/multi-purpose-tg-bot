using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core
{
    public class RouterConfig
    {
        private IServiceProvider _provider;
        public RouterConfig(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IController DefaultController => (IController)_provider.GetService(DefaultControllerPreset);

        public List<(IFilter, IController)> Controllers
        {
            get
            {
                return ControllerPresets.Select(pair =>
                        ((IFilter)_provider.GetService(pair.Filter),
                        (IController)_provider.GetService(pair.Controller)))
                    .ToList();
            }
        }

        public Type DefaultControllerPreset { get; set; }
        public List<RouterConfigPair> ControllerPresets { get; set; }
        
    }

    public class RouterConfigPair
    {
        public Type Filter { get; set; }

        public Type Controller { get; set; }
    }
}