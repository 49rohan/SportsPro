using Microsoft.AspNetCore.Identity;

namespace SportsPro.Models
{
    public class User : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? RoleNames { get; set; }
    }
}