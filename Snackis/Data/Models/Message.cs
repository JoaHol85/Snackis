using Snackis.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string TextMessage { get; set; }
        public DateTime Time { get; set; }
        public int TimesReported { get; set; }
        public int Likes { get; set; }

        public string SnackisUserId { get; set; }
        public SnackisUser SnackisUser { get; set; }

        public int? MessageId { get; set; }

        public int SubThreadId { get; set; }
        public SubThread SubThread { get; set; }

        public SmileyInfo SmileyInfo { get; set; }

        // nytt nedanför

        public ICollection<MessageImage> MessageImages { get; set; }

    }
}
