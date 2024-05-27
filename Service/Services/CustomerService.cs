using AutoMapper;
using Azure;
using Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.UnitOfWork;
using Service.Commons;
using Service.IServices;
using Service.ViewModel.Request;
using Service.ViewModel.Requet;
using Service.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<(string Token, LoginResponse loginResponse)> AuthorizeUser(LoginRequest loginRequest)
        {
            var member = _unitOfWork.customerRepository
                .Get(filter: a => a.EmailAddress == loginRequest.EmailAddress
                && a.Password == loginRequest.Password && a.CustomerStatus == 1).FirstOrDefault();
            if (member != null)
            {
                string token = GenerateToken(member);
                var adminResponse = _mapper.Map<LoginResponse>(member);
                return (token, adminResponse);
            }

            var adminEmail = _configuration["AdminUser:EmailAddress"];
            var adminPassword = _configuration["AdminUser:Password"];

            if (loginRequest.EmailAddress == adminEmail && loginRequest.Password == adminPassword)
            {
                var defaultAdminResponse = _mapper.Map<LoginResponse>(member);

                string token = GenerateDefaultToken();

                return (token, defaultAdminResponse);
            }


            return (null, null);
        }

        private string GenerateDefaultToken()
        {
            var claims = new List<Claim>
    {
                new Claim("EmailAddress", _configuration["AdminUser:EmailAddress"]),
        new Claim("Role", _configuration["AdminUser:Role"]),
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateToken(Customer info)
        {
            List<Claim> claims = new List<Claim>()
        {
                new Claim(ClaimTypes.NameIdentifier, info.CustomerId.ToString()),
            new Claim("EmailAddress", info.EmailAddress),
            };

            var role = _configuration["DefaultUserRole"];
            claims.Add(new Claim("Role", role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


        public Task<OperationResult<IEnumerable<AccountResponse>>> GetAll(bool? isAscending,
            string? orderBy = null,
            Expression<Func<Customer, bool>>? filter = null,
            string[]? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10)
        {
            var result = new OperationResult<IEnumerable<AccountResponse>>();
            try
            {
                var entities = _unitOfWork.customerRepository.FilterAll(isAscending, orderBy, filter, includeProperties, pageIndex, pageSize);
                //var entities = _unitOfWork.customerRepository.GetAllAsync();
                foreach (var entity in entities)
                {
                    Console.WriteLine(entity);
                }

                result.Payload = _mapper.Map<IEnumerable<AccountResponse>>(entities);
                result.StatusCode = StatusCode.Ok;
                result.Message = "Get all customers successfully";
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return Task.FromResult(result);
        }

        public async Task<OperationResult<AccountResponse>> GetById(int objectId)
        {
            var result = new OperationResult<AccountResponse>();
            try
            {
                var entity = await _unitOfWork.customerRepository.GetByIdAsync(objectId);
                result.Payload = _mapper.Map<AccountResponse>(entity);
                result.StatusCode = StatusCode.Ok;

            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<AccountResponse>> Create(
         AccountRequestCreate accountRequestCreate)
        {
            var result = new OperationResult<AccountResponse>();
            try
            {
            

                var checkPhone =
                    await _unitOfWork.customerRepository.FindSingleAsync(
                        x => x.Telephone == accountRequestCreate.Telephone);
                if (checkPhone != null)
                {
                    result.AddError(StatusCode.BadRequest, "Phone already exists");
                    result.IsError = true;
                    return result;
                }

                var newAccount = _mapper.Map<Customer>(accountRequestCreate);

                /*newAccount.PasswordHash =
                    _userManager.PasswordHasher.HashPassword(newAccount, accountRequestCreate.Password);*/

                await _unitOfWork.customerRepository.AddAsync(newAccount);
                var count = await _unitOfWork.SaveChangesAsync();


                if (count > 0)
                {
                    result.Payload = _mapper.Map<AccountResponse>(newAccount);
                    result.StatusCode = StatusCode.Created;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Create Customer Failed");
                }

            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<bool>> Delete(int id)
        {
            var result = new OperationResult<bool>();
            try
            {
                var deleteOb = await _unitOfWork.customerRepository.GetByIdAsync(id);
                if (deleteOb == null)
                {
                    result.AddError(StatusCode.NotFound, "Customer not found");
                    return result;
                }

                _unitOfWork.customerRepository.SoftRemove(deleteOb);
                var count = await _unitOfWork.SaveChangesAsync();
                if (count > 0)
                {
                    result.Payload = true;
                    result.StatusCode = StatusCode.Ok;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Delete Customer Failed");
                }
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<AccountResponse>> Update(int id, AccountRequestCreate objectRequestUpdate)
        {
            var result = new OperationResult<AccountResponse>();
            try
            {
                var entity = await _unitOfWork.customerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    result.AddError(StatusCode.NotFound, "Customer not found");
                    return result;
                }

                _mapper.Map(objectRequestUpdate, entity);
                _unitOfWork.customerRepository.Update(entity);
                var count = _unitOfWork.SaveChangesAsync();
                if (count.Result > 0)
                {
                    result.Payload = _mapper.Map<AccountResponse>(entity);
                    result.StatusCode = StatusCode.Ok;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update Area Failed");
                }
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<IEnumerable<AccountResponse>>> Search(string keyword, int pageIndex = 0, int pageSize = 10)
        {
            var result = new OperationResult<IEnumerable<AccountResponse>>();
            try
            {
                var entities = await _unitOfWork.customerRepository.FindAsync(
                    c => c.CustomerFullName.Contains(keyword) ||
                         c.EmailAddress.Contains(keyword) ||
                         c.Telephone.Contains(keyword),
                    pageIndex,
                    pageSize
                );
                result.Payload = _mapper.Map<IEnumerable<AccountResponse>>(entities);
                result.StatusCode = StatusCode.Ok;
                result.Message = "Search customers successfully";
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        //public async Task<OperationResult<AccountResponse>> GetByEmail(string email)
        //{
        //    var customer = await _unitOfWork.customerRepository.GetByEmailAsync(email);
        //    if (customer == null)
        //    {
        //        return OperationResult<AccountResponse>.Fail(StatusCode.NotFound, "Customer not found.");
        //    }

        //    var customerResponse = _mapper.Map<AccountResponse>(customer);
        //    return OperationResult<AccountResponse>.Success(customerResponse);
        //}

        //public async Task<OperationResult<AccountResponse>> UpdateByEmail(string email, AccountRequestCreate requestModel)
        //{
        //    var existingCustomer = await _unitOfWork.customerRepository.GetByEmailAsync(email);
        //    if (existingCustomer == null)
        //    {
        //        return OperationResult<AccountResponse>.Fail(StatusCode.NotFound, "Customer not found.");
        //    }

        //    // Update customer properties based on the request model
        //    _mapper.Map(requestModel, existingCustomer);

        //    _unitOfWork.customerRepository.Update(existingCustomer);
        //    await _unitOfWork.SaveChangesAsync();

        //    var updatedCustomerResponse = _mapper.Map<AccountResponse>(existingCustomer);
        //    return OperationResult<AccountResponse>.Success(updatedCustomerResponse);
        //}

    }
}
