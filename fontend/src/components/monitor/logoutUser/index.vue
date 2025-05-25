<template></template>
<script setup>
import { useUserStore } from "@/stores/user.js";
import { useRouter } from "vue-router";
import { computed, ref } from "vue";
import "vue3-toastify/dist/index.css";
import axios from "axios";
import API_ENDPOINTS from "@/api/api";

const router = useRouter();
const userStore = useUserStore();

// Lấy thông tin người dùng từ store
const user = computed(() => userStore.user);
console.log("user", user.value);
const response = await axios.post(API_ENDPOINTS.UPDATE_ENDTIME_CASH_REGISTER, {
  UserId: user.value.userId,
});
console.log("response", response.data);
if (response.data.message !== 1) {
  toast.error("Error update Cash Register!", {
    position: "top-right",
    autoClose: 3000,
    hideProgressBar: false, // Hiện thanh tiến trình
    closeOnClick: true, // Đóng khi nhấp vào thông báo
    pauseOnHover: true, // Dừng khi di chuột lên thông báo
    draggable: true, // Kéo thông báo
    progress: undefined, // Tiến độ (nếu có)
  });
} else {
  userStore.clearUser();
  router.push("/login");
}
</script>
