<template></template>
<script setup>
import { useUserStore } from "@/stores/user.js";
import { useRouter } from "vue-router";
import { computed, ref } from "vue";
import "vue3-toastify/dist/index.css";

const router = useRouter();
const userStore = useUserStore();

// Lấy thông tin người dùng từ store
const user = computed(() => userStore.user);
sessionStorage.removeItem("token");
userStore.clearUser();
router.push("/login");
</script>
