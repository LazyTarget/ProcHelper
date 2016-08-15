using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Remotus.Base;
using Remotus.Base.Interfaces.Net;
using ConnectionState = Remotus.Base.Interfaces.Net.ConnectionState;

namespace Remotus.Core.Net.Client
{
    public class HubConnector : IHubConnector
    {
        private static readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod()?.DeclaringType?.FullName);

        private readonly HubConnection _hubConnection;
        private bool _isDisposing;
        private bool _ensureReconnect;
        private bool _isReconnecting;
        private readonly object _reconnectLock = new object();

        public HubConnector(HubConnection hubConnection)
        {
            if (hubConnection == null)
                throw new ArgumentNullException(nameof(hubConnection));
            _hubConnection = hubConnection;
            _hubConnection.StateChanged -= HubConnection_OnStateChanged;
            _hubConnection.StateChanged += HubConnection_OnStateChanged;
        }


        public event EventHandler<HubConnectionStateChange> StateChanged;

        public string ConnectionId              => _hubConnection?.ConnectionId;
        public bool Connected                   => _hubConnection?.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected;
        public ConnectionState State
        {
            get
            {
                ConnectionState state;
                //if (_isReconnecting)
                //    state = Base.Net.ConnectionState.Reconnecting;
                //else
                    state = (ConnectionState)(int)(_hubConnection?.State ?? Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected);
                return state;
            }
        }


        public async Task Connect()
        {
            var connection = _hubConnection;
            if (connection != null)
            {
                try
                {
                    await connection.Start();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public bool EnsureReconnecting()
        {
            _ensureReconnect = true;

            var result = false;
            try
            {
                if (_hubConnection != null)
                {
                    // Check if already connected/connecting...
                    if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connecting)
                        return false;
                    if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                        return false;
                    if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Reconnecting)
                        return true;

                    // Trigger reconnection logic
                    if (!result)
                    {
                        //result = _hubConnection.EnsureReconnecting();
                    }

                    if (!result)
                    {
                        result = true;
                        ThreadPool.QueueUserWorkItem(Reconnect);
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        private void Reconnect(object state)
        {
            lock (_reconnectLock)
            {
                while (!_isDisposing && _hubConnection.State != Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                        return;
                    if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connecting)
                        return;
                    if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Reconnecting)
                        return;

                    if (_isReconnecting)
                    {
                        var timeout = TimeSpan.FromSeconds(3);
                        _log.Debug($"HubConnector:Reconnect() Waiting {timeout} before trying to reconnect...");
                        Thread.Sleep(timeout);
                    }

                    Task task = null;
                    try
                    {
                        _isReconnecting = true;
                        task = Connect();
                        task.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {

                    }
                }
                _isReconnecting = false;
            }
        }


        public void Disconnect()
        {
            _ensureReconnect = false;

            var error = new Exception("My custom exc. Closing hub connection...");
            try
            {
                _log.Info($"HubConnector:Disconnect() Disconnecting...");
                _hubConnection?.Stop(error);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void HubConnection_OnStateChanged(StateChange stateChange)
        {
            _log.Debug($"HubConnector:OnStateChanged() OldState: {stateChange.OldState}");
            _log.Info($"HubConnector:OnStateChanged() NewState: {stateChange.NewState}");
            
            if (stateChange.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            {
                _log.Info($"HubConnector:OnStateChanged() Connected as: {ConnectionId}");
            }
            else if (stateChange.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
            {
                if (_ensureReconnect)
                {
                    _log.Debug($"HubConnector:OnStateChanged() Was disconnected, ensure reconnect is set, reconnecting...");
                    EnsureReconnecting();
                }
            }

            var oldState = (ConnectionState) (int) (stateChange.OldState);
            var newState = (ConnectionState) (int) (stateChange.NewState);
            var args = new HubConnectionStateChange(oldState, newState);
            StateChanged?.Invoke(this, args);
        }
        


        public void Dispose()
        {
            Disconnect();

            _hubConnection.StateChanged -= HubConnection_OnStateChanged;
            _isDisposing = true;
        }
    }
}
