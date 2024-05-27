using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.Services;
using System;

namespace StudentNameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingReportController : BaseController
    {
        private readonly IBookingReportService _bookingService;

        public BookingReportController(IBookingReportService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Get booking report for a specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the report period.</param>
        /// <param name="endDate">The end date of the report period.</param>
        /// <returns>A report of bookings within the specified date range.</returns>
        /// <remarks>
        ///     Sample request:
        ///
        ///         GET 
        ///         StartDate = 1999-01-01T00:00:00Z
        ///         EndDate = 2023-01-01T00:00:00Z
        /// </remarks>
        [HttpGet("report")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBookingReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var report = _bookingService.GetBookingReport(startDate, endDate);
            return Ok(report);
        }
    }
}
