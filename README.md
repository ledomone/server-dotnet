# Server-dotnet exercise

This is a brief exercise to demonstrate and display proficiency in ASP.NET. The stages set in this exercise range from fairly straight-forward, to more intermediate and potentially challenging tasks.

## Requirements

1. The service must manage and persist a simple domain of three entities, Users, Organizations, and Orders.
    - A user has an Id, First, LastName, Email, OrganizationId, and DateCreated
    - An organization has an Id, Name, Industry, and DateFounded
    - An order is comrosied of an Id, OrderDate, TotalAmount, UserId, and OrganizationId. - - The UserId and OrganizationId values must reference valid records.
2. The service defines the following RESTful API endpoints:
    - GET /api/[entity] – returns all items (e.g., GetAllUsers()). This endpoint for the entities should be pageable.
    - GET /api/[entity]/{id} – returns a single item by id (e.g., GetUserById(int id))
    - POST /api/[entity] – creates a new item (e.g., CreateUser(User user))
    - PUT /api/[entity]/{id} – updates an existing item (e.g., UpdateUser(int id, User user))
    - DELETE /api/[entity]/{id} – deletes an item (e.g., DeleteUser(int id))
3. The endpoints should validate input on POST/PUT operations and return the appropriate HTTP statuses codes:
    - User first and last name should not be null or whitespace
    - Organization name cannot not be null or whitespace
    - An order's TotalAmount must be > 0
    - All date values must occur before the current timestamp
4. GET /api/orders/{id} should return the order as well as the associated user and organization.
5. The application should publish its OpenAPI spec on GET /swagger
6. The application should respond to `/health` and `/readiness` probes

## Non-functional requirements
### Core requirements
1. EF core should be used to manage database interactions
2. The concerns of controller, business and data access logic should be separated.
3. Domain entities should not leak to HTTP response payloads
4. The system should log the following:
    - Database state change operations at an `Information` level
    - HTTP headers at a `Debug` level
5. Business logic should be unit tested
6. The service, including any dependencies (e.g. database) should be deployable via docker
7. Unhandled controller errors should return a custom message and not the developer exception page

### Bonus requirements
1. HTTP clients of the service should be able to leverage headers for caching with GET requests:
    - User and Organization responses should be cachable for 10 minutes
    - Order responses should be cachable using the ETAG pattern
2. The server should cache respones in memory. Each cache entry should have a limited lifetime of 10 minutes. When cached data changes, it should be invalidated from the cache.
3. Oraganization's access to the API should be rate limited to 30 requests per minute.
4. The service should implement basic OAuth/JWT authentication; all routes should be authorized.
5. The database should be secured. The application should discover secrets via configuration.
