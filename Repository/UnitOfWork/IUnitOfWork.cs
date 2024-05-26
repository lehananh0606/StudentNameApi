
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Storage;
using Data.Models;
using Repository.IRepositories;

namespace Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public ICustomerRepository customerRepository { get; }
        public IRoomInformationRepository roomRepository { get; }
        public IBookingReservationRepository bookingRepository { get; }
        public IBookingDetailRepository bookingDetailRepository { get; }
        public IRoomTypeRepository roomTypeRepository { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        
        int Save();
        Task<int> SaveChangesAsync();
    }
}
