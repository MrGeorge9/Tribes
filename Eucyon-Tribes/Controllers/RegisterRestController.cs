using Eucyon_Tribes.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    public class RegisterRestController : ControllerBase
    {
        private readonly UserRestController playerController;
  
        public RegisterRestController(UserRestController playerController)
        {
            this.playerController = playerController;
        }
        
        [HttpGet("registration")]
        public IActionResult Register(UsersInputDto user)
        {         
            this.playerController.Store(user);
           return Ok();
        }
    }
}
