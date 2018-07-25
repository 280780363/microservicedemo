using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data
{
    public class DemoIdentityDbContext : IdentityDbContext<DemoUsers, DemoRoles, Guid>
    {
        public DemoIdentityDbContext(DbContextOptions<DemoIdentityDbContext> options) : base(options) {
        }
    }
}
