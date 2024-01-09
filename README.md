# About ProductManagement Project
Product management project is a show case of how to follow 'Clean Architecture' pattern. The goal of clean architecture is to create a separation of concerns in software systems by organizing code in a way that makes it independent of external frameworks, databases, and delivery mechanisms. This architecture promotes maintainability, testability, and scalability by enforcing a clear separation between the core business logic and external dependencies.

Following the pattern the application was divided in 4 main layers:
1. Presentation Layer (API Controllers):
   - Responsible for handling incoming HTTP requests.
   - Invokes the appropriate methods in the application layer.
   - Serializes data for the response.
1. Application Layer:
   - Contains the business logic and application-specific rules.
   - Orchestrates interactions between the presentation layer and the domain layer.
   - Often, the application layer is divided into services that represent different business capabilities (e.g., ProductService, CategoryService).
1. Domain Layer:
   - Represents the core business entities and rules.
   - Contains the domain models or entities (e.g., Product, Category).
   - Enforces business rules and validations.
1. Infrastructure Layer:
   - Contains infrastructure-related concerns such as configuration, data access, email and other communication mechanisms.
   - May also include external services, third-party libraries, or tools.
 
Other layers that can be considered and implemented, as the project scales, are:
 1. Data Access Layer:
    - Extract from the infrastructure layer the interaction with the database.
 1. Cross-cutting Concerns Layer:
    - Deals with concerns that span multiple layers.
   
## How to use
1. Clone/download the project.
2. Open in visual studio and restore nuget packages.
3. Run the ProductManagement.API project located in presentation folder
4. Swagger home page should oppen at localhost/swagger/index.html
5. Create a product by calling the POST endpoint
6. Call any other endpoint


## Endpoints supported:
- GetProducts - get all products.
- GetProduct - get product by id.
- CreateProduct - add a new product.
- SetQuantity - set the quantity of a product. When quantity = 0 then is considered as 'Out Of Stock'. If quantity > 0 then is considered 'In Stock'.
- UpdateProduct - update an existing product.
- HardDelete - permanently delete a product, considered as the product not provided any more.
- SoftDelete - mark a product as deleted, considered as not available.
- Restore - mark a product a not deleted, considered as the product available again.

## Technologies used
- .Net 8 (C# 12)
- Entity Framework 8 (in this show case the in-memory database was used)

## Libraries used
- xunit
- Moq
- AutoMapper
- FluentValidation
- MailKit
- Swashbuckle (Swagger)

## Future extentions
Throughout the project 'TODO' tags are left to indicate areas of code that require attention or completion. Please for better visibility search the solution for 'TODO'.
A few main features that can be added/expanded are:
- Enrich 'Product' entity with more properties and rules
- Add more entities for example 'Product Category'
- More unit test coverage, they are never enough - Due to time limitation at the moment there are unit test for ProductsController and a couple for ProductsService
- Write functional tests to ensure that it the solution meets the specified requirements and functions correctly from an end-user perspective
- Write integration tests to detect issues that may arise from the integration of different modules or services.
- Remove in-memory database and support an actual storage medioum like an sql database. Configuration is in place, but commented out.
- Integrate with other services using a message broker like azure service bus or any equivalent
  - Main integration that this project can benefit from are:
    - integrate with orders service
    - integrate with audits service
    - integrate with exceptions handler service
    - integrate with notification service - responsible to send out emails, real time notification
- Add Authentication and Authorization. Set up an identity provider (i.e IdentityServer) and integrate with the service. For this purpose code was added and commented out so anyone can run the project locally.
- Introduce secret storage like azure KeyVault or equivalent
