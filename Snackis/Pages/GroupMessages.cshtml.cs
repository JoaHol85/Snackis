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

        [BindProperty]
        public Group AGroup { get; set; }

        public List<Group> ListOfGroups { get; set; }
        public List<SnackisUser> Users { get; set; }





        public async Task OnGet()
        {
            Users = await _userServices.GetAllUsersAsync();
            ListOfGroups = await _groupServices.GetAllGroupsAsync();
            //var user = await _userManager.GetUserAsync(User);
            //ListOfGroups = user.Groups.ToList(); 
        }

        public async Task OnPost()
        {
            var user = await _userManager.GetUserAsync(User);
            AGroup.GroupStartedById = user.Id;
        }
    }
}
