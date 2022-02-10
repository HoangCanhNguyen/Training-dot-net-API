using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                    new Country
                    {
                        Id = 1,
                        Name = "Vietnam",
                        ShortName = "VI"
                    },
                    new Country
                    {
                        Id = 2,
                        Name = "United State of America",
                        ShortName = "USA"
                    },
                    new Country
                    {
                        Id = 3,
                        Name = "Singapo",
                        ShortName = "SI"
                    }
                );
            builder.Entity<Hotel>().HasData(
                    new Hotel
                    {
                        Id = 1,
                        Name = "Hanoi Resort",
                        Address = "Long Bien, Hanoi",
                        Rating = 4.5,
                        CountryId = 1,
                    },
                    new Hotel
                    {
                        Id = 2,
                        Name = "Halong Resort",
                        Address = "58 Ha Long",
                        Rating = 4.2,
                        CountryId = 2,
                    },
                    new Hotel
                    {
                        Id = 3,
                        Name = "SaiGon Resort",
                        Address = "48 Duy Tan, Hanoi",
                        Rating = 4.3,
                        CountryId = 1,
                    }

            );
        }

    }
}
