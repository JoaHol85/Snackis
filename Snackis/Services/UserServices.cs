using Microsoft.AspNetCore.Identity;
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

    }
}
