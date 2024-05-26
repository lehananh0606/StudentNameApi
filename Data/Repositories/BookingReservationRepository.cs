using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class BookingReservationRepository
    {
        private List<BookingReservation> _bookingReservations;

        public BookingReservationRepository()
        {
            _bookingReservations = new List<BookingReservation>();
        }

        public IEnumerable<BookingReservation> GetAllBookingReservations()
        {
            return _bookingReservations;
        }

        public BookingReservation GetBookingReservationById(int id)
        {
            return _bookingReservations.FirstOrDefault(b => b.BookingReservationId == id);
        }

        public void AddBookingReservation(BookingReservation bookingReservation)
        {
            _bookingReservations.Add(bookingReservation);
        }

        public void UpdateBookingReservation(BookingReservation bookingReservation)
        {
            var existingBookingReservation = _bookingReservations.FirstOrDefault(b => b.BookingReservationId == bookingReservation.BookingReservationId);
            if (existingBookingReservation != null)
            {
                existingBookingReservation.BookingDate = bookingReservation.BookingDate;
                existingBookingReservation.TotalPrice = bookingReservation.TotalPrice;
                existingBookingReservation.CustomerId = bookingReservation.CustomerId;
                existingBookingReservation.BookingStatus = bookingReservation.BookingStatus;
            }
        }

        public void DeleteBookingReservation(int id)
        {
            var existingBookingReservation = _bookingReservations.FirstOrDefault(b => b.BookingReservationId == id);
            if (existingBookingReservation != null)
            {
                _bookingReservations.Remove(existingBookingReservation);
            }
        }
    }
}
