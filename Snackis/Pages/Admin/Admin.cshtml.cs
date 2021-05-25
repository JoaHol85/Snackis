using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Areas.Identity.Data;
using Snackis.Data.Models;
using Snackis.Services;

namespace Snackis.Pages.Admin
{
    public class AdminModel : PageModel
    {
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly AdminServices _adminServices;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminModel(RoleManager<IdentityRole> roleManager, AdminServices adminServices, SignInManager<SnackisUser> signInManager, UserManager<SnackisUser> userManager)
        {
            _roleManager = roleManager;
            _adminServices = adminServices;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public MainThread AMainThread { get; set; }
        [BindProperty]
        public string RoleName { get; set; }



        public bool AdminIsLoggedIn { get; set; } = false;
        public List<IdentityRole> RoleList { get; set; }
        public List<SnackisUser> UserList { get; set; }



        public async Task<IActionResult> OnGet()
        {
            IdentityUser identity = await _userManager.GetUserAsync(User);

            if (_signInManager.IsSignedIn(User) && identity.UserName == "Admin@admin.admin")
            {
                AdminIsLoggedIn = true;
                RoleList = _roleManager.Roles.ToList();
                //ska lägga in users i UserList!
            }
            else
            {
                return Redirect("/Login");
            }
            return Page();
        }

        public async Task OnPostMainThreadAsync()
        {
            await _adminServices.PostMainThreadAsync(AMainThread);
        }

        public async Task<IActionResult> OnPostCreateRoleAsync()
        {
            if (RoleName != null)
            {
                await CreateRole(RoleName);
            }
            return RedirectToPage("/Admin/Admin");
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
