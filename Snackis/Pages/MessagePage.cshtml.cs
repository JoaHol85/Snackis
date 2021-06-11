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
        private readonly SubThreadServices _subServices;
        private readonly MessageServices _messageServices;
        private readonly UserServices _userServices;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly UserManager<SnackisUser> _userManager;

        public MessagePageModel(UserServices userServices, MessageServices messageServices, SubThreadServices subServices, SignInManager<SnackisUser> signInManager, UserManager<SnackisUser> userManager)
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
                //if (messageId != 0)
                //{
                AMessage.MessageImages = new List<MessageImage>();
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    MessageImage img = new();
                    //var file = files[0];
                    img.Title = file.FileName;
                    //img.MessageId = messageId;

                    using (MemoryStream ms = new())
                    {
                        file.CopyTo(ms);
                        img.Data = ms.ToArray();
                    }
                    AMessage.MessageImages.Add(img);
                }
                int messageId = await _messageServices.SaveMessageAsync(AMessage, user, SubThreadId);
                    //bool done = await _messageServices.SaveMessageImage(img);
                    //if (!done)
                    //{
                    //    CompleteMessage = "Det gick inte att ladda upp fler bilder till detta meddelande, högst tillåtna antal = 3.";
                    //}
                //}
            }
            await OnGetAsync();
        }

        public async Task OnPostPostCommentAsync()
        {
            if (AMessage.TextMessage != null && AMessage.MessageId != null)
            {
                var user = await _userManager.GetUserAsync(User);
                await _messageServices.SaveMessageAsync(AMessage, user, AMessage.SubThreadId);
                SubThreadId = AMessage.SubThreadId;
            }
            await OnGetAsync();
        }

        public string GetUserImage(SnackisUser user)
        {
            string ImageUrl;
            UserImage img = _userServices.GetUserImage(user);
            if (img == null)
            {
                ImageUrl = "http://placehold.it/300x300";
            }
            else
            {
                string imageBase64Data = Convert.ToBase64String(img.Data);
                ImageUrl = string.Format($"data:image/jpg;base64, {imageBase64Data}");
            }
            return ImageUrl;
        }

        public string GetImage(MessageImage image)
        {
            string ImageUrl;
            MessageImage img = image;
            if (img == null)
            {
                ImageUrl = "http://placehold.it/300x300";
            }
            else
            {
                string imageBase64Data = Convert.ToBase64String(img.Data);
                ImageUrl = string.Format($"data:image/jpg;base64, {imageBase64Data}");
            }
            return ImageUrl;
        }


    }
}
