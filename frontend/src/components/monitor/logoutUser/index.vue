<template></template>
<script setup>
import { useUserStore } from "@/stores/user.js";
import { useRouter } from "vue-router";
import { computed, ref } from "vue";
import "vue3-toastify/dist/index.css";
import axios from "axios";
import API_ENDPOINTS from "@/api/api";
import { showToast } from "@/styles/handmade";

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
  showToast("Có lỗi!", "error");
} else {
  sessionStorage.removeItem("token");
  userStore.clearUser();
  router.push("/login");
}
</script>
