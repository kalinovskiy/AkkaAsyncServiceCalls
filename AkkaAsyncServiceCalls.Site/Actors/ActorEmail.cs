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
    internal class ActorEmail : BaseSenderActor<DtoEmailMessage>
    {
        public ActorEmail(IServiceScopeFactory scopeFactory, CircuitBreaker breaker)
            :base(scopeFactory, breaker)
        {
        }

        protected override async Task SpecificProcessAction(DtoEmailMessage msg)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;

                var traceService = provider.GetService<ITraceService>();
                await traceService.WriteMessageAsync(
                    $"EMAIL_SENDER: sending email for {msg.Email}");

                var settingsService = provider.GetService<ISettingsService>();
                var url = settingsService.GetEmailServiceUrl();

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
