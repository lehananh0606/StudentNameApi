using AutoMapper;
using Data.Models;
using Repository.UnitOfWork;
using Service.Commons;
using Service.IServices;
using Service.ViewModel.Request;
using Service.ViewModel.Requet;
using Service.ViewModel.Response;
using Service.ViewModel.Response.Service.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class RoomInformationService : IRoomInformationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoomInformationService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Task<OperationResult<IEnumerable<RoomInformationResponse>>> GetAll(bool? isAscending,
            string? orderBy = null,
            Expression<Func<RoomInformation, bool>>? filter = null,
            string[]? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10)
        {
            var result = new OperationResult<IEnumerable<RoomInformationResponse>>();
            try
            {
                var entities = _unitOfWork.roomRepository.FilterAll(isAscending, orderBy, filter, includeProperties, pageIndex, pageSize);
                foreach (var entity in entities)
                {
                    Console.WriteLine(entity);
                }

                result.Payload = _mapper.Map<IEnumerable<RoomInformationResponse>>(entities);
                result.StatusCode = StatusCode.Ok;
                result.Message = "Get all room information successfully";
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return Task.FromResult(result);
        }

        public async Task<OperationResult<RoomInformationResponse>> GetById(int objectId)
        {
            var result = new OperationResult<RoomInformationResponse>();
            try
            {
                var entity = await _unitOfWork.roomRepository.GetByIdAsync(objectId);
                result.Payload = _mapper.Map<RoomInformationResponse>(entity);
                result.StatusCode = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<RoomInformationResponse>> Create(
            RoomInformationRequestCreate roomInformationRequestCreate)
        {
            var result = new OperationResult<RoomInformationResponse>();
            try
            {
                var newRoomInformation = _mapper.Map<RoomInformation>(roomInformationRequestCreate);

                await _unitOfWork.roomRepository.AddAsync(newRoomInformation);
                var count = await _unitOfWork.SaveChangesAsync();

                if (count > 0)
                {
                    result.Payload = _mapper.Map<RoomInformationResponse>(newRoomInformation);
                    result.StatusCode = StatusCode.Created;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Create Room Information Failed");
                }

            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<bool>> Delete(int id)
        {
            var result = new OperationResult<bool>();
            try
            {
                var deleteRoomInformation = await _unitOfWork.roomRepository.GetByIdAsync(id);
                if (deleteRoomInformation == null)
                {
                    result.AddError(StatusCode.NotFound, "Room information not found");
                    return result;
                }

                _unitOfWork.roomRepository.Remove(deleteRoomInformation);
                var count = await _unitOfWork.SaveChangesAsync();

                if (count > 0)
                {
                    result.Payload = true;
                    result.StatusCode = StatusCode.Ok;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Delete Room Information Failed");
                }
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<RoomInformationResponse>> Update(int id, RoomInformationRequestCreate roomInformationRequestUpdate)
        {
            var result = new OperationResult<RoomInformationResponse>();
            try
            {
                var existingRoomInformation = await _unitOfWork.roomRepository.GetByIdAsync(id);
                if (existingRoomInformation == null)
                {
                    result.AddError(StatusCode.NotFound, "Room information not found");
                    return result;
                }

                _mapper.Map(roomInformationRequestUpdate, existingRoomInformation);
                _unitOfWork.roomRepository.Update(existingRoomInformation);
                var count = await _unitOfWork.SaveChangesAsync();

                if (count > 0)
                {
                    result.Payload = _mapper.Map<RoomInformationResponse>(existingRoomInformation);
                    result.StatusCode = StatusCode.Ok;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Room Information Failed");
                }
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<IEnumerable<RoomInformationResponse>>> Search(string keyword, int pageIndex = 0, int pageSize = 10)
        {
            var result = new OperationResult<IEnumerable<RoomInformationResponse>>();
            try
            {
                var entities = await _unitOfWork.roomRepository.FindAsync(
                    r => r.RoomNumber.Contains(keyword) ||
                         r.RoomDetailDescription.Contains(keyword),
                    pageIndex,
                    pageSize
                );
                result.Payload = _mapper.Map<IEnumerable<RoomInformationResponse>>(entities);
                result.StatusCode = StatusCode.Ok;
                result.Message = "Search room informations successfully";
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
