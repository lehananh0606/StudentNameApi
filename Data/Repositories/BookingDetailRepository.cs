using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class BookingDetailRepository
    {
        private List<BookingDetail> _bookingDetails;

        public BookingDetailRepository()
        {
            _bookingDetails = new List<BookingDetail>();
        }

        public IEnumerable<BookingDetail> GetAllBookingDetails()
        {
            return _bookingDetails;
        }

        public BookingDetail GetBookingDetailById(int bookingId, int roomId)
        {
            return _bookingDetails.FirstOrDefault(b => b.BookingReservationId == bookingId && b.RoomId == roomId);
        }

        public void AddBookingDetail(BookingDetail bookingDetail)
        {
            _bookingDetails.Add(bookingDetail);
        }

        public void UpdateBookingDetail(BookingDetail bookingDetail)
        {
            var existingBookingDetail = _bookingDetails.FirstOrDefault(b => b.BookingReservationId == bookingDetail.BookingReservationId && b.RoomId == bookingDetail.RoomId);
            if (existingBookingDetail != null)
            {
                existingBookingDetail.StartDate = bookingDetail.StartDate;
                existingBookingDetail.EndDate = bookingDetail.EndDate;
                existingBookingDetail.ActualPrice = bookingDetail.ActualPrice;
            }
        }

        public void DeleteBookingDetail(int bookingId, int roomId)
        {
            var existingBookingDetail = _bookingDetails.FirstOrDefault(b => b.BookingReservationId == bookingId && b.RoomId == roomId);
            if (existingBookingDetail != null)
            {
                _bookingDetails.Remove(existingBookingDetail);
            }
        }
    }
}
