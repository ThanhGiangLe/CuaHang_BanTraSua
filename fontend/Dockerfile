# Dùng Node.js để xây dựng ứng dụng frontend
FROM node:16 AS build

# Đặt thư mục làm việc
WORKDIR /app

# Sao chép các tệp cấu hình (package.json, package-lock.json, nếu có) vào container
COPY package*.json ./

# Cài đặt các dependencies bằng npm
RUN npm install

# Sao chép toàn bộ mã nguồn frontend vào container
COPY . ./

# Build ứng dụng frontend bằng npm
RUN npm run build

# Sử dụng nginx để chạy ứng dụng
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
