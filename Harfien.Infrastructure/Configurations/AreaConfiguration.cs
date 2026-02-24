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
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            var fixedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                    new Area { Id = 1, CityId = 1, Name = "Maadi", CreatedAt = fixedDate },
                    new Area { Id = 2, CityId = 1, Name = "Helwan", CreatedAt = fixedDate },
                    new Area { Id = 3, CityId = 1, Name = "Ramses", CreatedAt = fixedDate },
                    new Area { Id = 4, CityId = 2, Name = "El Omarania", CreatedAt = fixedDate },
                    new Area { Id = 5, CityId = 3, Name = "Naga Elarab", CreatedAt = fixedDate }
                    //,
                    //new Area { Id = 6, CityId = 5, Name ="Olaya", CreatedAt = fixedDate }
                    );
        }
    }
}
