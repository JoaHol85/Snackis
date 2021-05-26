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
    public class GroupMessagesModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly UserServices _userServices;
        private readonly GroupServices _groupServices;


        public GroupMessagesModel(GroupServices groupServices, UserServices userServices, UserManager<SnackisUser> userManager)
        {
            _groupServices = groupServices;
            _userServices = userServices;
            _userManager = userManager;
        }
        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int MessageInGroup { get; set; }


        [BindProperty]
        public GroupMessage AGroupMessage { get; set; }
        [BindProperty]
        public Group AGroup { get; set; }

        public List<Group> ListOfGroups { get; set; }
        public List<SnackisUser> Users { get; set; }
        public List<GroupMessage> ListOfGroupMessages { get; set; }




        public async Task OnGetAsync()
        {
            ListOfGroups = await _groupServices.GetAllGroupsAsync();
            
            if (GroupId != 0)
            {
                ListOfGroupMessages = await _groupServices.GetAllGroupMessagesInGroup(GroupId);
            }
        }

        public async Task OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            await _groupServices.SaveGroupMessage(user, GroupId, AGroupMessage);
        }

        public async Task OnPostCreateGroupAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            AGroup.GroupStartedById = user.Id;
            await _groupServices.SaveGroup(AGroup);
            await OnGetAsync();
        }
        public string GetUserNickName(string id)
        {
            var user = _userServices.GetUser(id);
            return user.NickName;
        }

    }
}
