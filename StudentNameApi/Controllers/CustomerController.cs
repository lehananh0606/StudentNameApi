using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.ViewModel.Requet;
using System.Net;

namespace StudentNameApi.Controllers
{
    [Route("api/customer-management")]
    [ApiController]
    public class CustomerController :  BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        /// <summary>
        /// Get customers based on provided filters.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="id"></param>
        /// <param name="CustomerFullName"></param>
        /// <param name="Telephone"></param>
        /// <param name="EmailAddress"></param>
        /// <param name="CustomerStatus"></param>
        /// <param name="CustomerBirthday"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending"></param>
        /// <param name="includeProperties"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// A collection of customers matching the specified criteria.
        /// </returns>
        /// <remarks>
        ///     Sample request:
        ///
        ///         GET 
        ///         id = 1
        ///         id=1
        ///         CustomerFullName=John Doe
        ///         Telephone=1234567890
        ///         EmailAddress=johndoe@example.com
        ///         CustomerStatus=1
        ///         CustomerBirthday=2023-05-26
        ///         orderBy=CustomerFullName
        ///         isAscending=true
        ///         includeProperties=Orders,Addresses
        ///         pageIndex=0
        ///         pageSize=10
        /// </remarks>
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers([FromQuery] string? keyword,
            [FromQuery] int? id,
            [FromQuery] string? CustomerFullName,
            [FromQuery] string? Telephone,
            [FromQuery] string? EmailAddress,
            [FromQuery] byte? CustomerStatus,
            [FromQuery] DateOnly? CustomerBirthday,
            [FromQuery] string? orderBy,
            [FromQuery] bool? isAscending,
            [FromQuery] string[]? includeProperties,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var response = await _customerService.GetAll(
                isAscending: isAscending,
                filter: x => (!id.HasValue || x.CustomerId == id) &&
                         (string.IsNullOrEmpty(keyword) || x.CustomerFullName.Contains(keyword) ||
                          x.EmailAddress.Contains(keyword) || x.Telephone.Contains(keyword)  &&
                         (string.IsNullOrEmpty(CustomerFullName) || x.CustomerFullName.Contains(CustomerFullName)) &&
                         (string.IsNullOrEmpty(EmailAddress) || x.EmailAddress.Contains(EmailAddress)) &&
                         (string.IsNullOrEmpty(Telephone) || x.Telephone.Contains(Telephone)) ),
                orderBy: orderBy, // Pass the string representation of the property name
                includeProperties: includeProperties,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response); 
        }


        /// <summary>
        /// get customer by id
        /// </summary>
        /// <param name="id"> id customer</param>
        /// <returns> 1 cusomer</returns>
        /// /// <remarks>
        ///     Sample request:
        ///
        ///         GET 
        ///         id = 1
        /// </remarks>
        [HttpGet("customers/{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var response = await _customerService.GetById(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);

        }

        /// <summary>
        /// create a customer
        /// </summary>
        /// <param name="requestCreateModel">model create</param>
        /// <returns> created customer</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///         "CustomerFullName": "John Doe",
        ///         "Telephone": "0912345678",
        ///         "EmailAddress": "johndoe@example.com",
        ///         "CustomerBirthday": "1990-01-01",
        ///         "Password": "your_password"
        ///     }
        /// </remarks>
        /// <response code="201">Created new customer successfully.</response>
        [HttpPost("customers")]
         public async Task<IActionResult> CreateCustomer([FromBody] AccountRequestCreate requestCreateModel)
         {
            var response = await _customerService.Create(requestCreateModel);
            return response.IsError
                ? HandleErrorResponse(response.Errors)
                : Created($"customer/{response.Payload.CustomerId}", response);
        }


        /// <summary>
        /// Update customer information.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="requestModel">The model containing updated customer information.</param>
        /// <returns>The updated customer information.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT 
        ///     {
        ///         "CustomerFullName": "Jane Doe",
        ///         "Telephone": "0987654321",
        ///         "EmailAddress": "janedoe@example.com",
        ///         "CustomerBirthday": "1995-08-15",
        ///         "Password": "new_password"
        ///     }
        /// </remarks>
        [HttpPut("customers/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] AccountRequestCreate requestModel)
        {
            var response = await _customerService.Update(id, requestModel);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Delete customer information by its ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("customers/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var response = await _customerService.Delete(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Search for customers by keyword.
        /// </summary>
        /// <param name="keyword">The keyword to search for in customer information.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A collection of customers matching the search criteria.</returns>
        [HttpGet("customers/search")]
        public async Task<IActionResult> SearchCustomers([FromQuery] string keyword, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var response = await _customerService.Search(keyword, pageIndex, pageSize);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
