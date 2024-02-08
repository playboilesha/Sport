using Microsoft.AspNetCore.Identity;

namespace Sport.Models
{
    public class User : IdentityUser
    {

        public int Year { get; set; }
        public string FirstName {get; set;}
        public string LastName { get; set; }
        
    }
}