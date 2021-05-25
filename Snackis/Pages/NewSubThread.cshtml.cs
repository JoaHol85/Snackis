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

namespace Snackis.Pages.Shared
{
    public class NewSubThreadModel : PageModel
    {
        private readonly SubThreadServices _subService;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;

        public NewSubThreadModel(SubThreadServices subService, UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager)
        {
            _subService = subService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty(SupportsGet = true)]
        public int MainThreadId { get; set; }

        [BindProperty]
        public SubThread ASubThread { get; set; }
        [BindProperty]
        public Message AMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.GetUserAsync(User);
            int subThreadId = await _subService.CreateNewSubThreadAsync(ASubThread, AMessage, user);
            return RedirectToPage($"./MessagePage", new { SubThreadId = subThreadId });
        }
    }
}
