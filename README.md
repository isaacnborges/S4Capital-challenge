# S4Capital-challenge
Technical project to S4Capital challenge

## Project Structure
The solution is structured as follows:

- src/
  - S4Capital.Api
- tests/
  - S4Capital.Tests

## Project information

The project adopted a structured approach with a clear separation of concerns. The project is organized into distinct folders: 
- Api 
- Core 
- Domain
- Infrastructure. 

This approach brings several advantages to our development process.

By isolating different components of the application into their respective folders, it 
create a clear boundary for each part of the system. This separation allows us to focus on one aspect at a time, making it easier to understand, modify, and extend our codebase.

The project embraced the Commands and Queries pattern using MediatR. This pattern further reduces coupling between components. Commands represent actions that change the state of the system, while Queries retrieve information without side effects. This separation not only simplifies unit testing but also ensures that changes to one part of the system have minimal impact on others.

### Technologies Used in the project
- [.Net 7](https://dotnet.microsoft.com/download/dotnet/7.0)
- [ASP.NET](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-7.0?view=aspnetcore-7.0)
- [Sql Server 2019](https://www.microsoft.com/sql-server/sql-server-2019)
- [MediatR](https://github.com/jbogard/MediatR)
- [FluentValidation](https://fluentvalidation.net/) for request Dto validations

### Technologies Used in the tests
- [.Net 7](https://dotnet.microsoft.com/download/dotnet/7.0)
- [TestContainers](https://dotnet.testcontainers.org/)
- [xUnit](https://xunit.net/) for testing
- [Bogus](https://github.com/bchavez/Bogus) for generating mock objects
- [Moq](https://github.com/moq/moq) and AutoMoq for unit testing
- [FluentAssertions](https://fluentassertions.com/) for more readable and expressive assertions
- [AspNetCore Testing](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0) for integration tests using `WebApplicationFactory`

## Dependencies 

- [Docker](https://docs.docker.com/get-docker/)

## Build
To build the project, follow the instructions below:
1. Make sure you have [.Net 7](https://dotnet.microsoft.com/download/dotnet/7.0) installed.
2. In the terminal, navigate to the root directory of the project.
3. Run the following command to execute the project:
```
dotnet build
```

## Run using Docker
To run the program in a Docker container using the provided Dockerfile, follow the instructions below:
1. In the terminal, navigate to the root directory of the project.
2. Run the following command to execute the project:

```
docker-compose up
```

### Tests 
1. Integration tests needs Docker to run.
2. In the terminal, navigate to the root directory of the project.
3. Run the following command to execute the tests:

```
dotnet test .\tests\Number8.Tests.csproj
```

### Documentation
- Postman
    - Open `docs` folder, inside has a [postman](https://www.postman.com/) collection that could be used for test.
- Swagger - After run the api the swagger endpoint will be available
    - [http://localhost:8000/swagger/](http://localhost:8000/swagger/index.html)

<img src="https://raw.githubusercontent.com/isaacnborges/S4Capital-challenge/main/images/swagger.png"/>

### Sotware flow
- A user logs in to the API using the `/login` endpoint in the `AuthController`.
- The API validates the user's credentials and returns a token if the user is authenticated.
- The user can then access the PostController endpoints, but only if they have the correct role.

    - Writer Role:
        - CreatePost: Writers can create new posts.
        - EditPost: Writers can edit their own posts.
        - Get Own Posts: Writers can retrieve a list of their own posts.
        - Submit Approved Post: Writers can submit a post they've created once it's approved.

    - Editor Role:
        - Approve Pending Post: Editors can approve pending posts created by Writers.
        - Reject Pending Post: Editors can reject pending posts created by Writers.
        - Get Pending Posts: Editors can view a list of pending posts for approval.
    
    - Public Role:
        - Get All Published Posts: The Public role can retrieve a list of all published posts.
        - Add Comments to Published Post: Public users can add comments to published posts.

<img src="https://raw.githubusercontent.com/isaacnborges/S4Capital-challenge/main/images/flow.png"/>

### Seed Data for users

For testing purposes and to simulate different roles, a set of predefined users with distinct roles have been seeded into the application's database.
The following roles are available: `Writer`, `Editor`, and `Public`.

User Information:
- Writer Role:
    - Users: `writer1`, `writer2`
    - Password: `Password123`

- Editor Role:
    - Users: `editor1`, `editor2`
    - Password: `Password123`

- Public Role:
    - Users: `public1`, `public2`
    - Password: `Password123`

These predefined users allow for easy testing and demonstration of the API's role-based authentication and authorization mechanisms. 