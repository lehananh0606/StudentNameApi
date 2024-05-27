using System;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModel.Request
{
    public class BookingDetailRequest
    {
        [Required]
        public int BookingReservationId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly StartDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly EndDate { get; set; }

        public decimal? ActualPrice { get; set; }
    }
}
