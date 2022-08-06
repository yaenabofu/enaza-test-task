using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Repositories
{
    public class UserStateRepository : IRepository<UserState>
    {
        private readonly DatabaseContext databaseContext;

        public UserStateRepository(DatabaseContext DatabaseContext)
        {
            this.databaseContext = DatabaseContext;
        }
        public async Task<UserState> Get(int userStateId)
        {
            var userState = await databaseContext.UserStates.FindAsync(userStateId);

            return userState;
        }

        public async Task<IEnumerable<UserState>> Get()
        {
            var userStates = await databaseContext.UserStates.ToArrayAsync();

            return userStates;
        }
        public async Task<bool> Add(UserState userState)
        {
            if (userState == null)
            {
                return false;
            }

            await databaseContext.UserStates.AddAsync(userState);
            await databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Remove(int userStateId)
        {
            UserState userState = await Get(userStateId);

            if (userState != null)
            {
                databaseContext.UserStates.Remove(userState);
                await databaseContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<UserState> Update(UserState userState)
        {
            databaseContext.Entry(userState).State = EntityState.Modified;
            await databaseContext.SaveChangesAsync();

            return userState;
        }
    }
}
