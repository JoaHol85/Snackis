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
    public class GroupMessagesModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly IUserServices _userServices;
        private readonly IGroupServices _groupServices;


        public GroupMessagesModel(IGroupServices groupServices, IUserServices userServices, UserManager<SnackisUser> userManager)
        {
            _groupServices = groupServices;
            _userServices = userServices;
            _userManager = userManager;
        }
        [BindProperty(SupportsGet = true)]
        public int GroupId { get; set; }

        [BindProperty]
        public GroupMessage AGroupMessage { get; set; }
        [BindProperty]
        public Group AGroup { get; set; }
        [BindProperty]
        public string AddUserToGroup { get; set; }
        [BindProperty]
        public string RemoveUserFromGroup { get; set; }

        public List<SnackisUser> ListOfUsers { get; set; }
        public List<SnackisUser> ListOfUsersNotInGroup { get; set; }
        public List<GroupMessage> ListOfGroupMessages { get; set; }
        public Group Group { get; set; }
        public List<Group> ListOfGroupWithUser { get; set; }



        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                ListOfGroupWithUser = await _groupServices.GetAllGroupsFromUserAsync(user);
            }
            if (GroupId != 0)
            {
                Group = await _groupServices.GetSingleGroupByIdAsync(GroupId);
                ListOfGroupMessages = Group.GroupMessages.ToList();
                ListOfUsers = await _userServices.GetAllUsersAsync();
                List<SnackisUser> list = new();
                foreach (var aUser in ListOfUsers)
                {
                    bool inGroup = Group.Users.Contains(aUser);
                    if (!inGroup && Group.GroupStartedById != aUser.Id)
                    {
                        list.Add(aUser);
                    }
                }
                ListOfUsersNotInGroup = list;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (AGroupMessage.Text != null)
            {
                var user = await _userManager.GetUserAsync(User);
                AGroupMessage.MessageImages = new List<MessageImage>();
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
                    AGroupMessage.MessageImages.Add(img);
                }
                await _groupServices.SaveGroupMessageAsync(user, GroupId, AGroupMessage);
            }
            if (AddUserToGroup != null)
            {
                await _groupServices.AddUserToGroupAsync(AddUserToGroup, GroupId);
            }
            if (RemoveUserFromGroup != null)
            {
                await _groupServices.RemoveUserFromGroup(RemoveUserFromGroup, GroupId);
            }
            return Redirect($"/GroupMessages?GroupId={GroupId}");
        }

        public async Task OnPostCreateGroupAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            AGroup.GroupStartedById = user.Id;
            await _groupServices.SaveGroupAsync(AGroup);
            await OnGetAsync();
        }
       
    }
}
