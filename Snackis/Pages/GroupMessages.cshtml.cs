using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Areas.Identity.Data;
using Snackis.Data.Models;

namespace Snackis.Pages
{
    public class GroupMessagesModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;


        public GroupMessagesModel(UserManager<SnackisUser> userManager)
        {
            _userManager = userManager;
        }


        public List<Group> ListOfGroups { get; set; }







        public async Task OnGet()
        {
            //var user = await _userManager.GetUserAsync(User);
            //ListOfGroups = user.Groups.ToList(); 
        }
    }
}
