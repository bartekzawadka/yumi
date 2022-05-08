FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY . .
EXPOSE 9000
ENTRYPOINT ["dotnet", "Yumi.Api.dll", "--urls=http://0.0.0.0:9000/"]
