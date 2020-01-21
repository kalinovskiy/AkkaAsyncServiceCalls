namespace AkkaAsyncServiceCalls.ServiceSender.Services
{
    public class Settings : ISettings
    {
        public bool IsEmailSenderActive { get; set; } = false;
        public bool IsSmsSenderActive { get; set; } = false;
    }
}
