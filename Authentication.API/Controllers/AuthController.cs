using Microsoft.AspNetCore.Mvc;
using Authentication.API.Helpers;
using Authentication.Core.DTOs;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequestDto registerDto)
        {
            if (await _userService.GetUserByEmailAsync(registerDto.Email) != null)
                return BadRequest(new { Message = "Email is already in use!" });

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                BirthDate = registerDto.DateOfBirth,
                Gender = registerDto.Gender
            };

            await _userService.AddUserAsync(user);
            return Ok(new { Message = "User registered successfully!" });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestDto loginDto)
        {
            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return Unauthorized(new { Message = "Invalid credentials" });

            // Token üretimi
            var token = JwtHelper.GenerateToken(user);
    
            // Token ile başarılı yanıt dönme
            return Ok(new { Token = token });
        }

        // Get a user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            return Ok(user);
        }

        // Get all users
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Update a user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RegisterRequestDto updateDto)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            user.Username = updateDto.Username;
            user.Email = updateDto.Email;
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.PhoneNumber = updateDto.PhoneNumber;
            user.BirthDate = updateDto.DateOfBirth;
            user.Gender = updateDto.Gender;

            await _userService.UpdateUserAsync(user);
            return Ok(new { Message = "User updated successfully!" });
        }

        // Delete a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            await _userService.DeleteUserAsync(id);
            return Ok(new { Message = "User deleted successfully!" });
        }
    }
}
