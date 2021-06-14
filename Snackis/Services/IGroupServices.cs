using Snackis.Areas.Identity.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Services
{
    public interface IGroupServices
    {
        Task AddUserToGroupAsync(string userId, int groupId);
        Task<List<Group>> GetAllGroupsFromUserAsync(SnackisUser user);
        Task<Group> GetSingleGroupByIdAsync(int groupId);
        Task RemoveUserFromGroup(string userId, int groupId);
        Task SaveGroupAsync(Group group);
        Task SaveGroupMessageAsync(SnackisUser user, int groupId, GroupMessage message);
    }
}
