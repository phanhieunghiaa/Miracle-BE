using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Repositories.Enums;
using Services.Helpers;
using Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserControllers : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly IConfiguration _configuration;

        public UserControllers(IUserServices userService, JwtTokenHelper jwtTokenHelper, IConfiguration configuration)
        {
            _userService = userService;
            _jwtTokenHelper = jwtTokenHelper;
            _configuration = configuration;
        }

        #region Register User
        [HttpPost("User register")]
        public async Task<IActionResult> AddUser([FromBody] RegisterDTO userDto)
        {
            try
            {
                var result = await _userService.AddUserAsync(userDto.Email, userDto.FullName, userDto.UserName, userDto.Password);
                if (!result)
                {
                    return BadRequest("Email already exists.");
                }

                return Ok("User created successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Trả về thông báo lỗi định dạng email
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred."); // Trả về lỗi chung
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(string email, string code)
        {
            var result = await _userService.ConfirmUserAsync(email, code);
            if (result)
            {
                var user = await _userService.GetUserByEmail(email);
                return Ok(new { message = "User confirmed and registered successfully." });
            }
            return BadRequest(new { message = "Invalid verification code or email." });
        }
        #endregion

        #region
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            // Tạo token JWT và trả về cho người dùng
            var token = _jwtTokenHelper.GenerateJwtToken(user);
            return Ok(new { token });
        }
        #endregion

        #region Get by Id
        [HttpGet]
        [Route("GetUserByID")]
        public async Task<IActionResult> GetUserByID([Required] String id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = new
                {
                    user.Id,
                    user.UserName,
                    user.Password,
                    user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    user.Province,
                    user.District,
                    user.Address,
                    user.CreatedDate,
                    user.UpdatedDate,
                    user.Status,
                    Role = ((UserRole)user.Role).ToString(),
                    user.Cart
                };

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        #endregion

        #region Update user by Id
        [HttpPatch]
        [Route("UpdateUserByID")]
        public async Task<IActionResult> UpdateUserByID([FromQuery] String id, [FromBody] UpdateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.UpdateUserAsync(userDTO, id);
                return Ok(result ? "Update Successful" : "Update failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateUserStatusByID")]
        public async Task<IActionResult> UpdateUserByID([FromQuery] String id, [FromBody] string ustatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.UpdateUserStatusAsync(ustatus, id);
                return Ok(result ? "Update Successful" : "Update failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
        #endregion

        #region Get all users
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var user = await _userService.GetAllUsers();
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        #endregion

        #region Add user for admin
        [HttpPost("Add User For Admin")]
        public async Task<IActionResult> AddUserForAdmin(string email, string fullName, string userName, string password, string uRole)
        {
            try
            {
                var result = await _userService.AddUserWithoutRegisterAsync(email, fullName, userName, password, uRole);
                if (!result)
                {
                    return BadRequest("Email already exists.");
                }

                return Ok("User created successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        #endregion
    }
}
