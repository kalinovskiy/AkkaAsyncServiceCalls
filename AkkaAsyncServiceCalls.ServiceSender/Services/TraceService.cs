using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AkkaAsyncServiceCalls.ServiceSender.Services
{
    internal class TraceService : ITraceService
    {
        private readonly IHubContext<ChatHub> _hub;

        public TraceService(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public async Task WriteMessageAsync(string method, string message)
        {
            await _hub.Clients.All.SendAsync(method, $"{DateTime.Now:T}:{message}");
        }
    }
}
