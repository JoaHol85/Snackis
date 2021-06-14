using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public interface IMainThreadServices
    {
        Task<List<MainThread>> GetMainThreadsAsync();
        Task<MainThread> GetSingleMainThread(int MainThreadId);
    }
}
