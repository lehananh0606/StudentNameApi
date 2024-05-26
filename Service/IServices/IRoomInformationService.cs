using Data.Models;
using Service.Commons;
using Service.ViewModel.Request;
using Service.ViewModel.Response;
using Service.ViewModel.Response.Service.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IRoomInformationService
    {
        Task<OperationResult<IEnumerable<RoomInformationResponse>>> GetAll(bool? isAscending, string? orderBy = null, Expression<Func<RoomInformation, bool>>? filter = null, string[]? includeProperties = null, int pageIndex = 0, int pageSize = 10);
        Task<OperationResult<RoomInformationResponse>> GetById(int objectId);
        Task<OperationResult<RoomInformationResponse>> Create(RoomInformationRequestCreate objectRequestCreate);
        Task<OperationResult<bool>> Delete(int id);
        Task<OperationResult<RoomInformationResponse>> Update(int id, RoomInformationRequestCreate objectRequestUpdate);
    }
}
