using Enums;

namespace DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public RoleEnum Role { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
    }
}
