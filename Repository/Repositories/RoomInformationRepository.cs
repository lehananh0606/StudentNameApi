using Data.Models;
using Microsoft.EntityFrameworkCore;
using Repository.GenericRepository;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class RoomInformationRepository : GenericRepository<RoomInformation>, IRoomInformationRepository
    {
        
        public RoomInformationRepository(FuminiHotelManagementContext context) : base(context) { }

        public async Task<IEnumerable<RoomInformation>> FindAsync(Expression<Func<RoomInformation, bool>> predicate, int pageIndex, int pageSize)
        {
            return await _context.Set<RoomInformation>()
                .Where(predicate)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
