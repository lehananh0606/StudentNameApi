using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.ViewModel.Request;
using Service.ViewModel.Requet;
using Service.ViewModel.Response;
using System.Threading.Tasks;

namespace StudentNameApi.Controllers
{
    [Route("api/booking-reservation-management")]
    [ApiController]
    public class BookingReservationController : BaseController
    {
        private readonly IBookingReservationService _bookingReservationService;

        public BookingReservationController(IBookingReservationService bookingReservationService)
        {
            _bookingReservationService = bookingReservationService;
        }


        /// <summary>
        /// Get booking reservations based on provided filters.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="id"></param>
        /// <param name="bookingDate"></param>
        /// <param name="totalPrice"></param>
        /// <param name="customerId"></param>
        /// <param name="bookingStatus"></param>
        /// <param name="includeProperties"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// A collection of booking reservations matching the specified criteria.
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///     GET 
        ///     id=1
        ///     bookingDate=2023-05-26
        ///     totalPrice=100.50
        ///     customerId=5
        ///     bookingStatus=1
        ///     orderBy=bookingDate
        ///     isAscending=true
        ///     includeProperties=Customer,Room
        ///     pageIndex=0
        ///     pageSize=10
        /// </remarks>
        [HttpGet("booking-reservations")]
        public async Task<IActionResult> GetBookingReservations([FromQuery] string? keyword,
            [FromQuery] int? id,
            [FromQuery] DateOnly? bookingDate,
            [FromQuery] decimal? totalPrice,
            [FromQuery] int? customerId,
            [FromQuery] byte? bookingStatus,
            [FromQuery] string[]? includeProperties,
            [FromQuery] string? orderBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var response = await _bookingReservationService.GetAllBookingReservations(
                isAscending: isAscending,
                filter: x => (!id.HasValue || x.BookingReservationId == id) &&
                             (!bookingDate.HasValue || x.BookingDate == bookingDate) &&
                             (!totalPrice.HasValue || x.TotalPrice == totalPrice) &&
                             (!customerId.HasValue || x.CustomerId == customerId) &&
                             (!bookingStatus.HasValue || x.BookingStatus == bookingStatus) &&
                             (string.IsNullOrEmpty(keyword) ||
                              x.Customer.CustomerFullName.Contains(keyword) ||
                              x.Customer.EmailAddress.Contains(keyword) ||
                              x.Customer.Telephone.Contains(keyword)),
                orderBy: orderBy,
                includeProperties: includeProperties,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Get a booking reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking reservation.</param>
        /// <returns>The booking reservation with the specified ID.</returns>
        [HttpGet("booking-reservations/{id:int}")]
        public async Task<IActionResult> GetBookingReservation(int id)
        {
            var response = await _bookingReservationService.GetBookingReservationById(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Create a new booking reservation.
        /// </summary>
        /// <param name="requestModel">The model containing information to create the booking reservation.</param>
        /// <returns>The newly created booking reservation.</returns>
        /// <remarks>
        /// Sample request:
        ///     POST 
        ///     {
        ///         "BookingDate": "2023-05-26",
        ///         "TotalPrice": 100.50,
        ///         "CustomerId": 5,
        ///         "BookingStatus": 1
        ///     }
        /// </remarks>
        /// <response code="201">Created new booking reservation successfully.</response>
        [HttpPost("booking-reservations")]
        public async Task<IActionResult> CreateBookingReservation([FromBody] BookingReservationRequestCreate requestModel)
        {
            var response = await _bookingReservationService.CreateBookingReservation(requestModel);
            return response.IsError ? HandleErrorResponse(response.Errors) : Created($"api/bookingreservation/{response.Payload.BookingReservationId}", response);
        }

        /// <summary>
        /// Update an existing booking reservation.
        /// </summary>
        /// <param name="id">The ID of the booking reservation to update.</param>
        /// <param name="requestModel">The model containing updated information.</param>
        /// <returns>The updated booking reservation.</returns>
        [HttpPut("booking-reservations/{id:int}")]
        public async Task<IActionResult> UpdateBookingReservation(int id, [FromBody] BookingReservationRequestCreate requestModel)
        {
            var response = await _bookingReservationService.UpdateBookingReservation(id, requestModel);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Delete a booking reservation.
        /// </summary>
        /// <param name="id">The ID of the booking reservation to delete.</param>
        /// <returns>An action result indicating success or failure.</returns>
        [HttpDelete("booking-reservations/{id:int}")]
        public async Task<IActionResult> DeleteBookingReservation(int id)
        {
            var response = await _bookingReservationService.DeleteBookingReservation(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
