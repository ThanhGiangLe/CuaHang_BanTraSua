using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using testVue.Datas;
using testVue.ModelsRequest;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmailController(AppDbContext context)
        {
            _context = context;
        }

        // POST: http://localhost:5248/api/email/check
        [HttpPost("check")]
        public async Task<IActionResult> CheckEmail([FromBody] EmailCheckRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Ok(new { message = "Email không hợp lệ" });
            }

            // Kiểm tra xem email có tồn tại trong cơ sở dữ liệu không
            var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);

            if (userExists)
            {
                // Nếu email tồn tại
                return Ok(new { exists = true });
            }
            else
            {
                // Nếu email không tồn tại
                return Ok(new { exists = false });
            }
        }
    }
}
