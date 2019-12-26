using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

using CAT.Core;
using CAT.Core.Models;
using CAT.Core.Entities;
using CAT.Core.Identity;
using CAT.Core.Abstractions.Services;

namespace CAT.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public UserService(
            UserManager<User> userManager,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory  = jwtFactory;
            _jwtOptions  = jwtOptions.Value;
        }

        public async Task<Result> RegisterAsync(RegisterModel registerModel)
        {
            var user = registerModel.ToUserEntity();
            var userCreationResult = await _userManager.CreateAsync(user, registerModel.Password);

            return userCreationResult.Succeeded ?
                new Result() :
                new Result(userCreationResult.Errors.FirstOrDefault()?.Description);
        }

        public async Task<Result<JwtToken>> LoginAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginModel.Password);

            if (user == null || !isCorrectPassword)
                return new Result<JwtToken>(Constants.Identity.InvalidCredentials);

            return new Result<JwtToken>(new JwtToken
            {
                Token     = await _jwtFactory.GenerateEncodedTokenAsync(user.Id, user.Email),
                ExpiresIn = _jwtOptions.ValidFor.TotalSeconds
            });
        }
    }
}
