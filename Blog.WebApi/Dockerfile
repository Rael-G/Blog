FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Blog.WebApi/Blog.WebApi.csproj", "Blog.WebApi/"]
COPY ["Blog.Application/Blog.Application.csproj", "Blog.Application/"]
COPY ["Blog.Domain/Blog.Domain.csproj", "Blog.Domain/"]
COPY ["Blog.Persistance/Blog.Persistance.csproj", "Blog.Persistance/"]

RUN dotnet restore "Blog.WebApi/Blog.WebApi.csproj"

COPY ["Blog.WebApi/", "Blog.WebApi/"]
COPY ["Blog.Application/", "Blog.Application/"]
COPY ["Blog.Domain/", "Blog.Domain/"]
COPY ["Blog.Persistance/", "Blog.Persistance/"]
COPY ["Blog.UnitTest/", "Blog.UnitTest/"]

WORKDIR /src/Blog.WebApi

RUN dotnet build "Blog.WebApi.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Blog.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Blog.WebApi.dll"]