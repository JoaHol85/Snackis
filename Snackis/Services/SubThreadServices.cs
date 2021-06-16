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
    public class SubThreadServices : ISubThreadServices
    {
        private readonly SnackisContext _context;
        private readonly IMessageServices _messageServices;

        public SubThreadServices(SnackisContext context, IMessageServices messageServices)
        {
            _messageServices = messageServices;
            _context = context;
        }



        public async Task<List<SubThread>> GetMainIdSubThreadsAsync(int mainThreadId)
        {
            return await _context.SubThreads.Where(s => s.MainThreadId == mainThreadId).ToListAsync();
        }


        public async Task<SubThread> GetSingleSubThreadAync(int subThreadId)
        {
            var subThread = await _context.SubThreads.FindAsync(subThreadId);
            subThread.MainThread = await _context.MainThreads.FindAsync(subThread.MainThreadId);
            return subThread;
        }

        public async Task<int> CreateNewSubThreadAsync(SubThread subThread, Message message, SnackisUser user)
        {
            subThread.SnackisUserId = user.Id;
            message.SnackisUserId = user.Id;
            subThread.Time = DateTime.Now;
            _context.SubThreads.Add(subThread);
            await _context.SaveChangesAsync();
            message.Time = DateTime.Now;
            message.SubThreadId = subThread.Id;
            await _messageServices.SaveMessageAsync(message, user, subThread.Id);
            return subThread.Id;
        }
    }
}
