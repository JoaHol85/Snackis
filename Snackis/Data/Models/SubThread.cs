using Snackis.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class SubThread
    {
        public int Id { get; set; }
        public string HeaderText { get; set; }
        public DateTime Time { get; set; }

        public DateTime LatestMessage { get; set; }


        public int MainThreadId { get; set; }
        public MainThread MainThread { get; set; }

        public string SnackisUserId { get; set; }
        public SnackisUser SnackisUser { get; set; }


        public ICollection<Message> Messages { get; set; }
    }
}
