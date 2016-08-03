using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Remotus.Base;
using Remotus.Base.Net;

namespace Remotus.Core.Net.Client
{
    public class HubConnector : IHubConnector
    {
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
        public Base.Net.ConnectionState State
        {
            get
            {
                Base.Net.ConnectionState state;
                //if (_isReconnecting)
                //    state = Base.Net.ConnectionState.Reconnecting;
                //else
                    state = (Base.Net.ConnectionState)(int)(_hubConnection?.State ?? Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected);
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
                    result = _hubConnection.EnsureReconnecting();
                    if (!result)
                    {
                        ThreadPool.QueueUserWorkItem(delegate (object state)
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
                                    try
                                    {
                                        _isReconnecting = true;
                                        var task = Connect();
                                        task.Wait();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    finally
                                    {

                                    }
                                
                                    var timeout = TimeSpan.FromSeconds(3);
                                    Thread.Sleep(timeout);
                                }
                                _isReconnecting = false;
                            }
                        });
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


        public void Disconnect()
        {
            _ensureReconnect = false;

            var error = new Exception("My custom exc. Closing hub connection...");
            try
            {
                _hubConnection?.Stop(error);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void HubConnection_OnStateChanged(StateChange stateChange)
        {
            if (stateChange.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
            {
                if (_ensureReconnect)
                    EnsureReconnecting();
            }

            var oldState = (Base.Net.ConnectionState) (int) (stateChange.OldState);
            var newState = (Base.Net.ConnectionState) (int) (stateChange.NewState);
            var args = new HubConnectionStateChange(oldState, newState);
            StateChanged?.Invoke(this, args);
        }
        


        public void Dispose()
        {
            _hubConnection.StateChanged -= HubConnection_OnStateChanged;
            _isDisposing = true;
        }
    }
}
