import http from "k6/http";
import { sleep, check } from "k6";

export let options = {
  vus: 5,
  duration: "300s",
};

export default function () {
  let urlOrder = "http://localhost:5248/api/food/add-order";
  let urlOrderDetail = "http://localhost:5248/api/food/add-order-detail";

  let payloadOrder = JSON.stringify({
    userId: 1012,
    orderTime: "2025-07-20T22:00:12",
    tableId: 1,
    totalAmount: 43000,
    totalResult: 43000,
    status: "",
    discount: 0,
    tax: 0,
    receivedAmount: 50000,
    returnedAmount: 7000,
    paymentMethod: "Tiền mặt",
    phone: "",
  });

  let params = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  // 🔴 1. Gọi API add-order trước
  let resOrder = http.post(urlOrder, payloadOrder, params);

  check(resOrder, {
    "Add order status is 200": (r) => r.status === 200,
    "Add order response time < 500ms": (r) => r.timings.duration < 500,
  });

  // 🔴 3. Gọi API add-order-detail
  let payloadOrderDetail = JSON.stringify({
    orderId: 302,
    foodItemId: 1107, // Thay bằng id thật
    quantity: 2,
    price: 5000,
    isMainItem: 1,
    unit: "Phần",
    note: "Không đá",
    categoryId: 23,
    orderTime: "2025-07-20T22:00:12",
  });

  let resOrderDetail = http.post(urlOrderDetail, payloadOrderDetail, params);

  check(resOrderDetail, {
    "Add order detail status is 200": (r) => r.status === 200,
    "Add order detail response time < 500ms": (r) => r.timings.duration < 500,
  });

  sleep(3.5); // nghỉ 3.5 giây trước khi user lặp lại
}
