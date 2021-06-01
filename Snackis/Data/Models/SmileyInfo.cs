using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class SmileyInfo
    {
        [ForeignKey("Message")]
        public int Id { get; set; }
        public int HappySmiley { get; set; }
        public int LaughingSmiley { get; set; }
        public int SadSmiley { get; set; }
        public int AngrySmiley { get; set; }
        public int ThumbUp { get; set; }

        public Message Message { get; set; }

    }
}
