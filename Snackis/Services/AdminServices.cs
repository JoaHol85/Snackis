using Microsoft.AspNetCore.Identity;
using Snackis.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public class AdminServices: IAdminServices
    {
        private readonly SnackisContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminServices(SnackisContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }


        public async Task PostMainThreadAsync(MainThread mainThread)
        {
            _context.MainThreads.Add(mainThread);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRole(string roleName)       
        {
            bool exist = await _roleManager.RoleExistsAsync(roleName);

            if (!exist)
            {
                var role = new IdentityRole
                {
                    Name = roleName
                };
                await _roleManager.CreateAsync(role);
            }
        }


    }
}
