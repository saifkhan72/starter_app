using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Starter_app.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string? Name { get; set; }
    }
}
