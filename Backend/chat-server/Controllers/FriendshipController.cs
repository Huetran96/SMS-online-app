using chat_server.DTOs;
using chat_server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chat_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        // Thêm yêu cầu kết bạn (POST)
        [HttpPost("add-friend")]
        public async Task<IActionResult> AddFriend([FromBody] FriendshipCreateDto dto)
        {
            try
            {
                var result = await _friendshipService.AddFriend(dto);
                return Ok(result); // Trả về HTTP 200 với DTO phản hồi
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Trả về HTTP 500 nếu có lỗi
            }
        }

        // Lấy danh sách bạn bè của người dùng (GET)
        [HttpGet("friends/{userId}")]
        public async Task<IActionResult> GetFriends(string userId)
        {
            try
            {
                var result = await _friendshipService.GetFriends(userId);
                return Ok(result); // Trả về HTTP 200 với danh sách bạn bè
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Trả về HTTP 500 nếu có lỗi
            }
        }

        // Chấp nhận yêu cầu kết bạn (PUT)
        [HttpPut("accept-friend")]
        public async Task<IActionResult> AcceptFriend([FromQuery] string requestedId, [FromQuery] string acceptedId)
        {
            try
            {
                var result = await _friendshipService.AcceptFriend(requestedId, acceptedId);
                return Ok(result); // Trả về HTTP 200 sau khi chấp nhận yêu cầu
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Trả về HTTP 500 nếu có lỗi
            }
        }

        // Chặn người dùng (PUT)
        [HttpPut("block-user")]
        public async Task<IActionResult> BlockUser([FromQuery] string requestedId, [FromQuery] string acceptedId)
        {
            try
            {
                var result = await _friendshipService.BlockUser(requestedId, acceptedId);
                return Ok(result); // Trả về HTTP 200 sau khi chặn người dùng
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Trả về HTTP 500 nếu có lỗi
            }
        }

        // Xóa kết bạn (DELETE)
        [HttpDelete("remove-friend")]
        public async Task<IActionResult> RemoveFriend([FromQuery] string requestedId, [FromQuery] string acceptedId)
        {
            try
            {
                await _friendshipService.RemoveFriend(requestedId, acceptedId);
                return NoContent(); // Trả về HTTP 204 sau khi xóa kết bạn
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Trả về HTTP 500 nếu có lỗi
            }
        }

        // Lấy danh sách người dùng bị chặn (GET)
        [HttpGet("blocked-users/{userId}")]
        public async Task<IActionResult> GetBlockedUsers(string userId)
        {
            try
            {
                var result = await _friendshipService.GetBlockedUsers(userId);
                return Ok(result); // Trả về HTTP 200 với danh sách người bị chặn
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Trả về HTTP 500 nếu có lỗi
            }
        }

        // Lấy danh sách yêu cầu kết bạn chưa được chấp nhận (GET)
        [HttpGet("friend-requests/{userId}")]
        public async Task<IActionResult> GetFriendRequests(string userId)
        {
            try
            {
                var result = await _friendshipService.GetFriendRequests(userId);
                return Ok(result); // Trả về HTTP 200 với danh sách yêu cầu kết bạn
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Trả về HTTP 500 nếu có lỗi
            }
        }
    }

}
