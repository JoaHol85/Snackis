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

namespace Snackis.Pages
{
    public class SubThreadPageModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SubThreadServices _subService;
        private readonly MainThreadServices _mainService;
        private readonly UserServices _userServices;


        public SubThreadPageModel(UserServices userServices, MainThreadServices mainService, SubThreadServices subService, UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager)
        {
            _userServices = userServices;
            _mainService = mainService;
            _subService = subService;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [BindProperty(SupportsGet = true)]
        public int MainThreadId { get; set; }

        public List<SubThread> ListOfSubThreads { get; set; }
        public MainThread MainThreadCopy { get; set; }
        public bool UserIsSignedIn { get; set; } = false;

        public async Task<IActionResult> OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                UserIsSignedIn = true;
            }

            ListOfSubThreads = await _subService.GetMainIdSubThreadsAsync(MainThreadId);
            MainThreadCopy = await _mainService.GetSingleMainThread(MainThreadId);

            return Page();
        }

        public string GetUserNickName(string id)
        {
            var user = _userServices.GetUser(id);
            return user.NickName;
        }
    }
}
