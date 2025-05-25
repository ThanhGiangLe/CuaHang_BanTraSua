using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using testVue.Datas;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Twilio.TwiML.Messaging;
using testVue.ModelsRequest;
using testVue.Models;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            // Lấy thông tin người dùng từ cơ sở dữ liệu dựa trên số điện thoại
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == loginRequest.Phone);

            // Kiểm tra xem người dùng có tồn tại và mật khẩu có hợp lệ không
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                // Nếu không tìm thấy người dùng, trả về thông báo lỗi 401
                return Unauthorized(new { message = "Invalid phone or password" });
            }

            // Nếu thông tin hợp lệ, trả về thông tin người dùng (không trả về mật khẩu)
            return Ok(new
            {
                userId = user.UserId,
                fullName = user.FullName,
                phone = user.Phone,
                email = user.Email,
                role = user.Role,
                address = user.Address,
                avatar = user.Avatar
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
            var user = new UserDTO
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
            if(userId == 0)
            {
                return BadRequest("UserId không hợp lệ");
            }
            var user = await _context.Users.FindAsync(userId);
            if(user == null)
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
            if (request == null) {
                return BadRequest("Thông tin không hợp lệ");
            }
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null) {
                return NotFound("Không tìm thấy User đang tương tác");
            }
            if(request.NewPassword != "")
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
            catch (Exception ex) { 
                return StatusCode(500, new
                {
                    success = -1,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }
    }
}