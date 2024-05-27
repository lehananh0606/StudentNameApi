using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.ViewModel.Requet;
using System.Threading.Tasks;

namespace StudentNameApi.Controllers
{
    [Route("api/booking-detail-management")]
    [ApiController]
    public class BookingDetailController : BaseController
    {
        private readonly IBookingDetailService _bookingDetailService;

        public BookingDetailController(IBookingDetailService bookingDetailService)
        {
            _bookingDetailService = bookingDetailService;
        }

        /// <summary>
        /// Get booking details based on provided filters.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="id"></param>
        /// <param name="roomId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending"></param>
        /// <param name="includeProperties"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// A collection of booking details matching the specified criteria.
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     id = 1
        ///     roomId=1
        ///     startDate=2024-01-01
        ///     endDate=2024-01-15
        ///     orderBy=Room.RoomNumber
        ///     isAscending=true
        ///     includeProperties=Room
        ///     pageIndex=0
        ///     pageSize=10
        /// </remarks>
        [HttpGet("booking-details")]
        public async Task<IActionResult> GetBookingDetails([FromQuery] string? keyword,
            [FromQuery] int? id,
            [FromQuery] int? roomId,
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate,
            [FromQuery] string? orderBy,
            [FromQuery] bool? isAscending,
            [FromQuery] string[]? includeProperties,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var response = await _bookingDetailService.GetAllBookingDetails(
                isAscending: isAscending,
                filter: x => (!id.HasValue || x.BookingReservationId == id) &&
                             (!roomId.HasValue || x.RoomId == roomId) &&
                             (!startDate.HasValue || x.StartDate == startDate) &&
                             (!endDate.HasValue || x.EndDate == endDate) &&
                             (string.IsNullOrEmpty(keyword) ||
                              x.Room.RoomNumber.Contains(keyword) ),
                orderBy: orderBy,
                includeProperties: includeProperties,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Get booking detail by ID.
        /// </summary>
        /// <param name="id">The ID of the booking detail.</param>
        /// <returns>The booking detail with the specified ID.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     id = 1
        /// </remarks>
        [HttpGet("booking-details/{bookingReservationId:int}")]
        public async Task<IActionResult> GetBookingDetail(int bookingReservationId)
        {
            var response = await _bookingDetailService.GetByBookingReservationId(bookingReservationId);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
