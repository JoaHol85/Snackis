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
    public class MessagePageModel : PageModel
    {
        private readonly ISubThreadServices _subServices;
        private readonly IMessageServices _messageServices;
        private readonly IUserServices _userServices;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly UserManager<SnackisUser> _userManager;

        public MessagePageModel(IUserServices userServices, IMessageServices messageServices, ISubThreadServices subServices, SignInManager<SnackisUser> signInManager, UserManager<SnackisUser> userManager)
        {
            _userServices = userServices;
            _subServices = subServices;
            _messageServices = messageServices;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int SubThreadId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int SmileyNumber { get; set; }
        [BindProperty(SupportsGet = true)]
        public int MessageId { get; set; }


        [BindProperty]
        public Message AMessage { get; set; }

        public List<Message> ListOfMessages { get; set; }
        public SubThread SubThreadCopy { get; set; }
        public bool IsSignedIn { get; set; } = false;
        public string CompleteMessage { get; set; }


        public async Task OnGetAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                IsSignedIn = true;
            }
            if (SmileyNumber != 0 && _signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                await _messageServices.AddSmileyToMessageAsync(user, SmileyNumber, MessageId);
            }

            ListOfMessages = await _messageServices.GetSubIdMessagesAsync(SubThreadId);
            SubThreadCopy = await _subServices.GetSingleSubThreadAync(SubThreadId);
            
        }

        public async Task OnPost()
        {
            if (AMessage.TextMessage != null)
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
                await _messageServices.SaveMessageAsync(AMessage, user, SubThreadId);
            }
            await OnGetAsync();
        }

        public async Task OnPostPostCommentAsync()
        {
            if (AMessage.TextMessage != null && AMessage.MessageId != null)
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
                await _messageServices.SaveMessageAsync(AMessage, user, AMessage.SubThreadId);
            }
            SubThreadId = AMessage.SubThreadId;
            await OnGetAsync();
        }
    }
}
