namespace testVue.ModelsRequest
{
    public class UpdateUserRequest
    {
        public int UserId { get; set; } = default;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public string? NewPassword { get; set; }
    }
}
