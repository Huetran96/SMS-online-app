using chat_server.data;
using chat_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chat_server.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly AppDbContext _context;

        // Constructor: Khởi tạo FriendshipRepository với AppDbContext để tương tác với cơ sở dữ liệu
        public FriendshipRepository(AppDbContext context)
        {
            _context = context; // Gán context vào biến thành viên để sử dụng trong các phương thức
        }

        // Thêm một yêu cầu kết bạn vào cơ sở dữ liệu
        public async Task<Friendship> AddFriend(Friendship friendship)
        {
            _context.Friendships.Add(friendship); // Thêm đối tượng Friendship vào DbSet Friendships
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            return friendship; // Trả về đối tượng Friendship đã thêm
        }

        // Lấy danh sách bạn bè của người dùng theo userId
        public async Task<List<Friendship>> GetFriends(string userId)
        {
            return await _context.Friendships
                .Where(f => (f.RequestedId == userId || f.AcceptedId == userId) && f.Status == "Accepted") // Lọc danh sách bạn bè có trạng thái "Accepted"
                .ToListAsync(); // Chuyển đổi kết quả thành danh sách
        }

        // Chấp nhận một yêu cầu kết bạn
        public async Task<Friendship> AcceptFriend(string requestedId, string acceptedId)
        {
            // Tìm kiếm yêu cầu kết bạn dựa trên requestedId và acceptedId
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.RequestedId == requestedId && f.AcceptedId == acceptedId);

            if (friendship == null) return null; // Trả về null nếu không tìm thấy yêu cầu

            // Cập nhật trạng thái yêu cầu thành "Accepted"
            friendship.Status = "Accepted";
            friendship.UpdatedAt = DateTime.Now; // Cập nhật thời gian sửa đổi
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            return friendship; // Trả về đối tượng Friendship đã được chấp nhận
        }

        // Chặn một người dùng
        public async Task<Friendship> BlockUser(string requestedId, string acceptedId)
        {
            // Tìm kiếm yêu cầu kết bạn dựa trên requestedId và acceptedId
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.RequestedId == requestedId && f.AcceptedId == acceptedId);

            if (friendship == null) return null; // Trả về null nếu không tìm thấy yêu cầu

            // Cập nhật trạng thái yêu cầu thành "Blocked"
            friendship.Status = "Blocked";
            friendship.UpdatedAt = DateTime.Now; // Cập nhật thời gian sửa đổi
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            return friendship; // Trả về đối tượng Friendship đã bị chặn
        }

        // Xóa hoặc hủy kết bạn
        public async Task RemoveFriend(string requestedId, string acceptedId)
        {
            // Tìm kiếm yêu cầu kết bạn dựa trên requestedId và acceptedId
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.RequestedId == requestedId && f.AcceptedId == acceptedId);

            if (friendship != null) // Nếu tìm thấy yêu cầu
            {
                _context.Friendships.Remove(friendship); // Xóa yêu cầu kết bạn khỏi DbSet
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            }
        }

        // Lấy danh sách người dùng bị chặn
        public async Task<List<Friendship>> GetBlockedUsers(string userId)
        {
            return await _context.Friendships
                .Where(f => (f.RequestedId == userId || f.AcceptedId == userId) && f.Status == "Blocked") // Lọc danh sách người dùng bị chặn
                .ToListAsync(); // Chuyển đổi kết quả thành danh sách
        }

        // Lấy danh sách yêu cầu kết bạn chưa được chấp nhận
        public async Task<List<Friendship>> GetFriendRequests(string userId)
        {
            return await _context.Friendships
                .Where(f => f.AcceptedId == userId && f.Status == "Pending") // Lọc danh sách yêu cầu kết bạn có trạng thái "Pending"
                .ToListAsync(); // Chuyển đổi kết quả thành danh sách
        }
    }



}
