using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Areas.Identity.Data;
using Snackis.Data.Models;
using Snackis.Services;

namespace Snackis.Areas.Identity.Pages.Account.Manage
{
    public class ImageModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly UserServices _userServices;

        public ImageModel(
            UserManager<SnackisUser> userManager,
            UserServices userServices)
        {
            _userServices = userServices;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public string ImageUrl { get; set; }

        public SnackisUser SnackisUser { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            SnackisUser = user;
            
            UserImage img = _userServices.GetImage(user);
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
            await _userServices.SaveImage(img);
        }

    }
}
