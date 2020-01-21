using System;
using System.Collections.Generic;
using AkkaAsyncServiceCalls.Common.Dtos;
using Microsoft.Extensions.Configuration;

namespace AkkaAsyncServiceCalls.Site.Services
{
    internal class SettingsService : ISettingsService
    {
        private readonly IConfiguration _configuration;

        public SettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<DtoPerson> GetPersons()
        {
            var section = _configuration.GetSection("AppSettings:Persons");
            var persons = new List<DtoPerson>();
            section.Bind(persons);
            return persons;
        }

        public string GetEmailServiceUrl()
        {
            return _configuration.GetSection("AppSettings:EmailServiceUrl").Value;
        }

        public string GetSmsServiceUrl()
        {
            return _configuration.GetSection("AppSettings:SmsServiceUrl").Value;
        }

        public int GetCallAttemptsCount()
        {
            return Convert.ToInt32(_configuration.GetSection("AppSettings:CallAttemptsCount").Value);
        }

        public int GetCallTimeoutInSeconds()
        {
            return Convert.ToInt32(_configuration.GetSection("AppSettings:CallTimeoutInSeconds").Value);
        }

        public int GetCallPauseInSeconds()
        {
            return Convert.ToInt32(_configuration.GetSection("AppSettings:CallPauseInSeconds").Value);
        }
    }
}
