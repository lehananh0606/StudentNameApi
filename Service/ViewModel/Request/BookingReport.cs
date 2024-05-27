using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModel.Request
{

    public class BookingReport
    {
        public int BookingReservationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? ActualPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public byte? BookingStatus { get; set; }
    }

}
