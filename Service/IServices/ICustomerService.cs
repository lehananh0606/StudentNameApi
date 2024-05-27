using Data.Models;
using Service.Commons;
using Service.ViewModel.Request;
using Service.ViewModel.Requet;
using Service.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface ICustomerService
    {
        Task<(string Token, LoginResponse loginResponse)> AuthorizeUser(LoginRequest loginRequest);
        Task<OperationResult<IEnumerable<AccountResponse>>> GetAll(bool? isAscending, string? orderBy = null, Expression<Func<Customer, bool>>? filter = null, string[]? includeProperties = null, int pageIndex = 0, int pageSize = 10);
        Task<OperationResult<AccountResponse>> GetById(int objectId);
        Task<OperationResult<AccountResponse>> Create(AccountRequestCreate objectRequestCreate);
        Task<OperationResult<bool>> Delete(int id);
        Task<OperationResult<AccountResponse>> Update(int id, AccountRequestCreate objectRequestUpdate);
        Task<OperationResult<IEnumerable<AccountResponse>>> Search(string keyword, int pageIndex = 0, int pageSize = 10);
        //Task<OperationResult<AccountResponse>> GetByEmail(string email);
        //Task<OperationResult<AccountResponse>> UpdateByEmail(string email, AccountRequestCreate requestModel);
    }
}
