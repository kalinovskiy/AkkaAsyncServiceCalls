using System.Threading.Tasks;

namespace AkkaAsyncServiceCalls.Site.Services
{
    internal interface ITraceService
    {
        Task WriteMessageAsync(string message);

        Task Finish();
    }
}
