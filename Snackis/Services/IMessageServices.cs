using Snackis.Areas.Identity.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public interface IMessageServices
    {
        Task AddSmileyToMessageAsync(SnackisUser user, int smileyNumber, int messageId);
        Task ChangeRemovedMessage(Message message, string newText);
        Task DecreaseSmileyCountAsync(int oldSmileyNumber, int messageId);
        Task DeleteMessageAsync(int id);
        Task DeleteMessageImageAsync(int imageId);
        Task DeleterReportedMessagesAsync(int id);
        Task EditMessageTextAsync(string newText, int messageId);
        string GetMessageImage(MessageImage image);     //TEST
        Task<List<Message>> GetMessagesWithReportingsAsync();
        Task<List<ReportedMessage>> GetReportedMessagesAsync(int id);
        Task<Message> GetSingleMessageAsync(int messageId);
        Task<List<Message>> GetSubIdMessagesAsync(int subThreadId);
        Task IncreaseSmileyCountAsync(int smileyNumber, int messageId);
        Task ReportMessageAsync(Message message, string text);
        Task<int> SaveMessageAsync(Message message, SnackisUser user, int subThreadId);
        Task<bool> SaveMessageImage(MessageImage img);
    }
}
