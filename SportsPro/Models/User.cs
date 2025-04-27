using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SportsPro.Models
{
    public class User : IdentityUser 
    {
        public IList<string> RoleNames { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
