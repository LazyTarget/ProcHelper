using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.AspNet.SignalR.Client;
using Timer = System.Timers.Timer;

namespace Remotus.API.Hubs.Client
{
    public class AutoReconnectService : IClientHubManagerService
    {
        private readonly Timer _reconnectTimer;
        private ClientHubManager _manager;

        public AutoReconnectService()
        {
            _reconnectTimer = new Timer();
            _reconnectTimer.AutoReset = true;
            _reconnectTimer.Enabled = true;
            _reconnectTimer.Elapsed += ReconnectTimer_OnElapsed;

            AutoReconnect = true;
            Timeout = TimeSpan.FromSeconds(30);
            ReconnectInterval = TimeSpan.FromSeconds(10);
        }


        public bool AutoReconnect
        {
            get { return _reconnectTimer.Enabled; }
            set
            {
                _reconnectTimer.Enabled = value;
                if (_reconnectTimer.Enabled)
                {
                    if (_manager?.Connection?.State == ConnectionState.Disconnected)
                    {
                        _reconnectTimer.Start();
                    }
                }
            }
        }


        public TimeSpan Timeout { get; set; }


        public TimeSpan ReconnectInterval
        {
            get { return TimeSpan.FromMilliseconds(_reconnectTimer.Interval); }
            set { _reconnectTimer.Interval = value.TotalMilliseconds; }
        }




        public void Register(ClientHubManager manager)
        {
            if (manager?.Connection == null)
                throw new ArgumentNullException(nameof(manager));

            if (_manager != null)
            {
                Unregister(_manager);
            }
            manager.Connection.StateChanged += Connection_OnStateChanged;
            _manager = manager;
        }


        public void Unregister(ClientHubManager manager)
        {
            if (manager?.Connection != null)
                manager.Connection.StateChanged -= Connection_OnStateChanged;
            if (manager == _manager)
                _manager = null;
        }




        private void Connection_OnStateChanged(StateChange stateChange)
        {
            if (_manager?.Connection.State != ConnectionState.Disconnected)
                return;
            
            if (AutoReconnect)
            {
                _reconnectTimer.Stop();
                _reconnectTimer.Start();
            }
        }


        private void ReconnectTimer_OnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_manager?.Connection.State == ConnectionState.Connected)
            {
                _reconnectTimer.Stop();
                return;
            }

            if (AutoReconnect)
            {
                Reconnect();
            }
        }


        private void Reconnect()
        {
            try
            {
                if (_manager?.Connection.State != ConnectionState.Disconnected)
                    return;

                System.Console.WriteLine("Re-connecting to hub...");
                var task = _manager.Connection.Start();
                task.Wait(Timeout);
            }
            catch (AggregateException ex)
            {
                var msg = ex.GetBaseException().Message;
                System.Console.WriteLine("Error re-connecting to hub: " + msg);
                System.Diagnostics.Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                var msg = ex.GetBaseException().Message;
                System.Console.WriteLine("Error re-connecting to hub: " + msg);
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

    }
}