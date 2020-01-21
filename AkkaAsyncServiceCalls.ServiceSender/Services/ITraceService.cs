using System.Threading.Tasks;

namespace AkkaAsyncServiceCalls.ServiceSender.Services
{
    public interface ITraceService
    {
        Task WriteMessageAsync(string method, string message);
    }
}
