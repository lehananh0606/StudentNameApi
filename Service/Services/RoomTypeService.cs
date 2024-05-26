using AutoMapper;
using Data.Models;
using Repository.UnitOfWork;
using Service.Commons;
using Service.IServices;
using Service.ViewModel.Requet;
using Service.ViewModel.Response;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoomTypeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<IEnumerable<RoomTypeResponse>>> GetAllRoomTypes(bool? isAscending, string? orderBy = null, Expression<Func<RoomType, bool>>? filter = null, string[]? includeProperties = null, int pageIndex = 0, int pageSize = 10)
        {
            var result = new OperationResult<IEnumerable<RoomTypeResponse>>();
            try
            {
                var entities = _unitOfWork.roomTypeRepository.FilterAll(isAscending, orderBy, filter, includeProperties, pageIndex, pageSize);
                result.Payload = _mapper.Map<IEnumerable<RoomTypeResponse>>(entities);
                result.StatusCode = StatusCode.Ok;
                result.Message = "Get all room types successfully";
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<RoomTypeResponse>> GetRoomType(int roomTypeId)
        {
            var result = new OperationResult<RoomTypeResponse>();
            try
            {
                var entity = await _unitOfWork.roomTypeRepository.GetByIdAsync(roomTypeId);
                result.Payload = _mapper.Map<RoomTypeResponse>(entity);
                result.StatusCode = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }
    }
}
