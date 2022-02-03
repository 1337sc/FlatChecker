#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlatChecker.Data;
using FlatChecker.Models;
using FlatChecker.Models.Requests;
using FlatChecker.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using FlatChecker.Utils;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using FlatChecker.Dto;
using System.Security.Claims;
using FlatChecker.Models.Dto;

namespace FlatChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IOptions<AuthOptions> _authOptions;

        public UsersController(ApplicationContext context, IOptions<AuthOptions> authOptions)
        {
            _context = context;
            _authOptions = authOptions;
        }

        [HttpGet]
        public IEnumerable<UserDto> GetUser()
        {
            return _context.Users.Select(Map);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Map(user);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutUser(ChangeUserDataRequest request)
        {
            if (User.IsInRole(Role.Admin.ToString())
                || User.Identity.Name == request.UserId.ToString())
            {
                return Forbid();
            }
            try
            {
                var user = await _context.Users.FindAsync(request.UserId);
                user.Email = request.Email;
                user.PasswordHash = GetPasswordHash(request.Password);
                user.UpdatedDate = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(request.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UserLoginResponseDto>> PostUser(UserLoginRequest userRequest)
        {
            var user = Map(userRequest);
            user.UpdatedDate = DateTime.UtcNow;
            var userEntity = _context.Users.Add(user).Entity;
            await _context.SaveChangesAsync();

            var res = new UserLoginResponseDto()
            {
                Username = userEntity.Email,
                Token = Login(userRequest).Value.Token
            };
            return CreatedAtAction("GetUser", res);
        }

        [HttpPost("login")]
        public ActionResult<UserLoginResponseDto> Login(UserLoginRequest userLoginRequest)
        {
            var identity = GetIdentity(userLoginRequest.Email, GetPasswordHash(userLoginRequest.Password));
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: _authOptions.Value.Issuer,
                    audience: _authOptions.Value.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_authOptions.Value.Lifetime)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                        Base64UrlEncoder.DecodeBytes(_authOptions.Value.Salt)),
                        SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new UserLoginResponseDto
            {
                Token = string.Format(encodedJwt, "x"),
                Username = identity.Name
            };

            return response;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private User Map(UserLoginRequest user)
        {
            return new User()
            {
                Email = user.Email,
                Role = Role.User,
                PasswordHash = GetPasswordHash(user.Password)
            };
        }

        private UserDto Map(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                OwnedSuggestions = _context.Suggestions.Where(s => s.OwnerId == user.Id)
                    .Select(s => ModelsMapUtils.Map(s))
                    .ToList()
            };
        }

        private string GetPasswordHash(string password)
        {
            HMACSHA256 hashFunc = new(Encoding.Unicode.GetBytes(_authOptions.Value.Salt));
            return Encoding.Unicode.GetString(
                hashFunc.ComputeHash(Encoding.Unicode.GetBytes(password)));
        }

        private ClaimsIdentity GetIdentity(string username, string passwordHash)
        {
            User user = _context.Users.FirstOrDefault(x => x.Email == username && x.PasswordHash == passwordHash);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
