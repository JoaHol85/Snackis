using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public interface IAdminServices
    {
        Task PostMainThreadAsync(MainThread mainThread);
    }
}
