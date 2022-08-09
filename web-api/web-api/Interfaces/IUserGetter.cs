using System.Threading.Tasks;
using web_api.Models;

namespace web_api.Interfaces
{
    public interface IUserGetter
    {
        Task<User> Get(string login, string password);
        Task<User> Get(string login);
    }
}
