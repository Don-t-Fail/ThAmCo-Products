FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim
COPY products/ /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "ThAmCo.Products.dll"]