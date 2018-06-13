FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app

# Copy csproj files restore
COPY *.sln ./
COPY SharkFit.Data/*.csproj SharkFit.Data/
COPY SharkFit.Web/*.csproj SharkFit.Web/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish SharkFit.Web/SharkFit.Web.csproj -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime

WORKDIR /app
COPY --from=builder /app/SharkFit.Web/out .
ENTRYPOINT ["dotnet", "SharkFit.Web.dll"]
