using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Generator.Hubs
{
    public class StockDataHub: Hub
    {

        public async IAsyncEnumerable<string> streaming(CancellationToken stopToken)
        {
            while(!stopToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stopToken); 
                yield return DateTime.UtcNow.ToString("HH:mm:ss");
            }
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("update", message).ConfigureAwait(false);
        }
    }
}