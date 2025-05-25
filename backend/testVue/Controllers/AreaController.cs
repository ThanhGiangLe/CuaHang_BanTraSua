using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testVue.Datas;
using testVue.Models;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AreaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/area/get-all-table
        [HttpGet("get-all-table")]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllTable()
        {
            return await _context.Tables.ToListAsync();
        }
    }
}
