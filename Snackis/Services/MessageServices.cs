using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public class MessageServices
    {
        private readonly SnackisContext _context;

        public MessageServices(SnackisContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetSubIdMessagesAsync(int subThreadId)
        {
            return await _context.Messages.Where(m => m.SubThreadId == subThreadId).ToListAsync();
        }

        public async Task<Message> GetSingleMessage(int messageId)
        {
            return await _context.Messages.FirstAsync(m => m.Id == messageId);
        }

        public async Task<List<Message>> GetMessagesWithReportingsAsync()
        {
            return await _context.Messages.Where(m => m.TimesReported >= 1).ToListAsync();
        }

        public async Task<List<ReportedMessage>> GetReportedMessagesAsync(int id)
        {
            return await _context.ReportedMessages.Where(r => r.MessageId == id).ToListAsync();
        }

        public async Task DeleteMessage(int id)
        {
            Message msg = await _context.Messages.FirstAsync(m => m.Id == id);
            _context.Messages.Remove(msg);
            await _context.SaveChangesAsync();
        }

        public async Task DeleterReportedMessagesAsync(int id)
        {
            List<ReportedMessage> list = await GetReportedMessagesAsync(id);
            _context.ReportedMessages.RemoveRange(list);
            await _context.SaveChangesAsync();
        }

        public async Task SaveMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task ReportMessageAsync(Message message, string text)
        {
            _context.Entry(message).State = EntityState.Modified;
            ReportedMessage reportedMessage = new()
            {
                Text = text,
                MessageId = message.Id
            };
            _context.ReportedMessages.Add(reportedMessage);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeRemovedMessage(Message message)
        {
            // Här ska finnas kod för att ändra texten i meddelandet.
            message.TimesReported = 0;
            await _context.SaveChangesAsync();
        }

    }
}
