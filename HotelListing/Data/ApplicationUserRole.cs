using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HotelListing.Data
{
    public class ApplicationUserRole: IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
