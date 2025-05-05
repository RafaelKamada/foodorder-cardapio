# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de solução e projetos
COPY ["FoodOrder_Cardapio.sln", "./"]
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Tests/Tests.csproj", "Tests/"]

# Restaura pacotes
RUN dotnet restore "Api/Api.csproj"

# Copia o resto dos arquivos
COPY . .

# Build
WORKDIR /src/Api
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Fase de publicação
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Fase final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia arquivos publicados
COPY --from=publish /app/publish .

# Expose a porta
EXPOSE 80
EXPOSE 443

# Comando para executar a aplicação
ENTRYPOINT ["dotnet", "Api.dll"]