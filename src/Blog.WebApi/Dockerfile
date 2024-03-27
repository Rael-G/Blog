FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["src/Blog.WebApi/Blog.WebApi.csproj", "Blog.WebApi/"]
COPY ["src/Blog.Application/Blog.Application.csproj", "Blog.Application/"]
COPY ["src/Blog.Domain/Blog.Domain.csproj", "Blog.Domain/"]
COPY ["src/Blog.Persistance/Blog.Persistance.csproj", "Blog.Persistance/"]

RUN dotnet restore "Blog.WebApi/Blog.WebApi.csproj"

COPY ["src/Blog.WebApi/", "Blog.WebApi/"]
COPY ["src/Blog.Application/", "Blog.Application/"]
COPY ["src/Blog.Domain/", "Blog.Domain/"]
COPY ["src/Blog.Persistance/", "Blog.Persistance/"]
COPY ["src/Blog.UnitTest/", "Blog.UnitTest/"]

WORKDIR "/src/Blog.WebApi/"

RUN dotnet build "Blog.WebApi.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Blog.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Blog.WebApi.dll"]