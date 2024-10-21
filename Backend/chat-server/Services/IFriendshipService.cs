using chat_server.DTOs;

namespace chat_server.Services
{
    public interface IFriendshipService
    {
        // Phương thức này cho phép thêm một yêu cầu kết bạn mới và trả về thông tin về tình bạn được tạo ra dưới dạng DTO
        Task<FriendshipResponseDto> AddFriend(FriendshipCreateDto dto);

        // Phương thức này cho phép lấy danh sách bạn bè của người dùng dựa trên userId và trả về danh sách các DTO tình bạn
        Task<List<FriendshipResponseDto>> GetFriends(string userId);

        // Phương thức này cho phép chấp nhận yêu cầu kết bạn dựa trên requestedId và acceptedId và trả về thông tin về tình bạn đã được chấp nhận dưới dạng DTO
        Task<FriendshipResponseDto> AcceptFriend(string requestedId, string acceptedId);

        // Phương thức này cho phép chặn một người dùng dựa trên requestedId và acceptedId và trả về thông tin về tình bạn đã bị chặn dưới dạng DTO
        Task<FriendshipResponseDto> BlockUser(string requestedId, string acceptedId);

        // Phương thức này cho phép xóa một yêu cầu kết bạn dựa trên requestedId và acceptedId, không trả về giá trị
        Task RemoveFriend(string requestedId, string acceptedId);

        // Phương thức này cho phép lấy danh sách người dùng bị chặn dựa trên userId và trả về danh sách các DTO tình bạn
        Task<List<FriendshipResponseDto>> GetBlockedUsers(string userId);

        // Phương thức này cho phép lấy danh sách các yêu cầu kết bạn chưa được chấp nhận dựa trên userId và trả về danh sách các DTO tình bạn
        Task<List<FriendshipResponseDto>> GetFriendRequests(string userId);

    }

}
