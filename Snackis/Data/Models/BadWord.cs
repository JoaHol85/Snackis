using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class BadWord
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("censoredWord")]
        public string CensoredWord { get; set; }

    }
}
