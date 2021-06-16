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
    public class EditMessagePageModel : PageModel
    {
        private readonly IMessageServices _messageServices;
        private readonly IGroupServices _groupServices;
        private readonly UserManager<SnackisUser> _userManager;

        public EditMessagePageModel(UserManager<SnackisUser> userManager, IMessageServices messageServices, IGroupServices groupServices)
        {
            _userManager = userManager;
            _messageServices = messageServices;
            _groupServices = groupServices;
        }

        [BindProperty(SupportsGet = true)]
        public int MessageId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int GroupMessageId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int DeleteImageId { get; set; }


        [BindProperty]
        public string NewText { get; set; }


        public GroupMessage GroupMessageToEdit { get; set; }
        public List<MessageImage> GroupMessageImagesToEdit { get; set; }
        public Message MessageToEdit { get; set; }
        public List<MessageImage> MessageImagesToEdit { get; set; }


        public async Task OnGetAsync()
        {
            if (MessageId != 0 && GroupMessageId == 0)
            {
                if (DeleteImageId != 0)
                {
                    await _messageServices.DeleteMessageImageAsync(DeleteImageId);
                }
                MessageToEdit = await _messageServices.GetSingleMessageAsync(MessageId);
                MessageImagesToEdit = MessageToEdit.MessageImages.ToList();
            }
            if (MessageId == 0 && GroupMessageId != 0)
            {
                GroupMessageToEdit = await _groupServices.GetSingleGroupMessageAsync(GroupMessageId);
                GroupMessageImagesToEdit = GroupMessageToEdit.MessageImages.ToList();
            }
        }

        public async Task OnPostMessageImageAsync()
        {
            MessageImage img = new();
            var user = await _userManager.GetUserAsync(User);
            var files = Request.Form.Files;
            if (files.Count() != 0)
            {
                var file = files[0];
                img.Title = file.FileName;
                img.MessageId = MessageId;

                using (MemoryStream ms = new())
                {
                    file.CopyTo(ms);
                    img.Data = ms.ToArray();
                }
                await _messageServices.SaveMessageImage(img);
            }
            await OnGetAsync();
        }

        public async Task OnPostChangeMessageTextAsync()
        {
            await _messageServices.EditMessageTextAsync(NewText, MessageId);
            await OnGetAsync();
        }
    }
}
