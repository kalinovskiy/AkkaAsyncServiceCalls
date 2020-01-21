using System;
using System.ComponentModel.DataAnnotations;

namespace AkkaAsyncServiceCalls.ServiceSender.Data
{
    public class EmailMessage
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }
    }
}
