using Microsoft.EntityFrameworkCore;

namespace AkkaAsyncServiceCalls.ServiceSender.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<EmailMessage> EmailMessages { get; set; }

        public DbSet<SmsMessage> SmsMessages { get; set; }
    }
}
