using Microsoft.AspNetCore.Identity;

namespace CAT.Core.Entities
{
    public class User : IdentityUser<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
