# Mirama Microservice Backend

## Usage Guide

To meet security standards, the Auth server **must run on HTTPS**, even in development mode.  
When running the system locally with Docker, you need to create and trust a local development certificate.

## Project Specific Documentation

The project specific documentation can be found here - [Documentation](https://yannickullisch.github.io/mirama-services/).

### Prerequisites

- **Docker Compose** and the Docker daemon must be installed and running on your machine.
- [.NET SDK](https://dotnet.microsoft.com/download) installed for certificate generation (preferably .NET 10).

### Steps

1. **Create and trust a development certificate** (Mac/Linux, from the project root):

    ```sh
    mkdir certs
    dotnet dev-certs https -ep ./certs/dev.pfx -p devpassword
    dotnet dev-certs https --trust
    ```

2. **Ensure the certificate is in the `./certs` folder.**
   - If you use a different location, update the `volumes` path in your `docker-compose.yml` to match.

3. **For each service, create a new `.env` file in the directory of that service** (e.g., `./AccountService`, `./AuthService`).
   - Copy the development setup from the corresponding `.env.example` file's DEV section into your new `.env` file.
   - There might be some secrets that you will have to fill out yourself, e.g. Google ClientId and ClientSecret for the Google IdP.
   - Adjust any other values as needed for your local environment.

4. **Start all backend services:**

    ```sh
    docker compose up
    ```

If you are having issues, you can find relevant documentation at [.NET Docs](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-10.0)
