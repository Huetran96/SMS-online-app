namespace chat_server.DTOs
{
    public class FriendshipResponseDto
    {
        public int Id { get; set; }
        public string RequestedId { get; set; }
        public string AcceptedId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
