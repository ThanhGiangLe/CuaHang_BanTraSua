using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using testVue.Datas;
using testVue.Models;
using testVue.Models.Food;
using testVue.ModelsRequest;

namespace testVue.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-category")]
        public async Task<ActionResult<IEnumerable<FoodCategoryMdl>>> GetCategorys()
        {
            // Lấy danh sách người dùng từ cơ sở dữ liệu
            try
            {
                var categories = await _context.FoodCategorys.ToListAsync();
                return Ok(new { success = 1, data = categories });
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

        [HttpGet("get-all-food-items")]
        public async Task<ActionResult<IEnumerable<FoodItemMdl>>> GetFoodItems()
        {
            // Lấy danh sách người dùng từ cơ sở dữ liệu
            try
            {
                var foodItems = await _context.FoodItems.ToListAsync();
                return Ok(new { success = 1, data = foodItems });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = -1,
                    message = "Lỗi khi lấy danh sách món ăn.",
                    details = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [Authorize]
        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequestDTO orderRequest)
        {
            if (orderRequest == null)
            {
                return Ok(new { success = -1 });
            }
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdFromToken == null || userIdFromToken != orderRequest.UserId.ToString())
            {
                return Unauthorized(new { message = "Vui lòng đăng nhập lại để thực hiện thao tác!" });
            }
            if (!int.TryParse(userIdFromToken, out var userIdTryParse))
            {
                return Unauthorized(new { message = "Token không hợp lệ" });
            }
            var order = new OrderMdl
            {
                UserId = userIdTryParse,
                OrderTime = orderRequest.OrderTime,
                TableId = orderRequest.TableId,
                TotalAmount = orderRequest.TotalAmount,
                TotalResult = orderRequest.TotalResult,
                Status = orderRequest.Status,
                Discount = orderRequest.Discount,
                Tax = orderRequest.Tax,
                PaymentMethod = orderRequest.PaymentMethod
            };
            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = 1,
                    data = new
                    {
                        order.OrderId, // Trả về OrderId tự tăng
                        order.UserId,
                        order.OrderTime,
                        order.TableId,
                        order.TotalAmount,
                        order.Status,
                        order.Discount,
                        order.Tax,
                        order.PaymentMethod
                    }
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi lưu vào cơ sở dữ liệu
                return StatusCode(500, new
                {
                    success = -1,
                    message = ex.Message,
                });
            }
        }

        [HttpPost("add-order-detail")]
        public async Task<IActionResult> AddOrderDetail([FromBody] OrderDetailRequestDTO orderItemRequest)
        {
            if (orderItemRequest == null)
            {
                return BadRequest(new { success = -1, message = "Request không hợp lệ" });
            }
            var orderItem = new OrderDetailMdl
            {
                OrderId = orderItemRequest.OrderId,
                FoodItemId = orderItemRequest.FoodItemId,
                Quantity = orderItemRequest.Quantity,
                Price = orderItemRequest.Price,
                IsMainItem = orderItemRequest.IsMainItem,
                Unit = orderItemRequest.Unit,
                Note = orderItemRequest.Note,
                CategoryId = orderItemRequest.CategoryId,
                OrderTime = orderItemRequest.OrderTime,
            };
            _context.OrderDetails.Add(orderItem);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = 1,
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi lưu vào cơ sở dữ liệu
                return StatusCode(500, new
                {
                    success = -1,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }



        [Authorize]
        [HttpPost("add-food-item")]
        public async Task<IActionResult> AddFoodItem([FromBody] RequestFoodItemAddDTO request)
        {
            // Kiểm tra dữ liệu đầu vào (Validation)
            if (request == null)
            {
                return BadRequest("Invalid food item data.");
            }
            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken == "Customer" || roleFromToken == "Staff")
            {
                return Unauthorized(new { message = "Bạn không có quyền thao tác chức năng này!" });
            }

            var currentTime = DateTime.UtcNow.AddHours(7);

            // Tạo một đối tượng FoodItem từ RequestFoodItemAdd
            var foodItem = new FoodItemMdl
            {
                FoodName = request.FoodName,
                PriceListed = request.PriceListed,
                PriceCustom = request.PriceCustom,
                ImageUrl = request.ImageUrl,
                Unit = request.Unit ?? "Ly", // Nếu Unit không có, mặc định là "phần"
                CategoryId = request.CategoryId,
                Status = "Available", // Mặc định là "available"
                CreateDate = currentTime,
                CreateBy = request.CreateBy,
                UpdateDate = currentTime,
                UpdateBy = request.UpdateBy,
                IsMain = request.IsMain,
            };

            try
            {
                // Thêm FoodItem vào cơ sở dữ liệu
                _context.FoodItems.Add(foodItem);
                await _context.SaveChangesAsync();

                // Trả về ID của món ăn vừa thêm
                return Ok(new
                {
                    success = true,
                    categoryId = foodItem.CategoryId,
                    foodItemId = foodItem.FoodItemId,
                    foodName = foodItem.FoodName,
                    imageUrl = foodItem.ImageUrl,
                    priceCustom = foodItem.PriceCustom,
                    priceListed = foodItem.PriceListed,
                    status = foodItem.Status,
                    unit = foodItem.Unit,
                    createDate = foodItem.CreateDate,
                    createBy = foodItem.CreateBy,
                    updateDate = foodItem.UpdateDate,
                    updateBy = foodItem.UpdateBy,
                    isMain = foodItem.IsMain,
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/food/delete-food-item/{id}
        [HttpDelete("delete-food-item/{FoodItemId}")]
        public async Task<IActionResult> DeleteFoodItem(int FoodItemId)
        {
            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken == "Customer" || roleFromToken == "Staff")
            {
                return Unauthorized(new { message = "Bạn không có quyền thao tác chức năng này!" });
            }
            // Tìm món ăn theo ID trong cơ sở dữ liệu
            var foodItem = await _context.FoodItems.FindAsync(FoodItemId);

            if (foodItem == null)
            {
                // Nếu không tìm thấy món ăn với ID này, trả về lỗi 404 (Not Found)
                return NotFound(new { success = -1, message = "Food item not found." });
            }

            // Xóa món ăn từ cơ sở dữ liệu
            _context.FoodItems.Remove(foodItem);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                // Trả về phản hồi thành công
                return Ok(new { success = 1, message = "Food item deleted successfully." });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, new
                {
                    success = -1,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }


        // PUT: api/food/update-food-item/{FoodItemId}
        [HttpPut("update-food-item/{FoodItemId}")]
        public async Task<IActionResult> UpdateFoodItem(int FoodItemId, [FromBody] RequestUpdateFoodItemDTO updatedFoodItem)
        {
            // Kiểm tra dữ liệu đầu vào
            if (updatedFoodItem == null || FoodItemId != updatedFoodItem.FoodItemId)
            {
                return BadRequest(new { success = -1, message = "Dữ liệu yêu cầu không hợp lệ hoặc ID không khớp." });
            }
            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken == "Customer" || roleFromToken == "Staff")
            {
                return Unauthorized(new { message = "Bạn không có quyền thao tác chức năng này!" });
            }
            var currentTime = DateTime.UtcNow.AddHours(7);
            // Tìm FoodItem trong cơ sở dữ liệu
            var existingFoodItem = await _context.FoodItems.FindAsync(FoodItemId);
            if (existingFoodItem == null)
            {
                return NotFound(new { success = -1, message = "Món ăn không tồn tại." });
            }
            var excludedIds = new List<int> { 0, 1 };
            // Cập nhật thông tin món ăn
            existingFoodItem.FoodName = updatedFoodItem.FoodName ?? existingFoodItem.FoodName;
            existingFoodItem.PriceListed = updatedFoodItem.PriceListed > 0 ? updatedFoodItem.PriceListed : existingFoodItem.PriceListed;
            existingFoodItem.PriceCustom = updatedFoodItem.PriceCustom ?? existingFoodItem.PriceCustom;
            existingFoodItem.ImageUrl = updatedFoodItem.ImageUrl ?? existingFoodItem.ImageUrl;
            existingFoodItem.Unit = updatedFoodItem.Unit ?? existingFoodItem.Unit;
            existingFoodItem.CategoryId = updatedFoodItem.CategoryId ?? existingFoodItem.CategoryId;
            existingFoodItem.Status = updatedFoodItem.Status ?? existingFoodItem.Status;
            existingFoodItem.UpdateDate = currentTime;
            existingFoodItem.UpdateBy = updatedFoodItem.UpdateBy ?? "Admin";
            existingFoodItem.IsMain = excludedIds.Contains(updatedFoodItem.IsMain) ? updatedFoodItem.IsMain : existingFoodItem.IsMain;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                _context.FoodItems.Update(existingFoodItem);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = 1,
                    message = "Cập nhật món ăn thành công.",
                    data = new
                    {
                        existingFoodItem.FoodItemId,
                        existingFoodItem.FoodName,
                        existingFoodItem.PriceListed,
                        existingFoodItem.PriceCustom,
                        existingFoodItem.Unit,
                        existingFoodItem.CategoryId,
                        existingFoodItem.Status,
                        existingFoodItem.UpdateDate,
                        existingFoodItem.UpdateBy,
                        existingFoodItem.IsMain
                    }
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return StatusCode(500, new
                {
                    success = -1,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }
    }
}
