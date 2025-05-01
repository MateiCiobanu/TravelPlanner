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
        private readonly ITravelerTypeRepository _travelerTypeRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, ITravelerTypeRepository travelerTypeRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _travelerTypeRepository = travelerTypeRepository;
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
                    ModelState.AddModelError("", "User already exists");
                    return StatusCode(422, ModelState);
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userMap = _mapper.Map<User>(userDto);
                CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                userMap.PasswordHash = passwordHash;
                userMap.PasswordSalt = passwordSalt;


                if (!await _userRepository.CreateUser(userMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving user");
                    return StatusCode(500, ModelState);
                }

                foreach(TravelerTypeCreateDTO type in userDto.TravelerTypes)
                {
                    var travelerType = new TravelerType();
                    travelerType.TravelerTypeName = type.TravelerTypeName;
                    travelerType.PreferenceWeight = type.PreferenceWeight;
                    travelerType.UserId = userMap.Id;
                    if (!await _travelerTypeRepository.CreateTravelerType(travelerType))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving travelerType");
                        return StatusCode(500, ModelState);
                    }
                }

                return Ok("Successfully created");
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
