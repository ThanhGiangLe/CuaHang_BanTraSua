using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sourceAPI.Models.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testVue.Datas;
using testVue.ModelsRequest;

namespace sourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSetting;

        public AuthenController(AppDbContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSetting = jwtSettings;
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckEmail([FromBody] EmailCheckRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Ok(new { message = "Email không hợp lệ" });
            }

            var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);

            if (userExists)
            {
                return Ok(new { exists = true });
            }
            else
            {
                return Ok(new { exists = false });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { message = "Refresh token không tồn tại" });
            }
            var existingToken = await _context.RefreshTokens.Include(t => t.User).FirstOrDefaultAsync(t => t.Token == refreshToken);
            if (existingToken == null || existingToken.IsUsed || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.Now)
            {
                return BadRequest(new { message = "Refresh token không hợp lệ hoặc đã hết hạn" });
            }
            var user = existingToken.User ?? new();

            // Tạo access token mới
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName ?? ""),
                    new Claim(ClaimTypes.Role, user.Role ?? "Nhân viên"),
                }),
                Expires = DateTime.Now.AddHours(_jwtSetting.ExpireHours),
                Issuer = _jwtSetting.Issuer,
                Audience = _jwtSetting.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            existingToken.IsUsed = true;

            var newRefreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                ExpiresAt = DateTime.Now.AddHours(_jwtSetting.RefreshTokenExpireDays)
            };
            _context.RefreshTokens.Add(newRefreshToken);

            // Ghi vào cookie mới
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRefreshToken.ExpiresAt
            });
            await _context.SaveChangesAsync();

            return Ok(new { token = accessToken });
        }
    }
}
