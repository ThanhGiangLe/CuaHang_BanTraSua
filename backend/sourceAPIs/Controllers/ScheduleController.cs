using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sourceAPI.ModelsRequest;
using System.Security.Claims;
using testVue.Datas;
using testVue.Models;

namespace sourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("get-schedule-by-userid")]
        public async Task<IActionResult> GetScheduleByUserId([FromBody] UserIdRequest request)
        {
            if (request.UserId <= 0)
            {
                return BadRequest("Dữ liệu truyền không hợp lệ");
            }

            try
            {
                var schedules = await _context.Schedules
                    .AsNoTracking()
                    .Where(row => row.UserId == request.UserId)
                    .ToListAsync();

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;

                int daysInCurrentMonth = DateTime.DaysInMonth(year, month);
                if (schedules == null || schedules.Count == 0)
                {
                    var defautlSchedule = Enumerable.Range(1, daysInCurrentMonth).
                    Select((item) => new
                    {
                        Day = item,
                        ShiftCode = "O"
                    }).ToList();
                    return Ok(defautlSchedule);
                }else
                {
                    var result = schedules
                    .Select((item, index) => new
                    {
                        Day = index + 1,
                        ShiftCode = item.ShiftId
                    }).ToList();
                    if (result.Count < daysInCurrentMonth)
                    {
                        int days = daysInCurrentMonth - result.Count;
                        for(int i = 1; i <= days; i++)
                        {
                            result.Add(new
                            {
                                Day = result.Count + i,
                                ShiftCode = "O"
                            });
                        }
                    }
                    return Ok(result);
                }
            }
            catch (Exception ex){
                return StatusCode(500, $"Đã xảy ra lỗi khi xử lý yêu cầu: {ex.Message}");
            }
        }

        [HttpPost("update-schedule-by-userid")]
        public async Task<IActionResult> UpdateScheduleByUserId([FromBody] ScheduleInfoRequest request)
        {
            if(request == null || request.Schedules == null || request.Schedules.Count == 0)
            {
                return BadRequest("Lịch làm việc cập nhật không hợp lệ!");
            }
            try
            {
                var currentTime = DateTime.UtcNow.AddHours(7);
                foreach(var schedule in request.Schedules)
                {
                    if (schedule.Day > 0 && schedule.Day <= DateTime.DaysInMonth(request.Year, request.Month))
                    {
                        var scheduleDate = new DateTime(request.Year, request.Month, schedule.Day);
                        var existingSchedule = await _context.Schedules.FirstOrDefaultAsync(row => row.UserId == request.UserId && row.Date.Date == scheduleDate.Date);

                        if (existingSchedule != null)
                        {
                            if (existingSchedule.ShiftId != schedule.ShiftCode)
                            {
                                existingSchedule.ShiftId = schedule.ShiftCode;
                                existingSchedule.UpdateDate = currentTime;
                                existingSchedule.UpdateBy = request.UpdateBy;
                            }
                        }
                        else
                        {
                            var newSchedule = new ScheduleMdl
                            {
                                UserId = request.UserId,
                                Date = scheduleDate.Date,
                                ShiftId = schedule.ShiftCode,
                                CreateDate = currentTime,
                                CreateBy = request.UpdateBy,
                                UpdateDate = currentTime,
                                UpdateBy = request.UpdateBy,
                                BankAmount = 0,
                                CashAmount = 0,
                                ClosingCashAmount = 0,
                            };
                            _context.Schedules.Add(newSchedule);
                        }
                    }
                    else
                    {
                        continue;
                    }
                    
                }
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Cập nhật lịch làm việc thành công"
                });
            }catch(Exception e)
            {
                return StatusCode(500, $"Đã xảy ra lỗi khi xử lý yêu cầu: {e.Message}");
            }
        }


        [Authorize]
        [HttpPost("swap-schedule-shift")]
        public async Task<IActionResult> SwapScheduleShift([FromBody] SwapScheduleShiftRequest request)
        {
            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken == "Customer" || roleFromToken == "Staff")
            {
                return Forbid("Bearer");
            }

            if (request == null || request.FromUserId < 1 || request.ToUserId < 1)
            {
                return BadRequest("Thông tin truyền đi không hợp lệ!");
            }

            var fromUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.FromUserId);
            var toUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.ToUserId);

            if (fromUser == null || toUser == null)
            {
                return NotFound("Không tim thấy nhân sự!");
            }

            var scheduleDate = new DateTime(request.Year, request.Month, request.Day);

            var scheduleFromUser = await _context.Schedules.FirstOrDefaultAsync(row => row.UserId == fromUser.UserId &&
                row.Date.Date == scheduleDate.Date);
            var scheduleToUser = await _context.Schedules.FirstOrDefaultAsync(row => row.UserId == toUser.UserId &&
                row.Date.Date == scheduleDate.Date);
            if (scheduleFromUser == null || scheduleToUser == null)
            {
                return NotFound("Không tìm thấy lịch làm việc của một trong hai nhân sự.");
            }

            try
            {
                var temp = scheduleFromUser.ShiftId;
                scheduleFromUser.ShiftId = scheduleToUser.ShiftId;
                scheduleToUser.ShiftId = temp;

                _context.Schedules.Update(scheduleFromUser);
                _context.Schedules.Update(scheduleToUser);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = 1,
                    data = new
                    {
                        FromUserId = scheduleFromUser.UserId,
                        NewShiftCodeFromUser = scheduleFromUser.ShiftId,
                        ToUserId = scheduleToUser.UserId,
                        NewShiftCodeToUser = scheduleToUser.ShiftId
                    }
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Đã xảy ra lỗi khi xử lý yêu cầu: {e.Message}");
            }
        }
    }
}
