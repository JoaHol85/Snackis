using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Areas.Identity.Data;
using Snackis.Services;

namespace Snackis.Pages
{
    public class UserInfoModel : PageModel
    {
        private readonly UserServices _userService;
        private readonly UserManager<SnackisUser> _userManager;

        public UserInfoModel(UserServices userService, UserManager<SnackisUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }


        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        public SnackisUser SnackisUser { get; set; }



        public async Task OnGetAsync()
        {
            var user = await _userService.GetUserWithMessagesAndSubThreadsAsync(UserId);
            SnackisUser = user;
        }
    }
}
