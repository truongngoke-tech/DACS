namespace DACS.Models
{
    public class ProfileViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IFormFile? AvatarFile { get; set; }
        public string? CurrentAvatarUrl { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
