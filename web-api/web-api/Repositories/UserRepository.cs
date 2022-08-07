using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.Enums;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Repositories
{
    public class UserRepository : IRepository<User>, IUserValidator
    {
        private readonly DatabaseContext databaseContext;
        public UserRepository(DatabaseContext DatabaseContext)
        {
            this.databaseContext = DatabaseContext;
        }
        public async Task<User> Get(int userId)
        {
            var user = await databaseContext.Users.Include(c => c.UserGroup).Include(c => c.UserState)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            return user;
        }

        public async Task<IEnumerable<User>> Get()
        {
            var users = await databaseContext.Users.Include(c => c.UserGroup).Include(c => c.UserState).ToListAsync();

            return users;
        }

        public async Task<bool> Create(User user)
        {
            if (user == null)
            {
                return false;
            }

            bool isLoginExist = await CheckForExistingLogin(user);

            if (isLoginExist)
            {
                return false;
            }

            Dictionary<int, UserGroup> userGroupsDict =
                    await databaseContext.UserGroups.ToDictionaryAsync(p => p.UserGroupId, p => p);

            UserGroup userGroup = userGroupsDict[user.UserGroupId];

            if (userGroup.Code == UserGroupCode.Admin)
            {
                bool isEnoughAdmins = await CheckForEnoughAdmins();

                if (isEnoughAdmins)
                {
                    return false;
                }
            }

            HMACSHA256Repository hmacsha256Repository = new HMACSHA256Repository();
            string hashedPassword = hmacsha256Repository.Hash(user.Password);
            user.Password = hashedPassword;

            await databaseContext.Users.AddAsync(user);
            await databaseContext.SaveChangesAsync();

            await ChangeUserState(user);

            return true;
        }

        public async Task<bool> Delete(int userId)
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

        public async Task<bool> Update(User user)
        {
            if (user == null)
            {
                return false;
            }

            databaseContext.Entry(user).State = EntityState.Modified;
            await databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckForEnoughAdmins()
        {
            const int MaxUsersWithAdminCode = 1;

            Dictionary<UserGroupCode, UserGroup> userGroupsDict =
                    await databaseContext.UserGroups.ToDictionaryAsync(p => p.Code, p => p);

            var userAdminStateCode = userGroupsDict[UserGroupCode.Admin];

            var users = await Get();

            var usersWithAdminCode = users.Select(c => c).Where(c => c.UserGroupId == userAdminStateCode.UserGroupId).ToList();

            if (usersWithAdminCode.Count < MaxUsersWithAdminCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckForExistingLogin(User addingUser)
        {
            const int MaxUsersWithSameLogin = 0;

            var users = await Get();

            var usersWithTypedLogin = users.Select(c => c).Where(c => c.Login == addingUser.Login).ToList();

            if (usersWithTypedLogin.Count() == MaxUsersWithSameLogin)
            {
                return false;
            }

            return true;
        }
        public async Task<User> ChangeUserState(User addingUser)
        {
            Dictionary<UserStateCode, UserState> userStates =
                   await databaseContext.UserStates.ToDictionaryAsync(p => p.Code, p => p);

            int activeStateId = userStates[UserStateCode.Active].UserStateId;

            addingUser.UserStateId = activeStateId;

            await Update(addingUser);

            return addingUser;
        }
    }
}
