using Snackis.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public class AdminServices
    {
        private readonly SnackisContext _context;

        public AdminServices(SnackisContext context)
        {
            _context = context;
        }


        public async Task PostMainThreadAsync(MainThread mainThread)
        {
            _context.MainThreads.Add(mainThread);
            await _context.SaveChangesAsync();
        }

    }
}
