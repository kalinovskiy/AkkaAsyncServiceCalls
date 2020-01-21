using System;
using System.Collections.Generic;
using System.Linq;
using AkkaAsyncServiceCalls.Common.Dtos;

namespace AkkaAsyncServiceCalls.ServiceSender.Data
{
    internal class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public void SaveEmailMessage(DtoEmailMessage message)
        {
            _context.Add(new EmailMessage
            {
                Id = message.Id,
                CreateDate = DateTime.Now,
                Email = message.Email,
                Text = message.Text
            });
            _context.SaveChanges();
        }

        public void SaveSmsMessage(DtoSmsMessage message)
        {
            _context.Add(new SmsMessage
            {
                Id = message.Id,
                CreateDate = DateTime.Now,
                Phone = message.Phone,
                Text = message.Text
            });
            _context.SaveChanges();
        }

        public List<DtoEmailMessage> GetEmailMessages()
        {
            return _context.EmailMessages.Select(i => new DtoEmailMessage
            {
                Id = i.Id,
                Email = i.Email,
                Text = i.Text
            }).ToList();
        }

        public List<DtoSmsMessage> GetSmsMessages()
        {
            return _context.SmsMessages.Select(i => new DtoSmsMessage
            {
                Id = i.Id,
                Phone = i.Phone,
                Text = i.Text
            }).ToList();
        }
    }
}
