using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Snackis.Areas.Identity.Data;
using Snackis.Data;
using Snackis.Services;

namespace Snackis.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly SnackisContext _context;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IUserServices _userServices;

        public DeletePersonalDataModel(
            SnackisContext context,
            UserManager<SnackisUser> userManager,
            SignInManager<SnackisUser> signInManager,
            IUserServices userServices,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userServices = userServices;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            user.UserName = "";
            user.NormalizedUserName = "";
            user.Email = "";
            user.NormalizedEmail = "";
            user.PasswordHash = "";
            user.SecurityStamp = "";
            user.ConcurrencyStamp = "";
            user.PhoneNumber = "";

            await _context.SaveChangesAsync();

            await _userServices.RemoveUserFromGroupsAsync(user);



            //var result = await _userManager.DeleteAsync(user);
            //var userId = await _userManager.GetUserIdAsync(user);
            //if (!result.Succeeded)
            //{
            //    throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            //}

            await _signInManager.SignOutAsync();

            //_logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }


    }
}
