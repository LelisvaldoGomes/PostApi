# PostApi

This project is a simple API implementation in .NET using ASP.NET Core. It demonstrates CRUD operations on a collection of data objects and consumes an external API to fetch initial data.

## Features

- List all posts
- Get posts by ID
- Create a new post
- Update an existing post
- Delete a post

## Requirements

- .NET SDK (version 8.0.0)
- Visual Studio or Visual Studio Code

## Getting Started

1. Clone the repository:

   ```shell
   git clone https://github.com/LelisvaldoGomes/PostApi.git
   ```

2. Open the project in your preferred development environment.

3. Build the project to restore dependencies:

   ```shell
   dotnet build
   ```

4. Run the project:

   ```shell
   dotnet run
   ```

5. The API will be accessible at:
   ```shell
      "https://localhost:[port]/api"
   ```

## API Endpoints

- GET / - Get the API status
- GET /api/posts - Get all posts
- GET /api/post/{id} - Get a post by ID
- POST /api/post - Create a new post
- PUT /api/post/{id} - Update a post
- DELETE /api/post/{id} - Delete a post

## Contributing

Contributions are welcome! If you find any issues or want to add new features, feel free to open an issue or submit a pull request.

## License

Distributed under the MIT License.