using BussinessObject.Enums;

namespace BusinessLogic.DTO.UserDTO
{
    public class UserRequestDTO
    {
        public UserType Role { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; } = "Password123";
        public string? FullName { get; set; }
        public UserGender? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? LocationId { get; set; } // for staff 
    }
}
