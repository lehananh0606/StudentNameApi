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

        [HttpGet("booking-details/{id:int}")]
        public async Task<IActionResult> GetBookingDetail(int id)
        {
            var response = await _bookingDetailService.GetBookingDetailById(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
