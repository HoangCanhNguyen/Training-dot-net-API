using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HotelListing.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<IdentityRole> Roles { get; set; }

    }
}
