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

        //public async Task<List<Group>> GetAllGroupsAsync()
        //{
        //    //List<Group> groups = await _context.Groups.ToListAsync();
        //    //foreach (var group in groups)
        //    //{
        //    //    List<SnackisUser> list = _context.
        //    //}
        //}

        public async Task<Group> GetSingleGroupByIdAsync(int groupId)
        {
            Group group = await _context.Groups
                .Include(g => g.Users)
                .Include(g => g.GroupMessages)
                .SingleOrDefaultAsync(g => g.Id == groupId);
                    


            //Group group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            //group.GroupMessages = await _context.GroupMessages.Where(m => m.GroupId == groupId).ToListAsync();
            ////TEST
            //var usersInGroup = await _context.Users
            //    .Where(user => user.Groups.Select(grp => grp.Id).Contains(groupId)).ToListAsync();
            //group.Users = new List<SnackisUser>();
            //group.Users = usersInGroup;
            ////TEST
            return group;
        }

        public async Task AddUserToGroupAsync(string userId, int groupId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            Group group = await GetSingleGroupByIdAsync(groupId);
            var userAlreadyInGroup = group.Users.Contains(user);

            if (!userAlreadyInGroup)
            {
                user.Groups = new List<Group>();
                user.Groups.Add(group);
                await _context.SaveChangesAsync();
            }
        }
        //TEST!!!!!!!!!!!!! INTE KLARRRR!!!!
        public async Task RemoveUserFromGroup(string userId, int groupId)
        {

            var x = await _context.Groups
                .Include(g => g.Users)
                .SingleOrDefaultAsync(g => g.Id == groupId);

            var u = x.Users.FirstOrDefault(user => user.Id == userId);

            x.Users.Remove(u);
            await _context.SaveChangesAsync();

            //Group group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            //var x = await _context.Users
            //    .Where(user => user.Groups.Select(grp => grp.Id).Contains(groupId)).ToListAsync();
            //_context.Remove(x);
            //await _context.SaveChangesAsync();
            
            
            //_context.Groups.Remove(x => x.Users.Where(z => z.id == userId));



            //var group = this._context.Groups
            //    .Include(g => g.Users)
            //    .Include(g => g.GroupMessages)
            //    .SingleOrDefault(g => g.Id == groupId);

            //if (group != null)
            //{
            //    foreach (var gUser in group.Users
            //            .Where(u => u.Id == userId)
            //    {
            //        group.Users.Remove(gUser);
            //    }
            //    this._db.SaveChanges();
            //}

            




            //var group = await GetSingleGroupByIdAsync(groupId);
            //List<SnackisUser> newUsersList = group.Users.ToList();
            //newUsersList.Remove(user);
            //group.Users.Clear();
            //group.Users = (newUsersList);

            //await _context.SaveChangesAsync();
        }

        public async Task<List<Group>> GetAllGroupsFromUserAsync(SnackisUser user)
        {
            var allGroupsWithUser = await _context.Groups
                .Where(group => group.Users.Select(usr => usr.Id).Contains(user.Id)).ToListAsync();
            var allGroupsCreatedByUser = await _context.Groups
                .Where(group => group.GroupStartedById == user.Id).ToListAsync();
            allGroupsWithUser.AddRange(allGroupsCreatedByUser);
            return allGroupsWithUser;
        }

        public async Task SaveGroupMessageAsync(SnackisUser user, int groupId, GroupMessage message)
        {
            message.Time = DateTime.Now;
            message.GroupId = groupId;
            message.SnackisUserId = user.Id;
            _context.GroupMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task SaveGroupAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }
    }
}
