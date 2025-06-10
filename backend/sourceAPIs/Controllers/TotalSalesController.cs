using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sourceAPI.ModelsRequest;
using testVue.Datas;

namespace sourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalSalesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TotalSalesController(AppDbContext context) {
            _context = context;
        }

        [HttpGet("get-total-sales-all-employee")]
        public async Task<IActionResult> GetTotalSalesAllEmployee()
        {
            try
            {
                var currentDay = DateTime.UtcNow.Day;
                var result = await _context.Users
                        .Where(u => u.Role != "Customer")
                        .Join(_context.Schedules,
                            u => u.UserId,
                            s => s.UserId,
                            (u, s) => new { u, s })
                        .Where(resJoin => resJoin.s.Date.Day <= currentDay)
                        .OrderByDescending(resJoin => resJoin.s.Date)
                        .Select(resJoin => new {
                            resJoin.u.FullName,
                            resJoin.s.Date,
                            resJoin.s.ShiftId,
                            resJoin.s.CashAmount,
                            resJoin.s.BankAmount,
                            resJoin.s.ClosingCashAmount
                        })
                        .ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = e.Message });
            }
        }

        [HttpPost("get-total-sales-by-day-or-userid")]
        public async Task<IActionResult> GetTotalSalesEmployee([FromBody] GetTotalSalesRequest request)
        {
            try
            {
                int currentDay = DateTime.Now.Day;
                var query = _context.Users
                            .Where(u => u.Role != "Customer");
                if (request.UserId != 0)
                {
                    query = query.Where(u => u.UserId == request.UserId);
                }
                var result = await query
                    .Join(_context.Schedules,
                        u => u.UserId,
                        s => s.UserId,
                        (u, s) => new { u, s })
                    .Where(resJoin => request.Day == default(DateTime) || resJoin.s.Date == request.Day && resJoin.s.Date.Day <= currentDay)
                    .OrderByDescending(resJoin => resJoin.s.Date)
                    .Select(resJoin => new {
                        resJoin.u.FullName,
                        resJoin.s.Date,
                        resJoin.s.ShiftId,
                        resJoin.s.CashAmount,
                        resJoin.s.BankAmount,
                        resJoin.s.ClosingCashAmount
                    })
                    .ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = e.Message });
            }
        }
    }
}
