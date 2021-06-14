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

namespace Snackis.Pages.Shared
{
    public class NewSubThreadModel : PageModel
    {
        private readonly ISubThreadServices _subService;
        private readonly UserManager<SnackisUser> _userManager;

        public NewSubThreadModel(ISubThreadServices subService, UserManager<SnackisUser> userManager)
        {
            _subService = subService;
            _userManager = userManager;
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
            if (AMessage.TextMessage != null && ASubThread.HeaderText != null)
            {
                var user = await _userManager.GetUserAsync(User);
                AMessage.MessageImages = new List<MessageImage>();
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    MessageImage img = new();
                    img.Title = file.FileName;

                    using (MemoryStream ms = new())
                    {
                        file.CopyTo(ms);
                        img.Data = ms.ToArray();
                    }
                    AMessage.MessageImages.Add(img);
                }
                int subThreadId = await _subService.CreateNewSubThreadAsync(ASubThread, AMessage, user);
                return RedirectToPage($"./MessagePage", new { SubThreadId = subThreadId });
            }
            return Page();
        }
    }
}
