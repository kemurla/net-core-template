using CAT.Core.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CAT.DataAccess.ORM
{
    public class CATDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public CATDbContext(DbContextOptions options) : base(options) { }
    }
}
