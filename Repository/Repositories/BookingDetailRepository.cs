using Data.Models;
using Repository.IRepositories;
using Repository.GenericRepository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class BookingDetailRepository : GenericRepository<BookingDetail>, IBookingDetailRepository
    {

        public BookingDetailRepository(FuminiHotelManagementContext context) : base(context)
        {
            }


    }
}
