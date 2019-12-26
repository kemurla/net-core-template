using CAT.Core.Models;

using System.Threading.Tasks;

namespace CAT.Core.Abstractions.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a user with a given email & password.
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        Task<Result> RegisterAsync(RegisterModel registerModel);

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        Task<Result<JwtToken>> LoginAsync(LoginModel loginModel);
    }
}
