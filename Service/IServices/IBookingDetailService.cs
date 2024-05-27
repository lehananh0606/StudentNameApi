using Data.Models;
using Service.Commons;
using Service.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IBookingDetailService
    {
        Task<OperationResult<IEnumerable<BookingDetailResponse>>> GetAllBookingDetails(
            bool? isAscending,
            string? orderBy = null,
            Expression<Func<BookingDetail, bool>>? filter = null,
            string[]? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10
        );

        Task<OperationResult<IEnumerable<BookingDetailResponse>>> GetByBookingReservationId(int bookingReservationId);
    }
}
