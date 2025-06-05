<template></template>
<script setup>
import { useUserStore } from "@/stores/user.js";
import { useRouter } from "vue-router";
import { computed, ref } from "vue";
import "vue3-toastify/dist/index.css";
import axios from "axios";
import API_ENDPOINTS from "@/api/api";
import { showToast } from "@/styles/handmade";
import axiosClient from "@/services/utils/axiosClient";

const router = useRouter();
const userStore = useUserStore();

// Lấy thông tin người dùng từ store
const user = computed(() => userStore.user);
console.log("user", user.value);
sessionStorage.removeItem("token");
userStore.clearUser();
router.push("/login");
</script>
