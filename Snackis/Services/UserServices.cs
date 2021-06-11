using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snackis.Areas.Identity.Data;
using Snackis.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public class UserServices
    {
        private readonly SnackisContext _context;
        private readonly GroupServices _groupService;

        public UserServices(SnackisContext context, GroupServices groupService)
        {
            _groupService = groupService;
            _context = context;
        }

        public SnackisUser GetUser(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public async Task<List<SnackisUser>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                //user.Groups = new List<Group>();
                user.Groups = new List<Group>(await _groupService.GetAllGroupsFromUserAsync(user));
            }
            return users;
        }

        public async Task<SnackisUser> GetUserWithMessagesAndSubThreadsAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Messages)
                .Include(u => u.SubThreads)
                .FirstAsync(u => u.Id == userId);

            foreach (var message in user.Messages)
            {
                message.SubThread = await _context.SubThreads.FirstAsync(s => s.Id == message.SubThreadId);
            }

            return user;
        }

        public async Task SaveUserImage(UserImage img)
        {
            if (_context.UserImages.FirstOrDefault(i => i.SnackisUserId == img.SnackisUserId) != null)
            {
                _context.UserImages
                    .Remove(_context.UserImages.FirstOrDefault(i => i.SnackisUserId == img.SnackisUserId));
            }
            _context.Add(img);
            await _context.SaveChangesAsync();
        }




        public UserImage GetUserImage(SnackisUser user)
        {
            return _context.UserImages.FirstOrDefault(i => i.SnackisUserId == user.Id);
        }

    }
}
