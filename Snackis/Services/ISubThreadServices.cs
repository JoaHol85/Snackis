using Snackis.Areas.Identity.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public interface ISubThreadServices
    {
        Task<int> CreateNewSubThreadAsync(SubThread subThread, Message message, SnackisUser user);
        Task<List<SubThread>> GetMainIdSubThreadsAsync(int mainThreadId);
        Task<SubThread> GetSingleSubThreadAync(int subThreadId);
    }
}
