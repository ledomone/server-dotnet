namespace server_dotnet.Controllers.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public int UserId { get; set; }

        public int OrganizationId { get; set; }

    }
}
