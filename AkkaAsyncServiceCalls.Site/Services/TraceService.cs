using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AkkaAsyncServiceCalls.Site.Services
{
    internal class TraceService : ITraceService
    {
        private readonly IHubContext<ChatHub> _hub;

        public TraceService(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public async Task WriteMessageAsync(string message)
        {
            await _hub.Clients.All.SendAsync("TraceMessage", $"{DateTime.Now:T}:{message}");
        }

        public async Task Finish()
        {
            await _hub.Clients.All.SendAsync("Finish");
        }
    }
}
