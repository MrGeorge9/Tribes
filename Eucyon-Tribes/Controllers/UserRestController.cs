using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.UserModels;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserRestController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserRestController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult UserLogin(UserLoginDto login)
        {
            string message = _userService.Login(login);
            if (!message.EndsWith("in"))
            {
                return BadRequest(new ErrorDTO(message));
            }
            return Ok(new StatusDTO(message));
        }

        [HttpGet("info")]
        public IActionResult UserInformation(string name)
        {
            User info = _userService.UserInfo(name);
            if (info == null) return NotFound(new ErrorDTO("User not in database"));
            return Ok(info);
        }

        [HttpGet("")]
        public IActionResult Index(int page, int itemCount)
        {
            List<UserResponseDto> users = _userService.ListAllUsers(page, itemCount);
            if (users == null)
            {
                ErrorDTO error = new("No users in database");
                return NotFound(error);
            }
            return Ok(users);
        }

        [HttpPost("")]
        public IActionResult Store(UsersInputDto users)
        {
            if (users == null)
            {
                ErrorDTO error = new("No valid input object");
                return BadRequest(error);
            }
            return Ok(new StatusDTO(_userService.StoreUsers(users) + "users added to database"));
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            if (id < 1)
            {
                ErrorDTO e = new("Invalid id");
                return StatusCode(400, e);
            }
            UserResponseDto info = _userService.ShowUser(id);
            if (info == null)
            {
                ErrorDTO errorMessage = new("Player not found");
                return NotFound(errorMessage);
            }
            return Ok(info);
        }

        [HttpGet("{id}/edit")]
        public IActionResult Edit(int id, string name, string password)
        {
            if (id < 1)
            {
                ErrorDTO e = new("Invalid id");
                return StatusCode(400, e);
            }
            if (_userService.EditUser(id, name, password))
            {
                return Ok(new StatusDTO("User ID: " + id + "changed name to: " + name));
            }
            else
            {
                ErrorDTO message = new("Not an existing ID or not matching password");
                return BadRequest(message);
            }
        }

        [HttpPost("create")]
        public IActionResult UserCreate(UserCreateDto create)
        {
            var result = _userService.CreateUser(create, null, 0);
            if (result.ElementAt(0).Key != 201)
            {
                return StatusCode(result.ElementAt(0).Key, new ErrorDTO(result.ElementAt(0).Value));
            }
            return StatusCode(201, new StatusDTO("ok"));
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Destroy(int id, string password)
        {
            if (id < 1)
            {
                ErrorDTO e = new("Invalid id");
                return StatusCode(400, e);
            }
            if (_userService.DestroyUser(id, password))
            {
                return Ok(new StatusDTO("User ID: " + id + " has been removed"));
            }
            ErrorDTO error = new("Wrong UserID or password");
            return NotFound(error);
        }

        [HttpGet("info/admin")]
        public IActionResult UsersInfoDetailedForAdmin(string admin)
        {
            List<UserDetailDto> users = _userService.UsersInfoDetailedForAdmin(admin);

            if (users == null)
            {
                return NotFound(new ErrorDTO("No users in database or wrong admin Password"));
            }
            return Ok(users);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserCreateDto user)
        {
            if (id < 1)
            {
                ErrorDTO e = new("Invalid id");
                return StatusCode(400, e);
            }
            if (!_userService.UpdateUser(id, user))
            {
                ErrorDTO error = new("Wrong UserID or password or existing email");
                return NotFound(error);
            }
            else
            {
                return Ok(new StatusDTO("User ID: " + id + " has been updated successfully"));
            }
        }
    }
}