<template>
  <v-card class="pa-5" max-width="700px" style="margin: 0 auto">
    <div>
      <div
        class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between"
      >
        Password
      </div>
      <v-text-field
        :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
        :type="visible ? 'text' : 'password'"
        density="compact"
        placeholder="Enter your new password"
        prepend-inner-icon="mdi-lock-outline"
        variant="outlined"
        v-model="newPassword"
        @click:append-inner="visible = !visible"
      ></v-text-field>

      <div
        class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between"
      >
        Confirm Password
      </div>
      <v-text-field
        :append-inner-icon="visibleConfirm ? 'mdi-eye-off' : 'mdi-eye'"
        :type="visibleConfirm ? 'text' : 'password'"
        density="compact"
        placeholder="Enter your new password"
        prepend-inner-icon="mdi-lock-outline"
        variant="outlined"
        v-model="newConfirmPassword"
        @click:append-inner="visibleConfirm = !visibleConfirm"
      ></v-text-field>

      <v-btn
        class="mb-4 mt-4"
        color="blue"
        size="large"
        variant="tonal"
        block
        @click="UpdatePassword()"
      >
        Update Password
      </v-btn>
    </div>
  </v-card>
</template>

<script setup>
import { useUserStore } from "@/stores/user.js";
import { useRouter } from "vue-router";
import axios from "axios";
import { computed, ref } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { showToast } from "@/styles/handmade";
import { useLogin } from "/src/composables/authen/useLogin.js";

const { updatePassword } = useLogin();

const visible = ref(false);
const visibleConfirm = ref(false);
const router = useRouter();
const userStore = useUserStore();
const dialogVisibleInfo = ref(false);
const newPassword = ref("");
const newConfirmPassword = ref("");

const user = computed(() => userStore.user);

async function UpdatePassword() {
  if (newPassword.value.trim() == "" || newConfirmPassword.value.trim() == "") {
    showToast("Không được để trống thông tin!", "warn");
    return;
  }
  if (newPassword.value !== newConfirmPassword.value) {
    showToast("Mật khẩu không trùng khớp!", "warn");
    return;
  }
  try {
    const request = {
      Email: user.value.email,
      NewPassword: newConfirmPassword.value,
    };
    const response = await updatePassword(request);

    if (response.result.success) {
      showToast("Thay đổi mật khẩu thành công!", "success");
      setTimeout(() => {
        router.push("/login");
      }, 2000);
    } else {
      showToast("Thay đổi mật khẩu thất bại!", "error");
    }
  } catch (error) {
    showToast("Lỗi hệ thống!", "error");
  }
}
</script>
