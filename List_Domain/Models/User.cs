using Microsoft.AspNetCore.Identity;

namespace List_Domain.Models
{
    public class User : IdentityUser<int>
    {
        public string? Name { get; set; }

        public DateTime? ArchivalDate { get; set; }
    }
}
