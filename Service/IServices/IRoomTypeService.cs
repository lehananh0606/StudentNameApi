using Data.Models;
using Service.Commons;
using Service.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IRoomTypeService
    {
        Task<OperationResult<IEnumerable<RoomTypeResponse>>> GetAllRoomTypes(bool? isAscending, string? orderBy = null, Expression<Func<RoomType, bool>>? filter = null, string[]? includeProperties = null, int pageIndex = 0, int pageSize = 10);
        Task<OperationResult<RoomTypeResponse>> GetRoomType(int roomTypeId);
    }
}
