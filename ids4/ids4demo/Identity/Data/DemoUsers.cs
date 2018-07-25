using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data
{
    public class DemoUsers : IdentityUser<Guid>
    {
        public string Avatar { get; set; }
    }
}
