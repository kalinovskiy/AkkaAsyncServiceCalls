using System;
using System.ComponentModel.DataAnnotations;

namespace AkkaAsyncServiceCalls.ServiceSender.Data
{
    public class SmsMessage
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Phone { get; set; }

        public string Text { get; set; }
    }
}
