using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Akka.Pattern;
using AkkaAsyncServiceCalls.Common.Dtos;
using AkkaAsyncServiceCalls.Site.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AkkaAsyncServiceCalls.Site.Actors
{
    internal class ActorSms : BaseSenderActor<DtoSmsMessage>
    {
        public ActorSms(IServiceScopeFactory scopeFactory, CircuitBreaker breaker) 
            :base(scopeFactory, breaker)
        {
        }

        protected override async Task SpecificProcessAction(DtoSmsMessage msg)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;

                var traceService = provider.GetService<ITraceService>();
                await traceService.WriteMessageAsync($"SMS_SENDER {Self}: sending sms for {msg.Phone}");

                var settingsService = provider.GetService<ISettingsService>();
                var url = settingsService.GetSmsServiceUrl();

                var json = JsonConvert.SerializeObject(msg);

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync($"{url}",
                    new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpRequestException($"{response.StatusCode}");
                }
            }
        }
    }
}
