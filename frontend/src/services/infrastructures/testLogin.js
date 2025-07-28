import http from "k6/http";
import { sleep, check } from "k6";

export let options = {
  vus: 10, // số lượng user ảo chạy cùng lúc
  duration: "30s", // chạy trong 30 giây
};

export default function () {
  let url = "http://localhost:5248/api/user/login";

  let payload = JSON.stringify({
    phone: "0368555059",
    password: "123456",
  });

  let params = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  let res = http.post(url, payload, params);

  // Kiểm tra kết quả trả về
  check(res, {
    "status is 200": (r) => r.status === 200,
    "response time < 500ms": (r) => r.timings.duration < 500,
  });

  sleep(1); // nghỉ 1 giây trước khi user lặp lại request
}
