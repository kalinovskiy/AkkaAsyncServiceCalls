using System;
using System.Collections.Concurrent;
using System.Net.Http;
using Akka.Actor;
using Akka.Pattern;
using AkkaAsyncServiceCalls.Common.Dtos;
using AkkaAsyncServiceCalls.Site.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AkkaAsyncServiceCalls.Site.Actors
{
    internal class ActorCoordinator : UntypedActor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly Props _emailProps;
        private readonly Props _smsProps;

        private readonly ConcurrentDictionary<string, IActorRef> _dictEmailActors =
            new ConcurrentDictionary<string, IActorRef>();

        private readonly ConcurrentDictionary<string, IActorRef> _dictSmsActor =
            new ConcurrentDictionary<string, IActorRef>();

        public ActorCoordinator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            using (var scope = _scopeFactory.CreateScope())
            {
                var traceService = scope.ServiceProvider.GetService<ITraceService>();

                _emailProps = Props.Create(() => new ActorEmail(scopeFactory, CreateBreaker()));
                _smsProps = Props.Create(() => new ActorSms(scopeFactory, CreateBreaker()));

                //traceService.WriteMessageAsync($"COORDINATOR {Self}: created");
            }
        }

        protected override void OnReceive(object message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var traceService = scope.ServiceProvider.GetService<ITraceService>();

                if (message is DtoCompleted cmp)
                {
                    //traceService.WriteMessageAsync(
                    //    $"COORDINATOR {Self}: sending is completed, sender={cmp.ActorRef}");
                    traceService.Finish();
                }
                if (message is Terminated trm)
                {
                    //traceService.WriteMessageAsync(
                    //    $"COORDINATOR {Self}: sending is terminated, error={trm}");
                    traceService.Finish();
                }
                else if (message is DtoNotification msg)
                {
                    traceService.WriteMessageAsync(
                        $"COORDINATOR: sending notifications for { msg.Person.LastName}");
                    ProcessMessage(msg);
                }
            }
        }

        private void ProcessMessage(DtoNotification msg)
        {
            var person = msg.Person;

            //email
            if (!_dictEmailActors.TryGetValue(person.Email, out var emailActor))
            {
                emailActor = Context.ActorOf(_emailProps, $"EmailSender_{msg.Id}");

                _dictEmailActors.TryAdd(person.Email, emailActor);

                Context.Watch(emailActor);
            }

            emailActor.Tell(new DtoEmailMessage
            {
                Id = msg.Id,
                Email = person.Email,
                Text = $"Email for {person.LastName} {person.FirstName}: {msg.Text}"
            }, Self);

            //sms
            if (!_dictSmsActor.TryGetValue(person.Phone, out var smsActor))
            {
                smsActor = Context.ActorOf(_smsProps, $"SmsSender_{msg.Id}");

                _dictSmsActor.TryAdd(person.Phone, smsActor);

                Context.Watch(emailActor);
            }

            smsActor.Tell(new DtoSmsMessage
            {
                Id = msg.Id,
                Phone = person.Phone,
                Text = $"Sms for {person.LastName} {person.FirstName}: {msg.Text}"
            });
        }

        //protected override void PreStart()
        //{
        //    using (var scope = _scopeFactory.CreateScope())
        //    {
        //        var traceService = scope.ServiceProvider.GetService<ITraceService>();
        //        traceService.WriteMessageAsync($"COORDINATOR {Self}: PreStart");
        //    }

        //    base.PreStart();
        //}

        //protected override void PostStop()
        //{
        //    using (var scope = _scopeFactory.CreateScope())
        //    {
        //        var traceService = scope.ServiceProvider.GetService<ITraceService>();
        //        traceService.WriteMessageAsync($"COORDINATOR {Self}: PostStop");
        //    }

        //    base.PostStop();
        //}

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                ex =>
                {
                    if (ex is HttpRequestException || ex is OpenCircuitException)
                    {
                        return Directive.Restart;
                    }
                    return Directive.Stop;
                });
        }

        private CircuitBreaker CreateBreaker()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var settingsService = scope.ServiceProvider.GetService<ISettingsService>();

                var breaker = new CircuitBreaker(
                    settingsService.GetCallAttemptsCount(),
                    TimeSpan.FromSeconds(settingsService.GetCallTimeoutInSeconds()),
                    TimeSpan.FromSeconds(settingsService.GetCallPauseInSeconds()));
                return breaker;
            }
        }
    }
}
