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
    public class GroupServices
    {
        private readonly SnackisContext _context;

        public GroupServices(SnackisContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<List<GroupMessage>> GetAllGroupMessagesInGroup(int groupId)
        {
            return await _context.GroupMessages.Where(g => g.GroupId == groupId).ToListAsync();
        }

        public async Task SaveGroupMessage(SnackisUser user, int groupId, GroupMessage message)
        {
            message.Time = DateTime.Now;
            message.GroupId = groupId;
            message.SnackisUserId = user.Id;
            _context.GroupMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task SaveGroup(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }
    }
}
