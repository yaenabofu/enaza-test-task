using System.Threading.Tasks;
using web_api.Models;

namespace web_api.Interfaces
{
    public interface IUserValidator
    {
        Task<bool> CheckForEnoughAdmins();
        Task<bool> CheckForExistingLogin(User addingUser);
    }
}
