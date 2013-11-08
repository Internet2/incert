using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Org.InCommon.InCert.BootstrapperEngine.Handlers
{
    class LaunchActionHandler
    {
        private readonly Dispatcher _dispatcher;
        private readonly Dictionary<LaunchAction, Action> _stateHandlers;

        public LaunchActionHandler(Dispatcher dispatcher, Dictionary<LaunchAction, Action> handlers)
        {
            _dispatcher = dispatcher;
            _stateHandlers = handlers;
        }

        public LaunchActionHandler(Dispatcher dispatcher): this (dispatcher, new Dictionary<LaunchAction, Action>())
        {
        }

        public void AddHandler(LaunchAction state, Action handler)
        {
            _stateHandlers[state] = handler;
        }

        public void HandleState(LaunchAction state)
        {
            if (!_stateHandlers.ContainsKey(state))
                return;

            _dispatcher.Invoke(_stateHandlers[state]);
        }
    }
}
