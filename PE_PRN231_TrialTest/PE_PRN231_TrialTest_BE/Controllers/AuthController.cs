using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PE.Core.Commons;
using PE.Core.Contracts;
using PE.Core.Dtos;

namespace PE_PRN231_TrialTest_BE.Controllers
{
    [Route("api/premier-league")]
    [ApiController]
    public class AuthController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        /// <summary>
        /// Sign in 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("account/sign-in")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<ActionResult<ApiResponseModel<SigninAccountResponse>>> Login([FromBody] SigninRequest request)
        {
            //if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            //    return BadRequest(new ApiResponseModel<string> { Message = "Email and password are required!", StatusCode = System.Net.HttpStatusCode.BadRequest });

            if (!ModelState.IsValid) return BadRequest();
            var account = await _accountService.AutheticateUser(request.Email, request.Password);
            if (account is null) return NotFound(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "Account is not found! Please check email and password again.",
                Response = null
            });
            return Ok(new ApiResponseModel<SigninAccountResponse>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Success",
                Response = account
            });
        }
    }
}
