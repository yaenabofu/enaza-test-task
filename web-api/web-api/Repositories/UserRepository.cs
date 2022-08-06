using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_api.Enums;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly DatabaseContext databaseContext;
        public UserRepository(DatabaseContext DatabaseContext)
        {
            this.databaseContext = DatabaseContext;
        }
        public async Task<User> Get(int userId)
        {
            var user = await databaseContext.Users.FindAsync(userId);

            return user;
        }

        public async Task<IEnumerable<User>> Get()
        {
            var users = await databaseContext.Users.ToListAsync();

            return users;
        }

        public async Task<bool> Add(User user)
        {
            if (user == null)
            {
                return false;
            }

            await databaseContext.Users.AddAsync(user);
            await databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Remove(int userId)
        {
            User user = await Get(userId);

            if (user != null)
            {
                Dictionary<UserStateCode, UserState> userStatesDict = await databaseContext.UserStates.ToDictionaryAsync(p => p.Code, p => p);
                var userBlockedStateCode = userStatesDict[UserStateCode.Blocked];

                user.UserStateId = userBlockedStateCode.UserStateId;

                databaseContext.Entry(user).State = EntityState.Modified;
                await databaseContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<User> Update(User user)
        {
            databaseContext.Entry(user).State = EntityState.Modified;
            await databaseContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> CheckForAdmin()
        {
            const int MaxUsersWithAdminCode = 1;

            Dictionary<UserGroupCode, UserGroup> userGroupsDict =
                    await databaseContext.UserGroups.ToDictionaryAsync(p => p.Code, p => p);

            var userAdminStateCode = userGroupsDict[UserGroupCode.Admin];

            var users = await Get();

            var usersWithAdminCode = users.Select(c => c).Where(c => c.UserGroupId == userAdminStateCode.UserGroupId).ToList();

            if (usersWithAdminCode.Count < MaxUsersWithAdminCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CheckForExistingLogin(User addingUser)
        {
            const int MaxUsersWithSameLogin = 0;

            var users = await Get();

            var usersWithTypedLogin = users.Select(c => c).Where(c => c.Login == addingUser.Login).ToList();

            if (usersWithTypedLogin.Count() == MaxUsersWithSameLogin)
            {
                return true;
            }

            return false;
        }
    }
}
