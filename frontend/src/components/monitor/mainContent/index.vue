<template>
  <div class="d-flex align-center justify-center h-100">
    <h1>Hello {{ user.fullName }}</h1>
  </div>
</template>

<script setup>
import { useUserStore } from "@/stores/user.js";
const userStore = useUserStore();

const user = computed(() => userStore.user);
</script>
