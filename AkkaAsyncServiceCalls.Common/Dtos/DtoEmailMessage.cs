using System;

namespace AkkaAsyncServiceCalls.Common.Dtos
{
    public class DtoEmailMessage
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }
    }
}
