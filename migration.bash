#!/bin/sh

dotnet ef migrations add test --project AccountService/src/AccountService.Infrastructure -o AccountService/src/AccountService.Infrastructure/Persistence/Migrations