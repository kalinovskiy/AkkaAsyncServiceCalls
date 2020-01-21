using System.Net;
using AkkaAsyncServiceCalls.Common.Dtos;
using AkkaAsyncServiceCalls.ServiceSender.Data;
using AkkaAsyncServiceCalls.ServiceSender.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AkkaAsyncServiceCalls.ServiceSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ITraceService _traceService;
        private readonly ISettings _settings;

        public MessagesController(
            IMessageRepository messageRepository,
            ITraceService traceService,
            ISettings settings)
        {
            _messageRepository = messageRepository;
            _traceService = traceService;
            _settings = settings;
        }

        [Route("email")]
        [HttpPost]
        public ActionResult SendEmail(DtoEmailMessage message)
        {
            if (!_settings.IsEmailSenderActive)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            _messageRepository.SaveEmailMessage(message);

            _traceService.WriteMessageAsync(
                "EmailMessage",
                $"{message.Email}: {message.Text}"
            );

            return Ok();
        }

        [Route("email")]
        [HttpGet]
        public ActionResult<DtoEmailMessage> GetEmailMessages()
        {
            if (!_settings.IsEmailSenderActive)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return Ok(_messageRepository.GetEmailMessages());
        }

        [Route("sms")]
        [HttpPost]
        public ActionResult SendSms(DtoSmsMessage message)
        {
            if (!_settings.IsSmsSenderActive)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            _messageRepository.SaveSmsMessage(message);
            
            _traceService.WriteMessageAsync(
                "SmsMessage",
                $"{message.Phone}: {message.Text}"
            );

            return Ok();
        }

        [Route("sms")]
        [HttpGet]
        public ActionResult<DtoSmsMessage> GetSmsMessages()
        {
            if (!_settings.IsSmsSenderActive)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return Ok(_messageRepository.GetSmsMessages());
        }
    }
}
