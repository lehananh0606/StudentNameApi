using Data.Models;
using Service.Commons;
using Service.ViewModel.Request;
using Service.ViewModel.Requet;
using Service.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IBookingReservationService
    {
        Task<OperationResult<IEnumerable<BookingReservationResponse>>> GetAllBookingReservations(
            bool? isAscending,
            string? orderBy = null,
            Expression<Func<BookingReservation, bool>>? filter = null,
            string[]? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10);

        Task<OperationResult<BookingReservationResponse>> GetBookingReservationById(int bookingReservationId);

        Task<OperationResult<BookingReservationResponse>> CreateBookingReservation(BookingReservationRequestCreate requestCreate);

        Task<OperationResult<bool>> DeleteBookingReservation(int id);

        Task<OperationResult<BookingReservationResponse>> UpdateBookingReservation(int id, BookingReservationRequestCreate requestUpdate);
    }
}
