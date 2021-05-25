using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Data.Models
{
    public class MainThread
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleExample1 { get; set; }
        public string TitleExample2 { get; set; }
        public string TitleExample3 { get; set; }

        public ICollection<SubThread> SubThreads { get; set; }
    }
}
