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

        public async Task<OperationResult<IEnumerable<BookingDetailResponse>>> GetByBookingReservationId(int bookingReservationId)
        {
            var result = new OperationResult<IEnumerable<BookingDetailResponse>>();
            try
            {
                var entities = await _unitOfWork.bookingDetailRepository.GetByBookingReservationIdAsync(bookingReservationId);
                result.Payload = _mapper.Map<IEnumerable<BookingDetailResponse>>(entities);
                result.StatusCode = StatusCode.Ok;
                result.Message = "Get booking details by BookingReservationId successfully";
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
