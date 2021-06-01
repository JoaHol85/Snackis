using Snackis.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class SmileyMessageUser
    {
        public int Id { get; set; }
        public int SmileyNumber { get; set; }
        public int SmileyInfoId { get; set; }
        public SmileyInfo SmileyInfo { get; set; }
        public string SnackisUserId { get; set; }
        public SnackisUser SnackisUser { get; set; }
    }
}
