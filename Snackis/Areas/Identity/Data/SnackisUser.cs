using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Snackis.Data.Models;

namespace Snackis.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SnackisUser class
    public class SnackisUser : IdentityUser
    {
        [PersonalData]
        public string NickName { get; set; }
        public ICollection<SubThread> SubThreads { get; set; }
        public ICollection<Message> Messages { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public ICollection<GroupMessage> GroupMessages { get; set; }

    }
}
