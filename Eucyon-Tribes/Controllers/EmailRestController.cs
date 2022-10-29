using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.UserModels;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailRestController : ControllerBase    
    {
       
        private readonly IUserService _userService;

        public EmailRestController(IUserService userService)        
        {            
            _userService = userService;
        }

        [HttpGet("verify/{token}")]
        public IActionResult Verify(string token)
        {
            var response = _userService.VerifyUserWithEmail(token);

            switch (response)
            {
                case "Invalid token":
                    return Unauthorized(new ErrorDTO("The token is invalid or has expired. Please request new verification email"));
                case "User already verified":
                    return BadRequest(new ErrorDTO(response));
                default:
                    return Ok(new StatusDTO(response));
            }
        }

        [HttpPost("newToken")]
        public IActionResult NewToken(UserLoginDto login)
        {
            var response = _userService.NewTokenGeneration(login);

            if (response.Equals("New verification email has been sent"))
            {
                return Ok(new StatusDTO(response));
            }
            return BadRequest(new ErrorDTO(response));
        }

        [HttpPost("reset-password")]
        public IActionResult NewPasswordRequest(EmailForPasswordResetDto emailForPasswordResetDto)
        {
            var response = _userService.NewPasswordRequest(emailForPasswordResetDto);

            switch (response)
            {
                case "Email is required":
                    return BadRequest(new ErrorDTO(response));
                case "Email is not verified":
                    return Unauthorized(new ErrorDTO(response));
                default:
                    return Ok(new StatusDTO(response));
            }          
        }

        [HttpGet("reset-password/{token}")]
        public IActionResult NewPasswordVerification(string token)
        {
            var response = _userService.NewPasswordVerification(token);

            switch (response)
            {
                case "The token is invalid or has expired. Please request new verification email":
                    return Unauthorized(new ErrorDTO(response));
                case "User forgotten password token already verified":
                    return BadRequest(new ErrorDTO(response));                
                default:
                    return Ok(new StatusDTO(response));
            }         
        }

        [HttpPost("reset-password/{token}")]
        public IActionResult NewPasswordGeneration(string token, NewPasswordDTO newPasswordDTO)
        {
            var response = _userService.NewPasswordGeneration(token, newPasswordDTO);

            switch (response)
            {
                case "The token is invalid or has expired. Please request new verification email":
                    return Unauthorized(new ErrorDTO(response));
                case "Password token has not been verified yet. Please check your email":
                    return BadRequest(new ErrorDTO(response));
                case "Password is required":
                    return BadRequest(new ErrorDTO(response));
                case "Password must be at least 8 characters long":
                    return BadRequest(new ErrorDTO(response));
                default:
                    return Ok(new StatusDTO(response));                    
            }         
        }
    }
}
