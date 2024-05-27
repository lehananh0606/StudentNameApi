using Data.Models;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IRoomInformationRepository : IGenericRepository<RoomInformation>
    {
        Task<IEnumerable<RoomInformation>> FindAsync(Expression<Func<RoomInformation, bool>> predicate, int pageIndex, int pageSize);
    }
}
