using AutoMapper;
using Data.Models;
using Repository.UnitOfWork;
using Service.Commons;
using Service.IServices;
using Service.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BookingDetailService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<IEnumerable<BookingDetailResponse>>> GetAllBookingDetails(
            bool? isAscending,
            string? orderBy = null,
            Expression<Func<BookingDetail, bool>>? filter = null,
            string[]? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10)
        {
            var result = new OperationResult<IEnumerable<BookingDetailResponse>>();
            try
            {
                var entities = _unitOfWork.bookingDetailRepository.FilterAll(
                    isAscending, orderBy, filter, includeProperties, pageIndex, pageSize);
                result.Payload = _mapper.Map<IEnumerable<BookingDetailResponse>>(entities);
                result.StatusCode = StatusCode.Ok;
                result.Message = "Get all booking details successfully";
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<BookingDetailResponse>> GetBookingDetailById(int bookingDetailId)
        {
            var result = new OperationResult<BookingDetailResponse>();
            try
            {
                var entity = await _unitOfWork.bookingDetailRepository.GetByIdAsync(bookingDetailId);
                if (entity == null)
                {
                    result.AddError(StatusCode.NotFound, "Booking detail not found");
                    return result;
                }
                result.Payload = _mapper.Map<BookingDetailResponse>(entity);
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
