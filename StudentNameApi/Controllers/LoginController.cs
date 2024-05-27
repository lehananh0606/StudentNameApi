using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.ViewModel.Requet;
using System.Net;

namespace StudentNameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public LoginController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        /// <remarks>
        ///     Sample request:
        ///
        ///         GET 
        ///         email = admin@FUMiniHotelSystem.com
        ///         passwork=@@abc123@@
        ///         ROLE Admin
        ///         
        ///         email = WilliamShakespeare@FUMiniHotel.org
        ///         passwork = 123@
        ///         ROLE User
        /// 
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _customerService.AuthorizeUser(loginRequest);
            if (result.Token != null)
            {
                return Ok(new { result.Token, LoginResponse = result.loginResponse });
            }
            else
            {
                return Ok(HttpStatusCode.Unauthorized);
            }
        }
    }
}
