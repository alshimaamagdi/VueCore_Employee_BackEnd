using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Models.Domain
{
    public class Users:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
