using Data.Models;
using Microsoft.EntityFrameworkCore;
using Repository.GenericRepository;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {

        public CustomerRepository(FuminiHotelManagementContext context) : base(context)
        {
            
        }
    }
}
