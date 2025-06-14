using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server_dotnet.Domain.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]

        public Organization Organization { get; set; } = null!;
    }
}
