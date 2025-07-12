using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sourceAPI.ModelsRequest;
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
            var fullNameFromToken = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userIdFromToken == null || userIdFromToken != orderRequest.UserId.ToString())
            {
                return Forbid("Bearer");
            }
            if (!int.TryParse(userIdFromToken, out var userIdTryParse))
            {
                return Forbid("Bearer");
            }

            var currentDay = DateTime.Now;
            var scheduleOfDay = _context.Schedules.FirstOrDefault(row => row.UserId == userIdTryParse && row.Date.Date == currentDay.Date);

            if (scheduleOfDay == null)
            {
                return NotFound("Hãy đăng ký lịch làm việc trước khi thao tác");
            }else
            {
                var customer = await _context.Users.FirstOrDefaultAsync(u => u.Phone == orderRequest.Phone);
                var scheduleShiftOfDay = await _context.ScheduleShifts.OrderByDescending(x => x.StartTime).FirstOrDefaultAsync(row => row.ScheduleId == scheduleOfDay.ScheduleId);

                if (scheduleShiftOfDay == null)
                {
                    return NotFound("Không tìm thấy phiên mở ca của bạn. Hãy mở ca trước khi thao tác");
                }else
                {
                    if(scheduleShiftOfDay?.EndTime != null || scheduleShiftOfDay?.ClosingCashAmount != 0)
                    {
                        return NotFound("Hãy mở ca mới trước khi thao tác");
                    }
                }
                if (orderRequest.PaymentMethod.Trim().ToLower() == "tiền mặt")
                {
                    scheduleShiftOfDay.ReceivedTotalAmount += orderRequest.ReceivedAmount;
                    scheduleShiftOfDay.ReturnedTotalAmount += orderRequest.ReturnedAmount;
                    if (customer != null)
                    {
                        customer.Point += orderRequest.TotalResult * 0.1m;
                    }
                }
                else if(orderRequest.PaymentMethod.Trim().ToLower() == "chuyển khoản")
                {
                    if (customer != null)
                    {
                        customer.Point += orderRequest.TotalResult * 0.1m;
                    }
                }
                else
                {
                    if(customer != null)
                    {
                        if (customer.Point < orderRequest.TotalResult)
                        {
                            return Ok(new { success = -20 });
                        }
                        else
                        {
                            customer.Point -= orderRequest.TotalResult;
                        }
                    }else
                    {
                        return Ok(new { success = -26 });
                    }
                }
                var order = new OrderMdl
                {
                    UserId = userIdTryParse,
                    OrderTime = currentDay,
                    TableId = orderRequest.TableId,
                    TotalAmount = orderRequest.TotalAmount,
                    TotalResult = orderRequest.TotalResult,
                    Status = orderRequest.Status,
                    Discount = orderRequest.Discount,
                    Tax = orderRequest.Tax,
                    ReceivedAmount = orderRequest.ReceivedAmount,
                    ReturnedAmount = orderRequest.ReturnedAmount,
                    PaymentMethod = orderRequest.PaymentMethod
                };
                if(customer != null)
                {
                    _context.Users.Update(customer);
                }
                _context.Orders.Add(order);
                _context.Schedules.Update(scheduleOfDay);
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
                            order.ReceivedAmount,
                            order.ReturnedAmount,
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
            if (request == null)
            {
                return BadRequest("Dữ liệu yêu cầu truyền đi không hợp lệ.");
            }
            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken?.ToLower() == "khách hàng" || roleFromToken?.ToLower() == "nhân viên")
            {
                return Forbid("Bearer");
            }

            var currentTime = DateTime.Now;

            var foodItem = new FoodItemMdl
            {
                FoodName = request.FoodName,
                PriceListed = request.PriceListed,
                PriceCustom = request.PriceCustom,
                ImageUrl = request.ImageUrl,
                Unit = request.Unit ?? "Ly", 
                CategoryId = request.CategoryId,
                Status = "Đang kinh doanh", 
                CreateDate = currentTime,
                CreateBy = request.CreateBy,
                UpdateDate = currentTime,
                UpdateBy = request.UpdateBy,
                IsMain = request.IsMain,
                Point = request.Point,
            };

            try
            {
                _context.FoodItems.Add(foodItem);
                await _context.SaveChangesAsync();

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
                    point = foodItem.Point,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi trong quá trình xử lý: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("delete-food-item/{FoodItemId}")]
        public async Task<IActionResult> DeleteFoodItem(int FoodItemId)
        {
            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken?.ToLower() == "khách hàng" || roleFromToken?.ToLower() == "nhân viên")
            {
                return Forbid("Bearer");
            }
            var foodItem = await _context.FoodItems.FindAsync(FoodItemId);
            if (foodItem == null)
            {
                return NotFound(new { success = -1, message = "Không tìm thấy món cần xóa" });
            }

            var listOrderDetails = await _context.OrderDetails.Where(item => item.FoodItemId == FoodItemId).ToListAsync();

            _context.OrderDetails.RemoveRange(listOrderDetails);
            await _context.SaveChangesAsync();


            _context.FoodItems.Remove(foodItem);

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new { success = 1, message = "Xóa món thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = -1,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        [Authorize]
        [HttpPut("update-food-item/{FoodItemId}")]
        public async Task<IActionResult> UpdateFoodItem(int FoodItemId, [FromBody] RequestUpdateFoodItemDTO updatedFoodItem)
        {
            if (updatedFoodItem == null || FoodItemId != updatedFoodItem.FoodItemId)
            {
                return BadRequest(new { success = -1, message = "Dữ liệu yêu cầu không hợp lệ hoặc ID không khớp." });
            }

            var roleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleFromToken?.ToLower() == "nhân viên" || roleFromToken?.ToLower() == "khách hàng")
            {
                return Forbid("Bearer");
            }

            var currentTime = DateTime.Now;
            var existingFoodItem = await _context.FoodItems.FindAsync(FoodItemId);
            if (existingFoodItem == null)
            {
                return NotFound(new { success = -1, message = "Món ăn không tồn tại." });
            }
            var excludedIds = new List<int> { 0, 1 };
            existingFoodItem.FoodName = updatedFoodItem.FoodName ?? existingFoodItem.FoodName;
            existingFoodItem.PriceListed = updatedFoodItem.PriceListed > 0 ? updatedFoodItem.PriceListed : existingFoodItem.PriceListed;
            existingFoodItem.PriceCustom = updatedFoodItem.PriceCustom ?? existingFoodItem.PriceCustom;
            existingFoodItem.ImageUrl = updatedFoodItem.ImageUrl ?? existingFoodItem.ImageUrl;
            existingFoodItem.Unit = updatedFoodItem.Unit ?? existingFoodItem.Unit;
            existingFoodItem.CategoryId = updatedFoodItem.CategoryId ?? existingFoodItem.CategoryId;
            existingFoodItem.Status = updatedFoodItem.Status ?? existingFoodItem.Status;
            existingFoodItem.UpdateDate = currentTime;
            existingFoodItem.UpdateBy = updatedFoodItem.UpdateBy ?? "Quản lý";
            existingFoodItem.IsMain = excludedIds.Contains(updatedFoodItem.IsMain) ? updatedFoodItem.IsMain : existingFoodItem.IsMain;
            existingFoodItem.Point = updatedFoodItem.Point ?? existingFoodItem?.Point;

            try
            {
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
                        existingFoodItem.IsMain,
                        existingFoodItem.Point
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = -1,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("get-all-orderitem-top-bestselling")] 
        public async Task<IActionResult> GetAllOrderItemTopBestSelling([FromBody] TopItemsRequest request)
        {
            try
            {
                var bestSellingItems = await _context.OrderDetails
                    .GroupBy(orderItem => new { orderItem.FoodItemId }) // Nhóm theo FoodItemId và FoodName
                    .Select(g => new
                    {
                        FoodItemId = g.Key.FoodItemId,
                        QuantitySold = g.Sum(item => item.Quantity) // Đếm số lượng bán ra
                    })
                    .OrderByDescending(x => x.QuantitySold) // Sắp xếp giảm dần theo số lượng bán ra
                    .Join(_context.FoodItems,
                        o => o.FoodItemId,
                        f => f.FoodItemId,
                        (o, f) => new
                        {
                            f.FoodItemId,
                            f.FoodName,
                            f.IsMain,
                        })
                    .Where(x => x.IsMain == 1)
                    .Take(request.Top > 0 ? request.Top : 5)
                    .ToListAsync();

                // Trả kết quả về client
                return Ok(bestSellingItems);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = ex.Message });
            }
        }

        [HttpPost("addition-point-by-userid")]
        public async Task<IActionResult> AdditionPointByUserId([FromBody] AdditonPointRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == request.Phone);
            if (user == null)
            {
                return Ok(new
                {
                    susseces = -1
                });
            }
            user.Point += request.Point;
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    susseces = 1
                });
            }
            catch (Exception ex) {
                return Ok(new
                {
                    susseces = -1
                });
            }
        }
    }
}
