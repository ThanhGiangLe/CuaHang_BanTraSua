using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using testVue.Datas;

namespace testVue.Handls
{
    public class UserHub : Hub
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserConnectionManager _userConnectionManager;
        public UserHub(AppDbContext appDbContext, UserConnectionManager userConnectionManager)
        {
            _appDbContext = appDbContext;
            _userConnectionManager = userConnectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            var result = Int32.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out int userId);
            if (result)
            {
                _userConnectionManager.AddConnection(userId, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var result = Int32.TryParse(Context.GetHttpContext()?.Request.Query["userId"].ToString(), out int userId);

            if (result) { 
                _userConnectionManager.RemoveConnection(userId, Context.ConnectionId);
                await Task.Delay(5000);
                if(!_userConnectionManager.HasConnections(userId))
                {
                    var updateResult = await UpdateLogoutTime(userId);
                    if (updateResult != 1)
                    {
                        Console.WriteLine($"❌ Error updating logout time for user {userId}");
                    }
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<int> UpdateLogoutTime(int userId)
        {
            if (userId < 1)
            {
                Console.WriteLine("Invalid userId.");
                return -1;
            }

            var time = DateTime.Now;
            var user = await _appDbContext.CashRegisters.OrderByDescending(x => x.CashRegisterId).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return -1;  // Không tìm thấy người dùng
            }

            user.EndTime = time;
            _appDbContext.CashRegisters.Update(user);
            await _appDbContext.SaveChangesAsync();

            return 1;  // Thành công
        }
    }
}
