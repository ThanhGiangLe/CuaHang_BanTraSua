using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sourceAPI.ModelsRequest;
using System.Linq;
using testVue.Datas;
using testVue.Models;
using testVue.ModelsRequest;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // Phần BÁO CÁO THEO DOANH THU
        [HttpGet("total-revenue-employee")] // Không dùng 
        public async Task<IActionResult> GetTotalRevenueByUser()
        {
            var result = await _context.Orders
                .Join(
                    _context.Users,
                    order => order.UserId,
                    user => user.UserId,
                    (order, user) => new { order.UserId, user.FullName, order.TotalAmount }
                )
                .GroupBy(x => new { x.UserId, x.FullName })
                .Select(g => new
                {
                    UserId = g.Key.UserId,
                    FullName = g.Key.FullName,
                    TotalRevenue = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost("total-revenue-employee-time")] // Có dùng
        public async Task<IActionResult> GetTotalRevenueByUserTime([FromBody] TimeRequestDTO request)
        {
            try
            {
                // Kiểm tra nếu `request` hoặc `Date` không được cung cấp hoặc không hợp lệ
                if (request == null || string.IsNullOrWhiteSpace(request.Date) ||
                    !DateTime.TryParseExact(request.Date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDate))
                {
                    return BadRequest(new { error = "Định dạng ngày không hợp lệ. Vui lòng sử dụng định dạng dd-MM-yyyy." });
                }

                // Lấy thời gian bắt đầu và kết thúc của ngày được truyền vào
                DateTime startOfDay = selectedDate.Date; // 00:00:00 của ngày được chọn
                DateTime endOfDay = selectedDate.Date.AddDays(1).AddTicks(-1); // 23:59:59 của ngày được chọn

                // Lọc dữ liệu đơn hàng theo ngày và tính tổng doanh thu theo từng nhân viên
                var result = await _context.Orders
                    .Where(order => order.OrderTime >= startOfDay && order.OrderTime <= endOfDay)
                    .Join(
                        _context.Users,
                        order => order.UserId,
                        user => user.UserId,
                        (order, user) => new { order.UserId, user.FullName, order.TotalResult }
                    )
                    .GroupBy(x => new { x.UserId, x.FullName })
                    .Select(g => new
                    {
                        UserId = g.Key.UserId,
                        FullName = g.Key.FullName,
                        TotalRevenue = g.Sum(x => x.TotalResult)
                    })
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = ex.Message });
            }
        }

        [HttpPost("total-revenue-employee-time-month")] // Có dùng 
        public async Task<IActionResult> GetTotalRevenueByUserTimeMonth([FromBody] TimeRequestDTO request)
        {
            try
            {
                // Kiểm tra nếu `request` hoặc `Date` không được cung cấp hoặc không hợp lệ
                if (request == null || string.IsNullOrWhiteSpace(request.Date) ||
                    !DateTime.TryParseExact(request.Date, "MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime selectedMonth))
                {
                    return BadRequest(new { error = "Định dạng tháng không hợp lệ. Vui lòng sử dụng định dạng MM-yyyy." });
                }

                // Lấy thời gian bắt đầu và kết thúc của tháng được truyền vào
                DateTime startOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1); // Ngày đầu tiên của tháng
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Ngày cuối cùng của tháng

                // Lọc dữ liệu đơn hàng theo tháng và tính tổng doanh thu theo từng nhân viên
                var result = await _context.Orders
                    .Where(order => order.OrderTime >= startOfMonth && order.OrderTime <= endOfMonth)
                    .Join(
                        _context.Users,
                        order => order.UserId,
                        user => user.UserId,
                        (order, user) => new { order.UserId, user.FullName, order.TotalResult }
                    )
                    .GroupBy(x => new { x.UserId, x.FullName })
                    .Select(g => new
                    {
                        UserId = g.Key.UserId,
                        FullName = g.Key.FullName,
                        TotalRevenue = g.Sum(x => x.TotalResult)
                    })
                    .ToListAsync();

                return Ok(result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = ex.Message });
            }
        }

        [HttpGet("total-revenue-category")] // Không dùng 
        public async Task<IActionResult> GetTotalRevenueByCategory()
        {
            // Truy vấn tổng doanh thu theo từng danh mục món ăn
            var revenueByCategory = await _context.OrderDetails
                .Join(_context.FoodCategorys,
                      orderItem => orderItem.CategoryId,
                      foodCategory => foodCategory.CategoryId,
                      (orderItem, foodCategory) => new { foodCategory.CategoryName, orderItem.Price, orderItem.Quantity })
                .GroupBy(x => x.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalRevenue = g.Sum(x => x.Price * x.Quantity)
                })
                .ToListAsync();

            return Ok(revenueByCategory);
        }

        [HttpPost("total-revenue-category-time")] // Có dùng
        public async Task<IActionResult> GetTotalRevenueByCategoryTime(TimeRequestDTO request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Date) ||
                    !DateTime.TryParseExact(request.Date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDate))
                {
                    return BadRequest(new { error = "Định dạng ngày không hợp lệ. Vui lòng sử dụng định dạng dd-MM-yyyy." });
                }

                DateTime startOfDay = selectedDate.Date; // 00:00:00 của ngày được chọn
                DateTime endOfDay = selectedDate.Date.AddDays(1).AddTicks(-1); // 23:59:59 của ngày được chọn

                var revenueByCategory = await _context.OrderDetails
                    .Where(orderItem => orderItem.OrderTime >= startOfDay && orderItem.OrderTime <= endOfDay) // Lọc theo thời gian
                    .Join(
                        _context.FoodCategorys,
                        orderItem => orderItem.CategoryId,
                        foodCategory => foodCategory.CategoryId,
                        (orderItem, foodCategory) => new { foodCategory.CategoryName, orderItem.Price, orderItem.Quantity }
                    )
                    .GroupBy(x => x.CategoryName)
                    .Select(g => new
                    {
                        CategoryName = g.Key,
                        TotalRevenue = g.Sum(x => x.Price * x.Quantity)
                    })
                    .ToListAsync();

                return Ok(revenueByCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = ex.Message });
            }
        }

        [HttpPost("total-revenue-category-time-month")] //Có dùng
        public async Task<IActionResult> GetTotalRevenueByCategoryTimeMonth(TimeRequestDTO request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Date) ||
                    !DateTime.TryParseExact(request.Date, "MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime selectedMonth))
                {
                    return BadRequest(new { error = "Định dạng ngày không hợp lệ. Vui lòng sử dụng định dạng MM-yyyy." });
                }

                DateTime startOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1); // Ngày đầu tiên của tháng
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Ngày cuối cùng của tháng

                var revenueByCategory = await _context.OrderDetails
                    .Where(orderItem => orderItem.OrderTime >= startOfMonth && orderItem.OrderTime <= endOfMonth) // Lọc theo thời gian
                    .Join(
                        _context.FoodCategorys,
                        orderItem => orderItem.CategoryId,
                        foodCategory => foodCategory.CategoryId,
                        (orderItem, foodCategory) => new { foodCategory.CategoryName, orderItem.Price, orderItem.Quantity }
                    )
                    .GroupBy(x => x.CategoryName)
                    .Select(g => new
                    {
                        CategoryName = g.Key,
                        TotalRevenue = g.Sum(x => x.Price * x.Quantity)
                    })
                    .ToListAsync();

                return Ok(revenueByCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = ex.Message });
            }
        }

        [HttpPost("total-revenue-time")] // Có dùng
        public async Task<IActionResult> GetTotalAmountByDate([FromBody] TimeRequestDTO dateRequest)
        {
            try
            {
                DateTime selectedDate = DateTime.ParseExact(dateRequest.Date, "dd-MM-yyyy", null);

                DateTime startOfDay = selectedDate.Date; // 00:00:00 của ngày được chọn
                DateTime endOfDay = selectedDate.Date.AddDays(1).AddTicks(-1); // 23:59:59 của ngày được chọn

                DateTime startOfDayYesterday = startOfDay.AddDays(-1); // 00:00:00 của ngày hôm qua
                DateTime endOfDayYesterday = endOfDay.AddDays(-1); // 23:59:59 của ngày hôm qua

                var ordersInSelectedDateRange = _context.Orders
                    .Where(order => order.OrderTime.Date >= startOfDay && order.OrderTime.Date <= endOfDay);

                var totalAmount = await ordersInSelectedDateRange.SumAsync(order => order.TotalResult);
                var totalOrders = await ordersInSelectedDateRange.CountAsync();

                var ordersInYesterdayRange = _context.Orders
                    .Where(order => order.OrderTime.Date >= startOfDayYesterday && order.OrderTime.Date <= endOfDayYesterday);

                var totalAmountYesterday = await ordersInYesterdayRange.SumAsync(order => order.TotalResult);
                var totalOrdersYesterday = await ordersInYesterdayRange.CountAsync();

                return Ok(new
                {
                    TotalAmount = totalAmount,
                    TotalOrders = totalOrders,
                    TotalAmountYesterday = totalAmountYesterday,
                    TotalOrdersYesterday = totalOrdersYesterday
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = ex.Message });
            }
        }

        [HttpPost("total-revenue-time-month")] // Có dùng
        public async Task<IActionResult> GetTotalAmountByMonth([FromBody] TimeRequestDTO monthRequest)
        {
            try
            {
                DateTime selectedMonth = DateTime.ParseExact(monthRequest.Date, "MM-yyyy", null);

                DateTime startOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1); // Ngày đầu tiên của tháng
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Ngày cuối cùng của tháng

                DateTime startOfLastMonth = startOfMonth.AddMonths(-1); // Ngày đầu tiên của tháng trước
                DateTime endOfLastMonth = startOfMonth.AddTicks(-1); // Ngày cuối cùng của tháng trước

                var ordersInCurrentMonthRange = _context.Orders
                    .Where(order => order.OrderTime >= startOfMonth && order.OrderTime <= endOfMonth);

                var totalAmountCurrentMonth = await ordersInCurrentMonthRange.SumAsync(order => order.TotalResult);
                var totalOrdersCurrentMonth = await ordersInCurrentMonthRange.CountAsync();

                var ordersInLastMonthRange = _context.Orders
                    .Where(order => order.OrderTime >= startOfLastMonth && order.OrderTime <= endOfLastMonth);

                var totalAmountLastMonth = await ordersInLastMonthRange.SumAsync(order => order.TotalResult);
                var totalOrdersLastMonth = await ordersInLastMonthRange.CountAsync();

                return Ok(new
                {
                    TotalAmountCurrentMonth = totalAmountCurrentMonth,
                    TotalOrdersCurrentMonth = totalOrdersCurrentMonth,
                    TotalAmountLastMonth = totalAmountLastMonth,
                    TotalOrdersLastMonth = totalOrdersLastMonth
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra khi lấy dữ liệu", message = ex.Message });
            }
        }

        // Phần BÁO CÁO THEO CÁC MÓN BÁN CHẠY
        [HttpGet("get-all-orderitem-bestseling")] // Không dùng 
        public async Task<IActionResult> getAllOrderItemBestSeling()
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
                            o.QuantitySold
                        })
                    .Where(x => x.IsMain == 1)
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

        [HttpPost("get-all-orderitem-bestseling-currentday")] // Có dùng
        public async Task<IActionResult> getAllOrderItemBestSelingurrent(TimeRequestDTO request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Date) ||
                    !DateTime.TryParseExact(request.Date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDate))
                {
                    return BadRequest(new { error = "Định dạng ngày không hợp lệ. Vui lòng sử dụng định dạng dd-MM-yyyy." });
                }

                DateTime startOfDay = selectedDate.Date; // 00:00:00 của ngày được chọn
                DateTime endOfDay = selectedDate.Date.AddDays(1).AddTicks(-1); // 23:59:59 của ngày được chọn

                var bestSellingItems = await _context.OrderDetails
                    .Where(orderItem => orderItem.OrderTime >= startOfDay && orderItem.OrderTime <= endOfDay) // Lọc theo thời gian
                    .GroupBy(orderItem => new { orderItem.FoodItemId }) // Nhóm theo FoodItemId và FoodName
                    .Select(g => new
                    {
                        FoodItemId = g.Key.FoodItemId,
                        QuantitySold = g.Sum(item => item.Quantity), // Đếm số lượng bán ra
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
                            o.QuantitySold
                        })
                    .Where(x => x.IsMain == 1)
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

        [HttpPost("get-all-orderitem-bestseling-currentmonth")] // Có dùng
        public async Task<IActionResult> getAllOrderItemBestSelingurrentMonth(TimeRequestDTO request)
        {
            try
            {
                // Kiểm tra nếu `dateString` không hợp lệ
                if (string.IsNullOrWhiteSpace(request.Date) ||
                    !DateTime.TryParseExact(request.Date, "MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDate))
                {
                    return BadRequest(new { error = "Định dạng ngày không hợp lệ. Vui lòng sử dụng định dạng MM-yyyy." });
                }

                DateTime selectedMonth = DateTime.ParseExact(request.Date, "MM-yyyy", null);
                DateTime startOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1); // Ngày đầu tiên của tháng
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Ngày cuối cùng của tháng

                var bestSellingItems = await _context.OrderDetails
                    .Where(orderItem => orderItem.OrderTime >= startOfMonth && orderItem.OrderTime <= endOfMonth) // Lọc theo thời gian
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
                            o.QuantitySold
                        })
                    .Where(x => x.IsMain == 1)
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

        // Phần BÁO CÁO THEO TỔNG HÓA ĐƠN
        [HttpGet("get-all-order")] // Không dùng 
        public async Task<ActionResult<IEnumerable<OrderMdl>>> GetOrders()
        {
            var query = from order in _context.Orders
                        join user in _context.Users on order.UserId equals user.UserId
                        join table in _context.Tables on order.TableId equals table.TableId
                        select new OrderDetailShowViewReportDTO
                        {
                            OrderId = order.OrderId,
                            FullName = user.FullName,
                            OrderTime = order.OrderTime,
                            TableName = table.TableName,
                            TotalAmount = (decimal)order.TotalAmount,
                            ReturnedAmount = (decimal)order.ReturnedAmount,
                            ReceivedAmount = (decimal)order.ReceivedAmount,
                            Discount = (decimal)order.Discount,
                            PaymentMethod = order.PaymentMethod,
                        };

            var result = await query.OrderByDescending(x => x.OrderTime).ToListAsync();

            return Ok(result);
        }

        [HttpPost("get-all-order-currentday")] // Có dùng
        public async Task<ActionResult<IEnumerable<OrderMdl>>> GetOrdersByDate([FromBody] TimeRequestDTO timeRequest)
        {
            // Chuyển đổi chuỗi ngày "dd-MM-yyyy" thành kiểu DateTime
            if (!DateTime.TryParseExact(timeRequest.Date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
            {
                return BadRequest("Invalid date format. Please use 'dd-MM-yyyy'.");
            }

            // Lọc dữ liệu theo ngày
            var query = from order in _context.Orders
                        join user in _context.Users on order.UserId equals user.UserId
                        join table in _context.Tables on order.TableId equals table.TableId
                        where order.OrderTime.Date == parsedDate.Date // So sánh ngày-tháng-năm
                        select new OrderDetailShowViewReportDTO
                        {
                            OrderId = order.OrderId,
                            FullName = user.FullName,
                            OrderTime = order.OrderTime,
                            TableName = table.TableName,
                            TotalAmount = (decimal)order.TotalAmount,
                            ReturnedAmount = (decimal)order.ReturnedAmount,
                            ReceivedAmount = (decimal)order.ReceivedAmount,
                            Discount = (decimal)order.Discount,
                            PaymentMethod = order.PaymentMethod,
                        };

            var result = await query.OrderByDescending(x => x.OrderTime).ToListAsync();

            return Ok(result);
        }

        [HttpPost("get-all-order-currentmonth")]  // Có dùng
        public async Task<ActionResult<IEnumerable<OrderMdl>>> GetOrdersByDateMonth([FromBody] TimeRequestDTO timeRequest)
        {
            // Chuyển đổi chuỗi ngày "dd-MM-yyyy" thành kiểu DateTime
            if (!DateTime.TryParseExact(timeRequest.Date, "MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
            {
                return BadRequest("Invalid date format. Please use 'MM-yyyy'.");
            }

            // Lọc dữ liệu theo ngày
            var query = from order in _context.Orders
                        join user in _context.Users on order.UserId equals user.UserId
                        join table in _context.Tables on order.TableId equals table.TableId 
                        where order.OrderTime.Month == parsedDate.Month && order.OrderTime.Year == parsedDate.Year
                        select new OrderDetailShowViewReportDTO
                        {
                            OrderId = order.OrderId,
                            FullName = user.FullName,
                            OrderTime = order.OrderTime,
                            TableName = table.TableName,
                            TotalAmount = (decimal)order.TotalAmount,
                            ReturnedAmount = (decimal)order.ReturnedAmount,
                            ReceivedAmount = (decimal)order.ReceivedAmount,
                            Discount = (decimal)order.Discount,
                            PaymentMethod = order.PaymentMethod,
                        };

            var result = await query.OrderByDescending(x => x.OrderTime).ToListAsync();

            return Ok(result);
        }

        [HttpPost("get-all-order-fullname")] // Có dùng
        public async Task<ActionResult<IEnumerable<OrderMdl>>> GetOrdersByFullName([FromBody] TimeRequestDTO timeRequest)
        {
            // Chuyển đổi chuỗi ngày "dd-MM-yyyy" thành kiểu DateTime
            if (timeRequest == null)
            {
                return BadRequest("Invalid FullName");
            }

            // Lọc dữ liệu theo ngày
            var query = from order in _context.Orders
                        join user in _context.Users on order.UserId equals user.UserId
                        join table in _context.Tables on order.TableId equals table.TableId
                        where (string.IsNullOrEmpty(timeRequest.Date) || user.FullName.Contains(timeRequest.Date))
                        select new OrderDetailShowViewReportDTO
                        {
                            OrderId = order.OrderId,
                            FullName = user.FullName,
                            OrderTime = order.OrderTime,
                            TableName = table.TableName,
                            TotalAmount = (decimal)order.TotalAmount,
                            ReturnedAmount = (decimal)order.ReturnedAmount,
                            ReceivedAmount = (decimal)order.ReceivedAmount,
                            Discount = (decimal)order.Discount,
                            PaymentMethod = order.PaymentMethod,
                        };

            var result = await query.OrderByDescending(x => x.OrderTime).ToListAsync();

            return Ok(result);
        }

        [HttpPost("get-orderdetails-by-orderid")]
        public async Task<IActionResult> GetOrderDetailsByOrderId([FromBody] OrderIdRequest request)
        {
            var query = from orderDetail in _context.OrderDetails
                        join foodItem in _context.FoodItems
                            on orderDetail.FoodItemId equals foodItem.FoodItemId
                        where orderDetail.OrderId == request.OrderId
                        select new
                        {
                            foodItem.FoodName,
                            orderDetail.Quantity,
                            orderDetail.Price,
                            orderDetail.Note,
                            orderDetail.IsMainItem
                        };

            var result = await query.ToListAsync();
            return Ok(result);
        }
    }
}
