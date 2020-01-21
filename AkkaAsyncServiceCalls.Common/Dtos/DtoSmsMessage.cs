using System;

namespace AkkaAsyncServiceCalls.Common.Dtos
{
    public class DtoSmsMessage
    {
        public Guid Id { get; set; }

        public string Phone { get; set; }

        public string Text { get; set; }
    }
}
