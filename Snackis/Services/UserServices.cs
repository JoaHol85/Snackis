using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snackis.Areas.Identity.Data;
using Snackis.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public class UserServices
    {
        private readonly SnackisContext _context;

        public UserServices(SnackisContext context)
        {
            _context = context;
        }



        public SnackisUser GetUser(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public async Task<List<SnackisUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

    }
}
