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
        private readonly IBadWordGateway _gateway;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly IAdminServices _adminServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserServices _userServices; //NYTT

        public AdminModel(IUserServices userServices, RoleManager<IdentityRole> roleManager, IBadWordGateway gateway, IAdminServices adminServices, SignInManager<SnackisUser> signInManager, UserManager<SnackisUser> userManager)
        {
            _userServices = userServices;
            _roleManager = roleManager;
            _adminServices = adminServices;
            _signInManager = signInManager;
            _userManager = userManager;
            _gateway = gateway;
        }
        [BindProperty(SupportsGet = true)]
        public int DeleteBadWordId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AddUserIdToRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public string RemoveUserIdFromRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Role { get; set; }






        [BindProperty]
        public MainThread AMainThread { get; set; }
        [BindProperty]
        public string RoleName { get; set; }
        [BindProperty]
        public BadWord ABadWord { get; set; }


        public bool AdminIsLoggedIn { get; set; } = false;
        public List<IdentityRole> RoleList { get; set; }
        public List<SnackisUser> UserList { get; set; }
        public List<BadWord> ListOfBadWords { get; set; }



        public async Task<IActionResult> OnGet()
        {
            IdentityUser identity = await _userManager.GetUserAsync(User);
            if (DeleteBadWordId != 0)
            {
                await _gateway.DeleteBadWordAsync(DeleteBadWordId);
            }
            if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
            {
                AdminIsLoggedIn = true;
                RoleList = _roleManager.Roles.ToList();
                UserList = await _userServices.GetAllUsersAsync();
                ListOfBadWords = await _gateway.GetAllBadWordsAsync();
                if (AddUserIdToRole != null)
                {
                    var userToChangeRole = await _userManager.FindByIdAsync(AddUserIdToRole);
                    await _userManager.AddToRoleAsync(userToChangeRole, Role);
                }
                if (RemoveUserIdFromRole != null)
                {
                    var userToRemmoveFromRole = await _userManager.FindByIdAsync(RemoveUserIdFromRole);
                    await _userManager.RemoveFromRoleAsync(userToRemmoveFromRole, Role);
                }
            }
            else
            {
                return Redirect("/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostMainThreadAsync()
        {
            await _adminServices.PostMainThreadAsync(AMainThread);
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostCreateRoleAsync()
        {
            if (RoleName != null)
            {
                await _adminServices.CreateRoleAsync(RoleName);
            }
            return RedirectToPage("/Admin/Admin");
        }

        public async Task<IActionResult> OnPostAddBadWordAsync()
        {
            await _gateway.PostBadWordAsync(ABadWord);
            return RedirectToPage("/Admin/Admin");
        }

    }
}
