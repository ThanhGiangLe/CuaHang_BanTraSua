using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sourceAPI.Models;
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

        [HttpGet("get-all-schedule")]
        public async Task<ActionResult<IEnumerable<ScheduleMdl>>> GetAllSchedule()
        {
            try
            {
                var schedules = await _context.Schedules.ToListAsync();
                return Ok(new { success = 1, data = schedules });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = -1,
                    message = "Lỗi khi lấy danh sách danh mục.",
                    details = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
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
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int daysInCurrentMonth = DateTime.DaysInMonth(year, month);
                var currentTime = DateTime.Now;

                var schedules = await _context.Schedules
                    .AsNoTracking()
                    .Where(row => row.UserId == request.UserId && row.Date.Month == month)
                    .ToListAsync();

                if (schedules == null || schedules.Count == 0)
                {
                    var defautlSchedule = Enumerable.Range(1, daysInCurrentMonth)
                    .Select((item) => new
                    {
                        Day = item,
                        ShiftCode = "O"
                    }).ToList();
                    foreach (var schedule in defautlSchedule) {
                        var scheduleDate = new DateTime(year, month, schedule.Day);
                        var newSchedule = new ScheduleMdl
                        {
                            UserId = request.UserId,
                            ShiftId = schedule.ShiftCode,
                            Date = scheduleDate,
                            CreateBy = "Auto",
                            CreateDate = currentTime,
                            UpdateBy = "Auto",
                            UpdateDate = currentTime,
                        };
                        var newScheduleHistory = new ScheduleHistoryMdl
                        {
                            UserId = request.UserId,
                            Date = scheduleDate,
                            OldShiftId = schedule.ShiftCode,
                            ChangedBy = "Auto",
                            ChangedAt = currentTime
                        };
                        _context.Schedules.Add(newSchedule);
                        _context.ScheduleHistories.Add(newScheduleHistory);
                    }
                    await _context.SaveChangesAsync();
                    return Ok(defautlSchedule);
                }else
                {
                    var result = schedules
                    .Select((item, index) => new
                    {
                        Day = index + 1,
                        ShiftCode = item.ShiftId
                    }).ToList();
                    var daysInSchedule = result.Count;
                    if (daysInSchedule < daysInCurrentMonth)
                    {
                        int days = daysInCurrentMonth - daysInSchedule;
                        for(int i = 1; i <= days; i++)
                        {
                            var add = new
                            {
                                Day = daysInSchedule + i,
                                ShiftCode = "O"
                            };
                            result.Add(add);
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
                var currentTime = DateTime.Now;
                var daysInMonth = DateTime.DaysInMonth(request.Year, request.Month);
                foreach (var schedule in request.Schedules)
                {
                    if (schedule.Day > 0 && schedule.Day <= daysInMonth)
                    {
                        var scheduleDate = new DateTime(request.Year, request.Month, schedule.Day);
                        var existingSchedule = await _context.Schedules.FirstOrDefaultAsync(row => row.UserId == request.UserId && row.Date.Date == scheduleDate.Date);
                        var oldShift = existingSchedule?.ShiftId;
                        if (existingSchedule != null)
                        {
                            if (existingSchedule.ShiftId != schedule.ShiftCode)
                            {
                                existingSchedule.ShiftId = schedule.ShiftCode;
                                existingSchedule.UpdateDate = currentTime;
                                existingSchedule.UpdateBy = request.UpdateBy;

                                var newScheduleHistory = new ScheduleHistoryMdl
                                {
                                    UserId = request.UserId,
                                    Date = scheduleDate,
                                    OldShiftId = oldShift,
                                    ChangedBy = request.UpdateBy,
                                    ChangedAt = currentTime
                                };

                                _context.ScheduleHistories.Add(newScheduleHistory);
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
                            };
                            var newScheduleHistory = new ScheduleHistoryMdl
                            {
                                UserId = request.UserId,
                                Date = scheduleDate,
                                OldShiftId = schedule.ShiftCode,
                                ChangedBy = "Auto",
                                ChangedAt = currentTime
                            };
                            _context.Schedules.Add(newSchedule);
                            _context.ScheduleHistories.Add(newScheduleHistory);
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
            if (roleFromToken == "Nhân viên" || roleFromToken == "Khách hàng")
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
            var currentTime = DateTime.Now;

            var scheduleFromUser = await _context.Schedules.FirstOrDefaultAsync(row => row.UserId == fromUser.UserId &&
                row.Date.Date == scheduleDate.Date);
            var scheduleToUser = await _context.Schedules.FirstOrDefaultAsync(row => row.UserId == toUser.UserId &&
                row.Date.Date == scheduleDate.Date);

            var oldShiftFrom = scheduleFromUser?.ShiftId ?? "O";
            var oldShiftTo = scheduleToUser?.ShiftId ?? "O";

            if (scheduleFromUser == null || scheduleToUser == null)
            {
                return NotFound("Không tìm thấy lịch làm việc của một trong hai nhân sự.");
            }

            try
            {
                scheduleFromUser.ShiftId = oldShiftTo;
                scheduleToUser.ShiftId = oldShiftFrom;

                var newScheduleHistoryFromUser = new ScheduleHistoryMdl
                {
                    UserId = fromUser.UserId,
                    Date = scheduleDate,
                    OldShiftId = oldShiftFrom,
                    ChangedBy = request.UpdateBy,
                    ChangedAt = currentTime
                };
                var newScheduleHistoryToUser = new ScheduleHistoryMdl
                {
                    UserId = toUser.UserId,
                    Date = scheduleDate,
                    OldShiftId = oldShiftTo,
                    ChangedBy = request.UpdateBy,
                    ChangedAt = currentTime
                };
                _context.ScheduleHistories.Add(newScheduleHistoryFromUser);
                _context.ScheduleHistories.Add(newScheduleHistoryToUser);
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
        [HttpPost("open-shift")]
        public async Task<IActionResult> OpenShift([FromBody] OpenShiftAmountRequest request)
        {
            if (request == null || request.OpeningCashAmount < 0 || request.UserId < 1)
            {
                return BadRequest("Dữ liệu không hợp lệ!");
            }
            var currentDay = DateTime.Now;
            var scheduleOfDay = _context.Schedules.FirstOrDefault(row => row.UserId == request.UserId && row.Date.Date == currentDay.Date);
            if (scheduleOfDay == null || scheduleOfDay.ShiftId == "O")
            {
                return NotFound("Hãy đăng ký lịch làm việc khác ngày nghỉ trước khi mở ca!");
            }
            try
            {
                var newScheduleShift = new ScheduleShiftMdl
                {
                    ScheduleId = scheduleOfDay.ScheduleId,
                    StartTime = currentDay,
                    OpeningCashAmount = request.OpeningCashAmount
                };
                _context.ScheduleShifts.Add(newScheduleShift);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = 1,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                });
            }
        }
        [HttpPost("close-shift")]
        public async Task<IActionResult> CloseShift([FromBody] CloseShiftAmountRequest request)
        {
            if (request == null || request.ClosingCashAmount < 0 || request.UserId < 1)
            {
                return BadRequest("Dữ liệu không hợp lệ!");
            }
            var currentDay = DateTime.Now;
            var scheduleOfDay = await _context.Schedules.FirstOrDefaultAsync(row => row.UserId == request.UserId && row.Date.Date == currentDay.Date);
            if (scheduleOfDay == null)
            {
                return NotFound("Hãy đăng ký lịch làm việc trước khi thao tác");
            }
            try
            {
                var scheduleShift = await _context.ScheduleShifts.OrderByDescending(row => row.StartTime).FirstOrDefaultAsync(row => row.ScheduleId == scheduleOfDay.ScheduleId);
                if (scheduleShift == null)
                {
                    return NotFound("Không tìm thấy phiên mở ca của bạn!");
                }
                scheduleShift.ClosingCashAmount = request.ClosingCashAmount;
                scheduleShift.AdjustmentAmount = request.AdjustmentAmount;
                scheduleShift.AdjustmentReason = request.AdjustmentReason;
                scheduleShift.EndTime = currentDay; 

                var actualShift = scheduleShift.OpeningCashAmount + scheduleShift.ReceivedTotalAmount - scheduleShift.ReturnedTotalAmount - scheduleShift.AdjustmentAmount;
                var difference = request.ClosingCashAmount - actualShift;

                _context.Update(scheduleOfDay);
                await _context.SaveChangesAsync();

                if (difference == 0)
                {
                    return Ok(new
                    {
                        success = 0,
                    });
                }
                else if (difference < 0)
                {
                    return Ok(new
                    {
                        success = -1,
                        difference,
                    });
                }else
                {
                    return Ok(new
                    {
                        success = 1,
                        difference,
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                });
            }
        }

        [HttpPost("get-schedule-by-userid-today")]
        public async Task<IActionResult> GetScheduleByUserIdToday([FromBody] UserIdRequest request)
        {
            if(request == null || request.UserId < 1)
            {
                return BadRequest("UserId không hợp lệ");
            }
            try
            {
                var currentDate = DateTime.Now;
                var scheduleDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
                var schedule = await _context.Schedules.FirstOrDefaultAsync(row => row.UserId == request.UserId && row.Date.Date == scheduleDate.Date);
                if(schedule == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(new
                    {
                        success = 1,
                        data = schedule,
                    });
                }
            }
            catch (Exception ex) {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("get-all-schedule-history")]
        public async Task<IActionResult> GetAllScheduleHistory()
        {
            try
            {
                var ScheduleHistories = await _context.ScheduleHistories
                                                    .Join(
                                                        _context.Users,
                                                        sh => sh.UserId,
                                                        u => u.UserId,
                                                        (sh, u) => new { sh, u }
                                                    )
                                                    .Select(resultJoin => new {
                                                        FullName = resultJoin.u.FullName,
                                                        Date = resultJoin.sh.Date,
                                                        OldShiftId = resultJoin.sh.OldShiftId,
                                                        ChangedBy = resultJoin.sh.ChangedBy,
                                                        ChangedAt = resultJoin.sh.ChangedAt,
                                                        UserId = resultJoin.sh.UserId
                                                    }).ToListAsync();
                return Ok(new { success = 1, data = ScheduleHistories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = -1,
                    message = "Lỗi khi lấy danh sách danh mục.",
                    details = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
    }
}
