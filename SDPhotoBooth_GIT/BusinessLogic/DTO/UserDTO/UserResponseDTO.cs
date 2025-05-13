using BusinessLogic.DTO.LocationDTO;
using BusinessLogic.DTO.WalletDTO;
using BussinessObject.Enums;

namespace BusinessLogic.DTO.UserDTO
{
    public class UserResponseDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserGender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Avatar { get; set; }
        public string Role { get; set; }
        public bool IsBanned { get; set; }
        public LocationResponseDTO Location { get; set; }
        public WalletResponseDTO Wallet { get; set; }
        public MemberShipCardOnUserResponseDTO MembershipCard { get; set; }

    }

    public class MemberShipCardOnUserResponseDTO
    {
        public int Id { get; set; }
        public string CurrentLevel { get; set; }
        public int CurrentPoint { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}