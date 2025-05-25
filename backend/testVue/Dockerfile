# Sử dụng .NET SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Đặt thư mục làm việc
WORKDIR /testVue

# Sao chép file dự án .csproj vào container
COPY ./testVue.csproj ./

# Thực hiện restore các dependency
RUN dotnet restore

# Sao chép mã nguồn còn lại vào container
COPY ./ ./

# Build ứng dụng
RUN dotnet publish -c Release -o /out

# Sử dụng .NET Runtime để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out ./

# Mở cổng 5000
EXPOSE 5000

# Chạy ứng dụng
CMD ["dotnet", "testVue.dll"]
