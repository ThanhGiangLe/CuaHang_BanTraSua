using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class UserDTO
    {
        [Key]
        public int UserId { get; set; } = default;
        public string? FullName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
        public string? Avatar { get; set; } = string.Empty;
    }
}
