using Snackis.Areas.Identity.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public interface IUserServices
    {
        string ConvertUserImageToImage(SnackisUser user);
        Task<List<SnackisUser>> GetAllUsersAsync();
        SnackisUser GetUser(string id);
        UserImage GetUserImage(SnackisUser user);
        Task<SnackisUser> GetUserWithMessagesAndSubThreadsAsync(string userId);
        Task RemoveUserFromGroupsAsync(SnackisUser user);
        Task SaveUserImage(UserImage img);
    }
}
