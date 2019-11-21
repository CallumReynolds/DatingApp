using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    // This allows ASP to infer where data comes from e.g. Dtos
    // And specifies validation
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // Request is being validated by [ApiController]

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            // Check to see if the username is taken
            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            // Create a new user assigning Username to username value
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            // Register the new user with passed values
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            // Fix this to CreatedAtRoute
            return StatusCode(201);
        }
        

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            // Checking to make sure we have user in the database
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            // token contains 2 claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // Token Value should be randomly generated very long text
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            // sign in credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // token created here
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // create new Jwt handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // creates new Jwt Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}