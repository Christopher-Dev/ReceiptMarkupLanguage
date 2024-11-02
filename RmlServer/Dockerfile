# Base image for running the app in fast mode (default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Switch to root user to install packages and add fonts
USER root
RUN apt-get update && \
    apt-get install -y libfontconfig1 fontconfig

# Copy the Fonts folder from RmlServer project into the container's fonts directory
COPY RmlServer/Fonts /usr/share/fonts/truetype/custom_fonts

# Refresh the font cache to recognize the newly added fonts
RUN fc-cache -f -v

# Switch back to app user
USER app

# Build stage for building the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RmlServer/RmlServer.csproj", "RmlServer/"]
COPY ["RmlCommon/RmlCommon.csproj", "RmlCommon/"]
COPY ["RmlEditorWeb/RmlEditorWeb.csproj", "RmlEditorWeb/"]
RUN dotnet restore "./RmlServer/RmlServer.csproj"
COPY . .
WORKDIR "/src/RmlServer"
RUN dotnet build "./RmlServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage to create a publish-ready output
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RmlServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage for production, using the base image that already has libfontconfig1 installed
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RmlServer.dll"]
