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

        [HttpGet("get-all-table")]
        public async Task<ActionResult<IEnumerable<TableMdl>>> GetAllTable()
        {
            return await _context.Tables.Where(table => table.TableId != 1).ToListAsync();
        }
    }
}
