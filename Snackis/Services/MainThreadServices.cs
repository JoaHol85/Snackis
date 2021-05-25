using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public class MainThreadServices
    {
        private readonly SnackisContext _context;

        public MainThreadServices(SnackisContext context)
        {
            _context = context;
        }

        public async Task<List<MainThread>> GetMainThreadsAsync()
        {
            return await _context.MainThreads.ToListAsync();
        }

        public async Task<MainThread> GetSingleMainThread(int MainThreadId)
        {
            return await _context.MainThreads.FindAsync(MainThreadId);
        }

    }
}
