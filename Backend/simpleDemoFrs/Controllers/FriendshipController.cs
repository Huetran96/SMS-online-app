using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using simpleDemoFrs.DTOs;

namespace simpleDemoFrs.Controllers
{
    public class FriendshipController : Controller
    {
        private readonly HttpClient _httpClient;

        public FriendshipController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("https://localhost:7231/api/friendship/"); // Đặt URL của API backend
        }

        // Trang hiển thị form để thêm bạn bè
        [HttpGet]
        public IActionResult AddFriend()
        {
            return View();
        }

        // Xử lý gửi yêu cầu thêm bạn bè
        [HttpPost]
        public async Task<IActionResult> AddFriend(string requestedId, string acceptedId)
        {
            var requestBody = new
            {
                RequestedId = requestedId,
                AcceptedId = acceptedId
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("add-friend", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("FriendList");
            }

            ViewBag.Error = "Failed to add friend.";
            return View();
        }

        // Lấy danh sách bạn bè
        [HttpGet]
        public async Task<IActionResult> FriendList(string userId)
        {
            var response = await _httpClient.GetAsync($"friends/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var friends = JsonSerializer.Deserialize<List<FriendshipResponseDto>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(friends);
            }

            ViewBag.Error = "Failed to retrieve friends.";
            return View(new List<FriendshipResponseDto>());
        }

        // Chấp nhận yêu cầu kết bạn
        [HttpPost]
        public async Task<IActionResult> AcceptFriend(string requestedId, string acceptedId)
        {
            var response = await _httpClient.PutAsync($"accept-friend?requestedId={requestedId}&acceptedId={acceptedId}", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("FriendList", new { userId = acceptedId });
            }

            ViewBag.Error = "Failed to accept friend request.";
            return View();
        }
    }
}
