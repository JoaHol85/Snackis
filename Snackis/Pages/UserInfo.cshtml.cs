using System;
using System.Collections.Generic;
using System.IO;
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

        [BindProperty]
        public string ImageUrl { get; set; }

        public SnackisUser SnackisUser { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userService.GetUserWithMessagesAndSubThreadsAsync(UserId);
            SnackisUser = user;
            UserImage img = _userService.GetImage(user);
            if (img == null)
            {
                ImageUrl = "http://placehold.it/300x300";
            }
            else
            {
                string imageBase64Data = Convert.ToBase64String(img.Data);
                ImageUrl = string.Format($"data:image/jpg;base64, {imageBase64Data}");
            }

            return Page();
        }

        public async Task OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            await UploadImage(user);
            await OnGetAsync();
        }

        public async Task UploadImage(SnackisUser user)
        {
            UserImage img = new();
            var files = Request.Form.Files;
            var file = files[0];
            img.Title = file.FileName;
            img.SnackisUserId = user.Id;

            using (MemoryStream ms = new())
            {
                file.CopyTo(ms);
                img.Data = ms.ToArray();
            }
            await _userService.SaveImage(img);
        }



    }
}
