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
                var currentDay = DateTime.UtcNow.Day;
                var query = _context.Users.Where(u => u.Role != "Customer");

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

        [HttpPost("get-total-sales-on-day")]
        public async Task<IActionResult> GetTotalSales([FromBody] GetTotalSalesRequest request)
        {
            try
            {
                var currentDay = DateTime.UtcNow.Date;

                var query = from u in _context.Users
                            where u.Role != "Customer" && (request.UserId == 0 || u.UserId == request.UserId)
                            join s in _context.Schedules on u.UserId equals s.UserId
                            join ss in _context.ScheduleShifts on s.ScheduleId equals ss.ScheduleId
                            where request.Day == default(DateTime) || s.Date.Date == request.Day.Date && s.ShiftId != "O"
                            select new
                            {
                                s.ScheduleId,
                                u.UserId,
                                u.FullName,
                                s.Date,
                                s.ShiftId,
                                ss.OpeningCashAmount,
                                ss.ReceivedTotalAmount,
                                ss.ReturnedTotalAmount,
                                ss.AdjustmentAmount,
                                ss.ClosingCashAmount
                            };

                var result = await query
                    .GroupBy(x => new { x.UserId, x.FullName, x.Date.Date })
                    .Select(group => new
                    {
                        group.Key.FullName,
                        Date = group.Key.Date,
                        ShiftIds = string.Join(", ", group.Select(x => x.ShiftId).Distinct()),
                        TotalOpening = group.Sum(x => x.OpeningCashAmount),
                        TotalReceived = group.Sum(x => x.ReceivedTotalAmount),
                        TotalReturned = group.Sum(x => x.ReturnedTotalAmount),
                        TotalAdjustment = group.Sum(x => x.AdjustmentAmount),
                        TotalClosing = group.Sum(x => x.ClosingCashAmount),
                        Actual = group.Sum(x => x.OpeningCashAmount + x.ReceivedTotalAmount - x.ReturnedTotalAmount - x.AdjustmentAmount),
                        Difference = group.Sum(x => x.OpeningCashAmount + x.ReceivedTotalAmount - x.ReturnedTotalAmount - x.AdjustmentAmount - x.ClosingCashAmount)
                    })
                    .OrderByDescending(x => x.Date)
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
