using Microsoft.EntityFrameworkCore;
using Snackis.Areas.Identity.Data;
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
            var x = await _context.Messages.Where(m => m.SubThreadId == subThreadId).ToListAsync();

            foreach (var message in x)
            {
                message.SnackisUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == message.SnackisUserId);
                message.SmileyInfo = await _context.SmileyInfos.FirstOrDefaultAsync(s => s.Id == message.Id);
            }

            return x;
        }

        public async Task<Message> GetSingleMessage(int messageId)
        {
            var message = await _context.Messages.FirstAsync(m => m.Id == messageId);
            message.SnackisUser = await _context.Users.FirstAsync(u => u.Id == message.SnackisUserId);
            return message;
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

        public async Task SaveMessageAsync(Message message, SnackisUser user, int subThreadId)
        {
            message.SnackisUserId = user.Id;
            message.Time = DateTime.Now;
            message.SubThreadId = subThreadId;
            _context.Messages.Add(message);
            SmileyInfo smiley = new()
            {
                Message = message
            };
            _context.SmileyInfos.Add(smiley);
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

        public async Task ChangeRemovedMessage(Message message, string newText)
        {
            message.TextMessage = newText;
            // Här ska finnas kod för att ändra texten i meddelandet.
            message.TimesReported = 0;
            await _context.SaveChangesAsync();
        }
        
        public async Task AddSmileyToMessageAsync(SnackisUser user, int smileyNumber, int messageId)
        {
            var smiledMessageAlready = await _context.SmileyMessageUsers.FirstOrDefaultAsync(s => s.SmileyInfoId == messageId && s.SnackisUserId == user.Id);

            if(smiledMessageAlready != null)
            {
                await DecreaseSmileyCountAsync(smiledMessageAlready.SmileyNumber, messageId);
                _context.SmileyMessageUsers.Remove(smiledMessageAlready);
                await _context.SaveChangesAsync();
            }

            await IncreaseSmileyCountAsync(smileyNumber, messageId);

            SmileyMessageUser smi = new()
            {
                SmileyNumber = smileyNumber,
                SmileyInfoId = messageId,
                SnackisUserId = user.Id
            };
            _context.SmileyMessageUsers.Add(smi);
            await _context.SaveChangesAsync();
        }

        public async Task DecreaseSmileyCountAsync(int oldSmileyNumber, int messageId)
        {
            var smileyInfo = await _context.SmileyInfos.FirstOrDefaultAsync(s => s.Id == messageId);

            switch (oldSmileyNumber)
            {
                case 1:
                    smileyInfo.HappySmiley--;
                    break;
                case 2:
                    smileyInfo.LaughingSmiley--;
                    break;
                case 3:
                    smileyInfo.SadSmiley--;
                    break;
                case 4:
                    smileyInfo.AngrySmiley--;
                    break;
                case 5:
                    smileyInfo.ThumbUp--;
                    break;
            }

            await _context.SaveChangesAsync();
        }

        public async Task IncreaseSmileyCountAsync(int smileyNumber, int messageId)
        {
            var smileyInfo = await _context.SmileyInfos.FirstOrDefaultAsync(s => s.Id == messageId);
            switch (smileyNumber)
            {
                case 1:
                    smileyInfo.HappySmiley++;
                    break;
                case 2:
                    smileyInfo.LaughingSmiley++;
                    break;
                case 3:
                    smileyInfo.SadSmiley++;
                    break;
                case 4:
                    smileyInfo.AngrySmiley++;
                    break;
                case 5:
                    smileyInfo.ThumbUp++;
                    break;
            }
            await _context.SaveChangesAsync();

        }
    }
}
