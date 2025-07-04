<template>
  <div class="d-flex align-center justify-center" style="min-height: 100vh">
    <v-card
      class="mx-auto pa-12 pb-8"
      elevation="8"
      max-width="448"
      min-width="448"
      rounded="lg"
    >
      <!-- Nhập Email để nhận mã OTP -->
      <div v-if="visibleFillMail">
        <div class="text-subtitle-1 text-medium-emphasis">Email</div>
        <v-text-field
          density="compact"
          placeholder="Nhập email đã đăng ký tài khoản..."
          prepend-inner-icon="mdi-email-outline"
          variant="outlined"
          v-model="email"
        ></v-text-field>

        <router-link
          class="text-caption text-decoration-none text-blue"
          to="/login"
        >
          <v-icon>mdi-arrow-left</v-icon>
          Về trang ĐĂNG NHẬP
        </router-link>

        <v-btn
          class="mb-8 mt-8"
          color="blue"
          size="large"
          variant="tonal"
          block
          @click="sendOTP()"
        >
          Nhận OTP
        </v-btn>
      </div>

      <!-- Mail tồn tại, thực hiện nhập mã OTP -->
      <div v-else>
        <div v-if="visibleFillPassword">
          <v-otp-input v-model="enteredOtp" focus-all focused></v-otp-input>
          <v-btn
            class="mt-4"
            color="blue"
            size="large"
            variant="tonal"
            block
            @click="verifyOTP()"
            v-if="visibleResendOtp == false"
          >
            Xác thực OTP
          </v-btn>
          <v-btn
            class="mt-4"
            color="blue"
            size="large"
            variant="tonal"
            block
            @click="sendOTP()"
            v-if="visibleResendOtp == true"
          >
            Gửi lại OTP
          </v-btn>
        </div>

        <!-- Sau khi nhập mã OTP thành công. Thực hiện thay đổi mật khẩu mới -->
        <div v-else>
          <div
            class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between"
          >
            Mật khẩu
          </div>
          <v-text-field
            :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
            :type="visible ? 'text' : 'password'"
            density="compact"
            placeholder="Nhập mật khẩu mới..."
            prepend-inner-icon="mdi-lock-outline"
            variant="outlined"
            v-model="password"
            @click:append-inner="visible = !visible"
          ></v-text-field>

          <div
            class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between"
          >
            Xác nhận lại mật khẩu
          </div>
          <v-text-field
            :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
            :type="visible ? 'text' : 'password'"
            density="compact"
            placeholder="Xác nhận mật khẩu..."
            prepend-inner-icon="mdi-lock-outline"
            variant="outlined"
            v-model="confirmPassword"
            @click:append-inner="visible = !visible"
          ></v-text-field>

          <v-btn
            class="mb-8 mt-8"
            color="blue"
            size="large"
            variant="tonal"
            block
            @click="UpdatePassword()"
          >
            Cập nhật mật khẩu
          </v-btn>
        </div>
      </div>
    </v-card>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import emailjs from "emailjs-com";
import "vue3-toastify/dist/index.css";
import { showToast } from "@/styles/handmade";
import { useLogin } from "/src/composables/authen/useLogin.js";

const { checkEmail, updatePassword } = useLogin();
const router = useRouter();
const visible = ref(false); // Dùng để hiển thị mật khẩu...
const email = ref(""); // email để đăng nhập hoặc xác thực
const password = ref(""); // Mật khẩu
const confirmPassword = ref(""); // confirm mật khẩu
const enteredOtp = ref("");
const otp = ref("");

const visibleFillMail = ref(true);
const visibleFillPassword = ref(true);

// Thông tin tài khoản EmailJS
const serviceID = "service_cojqzzb";
const templateID = "template_bql1hr5";
const publicKey = "YVFyP3Zy91mr0Jc5W";

emailjs.init(publicKey);

const otpSendTime = ref(null); // Lưu thời gian lúc gửi OTP
const otpTimeout = 1 * 60 * 1000; // Thời gian 1 phút
const visibleResendOtp = ref(false);

async function sendOTP() {
  function generateOTP() {
    return Math.floor(100000 + Math.random() * 900000).toString(); // Mã OTP 6 chữ số
  }
  if (!email.value) {
    showToast("Vui lòng nhập Email!", "warn");
    return;
  }
  const request = {
    email: email.value,
  };
  const response = await checkEmail(request);
  console.log("response: ", response);
  try {
    if (response.exists) {
      otpSendTime.value = new Date().getTime(); // dùng để xác định thời gian gửi mã OTP đến người dùng
      otp.value = generateOTP();
      const templateParams = {
        email: email.value,
        otp_code: otp.value,
      };

      emailjs
        .send(serviceID, templateID, templateParams)
        .then((response) => {
          showToast("Hãy kiểm tra thông báo email!", "success");
          visibleFillMail.value = false;
          visibleResendOtp.value = false;
        })
        .catch((err) => {
          showToast("Có lỗi trong quá trình gửi mail.", "warn");
        });
    } else {
      showToast("Email không tồn tại.", "error");
    }
  } catch (error) {
    showToast("Lỗi trong quá trình xác minh tài khoản.", "error");
  }
}

function verifyOTP() {
  if (!enteredOtp.value) {
    showToast("Nhập mã nhận được từ email!", "warn");
    return;
  }
  let currentTime = new Date().getTime();
  if (currentTime - otpSendTime.value > otpTimeout) {
    showToast("OTP đã hết thời gian sử dụng!", "error");
    visibleResendOtp.value = true;
    otp.value = "";
    enteredOtp.value = "";
    return;
  }
  if (enteredOtp.value === otp.value) {
    visibleFillPassword.value = false;
    otp.value = ""; // Reset mã OTP
  } else {
    showToast("OTP không chính xác!", "warn");
  }
}

async function UpdatePassword() {
  if (password.value.trim() == "" || confirmPassword.value.trim() == "") {
    showToast("Hãy nhập đầy đủ thông tin yêu cầu!", "warn");
    return;
  }
  if (password.value !== confirmPassword.value) {
    showToast("Mật khẩu xác nhận không trùng khớp!", "warn");
    return;
  }
  try {
    const request = {
      Email: email.value,
      NewPassword: confirmPassword.value,
    };
    const response = await updatePassword(request);

    if (response.result.success) {
      showToast("Thay đổi mật khẩu thành công!", "success");
      setTimeout(() => {
        router.push("/login");
      }, 1500);
    } else {
      showToast("Lỗi trong quá trình cập nhật mật khẩu!", "error");
    }
  } catch (error) {
    showToast("Lỗi hệ thống!", "error");
  }
}
</script>
