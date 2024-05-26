using Data.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private FuminiHotelManagementContext _dbContext = new FuminiHotelManagementContext();
        public ICustomerRepository customerRepository { get; }
        public IRoomInformationRepository roomRepository { get; }
        public IBookingReservationRepository bookingRepository { get; }
        public IBookingDetailRepository bookingDetailRepository { get; }
        public IRoomTypeRepository roomTypeRepository { get; }

      
        public UnitOfWork(
        FuminiHotelManagementContext dbContext,
        ICustomerRepository customerRepo,
        IRoomInformationRepository roomRepo,
        IBookingReservationRepository bookingRepo,
        IBookingDetailRepository bookingDetailRepo,
        IRoomTypeRepository roomTypeRepo)
        {
            _dbContext = dbContext;
            customerRepository = customerRepo;
            roomRepository = roomRepo;
            bookingRepository = bookingRepo;
            bookingDetailRepository = bookingDetailRepo;
            roomTypeRepository = roomTypeRepo;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task<IDbContextTransaction> EndTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
            return transaction;
        }
    }
}
