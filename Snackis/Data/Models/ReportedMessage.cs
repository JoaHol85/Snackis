using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class ReportedMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }

    }
}
