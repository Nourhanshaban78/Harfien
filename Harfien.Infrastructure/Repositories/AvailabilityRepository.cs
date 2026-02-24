using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Infrastructure.Repositories
{
    public class AvailabilityRepository
       : GenericRepository<CraftsmanAvailability>, IAvailabilityRepository
    {
        private readonly HarfienDbContext _context;

        public AvailabilityRepository(HarfienDbContext context)
            : base(context)
        {
            _context = context;
        }

        // ==========================================
        // 🔥 Check if craftsman is available
        // ==========================================
        public async Task<bool> IsAvailableAsync(int craftsmanId, DateTime scheduledAt)
        {
            // مهم جدًا نتعامل كـ Local وقت المقارنة
            var localTime = scheduledAt.ToLocalTime();

            var day = (int)localTime.DayOfWeek;      // تحويل enum لـ int
            var time = localTime.TimeOfDay;          // ناخد الوقت فقط

            return await _context.CraftsmanAvailabilities
                .AnyAsync(a =>
                    a.CraftsmanId == craftsmanId &&
                    a.Day == day &&
                    a.IsAvailable &&
                    a.From <= time &&
                    a.To > time
                );
        }

        // ==========================================
        // 🔥 Get all available craftsmen at time
        // ==========================================
        public async Task<IEnumerable<int>> GetAvailableCraftsmenIdsAsync(DateTime scheduledAt)
        {
            var localTime = scheduledAt.ToLocalTime();
            var day = (int)localTime.DayOfWeek;
            var time = localTime.TimeOfDay;

            return await _context.CraftsmanAvailabilities
                .Where(a =>
                    a.Day == day &&
                    a.IsAvailable &&
                    a.From <= time &&
                    a.To > time
                )
                .Select(a => a.CraftsmanId)
                .Distinct()
                .ToListAsync();
        }

        // ==========================================
        public async Task<IEnumerable<CraftsmanAvailability>>
            GetAllByCraftsmanIdAsync(int craftsmanId)
        {
            return await _context.CraftsmanAvailabilities
                .Where(a => a.CraftsmanId == craftsmanId)
                .ToListAsync();
        }
    }

    }
