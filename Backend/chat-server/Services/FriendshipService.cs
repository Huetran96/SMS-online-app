using chat_server.DTOs;
using chat_server.Models;
using chat_server.Repositories;

namespace chat_server.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _repository;

        // Constructor: Khởi tạo FriendshipService với repository để tương tác với cơ sở dữ liệu
        public FriendshipService(IFriendshipRepository repository)
        {
            _repository = repository; // Gán repository vào biến thành viên để sử dụng trong các phương thức
        }

        // Thêm một yêu cầu kết bạn mới
        public async Task<FriendshipResponseDto> AddFriend(FriendshipCreateDto dto)
        {
            try
            {
                // Tạo một đối tượng Friendship mới từ DTO
                var friendship = new Friendship
                {
                    RequestedId = dto.RequestedId, // ID của người gửi yêu cầu kết bạn
                    AcceptedId = dto.AcceptedId,     // ID của người nhận yêu cầu kết bạn
                    Status = "Pending",               // Trạng thái yêu cầu là "Chờ xử lý"
                    CreateAt = DateTime.Now,        // Thời gian tạo yêu cầu
                    UpdatedAt = DateTime.Now         // Thời gian cập nhật yêu cầu
                };

                // Thêm yêu cầu kết bạn vào cơ sở dữ liệu thông qua repository
                var createdFriendship = await _repository.AddFriend(friendship);

                // Map (biến đổi) đối tượng Friendship thành DTO phản hồi
                return new FriendshipResponseDto
                {
                    RequestedId = createdFriendship.RequestedId,
                    AcceptedId = createdFriendship.AcceptedId,
                    Status = createdFriendship.Status,
                    CreatedAt = createdFriendship.CreateAt,
                    UpdatedAt = createdFriendship.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu thêm bạn thất bại
                throw new Exception("Failed to add friend", ex);
            }
        }

        // Lấy danh sách bạn bè của người dùng theo userId
        public async Task<List<FriendshipResponseDto>> GetFriends(string userId)
        {
            try
            {
                // Lấy danh sách bạn bè từ repository
                var friends = await _repository.GetFriends(userId);

                // Map danh sách bạn bè thành danh sách DTO phản hồi
                var friendsDto = friends.Select(f => new FriendshipResponseDto
                {
                    RequestedId = f.RequestedId, // ID của người gửi yêu cầu
                    AcceptedId = f.AcceptedId,     // ID của người nhận yêu cầu
                    Status = f.Status,              // Trạng thái của yêu cầu
                    CreatedAt = f.CreateAt,         // Thời gian tạo yêu cầu
                    UpdatedAt = f.UpdatedAt         // Thời gian cập nhật yêu cầu
                }).ToList();

                return friendsDto; // Trả về danh sách DTO phản hồi
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu không lấy được danh sách bạn bè
                throw new Exception("Failed to retrieve friends", ex);
            }
        }

        // Chấp nhận yêu cầu kết bạn
        public async Task<FriendshipResponseDto> AcceptFriend(string requestedId, string acceptedId)
        {
            try
            {
                // Chấp nhận yêu cầu kết bạn từ repository
                var friendship = await _repository.AcceptFriend(requestedId, acceptedId);
                if (friendship == null)
                    throw new Exception("Friendship not found"); // Ném ngoại lệ nếu không tìm thấy

                // Map đối tượng Friendship thành DTO phản hồi
                return new FriendshipResponseDto
                {
                    RequestedId = friendship.RequestedId, // ID của người gửi yêu cầu
                    AcceptedId = friendship.AcceptedId,     // ID của người nhận yêu cầu
                    Status = friendship.Status,              // Trạng thái của yêu cầu
                    CreatedAt = friendship.CreateAt,         // Thời gian tạo yêu cầu
                    UpdatedAt = friendship.UpdatedAt         // Thời gian cập nhật yêu cầu
                };
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu chấp nhận yêu cầu thất bại
                throw new Exception("Failed to accept friend", ex);
            }
        }

        // Chặn người dùng
        public async Task<FriendshipResponseDto> BlockUser(string requestedId, string acceptedId)
        {
            try
            {
                // Chặn người dùng từ repository
                var friendship = await _repository.BlockUser(requestedId, acceptedId);
                if (friendship == null)
                    throw new Exception("Friendship not found"); // Ném ngoại lệ nếu không tìm thấy

                // Map đối tượng Friendship thành DTO phản hồi
                return new FriendshipResponseDto
                {
                    RequestedId = friendship.RequestedId, // ID của người gửi yêu cầu
                    AcceptedId = friendship.AcceptedId,     // ID của người nhận yêu cầu
                    Status = friendship.Status,              // Trạng thái của yêu cầu
                    CreatedAt = friendship.CreateAt,         // Thời gian tạo yêu cầu
                    UpdatedAt = friendship.UpdatedAt         // Thời gian cập nhật yêu cầu
                };
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu chặn người dùng thất bại
                throw new Exception("Failed to block user", ex);
            }
        }

        // Xóa hoặc hủy kết bạn
        public async Task RemoveFriend(string requestedId, string acceptedId)
        {
            try
            {
                // Gọi phương thức xóa bạn từ repository
                await _repository.RemoveFriend(requestedId, acceptedId);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu xóa bạn thất bại
                throw new Exception("Failed to remove friend", ex);
            }
        }

        // Lấy danh sách người dùng bị chặn
        public async Task<List<FriendshipResponseDto>> GetBlockedUsers(string userId)
        {
            try
            {
                // Lấy danh sách người dùng bị chặn từ repository
                var blockedUsers = await _repository.GetBlockedUsers(userId);

                // Map danh sách người dùng bị chặn thành danh sách DTO phản hồi
                var blockedDto = blockedUsers.Select(b => new FriendshipResponseDto
                {
                    RequestedId = b.RequestedId, // ID của người gửi yêu cầu
                    AcceptedId = b.AcceptedId,     // ID của người nhận yêu cầu
                    Status = b.Status,              // Trạng thái của yêu cầu
                    CreatedAt = b.CreateAt,         // Thời gian tạo yêu cầu
                    UpdatedAt = b.UpdatedAt         // Thời gian cập nhật yêu cầu
                }).ToList();

                return blockedDto; // Trả về danh sách DTO phản hồi
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu không lấy được danh sách người dùng bị chặn
                throw new Exception("Failed to retrieve blocked users", ex);
            }
        }

        // Lấy danh sách yêu cầu kết bạn chưa được chấp nhận
        public async Task<List<FriendshipResponseDto>> GetFriendRequests(string userId)
        {
            try
            {
                // Lấy danh sách yêu cầu kết bạn từ repository
                var requests = await _repository.GetFriendRequests(userId);

                // Map danh sách yêu cầu thành danh sách DTO phản hồi
                var requestsDto = requests.Select(r => new FriendshipResponseDto
                {
                    RequestedId = r.RequestedId, // ID của người gửi yêu cầu
                    AcceptedId = r.AcceptedId,     // ID của người nhận yêu cầu
                    Status = r.Status,              // Trạng thái của yêu cầu
                    CreatedAt = r.CreateAt,         // Thời gian tạo yêu cầu
                    UpdatedAt = r.UpdatedAt         // Thời gian cập nhật yêu cầu
                }).ToList();

                return requestsDto; // Trả về danh sách DTO phản hồi
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu không lấy được danh sách yêu cầu kết bạn
                throw new Exception("Failed to retrieve friend requests", ex);
            }
        }

    }


}
