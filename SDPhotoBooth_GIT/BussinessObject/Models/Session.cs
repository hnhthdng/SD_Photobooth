using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace BussinessObject.Models
{
    public class Session : BaseEntityNoAudit
    {
        [Required]
        [StringLength(10, ErrorMessage = "Code cannot exceed 10 characters.")]
        public string Code { get; set; } = GenerateCode(10);

        public DateTime? Expired { get; set; }

        public bool IsActive { get; set; } = false;

        [Required]
        public int OrderId { get; set; }

        public virtual PhotoHistory PhotoHistory { get; set; }

        public int AbleTakenNumber { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        private static string GenerateCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using var rng = RandomNumberGenerator.Create();
            var result = new StringBuilder(length);
            var buffer = new byte[sizeof(uint)];

            for (var i = 0; i < length; i++)
            {
                rng.GetBytes(buffer);
                var randomValue = BitConverter.ToUInt32(buffer, 0);
                result.Append(chars[(int)(randomValue % chars.Length)]);
            }

            return result.ToString();
        }

    }
}
