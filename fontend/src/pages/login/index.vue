<template>
  <div class="d-flex align-center justify-center" style="min-height: 100vh">
    <v-card
      class="mx-auto pa-12 pb-8 bg-grey-darken-4"
      elevation="8"
      max-width="448"
      rounded="lg"
      @keydown.enter="verifyLoginAccount"
    >
      <div class="text-subtitle-1 text-medium-emphasis">Số điện thoại</div>
      <v-text-field
        density="compact"
        placeholder="Nhập tại đây..."
        prepend-inner-icon="mdi-phone-outline"
        variant="outlined"
        v-model="phone"
      ></v-text-field>

      <div
        class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between"
      >
        Mật khẩu
        <router-link
          class="text-caption text-decoration-none text-blue"
          to="/forgotpassword"
        >
          Quên mật khẩu?
        </router-link>
      </div>
      <v-text-field
        :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
        :type="visible ? 'text' : 'password'"
        density="compact"
        placeholder="Nhập tại đây..."
        prepend-inner-icon="mdi-lock-outline"
        variant="outlined"
        v-model="password"
        @click:append-inner="visible = !visible"
      ></v-text-field>

      <v-card class="mb-12" color="surface-variant" variant="tonal">
        <v-card-text
          class="text-medium-emphasis text-caption text-justify"
          style="color: #ffee58 !important"
        >
          <strong>Cảnh báo:</strong> Sau 3 lần đăng nhập thất bại liên tiếp, sẽ
          có thông báo được gửi đến chủ cửa hàng/quản lý. Nếu bạn cần đăng nhập
          ngay bây giờ, bạn cũng có thể nhấp vào "Quên mật khẩu?" bên dưới để
          đặt lại mật khẩu.
        </v-card-text>
      </v-card>

      <v-alert v-if="errorMessage" type="error" outlined>
        {{ errorMessage }}
      </v-alert>
    </v-card>

    <!-- Nút bay loạn xạ -->
    <v-btn
      :style="btnStyle"
      color="grey-lighten-2"
      size="large"
      variant="tonal"
      v-if="showButtonLogin"
      @click="verifyLoginAccount"
      @mouseenter="onHoverLoginBtn"
    >
      Đăng nhập
    </v-btn>

    <!-- Loading Overlay -->
    <v-overlay
      :model-value="isOverlay"
      persistent
      class="justify-center align-center"
    >
      <v-progress-circular indeterminate size="48" width="6" color="primary" />
    </v-overlay>
  </div>
</template>

<script setup>
import { useUserStore } from "@/stores/user";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { showToast } from "@/styles/handmade";

const userStore = useUserStore();
const router = useRouter();

const visible = ref(false);
const phone = ref("");
const password = ref("");
const errorMessage = ref("");
const isOverlay = ref(false);
const showButtonLogin = ref(true);
const quantityLogin = ref(0);

// ✅ Style nút có thể thay đổi vị trí
const btnStyle = ref({
  position: "fixed",
  top: "60%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  transition: "top 0.3s ease, left 0.3s ease",
});

const isValid = computed(() => phone.value && password.value);

function onHoverLoginBtn() {
  if (!isValid.value) {
    const btnWidth = 120;
    const btnHeight = 50;
    const maxX = window.innerWidth - btnWidth;
    const maxY = window.innerHeight - btnHeight;

    const randomX = Math.floor(Math.random() * maxX);
    const randomY = Math.floor(Math.random() * maxY);

    btnStyle.value.top = `${randomY}px`;
    btnStyle.value.left = `${randomX}px`;
  }
}

// ✅ Hàm đăng nhập
async function verifyLoginAccount() {
  if (!isValid.value) {
    showToast("Vui lòng nhập số điện thoại và mật khẩu!", "warn");
    return;
  }

  try {
    const response = await axios.post(API_ENDPOINTS.LOGIN, {
      Phone: phone.value,
      Password: password.value,
    });

    const user = response.data;
    userStore.setUser(user);
    isOverlay.value = true;

    const response2 = await axios.post(API_ENDPOINTS.IMPORT_CASH_REGISTER, {
      UserId: user.userId,
    });

    if (response2.data.message !== 1) {
      showToast("Lỗi ghi nhận thông tin Quản lý két", "error");
    }
    errorMessage.value = "";
    quantityLogin.value = 0;
    setTimeout(() => {
      router.push("/monitor");
    }, 1000);
  } catch (error) {
    quantityLogin.value++;
    if (quantityLogin.value > 3) {
      errorMessage.value =
        "Bạn đã nhập sai quá 3 lần. Liên hệ Quản lý hoặc Chủ cửa hàng để thay đổi mật khẩu.";
      showButtonLogin.value = false;
    } else {
      errorMessage.value = "";
    }
    if (error.response && error.response.status === 401) {
      showToast("Số điện thoại hoặc mật khẩu không đúng!", "error");
    } else {
      showToast("Lỗi hệ thông. Vui lòng thử lại!", "error");
    }
  }
}
</script>
