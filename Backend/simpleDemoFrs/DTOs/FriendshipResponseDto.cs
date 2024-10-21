namespace simpleDemoFrs.DTOs
{
    public class FriendshipResponseDto
    {
        public string RequestedId { get; set; }
        public string AcceptedId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
