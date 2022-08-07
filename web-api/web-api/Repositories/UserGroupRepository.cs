using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Repositories
{
    public class UserGroupRepository : IRepository<UserGroup>
    {
        private readonly DatabaseContext databaseContext;

        public UserGroupRepository(DatabaseContext DatabaseContext)
        {
            this.databaseContext = DatabaseContext;
        }
        public async Task<UserGroup> Get(int userGroupId)
        {
            var user = await databaseContext.UserGroups.FindAsync(userGroupId);

            return user;
        }
        public async Task<IEnumerable<UserGroup>> Get()
        {
            var users = await databaseContext.UserGroups.ToArrayAsync();

            return users;
        }
        public async Task<bool> Create(UserGroup userGroup)
        {
            if (userGroup == null)
            {
                return false;
            }

            await databaseContext.UserGroups.AddAsync(userGroup);
            await databaseContext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Delete(int userGroupId)
        {
            UserGroup userGroup = await Get(userGroupId);

            if (userGroup != null)
            {
                databaseContext.UserGroups.Remove(userGroup);
                await databaseContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<bool> Update(UserGroup userGroup)
        {
            if (userGroup == null)
            {
                return false;
            }

            databaseContext.Entry(userGroup).State = EntityState.Modified;
            await databaseContext.SaveChangesAsync();

            return true;
        }
    }
}
