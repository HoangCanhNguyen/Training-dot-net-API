using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class UserRepository : GenericRepository<ApplicationUser>
    {
        private readonly DatabaseContext _context;
        private DbSet<ApplicationUser> _db;

        public UserRepository(DatabaseContext context) : base(context)
        {
            _context = context;
            _db = _context.Set<ApplicationUser>();
        }

        public async Task<IList<ApplicationUser>> GetAll()
        {
            IQueryable<ApplicationUser> query = _db;
            var users = from u in _db
                        from ur in u.Roles
                        join r in _context.
        }
    }
}
