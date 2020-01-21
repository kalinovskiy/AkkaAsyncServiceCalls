using System.Collections.Generic;
using AkkaAsyncServiceCalls.Common.Dtos;

namespace AkkaAsyncServiceCalls.ServiceSender.Data
{
    public interface IMessageRepository
    {
        void SaveEmailMessage(DtoEmailMessage message);

        void SaveSmsMessage(DtoSmsMessage message);

        List<DtoEmailMessage> GetEmailMessages();

        List<DtoSmsMessage> GetSmsMessages();
    }
}
