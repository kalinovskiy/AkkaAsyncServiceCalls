using AkkaAsyncServiceCalls.Common.Dtos;

namespace AkkaAsyncServiceCalls.Site.Services
{
    public interface INotificationService
    {
        void SendNotification(DtoPerson person, string text);

        void Stop();
    }
}
