using Snackis.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class GroupMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public string SnackisUserId { get; set; }
        public SnackisUser SnackisUser { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

    }
}
