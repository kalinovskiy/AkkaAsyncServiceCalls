namespace AkkaAsyncServiceCalls.ServiceSender.Services
{
    public interface ISettings
    {
        public bool IsEmailSenderActive { get; set; }

        public bool IsSmsSenderActive { get; set; }
    }
}
