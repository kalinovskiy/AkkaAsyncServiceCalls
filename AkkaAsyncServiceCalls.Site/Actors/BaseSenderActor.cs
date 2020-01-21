using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Pattern;
using AkkaAsyncServiceCalls.Common.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace AkkaAsyncServiceCalls.Site.Actors
{
    public abstract class BaseSenderActor<TMessage> : ReceiveActor
    {
        protected IServiceScopeFactory ScopeFactory { get; }

        protected BaseSenderActor(IServiceScopeFactory scopeFactory, CircuitBreaker breaker)
        {
            ScopeFactory = scopeFactory;

            ReceiveAsync<TMessage>(async msg =>
            {
                await breaker.WithCircuitBreaker(() => Process(msg));
            });
        }

        protected override void PreRestart(Exception reason, object message)
        {
            if (message != null)
            {
                Self.Forward(message);
            }

            base.PreRestart(reason, message);
        }

        protected virtual async Task Process(TMessage msg)
        {
            await SpecificProcessAction(msg);

            Sender.Tell(new DtoCompleted
            {
                ActorRef = Self.ToString()
            });
        }

        protected abstract Task SpecificProcessAction(TMessage msg);
    }
}
