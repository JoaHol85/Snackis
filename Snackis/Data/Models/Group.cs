using Snackis.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int GroupStarterId { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<SnackisUser> Users { get; set; }
        public ICollection<GroupMessage> GroupMessages { get; set; }

    }
}
