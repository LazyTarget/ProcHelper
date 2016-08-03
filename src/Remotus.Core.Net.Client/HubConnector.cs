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
        public Base.Net.ConnectionState State   => (Base.Net.ConnectionState)(int)(_hubConnection?.State ?? Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected);


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
                            while (_hubConnection.State != Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                            {
                                if (_isDisposing)
                                    return;
                                if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                                    return;
                                try
                                {
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
