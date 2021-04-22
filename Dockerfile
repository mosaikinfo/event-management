FROM mcr.microsoft.com/dotnet/sdk:3.1-buster AS build

# Install NodeJS 10.x and NPM to build the Angular app
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - \
    && apt-get install -y nodejs

# Install node_modules
WORKDIR /source/backend/src/Web/ClientApp/
COPY backend/src/Web/ClientApp/*.json .
RUN npm install

# copy csproj and restore as distinct layers
WORKDIR /source
COPY backend/src/ApplicationCore/*.csproj ./backend/src/ApplicationCore/
COPY backend/src/Identity/*.csproj ./backend/src/Identity/
COPY backend/src/Infrastructure/*.csproj ./backend/src/Infrastructure/
COPY backend/src/Web/*.csproj ./backend/src/Web/
COPY ticket-generation/src/TicketGeneration/*.csproj ./ticket-generation/src/TicketGeneration/
RUN dotnet restore ./backend/src/Web/Web.csproj

# copy everything else and build app
COPY . .
RUN dotnet publish ./backend/src/Web/Web.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:3.1-buster-slim
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "EventManagement.WebApp.dll"]
