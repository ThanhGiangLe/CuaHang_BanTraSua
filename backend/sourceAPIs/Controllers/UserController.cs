using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using testVue.Datas;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Twilio.TwiML.Messaging;
using testVue.ModelsRequest;
using testVue.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using sourceAPI.Models.Token;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSetting;

        public UserController(AppDbContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSetting = jwtSettings;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMdl>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == loginRequest.Phone);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return Unauthorized(new { message = "Mật khẩu không đúng" });
            }

            // Tạo token để trả về người dùng
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSetting.Key);

            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role ?? "Staff")
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtSetting.ExpireHours),
                Issuer = _jwtSetting.Issuer,
                Audience = _jwtSetting.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDiscriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Tạo refreshToken và lưu vào HTTP only
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSetting.RefreshTokenExpireDays)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Đặt true nếu dùng HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpiresAt
            });

            // CHỉ trả về token
            return Ok(new
            {
                token = tokenString,
                data = new
                {
                    userId = user.UserId,
                    fullName = user.FullName,
                    phone = user.Phone,
                    email = user.Email,
                    role = user.Role,
                    address = user.Address,
                    avatar = user.Avatar
                }
            });
        }

        // POST: api/user/update-password
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDTO request)
        {
            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Invalid input.");
            }

            // Lấy thông tin người dùng
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Hash mật khẩu mới
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            // Cập nhật mật khẩu
            user.Password = hashedPassword; // Lưu mật khẩu đã được mã hóa
            _context.Users.Update(user);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi lưu vào cơ sở dữ liệu
                return StatusCode(500, "An error occurred while updating the password.");
            }
        }

        // POST: api/user/add
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequestDTO addUserRequest)
        {
            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrEmpty(addUserRequest.FullName) ||
                string.IsNullOrEmpty(addUserRequest.Phone) ||
                string.IsNullOrEmpty(addUserRequest.Email) ||
                string.IsNullOrEmpty(addUserRequest.Password) ||
                string.IsNullOrEmpty(addUserRequest.Role))
            {
                return Ok(new { success = -1 });
            }

            // Kiểm tra xem người dùng đã tồn tại hay chưa
            var existingUser = await _context.Users.AnyAsync(u => u.Phone == addUserRequest.Phone || u.Email == addUserRequest.Email);
            if (existingUser)
            {
                return Ok(new { success = 0 });
            }

            // Tạo đối tượng người dùng mới
            var user = new UserMdl
            {
                FullName = addUserRequest.FullName,
                Phone = addUserRequest.Phone,
                Email = addUserRequest.Email,
                Address = addUserRequest.Address,
                Password = BCrypt.Net.BCrypt.HashPassword(addUserRequest.Password), // Hash mật khẩu
                Role = addUserRequest.Role,
                Avatar = "/public/meo.jpg",
            };

            // Thêm người dùng vào cơ sở dữ liệu
            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = 1,
                    userId = user.UserId,
                    fullName = user.FullName,
                    phone = user.Phone,
                    email = user.Email,
                    role = user.Role,
                    address = user.Address,
                    avatar = user.Avatar
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi lưu vào cơ sở dữ liệu
                return StatusCode(500, "An error occurred while adding the user.");
            }
        }

        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (userId == 0)
            {
                return BadRequest("UserId không hợp lệ");
            }
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("Không tồn tại nhân viên cần xóa");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Succes",
                userRomeve = user
            });
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Thông tin không hợp lệ");
            }
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("Không tìm thấy User đang tương tác");
            }
            if (request.NewPassword != "")
            {
                var newPass = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.Password = newPass;
            }

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.Address = request.Address;
            user.Phone = request.Phone;
            user.Role = request.Role;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = 1,
                    message = "Cập nhật thông tin thành công.",
                    data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = -1,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if(string.IsNullOrEmpty(refreshToken) )
            {
                return Unauthorized(new { message = "Refresh token không tồn tại" });
            }
            var existingToken = await _context.RefreshTokens.Include(t => t.User).FirstOrDefaultAsync(t => t.Token == refreshToken);
            if(existingToken == null || existingToken.IsUsed || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Refresh token không hợp lệ hoặc đã hết hạn" });
            }
            var user = existingToken.User;

            // Tạo access token mới
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role ?? "Staff"),
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtSetting.ExpireHours),
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
                ExpiresAt = DateTime.UtcNow.AddHours(_jwtSetting.RefreshTokenExpireDays)
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