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

        [HttpPost("get-total-sales")]
        public async Task<IActionResult> GetTotalSalesAllEmployee([FromBody] GetTotalSalesRequest request)
        {
            try
            {
                var currentDay = DateTime.Now.Day;
                var query = _context.Users.Where(u => u.Role != "Khách hàng");

                if (request.UserId > 0)
                {
                    query = query.Where(u => u.UserId == request.UserId);
                }
                var result = await query
                                .Join(_context.Schedules,
                                    u => u.UserId,
                                    s => s.UserId,
                                    (u, s) => new { u, s })
                                .Where(resultJoin =>
                                    (request.Day == default(DateTime) || resultJoin.s.Date.Date == request.Day.Date) &&
                                    resultJoin.s.Date.Day <= currentDay && resultJoin.s.ShiftId != "O")
                                .OrderByDescending(resultJoin => resultJoin.s.Date)
                                .Select(resultJoin => new {
                                    resultJoin.s.ScheduleId,
                                    resultJoin.u.FullName,
                                    resultJoin.s.Date,
                                    resultJoin.s.ShiftId,
                                    TotalOpeningCashAmount = _context.ScheduleShifts
                                                        .Where(ss => ss.ScheduleId == resultJoin.s.ScheduleId)
                                                        .Sum(ss => ss.OpeningCashAmount),
                                    ReceivedTotalAmount = _context.ScheduleShifts
                                                        .Where(ss => ss.ScheduleId == resultJoin.s.ScheduleId)
                                                        .Sum(ss => ss.ReceivedTotalAmount),
                                    ReturnedTotalAmount = _context.ScheduleShifts
                                                        .Where(ss => ss.ScheduleId == resultJoin.s.ScheduleId)
                                                        .Sum(ss => ss.ReturnedTotalAmount),
                                    TotalAdjustmentAmount = _context.ScheduleShifts
                                                        .Where(ss => ss.ScheduleId == resultJoin.s.ScheduleId)
                                                        .Sum(ss => ss.AdjustmentAmount),
                                    TotalClosingCashAmount = _context.ScheduleShifts
                                                        .Where(ss => ss.ScheduleId == resultJoin.s.ScheduleId)
                                                        .Sum(ss => ss.ClosingCashAmount),
                                    ActualCash = _context.ScheduleShifts
                                                        .Where(ss => ss.ScheduleId == resultJoin.s.ScheduleId)
                                                        .Sum(ss => ss.OpeningCashAmount + ss.ReceivedTotalAmount -                                  ss.ReturnedTotalAmount - ss.AdjustmentAmount),
                                    Difference = _context.ScheduleShifts
                                                        .Where(ss => ss.ScheduleId == resultJoin.s.ScheduleId)
                                                        .Sum(ss => ss.ClosingCashAmount - (ss.OpeningCashAmount + ss.ReceivedTotalAmount -                                  ss.ReturnedTotalAmount - ss.AdjustmentAmount))
                                })
                                .ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = e.Message });
            }
        }

        
        [HttpPost("get-detail-total-sale-schedule")]
        public async Task<IActionResult> GetDetailTotalSaleSchedule([FromBody] ScheduleIdRequest request)
        {
            if (request == null || request.ScheduleId < 1)
            {
                return BadRequest("Không tìm thấy ca làm việc phù hợp!");
            }
            try
            {
                var result = await _context.ScheduleShifts.Where(ss => ss.ScheduleId == request.ScheduleId).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                });
            }
        }
    }
}
