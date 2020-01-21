using System.Collections.Generic;
using AkkaAsyncServiceCalls.Common.Dtos;

namespace AkkaAsyncServiceCalls.Site.Services
{
    public interface ISettingsService
    {
        List<DtoPerson> GetPersons();

        string GetEmailServiceUrl();

        string GetSmsServiceUrl();

        int GetCallAttemptsCount();

        int GetCallTimeoutInSeconds();

        int GetCallPauseInSeconds();
    }
}
