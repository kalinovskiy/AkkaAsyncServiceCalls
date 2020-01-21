using System;

namespace AkkaAsyncServiceCalls.Common.Dtos
{
    public class DtoNotification
    {
        public Guid Id { get; } = Guid.NewGuid();

        public DtoPerson Person { get; set; }

        public string Text { get; set; }
    }
}
