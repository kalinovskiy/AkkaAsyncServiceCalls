using Akka.Actor;
using AkkaAsyncServiceCalls.Common.Dtos;
using AkkaAsyncServiceCalls.Site.Actors;
using Microsoft.Extensions.DependencyInjection;

namespace AkkaAsyncServiceCalls.Site.Services
{
    internal class NotificationService : INotificationService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ActorSystem _system;

        private IActorRef _coordinator;
        private readonly object _locker = new object();

        public NotificationService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _system = ActorSystem.Create("Notifications");
        }

        public void SendNotification(DtoPerson person, string text)
        {
            lock (_locker)
            {
                if (_coordinator == null)
                {
                    _coordinator = _system.ActorOf(Props.Create(() =>
                        new ActorCoordinator(_serviceScopeFactory)), "SendersCoordinator");
                }
            }

            _coordinator.Tell(new DtoNotification
            {
                Person = person,
                Text = text
            });
        }

        public void Stop()
        {
            if (_coordinator != null)
            {
                _system.Stop(_coordinator);
                _coordinator = null;
            }
        }
    }
}
