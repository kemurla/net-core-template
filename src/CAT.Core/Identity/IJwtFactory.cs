using System.Threading.Tasks;

namespace CAT.Core.Identity
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedTokenAsync(string userId, string email);
    }
}
