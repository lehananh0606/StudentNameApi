using Data.Models;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> FindAsync(Expression<Func<Customer, bool>> predicate, int pageIndex, int pageSize);
        Task<Customer> GetByEmailAsync(string emailAddress);
    }


}
