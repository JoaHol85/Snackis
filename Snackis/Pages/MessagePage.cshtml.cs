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
                await _messageServices.SaveMessageAsync(AMessage, user, SubThreadId);

            }
            await OnGetAsync();
        }

        public string GetUserNickName(string id)
        {
            var user = _userServices.GetUser(id);
            return user.NickName;
        }

        public string GetUserImage(SnackisUser user)
        {
            string ImageUrl;
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
            return ImageUrl;
        }
    }
}
