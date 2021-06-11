using Snackis.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }

        public string SnackisUserId { get; set; }
        public SnackisUser SnackisUser { get; set; }

        //nytt nedan
        public int MessageId { get; set; }
    }
}
