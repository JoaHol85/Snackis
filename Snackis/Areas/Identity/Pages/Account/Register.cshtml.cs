using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Snackis.Areas.Identity.Data;
using Snackis.Data;
using Snackis.Services;

namespace Snackis.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly SnackisContext _context;
        private readonly IAdminServices _adminServices; //NYTT

        public RegisterModel(
            IAdminServices adminServices,
            SnackisContext context, 
            UserManager<SnackisUser> userManager,
            SignInManager<SnackisUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _adminServices = adminServices;
            _context = context; 
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public bool NickNameTaken { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]         
            [Display(Name = "NickName")]
            public string NickName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                List<string> nickNames = await _context.Users.Select(u => u.NickName.ToLower()).ToListAsync();
                NickNameTaken = nickNames.Contains(Input.NickName.ToLower());
                if (!NickNameTaken)
                {
                    
                    bool admin = _context.Users.Any(); //NYTT
                    //NYTT v
                    List<IdentityRole> roleList = await _context.Roles.ToListAsync();
                    if (roleList.Count() != 0)
                    {
                        bool adminInRoles = false;
                        bool userInRoles = false;
                        foreach (var role in roleList)
                        {
                            if (role.NormalizedName == "ADMIN")
                                adminInRoles = true;
                            if (role.NormalizedName == "USER")
                                userInRoles = true;    
                        }
                        if (!adminInRoles)
                        {
                            await _adminServices.CreateRoleAsync("Admin");
                        }
                        if (!userInRoles)
                        {
                            await _adminServices.CreateRoleAsync("User");
                        }
                    }
                    if (roleList.Count() == 0)
                    {
                        await _adminServices.CreateRoleAsync("Admin");
                        await _adminServices.CreateRoleAsync("User");
                    }

                    //NYTT ^
                    var user = new SnackisUser { UserName = Input.Email, Email = Input.Email, NickName = Input.NickName};
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    await _userManager.AddToRoleAsync(user, "User");
                    //NYTT v
                    if (!admin)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    //NYTT ^
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                } 

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
