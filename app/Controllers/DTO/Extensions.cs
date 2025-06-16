using server_dotnet.Domain.Entities;

namespace server_dotnet.Controllers.DTO
{
    public static class Extensions
    {
        public static OrganizationDTO ToDTO(this Organization organization)
        {
            if (organization == null)
            {
                return null!;
            }
            return new OrganizationDTO
            {
                Id = organization.Id,
                Name = organization.Name,
                Industry = organization.Industry,
                DateFounded = organization.DateFounded
            };
        }
        public static IEnumerable<OrganizationDTO> ToDTOs(this IEnumerable<Organization> organizations)
        {
            return organizations?.Select(o => o.ToDTO()) ?? Enumerable.Empty<OrganizationDTO>();
        }

        public static UserDTO ToDTO(this User user)
        {
            if (user == null)
            {
                return null!;
            }
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                OrganizationId = user.OrganizationId,
                DateCreated = user.DateCreated
            };
        }

        public static IEnumerable<UserDTO> ToDTOs(this IEnumerable<User> users)
        {
            return users?.Select(u => u.ToDTO()) ?? Enumerable.Empty<UserDTO>();
        }

        public static OrderDTO ToDTO(this Order order)
        {
            if (order == null)
            {
                return null!;
            }
            return new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                UserId = order.UserId,
                OrganizationId = order.OrganizationId,
            };
        }

        public static IEnumerable<OrderDTO> ToDTOs(this IEnumerable<Order> orders)
        {
            return orders?.Select(o => o.ToDTO()) ?? Enumerable.Empty<OrderDTO>();
        }

        public static FullOrderDTO ToFullOrderDTO(this Order order)
        {
            if (order == null)
            {
                return null!;
            }
            return new FullOrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                UserId = order.UserId,
                OrganizationId = order.OrganizationId,
                User = order.User.ToDTO(),
                Organization = order.Organization.ToDTO()
            };
        }
    }
}
