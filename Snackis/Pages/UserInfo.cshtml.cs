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
        private readonly IUserServices _userService;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly IGroupServices _groupServices;

        public UserInfoModel(IUserServices userService, UserManager<SnackisUser> userManager, IGroupServices groupServices)
        {
            _groupServices = groupServices;
            _userService = userService;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        [BindProperty]
        public string ImageUrl { get; set; }

        public SnackisUser SnackisUser { get; set; }
        public int SubThreadStarted { get; set; }
        public int ForumMessages { get; set; }
        public int MemberInGroups { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userService.GetUserWithMessagesAndSubThreadsAsync(UserId);
            SnackisUser = user;
            UserImage img = _userService.GetUserImage(user);
            if (img == null)
            {
                ImageUrl = "http://placehold.it/300x300";
            }
            else
            {
                string imageBase64Data = Convert.ToBase64String(img.Data);
                ImageUrl = string.Format($"data:image/jpg;base64, {imageBase64Data}");
            }
            var userInfo = await _userService.GetUserWithMessagesAndSubThreadsAsync(user.Id);
            SubThreadStarted = userInfo.SubThreads.Count();
            ForumMessages = userInfo.Messages.Count();
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
            if (files.Count() != 0)
            {
                var file = files[0];
                img.Title = file.FileName;
                img.SnackisUserId = user.Id;

                using (MemoryStream ms = new())
                {
                    file.CopyTo(ms);
                    img.Data = ms.ToArray();
                }
                await _userService.SaveUserImage(img);
            }
        }
    }
}
