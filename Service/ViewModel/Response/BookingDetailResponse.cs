using System;

namespace Service.ViewModel.Response
{
    public class BookingDetailResponse
    {
        public int BookingReservationId { get; set; }

        public int RoomId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public decimal? ActualPrice { get; set; }
    }
}
