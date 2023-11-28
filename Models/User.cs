using Microsoft.AspNetCore.Identity;

namespace BankingSystem.Models
{
    public class User : IdentityUser
    {
        public virtual Balance Balance { get; set; }
    }
}
