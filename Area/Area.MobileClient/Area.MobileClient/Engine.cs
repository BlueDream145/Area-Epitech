using Area.MobileClient.Client;
using Area.MobileClient.Managers;
using Area.Shared.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.MobileClient
{

    public class Engine
    {

        #region "Variables"

        public ClientData Data { get; set; }

        public SimpleClient Network { get; set; }

        private EngineState _state;

        public EngineState State
        {
            get
            {
                return (_state);
            }

            set
            {
                _state = value;
                if (OnStateChanged != null)
                    OnStateChanged(this, new StateChangedEventArgs(_state));
            }
        }

        public delegate void MessageEventHandler(object sender, StateChangedEventArgs args);

        public event MessageEventHandler OnStateChanged;

        #endregion

        #region "Builder"

        public Engine()
        {
            State = EngineState.Disconnected;
            Data = new ClientData();
        }

        #endregion

        #region "Methods"

        public void Dispose()
        {
            if (Data != null)
            {
                Data.Dispose();
                Data = null;
            }
            if (Network != null)
            {
                Network.Dispose();
                Network = null;
            }
            GC.Collect();
        }

        #endregion

        #region "Events"

        public class StateChangedEventArgs : EventArgs
        {
            EngineState State;

            public StateChangedEventArgs(EngineState _state)
            { State = _state; }
        };
        public void StateChanged(Object obj, StateChangedEventArgs eventArgs)
        {
            if (OnStateChanged != null)
            {
                OnStateChanged(obj, eventArgs);
            }
        }

        #endregion
    }
}
