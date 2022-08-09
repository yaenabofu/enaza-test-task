using System.Threading.Tasks;
using web_api.Models;

namespace web_api.Interfaces
{
    public interface IAuth
    {
        //Task<AuthenticatedResponse> Register(TokenApiModel tokenApiModel);
        Task<AuthenticatedResponse> SignIn(LoginModel loginModel);
        Task<bool> SignOut(string login);
    }
}
