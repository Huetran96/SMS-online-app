namespace chat_server.DTOs
{
    public class UpdateProfileDto
    {
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public string? Bio { get; set; }
        public string? Hobby { get; set; }
        public string? Sport { get; set; }
        public string? School { get; set; }
        public string? College { get; set; }
        public string? WorkStatus { get; set; }
        public string? Organization { get; set; }
    }
}
