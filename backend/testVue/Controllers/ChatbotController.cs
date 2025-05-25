using Microsoft.AspNetCore.Mvc;
using testVue.Datas;
using testVue.Models.ChatBot;
using testVue.Models.ChatBot.Models;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ChatbotController(AppDbContext appDbContext) { 
            _appDbContext = appDbContext;
        }

        [HttpPost("import-data-training")]
        public async Task<IActionResult> ImportDataTraining([FromBody] DataTrainingRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content) || string.IsNullOrWhiteSpace(request.Response))
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            try
            {
                var item = new AImodel
                {
                    CreateDate = DateTime.UtcNow,
                    Content = request.Content.Trim(),
                    Response = request.Response.Trim(),
                };

                await _appDbContext.AImodels.AddAsync(item);
                await _appDbContext.SaveChangesAsync();

                return Ok(new { message = "Success", id = item.ChatBotId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi server", details = ex.Message });
            }
        }

    }
}
