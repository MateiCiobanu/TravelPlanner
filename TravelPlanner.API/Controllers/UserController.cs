using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        // YENİ ENDPOINT - User bilgilerini ID ile getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Hassas bilgileri gizle
                var userInfo = new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    TravelerType = user.TravelerType,
                    CreatedAt = user.CreatedAt
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userDto)
        {
            try
            {
                if (userDto == null)
                    return BadRequest(ModelState);

                var user = await _userRepository.GetUserByEmail(userDto.Email);

                if (user != null)
                {
                    return StatusCode(422, "User already exists with this email.");

                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userMap = _mapper.Map<User>(userDto);
                userMap.TravelerType = "undefined";
                CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                userMap.PasswordHash = passwordHash;
                userMap.PasswordSalt = passwordSalt;


                if (!await _userRepository.CreateUser(userMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving user");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Invalid request");

                var user = await _userRepository.GetUserByEmail(request.Email);
                if (user == null)
                    return NotFound("User not found.");

                // Mevcut şifreyi doğrula
                if (!VerifyPasswordHash(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                    return BadRequest("Current password is incorrect.");

                // Yeni şifreyi hashle
                CreatePasswordHash(request.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);
                
                user.PasswordHash = newPasswordHash;
                user.PasswordSalt = newPasswordSalt;
                user.UpdatedAt = DateTime.UtcNow;

                if (!await _userRepository.Save())
                {
                    return StatusCode(500, "Failed to update password");
                }

                return Ok("Password updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Invalid request");

                var user = await _userRepository.GetUserByEmail(request.Email);
                if (user == null)
                    return NotFound("User not found.");

                // Profil bilgilerini güncelle
                user.Name = request.Name;
                user.Email = request.Email;
                user.TravelerType = request.TravelerType;
                user.UpdatedAt = DateTime.UtcNow;

                if (!await _userRepository.Save())
                {
                    return StatusCode(500, "Failed to update profile");
                }

                return Ok("Profile updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userDto)
        {
            try
            {
                if(userDto == null)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userRepository.GetUserByEmail(userDto.Email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                user.TravelerType = userDto.TravelerType;
                if (!await _userRepository.Save())
                {
                    ModelState.AddModelError("", "Something went wrong while updating user");
                    return StatusCode(500, ModelState);
                }
                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            try
            {
                User user = await _userRepository.GetUserByEmail(request.Email);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest("Wrong password.");
                }

                string token = CreateToken(user);

                //var refreshToken = GenerateRefreshToken();
                //SetRefreshToken(refreshToken);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private string CreateToken(User user)
        {
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);

        }
    }
}