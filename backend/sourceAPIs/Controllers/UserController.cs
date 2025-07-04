﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.AspNetCore.Authorization;
using sourceAPI.ModelsRequest;
using sourceAPI.Models;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMdl>>> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(new { success = 1, data = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = -1,
                    message = "Lỗi khi lấy danh sách tài khoản.",
                    details = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == loginRequest.Phone);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return BadRequest(new { message = "Mật khẩu không đúng" });
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

            // Chỉ trả về token
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


        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Invalid input.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

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

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequestDTO addUserRequest)
        {
            if (string.IsNullOrEmpty(addUserRequest.FullName) ||
                string.IsNullOrEmpty(addUserRequest.Phone) ||
                string.IsNullOrEmpty(addUserRequest.Email) ||
                string.IsNullOrEmpty(addUserRequest.Role))
            {
                return Ok(new { success = -1 });
            }

            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken == "Khách hàng" || roleFromToken == "Nhân viên")
            {
                return Forbid("Bearer");
            }

            // Kiểm tra xem người dùng đã tồn tại hay chưa
            var existingUser = await _context.Users.AnyAsync(u => u.Phone == addUserRequest.Phone || u.Email == addUserRequest.Email);
            if (existingUser)
            {
                return Ok(new { success = 0 });
            }

            var currentTime = DateTime.UtcNow.AddHours(7);
            var user = new UserMdl
            {
                FullName = addUserRequest.FullName,
                Phone = addUserRequest.Phone,
                Email = addUserRequest.Email,
                Address = addUserRequest.Address,
                Password = BCrypt.Net.BCrypt.HashPassword(addUserRequest.Password ?? "123"), // Hash mật khẩu
                Role = addUserRequest.Role ?? "Khách hàng",
                Avatar = addUserRequest.Avatar ?? "/public/meo.jpg",
                CreateDate = currentTime,
                CreateBy = addUserRequest.CreateBy,
                UpdateDate = currentTime,
                UpdateBy = addUserRequest.UpdateBy,
                Point = 20000,
                Status = "Busy"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (addUserRequest.Role != "Khách hàng")
            {
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int daysInCurrentMonth = DateTime.DaysInMonth(year, month);

                var defautlSchedule = Enumerable.Range(1, daysInCurrentMonth).
                    Select((item) => new
                    {
                        Day = item,
                        ShiftCode = "O"
                    }).ToList();
                foreach (var schedule in defautlSchedule)
                {
                    var scheduleDate = new DateTime(year, month, schedule.Day);
                    var newSchedule = new ScheduleMdl
                    {
                        UserId = user.UserId,
                        ShiftId = schedule.ShiftCode,
                        Date = scheduleDate,
                        CreateBy = "Auto",
                        CreateDate = currentTime,
                        UpdateBy = "Auto",
                        UpdateDate = currentTime,
                    };
                    var newScheduleHistory = new ScheduleHistoryMdl
                    {
                        UserId = user.UserId,
                        Date = scheduleDate,
                        OldShiftId = schedule.ShiftCode,
                        NewShiftId = schedule.ShiftCode,
                        ChangedBy = "Auto",
                        ChangedAt = currentTime
                    };
                    _context.Schedules.Add(newSchedule);
                    _context.ScheduleHistories.Add(newScheduleHistory);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = 1,
                    data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the user.");
            }
        }

        [Authorize]
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken == "Customer" || roleFromToken == "Staff")
            {
                return Forbid("Bearer");
            }
            var user = await _context.Users.FindAsync(userId);
            var schedules = _context.Schedules.Where(s => s.UserId == userId);
            if (user == null)
            {
                return NotFound("Không tồn tại nhân viên cần xóa");
            }
            _context.Users.Remove(user);
            _context.Schedules.RemoveRange(schedules);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Succes",
                userRomeve = user
            });
        }

        [Authorize]
        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Thông tin không hợp lệ");
            }

            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken == "Customer" || roleFromToken == "Staff")
            {
                return Forbid("Bearer");
            }
            var currentTime = DateTime.UtcNow.AddHours(7);
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
            user.UpdateDate = currentTime;
            user.UpdateBy = request.UpdateBy;
            if(request.Avatar != "")
            {
                user.Avatar = request.Avatar;
            }
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

        [HttpPost("get-email-by-phone")]
        public async Task<IActionResult> GetEmailByPhone([FromBody] PhoneRequest request)
        {
            if (request == null || request.Phone.Length < 10)
            {
                return BadRequest("Thông tin không hợp lệ!");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng!");
            }
            return Ok(new
            {
                success = 1,
                email = user.Email,
            });
        }
    }
}