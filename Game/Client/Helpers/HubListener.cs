using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class HubListener
    {
        private readonly NavigationManager _navManager;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public bool HasUnreadMessage { get; set; } = false;

        public HubConnection Connection { get; private set; }
        public MessageStorage MessageStorage { get; private set; }
        public event Action OnChange;
        public bool IsConnected => Connection.State == HubConnectionState.Connected;



        public HubListener(NavigationManager navManager, MessageStorage messageStorage, IAccessTokenProvider accessTokenProvider)
        {
            _navManager = navManager;
            MessageStorage = messageStorage;
            _accessTokenProvider = accessTokenProvider;

            Connection = new HubConnectionBuilder().WithUrl(_navManager.ToAbsoluteUri("/chathub"))
                .WithAutomaticReconnect()
                .Build();
        }

        public async Task StartAsync()
        {
            var tokenResult = await _accessTokenProvider.RequestAccessToken();

            Connection = new HubConnectionBuilder()
                .WithUrl(_navManager.ToAbsoluteUri("/chathub"), options =>
                {
                    if (tokenResult.TryGetToken(out var token))
                    {
                        options.Headers.Add("Authorization", $"Bearer {token.Value}");
                    }
                })
                .WithAutomaticReconnect()
                .Build();


            Connection.On<string, string, string>
            ("ReceiveMessage", (senderUser, targetUser, message) =>
            {
                var encodedMsg = $"{senderUser}: {message}";
                MessageStorage.AddMessage(encodedMsg);
                if (_navManager.Uri.Contains("chat") == false)
                    HasUnreadMessage = true;

                NotifyStateChanged();
            });

            Connection.On<string, string, string>
            ("ReceiveSystemMessage", (senderUser, targetUser, message) =>
            {
                var encodedMsg = $"{senderUser}: {message}";
                MessageStorage.AddMessage(encodedMsg);
                if (_navManager.Uri.Contains("chat") == false)
                    HasUnreadMessage = true;

                NotifyStateChanged();
            });

            await ConnectWithRetryAsync(Connection);
        }

        public static async Task<bool> ConnectWithRetryAsync(HubConnection connection)
        {
            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    await connection.StartAsync();
                    Debug.Assert(connection.State == HubConnectionState.Connected);
                    return true;
                }
                catch
                {
                    // Failed to connect, trying again in 5000 ms.
                    Debug.Assert(connection.State == HubConnectionState.Disconnected);
                    await Task.Delay(5000);
                }
            }
        }

        public async Task Send(string messageInput, string targetUsername = null)
        {
            if (!string.IsNullOrEmpty(messageInput))
            {
                await Connection.SendAsync("SendMessage", targetUsername, messageInput);
                NotifyStateChanged();
            }


        }

        public async Task SendSystemMessage(string targetUsername, string messageInput)
        {
            if (!string.IsNullOrEmpty(messageInput))
            {
                await Connection.SendAsync("SendSystemMessage", targetUsername, messageInput);
                NotifyStateChanged();
            }
        }


        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
