using Data.Models;
using Microsoft.EntityFrameworkCore;
using Repository.GenericRepository;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {

        public CustomerRepository(FuminiHotelManagementContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Customer>> FindAsync(Expression<Func<Customer, bool>> predicate, int pageIndex, int pageSize)
        {
            return await _context.Set<Customer>()
                .Where(predicate)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
