FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

COPY AccountService.slnx .
COPY . .

# This is the architecture you’re building for, which is passed in by the builder.
# Placing it here allows the previous steps to be cached across architectures.
ARG TARGETARCH

# Restore and build using the solution file
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore AccountService.slnx

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish src/AccountService.Api/AccountService.Api.csproj -c Release -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app

# Copy everything needed to run the app from the "build" stage.
COPY --from=build /app .

# Switch to a non-privileged user (defined in the base image) that the app will run under.
# See https://docs.docker.com/go/dockerfile-user-best-practices/
# and https://github.com/dotnet/dotnet-docker/discussions/4764
USER $APP_UID

ENTRYPOINT ["dotnet", "AccountService.Api.dll"]
