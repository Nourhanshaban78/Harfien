using Harfien.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Infrastructure.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            var fixedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                    new City { Id = 1, Name = "Cairo", CreatedAt = fixedDate },
                    new City { Id = 2, Name = "Giza", CreatedAt = fixedDate },
                    new City { Id = 3, Name = "Alexanderia", CreatedAt = fixedDate },
                    new City { Id = 4, Name = "Aswan", CreatedAt = fixedDate }
                    //,
                    //new City { Id = 5, Name = "Riyadh", CreatedAt = fixedDate } //sample for another country, does it need country table?
                    );
        }
    }
}
