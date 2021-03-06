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

        public EditMessagePageModel(IMessageServices messageServices, IGroupServices groupServices)
        {
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
        public Message MessageToEdit { get; set; }


        public async Task OnGetAsync()
        {
            if (MessageId != 0 && GroupMessageId == 0)
            {
                if (DeleteImageId != 0)
                {
                    try
                    {
                        await _messageServices.DeleteMessageImageAsync(DeleteImageId);
                    }
                    catch
                    {
                        Redirect($"/EditMessagePage?MessageId={MessageId}");
                    }
                }
                MessageToEdit = await _messageServices.GetSingleMessageAsync(MessageId);
            }
            if (MessageId == 0 && GroupMessageId != 0)
            {
                if (DeleteImageId != 0)
                {
                    try
                    {
                        await _messageServices.DeleteMessageImageAsync(DeleteImageId);
                    }
                    catch
                    {
                        Redirect($"/EditMessagePage?GroupMessageId={GroupMessageId}");
                    }
                }
                GroupMessageToEdit = await _groupServices.GetSingleGroupMessageAsync(GroupMessageId);
            }
        }

        public async Task<IActionResult> OnPostMessageImageAsync()
        {
            MessageImage img = new();
            var files = Request.Form.Files;
            if (files.Count() != 0)
            {
                var file = files[0];
                img.Title = file.FileName;

                if (MessageId != 0 && GroupMessageId == 0)
                {
                    img.MessageId = MessageId;
                }
                if (MessageId == 0 && GroupMessageId != 0)
                {
                    img.GroupMessageId = GroupMessageId;
                }

                using (MemoryStream ms = new())
                {
                    file.CopyTo(ms);
                    img.Data = ms.ToArray();
                }
                await _messageServices.SaveMessageImage(img);
            }

            if (MessageId != 0 && GroupMessageId == 0)
            {
                return Redirect($"/EditMessagePage?MessageId={MessageId}");
            }
            if (MessageId == 0 && GroupMessageId != 0)
            {
                return Redirect($"/EditMessagePage?GroupMessageId={GroupMessageId}");
            }
            return Redirect($"/EditMessagePage?MessageId={MessageId}");
        }

        public async Task<IActionResult> OnPostChangeMessageTextAsync()
        {
            if (MessageId != 0 && GroupMessageId == 0)
            {
                await _messageServices.EditMessageTextAsync(NewText, MessageId);
                var message = await _messageServices.GetSingleMessageAsync(MessageId);
                return Redirect($"/MessagePage?SubThreadId={message.SubThreadId}");
            }
            if (MessageId == 0 && GroupMessageId != 0)
            {
                await _groupServices.EditGroupMessageTextAsync(NewText, GroupMessageId);
                var groupmessage = await _groupServices.GetSingleGroupMessageAsync(GroupMessageId);
                return Redirect($"/GroupMessages?GroupId={groupmessage.GroupId}");
            }

            return null;
        }
    }
}
