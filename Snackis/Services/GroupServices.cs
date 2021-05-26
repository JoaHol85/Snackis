using Snackis.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public class GroupServices
    {
        private readonly SnackisContext _context;

        public GroupServices(SnackisContext context)
        {
            _context = context;
        }


    }
}
