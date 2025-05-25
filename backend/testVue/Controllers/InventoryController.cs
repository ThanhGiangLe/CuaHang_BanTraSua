using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testVue.Datas;
using testVue.Models;
using testVue.Models.Inventory.Inventory;
using testVue.ModelsRequest;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public InventoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("get-all-material")]
        public async Task<ActionResult<IEnumerable<MaterialDTO>>> GetAllMaterial()
        {
            return await _appDbContext.Materials.ToListAsync();
        }

        [HttpPost("update-quantity-material")]
        public async Task<IActionResult> UpdateQuantityMaterial([FromBody] UpdateQuantityMaterialDTO request)
        {
            if (request == null || request.AddedQuantity < 0) {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var material = await _appDbContext.Materials.FindAsync(request.MaterialId);
            if(material == null)
            {
                return NotFound("Không tìm thấy nguyên liệu.");
            }

            var now = DateTime.Now;
            if(material.MaterialType.ToLower().Equals("thực phẩm"))
            {
                material.ImportDate = now;
                material.ExpirationDate = now.AddDays(4);
            }else if(material.MaterialType.ToLower().Equals("nguyên liệu"))
            {
                material.ImportDate = now;
                material.ExpirationDate = now.AddDays(15);
            }
            else if(material.MaterialType.ToLower().Equals("gia vị")) {
                material.ImportDate = now;
                material.ExpirationDate = now.AddDays(30);
            }else
            {
                material.ImportDate = now;
                material.ExpirationDate = now.AddDays(1);
            }

            material.Quantity += request.AddedQuantity;

            // Cập nhật vào database
            _appDbContext.Materials.Update(material);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Cập nhật số lượng thành công!",
                UpdatedQuantity = material.Quantity,
                ImportDateRes = material.ImportDate,
                ExpirationDateRes = material.ExpirationDate,
            });
        }

        [HttpPost("cancel-all-goods")]
        public async Task<IActionResult> CancelAllGoods([FromBody] CancelAllGoodsRequest request)
        {
            if (request.MaterialId == 0)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            var material = await _appDbContext.Materials.FindAsync(request.MaterialId);
            if(material == null)
            {
                return NotFound("Không tìm thấy nguyên liệu.");
            }
            material.Quantity -= request.Quantity;
            var now = DateTime.Now;
            material.ImportDate = now;
            material.ExpirationDate = now;

            _appDbContext.Materials.Update(material);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Hủy bỏ nguyên liệu thành công",
                Id = material.MaterialId,
                UpdateQuantity = material.Quantity,
                ImportDateRes = material.ImportDate,
                ExpirationDateRes = material.ExpirationDate,
            });
        }

        [HttpPost("cancel-all-goods-detail")]
        public async Task<IActionResult> CancelAllGoodsDetail([FromBody] CancelAllGoodsRequestDetail request)
        {
            if (request == null || request.MaterialId == 0)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            var result = await _appDbContext.WarehouseHistorys
                .Where(item =>
                    item.MaterialId == request.MaterialId &&
                    EF.Functions.DateDiffSecond(item.ImportDate, request.ImportDate) == 0 &&
                    EF.Functions.DateDiffSecond(item.ExpirationDate, request.ExpirationDate) == 0
                ).FirstOrDefaultAsync();
            var material = await _appDbContext.Materials.FindAsync(request.MaterialId);
            if (result == null || material == null)
            {
                return NotFound("Không tìm thấy nguyên liệu.");
            }

            material.Quantity -= request.Quantity;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Remove(result);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Hủy bỏ nguyên liệu thành công",
            });
        }

        [HttpPost("import-warehouse-history")]
        public async Task<IActionResult> ImportWarehouseHistory([FromBody] ImportWarehouseRequest request)
        {
            try
            {
                if (request == null || request.MaterialId <= 0)
                {
                    return BadRequest("Dữ liệu không hợp lệ");
                }

                var material = _appDbContext?.Materials?.Find(request.MaterialId);
                if (material == null)
                {
                    return NotFound("Không tìm thấy nguyên liệu được nhập");
                }

                var now = DateTime.Now;
                var expirationDate = material.MaterialType.ToLower() switch
                {
                    "thực phẩm" => now.AddDays(4),
                    "nguyên liệu" => now.AddDays(15),
                    "gia vị" => now.AddDays(30),
                    _ => now.AddDays(10)
                };

                await _appDbContext.WarehouseHistorys
                    .Where(item => item.MaterialId == request.MaterialId && item.Using == 1)
                    .ForEachAsync(item => item.Using = 0);

                var newWarehouseHistory = new WarehouseHistoryDTO
                {
                    MaterialId = request.MaterialId,
                    ImportDate = now,
                    ExpirationDate = expirationDate, // Fix lỗi chính tả
                    Unit = request.Unit,
                    ImportPrice = request.ImportPrice,
                    Quantity = request.Quantity,
                    CurrentQuantity = request.Quantity,
                    Using = 1
                };

                _appDbContext.WarehouseHistorys.Add(newWarehouseHistory);
                await _appDbContext.SaveChangesAsync();

                return Ok(new { message = "Nhập kho thành công!", data = newWarehouseHistory });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        [HttpGet("get-warehouse-history/{materialId}")]
        public async Task<IActionResult> GetWarehouseHistoryByMaterialId(int materialId)
        {
            if (materialId <= 0) { 
                return BadRequest("Yêu cầu không hợp lệ");
            }
            var results = await (from wh in _appDbContext.WarehouseHistorys
                                 join m in _appDbContext.Materials on wh.MaterialId equals m.MaterialId
                                 where wh.MaterialId == materialId
                                 orderby wh.ImportDate descending   
                                 select new
                                 {
                                     wh.MaterialId,
                                     MaterialName = m.MaterialName,
                                     wh.Quantity,
                                     wh.CurrentQuantity,
                                     wh.Unit,
                                     wh.ImportDate,
                                     wh.ExpirationDate,
                                     wh.ImportPrice,
                                     wh.Using
                                 }).ToListAsync();
            if (results == null || results.Count == 0) {
                return Ok(new
                {
                    message = "Không tìm thấy nguyên liệu"
                });
            }
            return Ok(results);
        }
    }
}
