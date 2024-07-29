using Microsoft.AspNetCore.Identity;

namespace Simankova.UI.Models
{
    public class AppUser : IdentityUser
    {
        public byte[] Avatar { get; set; }
        public string MimeType { get; set; } = string.Empty;
    }
}
