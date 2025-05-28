using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testVue.Datas;
using testVue.Models;
using testVue.Models.Food;
using testVue.ModelsRequest;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/food/get-all-category
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

        // GET: api/food/get-all-food-items
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

        // POST: api/food/add-order
        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequestDTO orderRequest)
        {
            if (orderRequest == null)
            {
                return Ok(new { success = -1 });
            }
            var order = new OrderMdl
            {
                UserId = orderRequest.UserId,
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
                    details = ex.InnerException?.Message
                });
            }
        }

        // POST: api/food/add-order
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

        [HttpPost("add-food-item")]
        public async Task<IActionResult> AddFoodItem([FromBody] RequestFoodItemAddDTO request)
        {
            // Kiểm tra dữ liệu đầu vào (Validation)
            if (request == null)
            {
                return BadRequest("Invalid food item data.");
            }

            var currentTime = DateTime.UtcNow;

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

            // Tìm FoodItem trong cơ sở dữ liệu
            var existingFoodItem = await _context.FoodItems.FindAsync(FoodItemId);
            if (existingFoodItem == null)
            {
                return NotFound(new { success = -1, message = "Món ăn không tồn tại." });
            }

            var user = await _context.Users.FindAsync(updatedFoodItem.UserId);
            if (user == null)
            {
                return NotFound(new { success = -1, message = "Người dùng không tồn tại." });
            }

            // Cập nhật thông tin món ăn
            existingFoodItem.FoodName = updatedFoodItem.FoodName ?? existingFoodItem.FoodName;
            existingFoodItem.PriceListed = updatedFoodItem.PriceListed > 0 ? updatedFoodItem.PriceListed : existingFoodItem.PriceListed;
            existingFoodItem.PriceCustom = updatedFoodItem.PriceCustom ?? existingFoodItem.PriceCustom;
            existingFoodItem.ImageUrl = updatedFoodItem.ImageUrl ?? existingFoodItem.ImageUrl;
            existingFoodItem.Unit = updatedFoodItem.Unit ?? existingFoodItem.Unit;
            existingFoodItem.CategoryId = updatedFoodItem.CategoryId ?? existingFoodItem.CategoryId;
            existingFoodItem.Status = updatedFoodItem.Status ?? existingFoodItem.Status;
            existingFoodItem.UpdateDate = DateTime.Now;
            existingFoodItem.UpdateBy = user.FullName ?? "Admin";
            existingFoodItem.IsMain = updatedFoodItem.IsMain > 0 ? updatedFoodItem.IsMain : existingFoodItem.IsMain;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
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
