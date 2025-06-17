using System.ComponentModel.DataAnnotations;

namespace server_dotnet.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        public int OrganizationId { get; set; }

        public Organization Organization { get; set; } = null!;
    }
}
