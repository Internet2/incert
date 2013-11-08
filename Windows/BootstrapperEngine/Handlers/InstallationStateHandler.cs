using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;

namespace Org.InCommon.InCert.BootstrapperEngine.Handlers
{
    class InstallationStateHandler
    {
        private readonly Dispatcher _dispatcher;
        private readonly Dictionary<InstallationState, Action> _stateHandlers;

        public InstallationStateHandler(Dispatcher dispatcher, Dictionary<InstallationState, Action> handlers)
        {
            _dispatcher = dispatcher;
            _stateHandlers = handlers;
        }

        public InstallationStateHandler(Dispatcher dispatcher): this (dispatcher, new Dictionary<InstallationState, Action>())
        {
        }

        public void AddHandler(InstallationState state, Action handler)
        {
            _stateHandlers[state] = handler;
        }

        public void HandleState(InstallationState state)
        {
            if (!_stateHandlers.ContainsKey(state))
                return;

            _dispatcher.Invoke(_stateHandlers[state]);
        }
    }
}
