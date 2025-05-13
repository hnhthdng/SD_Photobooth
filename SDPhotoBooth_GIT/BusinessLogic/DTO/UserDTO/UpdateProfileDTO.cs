using BussinessObject.Enums;

namespace BusinessLogic.DTO.UserDTO
{
    public class UpdateProfileDTO
    {
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public UserGender? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
