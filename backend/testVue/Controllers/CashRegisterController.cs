using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testVue.Datas;
using testVue.Models;
using testVue.ModelsRequest;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashRegisterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CashRegisterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-cash-register")]
        public async Task<ActionResult<IEnumerable<CashRegisterDTO>>> GetAllCashRegisters()
        {
            var result = await (
                from cr in _context.CashRegisters
                join u in _context.Users on cr.UserId equals u.UserId
                select new 
                {
                    CashRegisterId = cr.CashRegisterId,
                    UserId = cr.UserId,
                    StartTime = cr.StartTime,
                    EndTime = cr.EndTime,
                    TotalIncome = cr.TotalIncome,
                    UserName = u.FullName,
                }).ToListAsync();

            return Ok(result);
        }

        [HttpPost("insert-cash-register")]
        public async Task<IActionResult> InsertCashRegister([FromBody] CashRegisterRequest request)
        {
            if (request == null) {
                return BadRequest("Dữ liệu rỗng.");
            }
            var time = DateTime.Now;
            var model = new CashRegisterDTO
            {
                UserId = request.UserId,
                StartTime = time,
                EndTime = time,
                TotalIncome = 0
            };
            _context.CashRegisters.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = 1,
                data = model
            });
        }
        [HttpPost("update-totalimcome-cash-register")]
        public async Task<IActionResult> UpdateTotalIncomeCashRegister([FromBody] CashRegisterUpdateTotalAmountRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dữ liệu rỗng.");
            }

            var user = await _context.CashRegisters.OrderByDescending(x => x.CashRegisterId).FirstOrDefaultAsync(u => u.UserId == request.UserId);
            if (user == null)
            {
                return NotFound("Không tìm thấy user trong bảng CashRegisters!");
            }
            user.TotalIncome += request.TotalAmount;

            _context.CashRegisters.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = 1,
                data = user
            });
        }

        [HttpPost("update-endtime-cash-register")]
        public async Task<IActionResult> UpdateEndTimeCashRegister([FromBody] CashRegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dữ liệu rỗng.");
            }
            var time = DateTime.Now;
            
            var user = await _context.CashRegisters.OrderByDescending(x => x.CashRegisterId).FirstOrDefaultAsync(u => u.UserId == request.UserId);
            if (user == null)
            {
                return NotFound("Không tìm thấy user trong bảng CashRegisters!");
            }
            user.EndTime = time;

            _context.CashRegisters.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = 1,
                data = user
            });
        }
    }
}
