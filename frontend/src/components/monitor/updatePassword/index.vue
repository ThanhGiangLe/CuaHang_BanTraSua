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
        :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
        :type="visible ? 'text' : 'password'"
        density="compact"
        placeholder="Enter your new password"
        prepend-inner-icon="mdi-lock-outline"
        variant="outlined"
        v-model="newConfirmPassword"
        @click:append-inner="visible = !visible"
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
      <!-- <v-btn
        color="red"
        size="large"
        variant="tonal"
        block
        @click="closeDialogInfo"
      >
        Close
      </v-btn> -->
    </div>
  </v-card>
</template>

<script setup>
import { useUserStore } from "@/stores/user.js";
import { useRouter } from "vue-router";
import { useGlobalStore } from "@/stores/app.js";
import axios from "axios";
import { computed, ref } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const visible = ref(false);
const router = useRouter();
const userStore = useUserStore();
const dialogVisibleInfo = ref(false);
const newPassword = ref("");
const newConfirmPassword = ref("");

// Lấy thông tin người dùng từ store
const user = computed(() => userStore.user);

// const closeDialogInfo = () => {
//   dialogVisibleInfo.value = false;
// };

async function UpdatePassword() {
  if (newPassword.value.trim() == "" || newConfirmPassword.value.trim() == "") {
    toast.warn("Please enter a valid Password.", {
      position: "top-right",
      autoClose: 3000,
      hideProgressBar: false, // Hiện thanh tiến trình
      closeOnClick: true, // Đóng khi nhấp vào thông báo
      pauseOnHover: true, // Dừng khi di chuột lên thông báo
      draggable: true, // Kéo thông báo
      progress: undefined, // Tiến độ (nếu có)
    });
    return;
  }
  if (newPassword.value !== newConfirmPassword.value) {
    toast.warn("Passwords do not match. Please try again.", {
      position: "top-right",
      autoClose: 3000,
      hideProgressBar: false, // Hiện thanh tiến trình
      closeOnClick: true, // Đóng khi nhấp vào thông báo
      pauseOnHover: true, // Dừng khi di chuột lên thông báo
      draggable: true, // Kéo thông báo
      progress: undefined, // Tiến độ (nếu có)
    });
    return;
  }
  try {
    const response = await axios.post(API_ENDPOINTS.UPDATE_PASSWORD, {
      Email: user.value.email,
      NewPassword: newConfirmPassword.value,
    });

    if (response.data.success) {
      toast.success("Password updated successfully!", {
        position: "top-right",
        autoClose: 2000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
      setTimeout(() => {
        router.push("/login");
      }, 2000);
    } else {
      toast.error("Failed to update password. Please check your OTP.", {
        position: "top-right",
        autoClose: 2000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
    }
  } catch (error) {
    toast.error(
      "An error occurred while updating the password. Please try again.",
      {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      }
    );
  }
}
</script>
