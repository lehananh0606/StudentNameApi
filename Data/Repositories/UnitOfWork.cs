using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private FuminiHotelManagementContext _context = new FuminiHotelManagementContext();
        private GenericRepository<Customer> customerRepository;
        private GenericRepository<RoomInformation> roomRepository;
        private GenericRepository<BookingReservation> bookingRepository;
        private GenericRepository<BookingDetail> bookingDetailRepository;
        private GenericRepository<RoomType> roomTypeRepository;

        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                return customerRepository ??= new GenericRepository<Customer>(_context);
            }
        }

        public GenericRepository<RoomInformation> RoomRepository
        {
            get
            {
                return roomRepository ??= new GenericRepository<RoomInformation>(_context);
            }
        }

        public GenericRepository<BookingReservation> BookingRepository
        {
            get
            {
                return bookingRepository ??= new GenericRepository<BookingReservation>(_context);
            }
        }

        public GenericRepository<BookingDetail> BookingDetailRepository
        {
            get
            {
                return bookingDetailRepository ??= new GenericRepository<BookingDetail>(_context);
            }
        }

        public GenericRepository<RoomType> RoomTypeRepository
        {
            get
            {
                return roomTypeRepository ??= new GenericRepository<RoomType>(_context);
            }
        }

        public UnitOfWork()
        {

        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
