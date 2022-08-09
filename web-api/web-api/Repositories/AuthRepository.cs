using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using web_api.Interfaces;
using web_api.Models;

namespace web_api.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserRepository _userRepository;
        private readonly JwtTokenRepository _jwtTokenRepository;
        public AuthRepository(JwtTokenRepository jwtTokenRepository, UserRepository UserRepository)
        {
            _jwtTokenRepository = jwtTokenRepository;
            _userRepository = UserRepository;
        }
        public async Task<AuthenticatedResponse> Register(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel == null)
            {
                return null;
            }

            var authenticatedResponse = await _jwtTokenRepository.Refresh(tokenApiModel);

            if(authenticatedResponse == null)
            {

            }

            return authenticatedResponse;
        }

        public async Task<AuthenticatedResponse> SignIn(LoginModel loginModel)
        {
            var user = await _userRepository.Get(loginModel.Login, loginModel.Password);

            if (user is null)
            {
                return null;
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.UserGroup.Code.ToString())
        };
            var accessToken = _jwtTokenRepository.GenerateAccessToken(claims);
            var refreshToken = _jwtTokenRepository.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userRepository.Update(user);

            return new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<bool> SignOut(string login)
        {
            bool res = await _jwtTokenRepository.Revoke(login);

            if (!res)
            {
                return false;
            }

            return true;
        }
    }
}
