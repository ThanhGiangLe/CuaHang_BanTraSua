<template>
  <v-card style="height: 100vh; max-width: 1280px; margin: 0 auto">
    <v-tabs v-model="tab">
      <v-tab prepend-icon="mdi-menu" value="listFood">Danh sách món ăn</v-tab>
      <v-tab prepend-icon="mdi-food" value="food">Quản lý món ăn</v-tab>
      <v-tab
        prepend-icon="mdi-map-outline"
        value="map"
        v-if="user.role !== 'Staff'"
        >Khu vực</v-tab
      >
      <v-tab
        prepend-icon="mdi-file-chart-outline"
        value="report"
        v-if="user.role !== 'Staff'"
        >Báo cáo</v-tab
      >
      <v-tab
        prepend-icon="mdi-account-group"
        value="users"
        v-if="user.role !== 'Staff'"
        >Nhân viên</v-tab
      >
      <v-tab
        prepend-icon="mdi-currency-usd"
        value="currency"
        v-if="user.role !== 'Staff'"
        >Két tiền</v-tab
      >
      <v-tab prepend-icon="mdi-cog" value="settings">Thay đổi mật khẩu</v-tab>
      <v-tab prepend-icon="mdi-logout" value="exit">Đăng xuất</v-tab>
    </v-tabs>

    <v-card-text style="height: 100%" class="pa-2">
      <v-tabs-window v-model="tab" style="height: 100%">
        <v-tabs-window-item value="listFood" style="height: 100%">
          <ListFoodManagement />
        </v-tabs-window-item>

        <v-tabs-window-item value="food" style="height: 100%">
          <MonitorFoodManagement />
        </v-tabs-window-item>

        <v-tabs-window-item value="map">
          <MonitorAreaManagement />
        </v-tabs-window-item>

        <v-tabs-window-item value="report" style="height: 100%">
          <MonitorReportManagement />
        </v-tabs-window-item>

        <v-tabs-window-item value="users" style="height: 100%">
          <MonitorEmployeeManagement />
        </v-tabs-window-item>

        <v-tabs-window-item value="currency">
          <MonitorCashRegisterManagement />
        </v-tabs-window-item>

        <v-tabs-window-item value="settings">
          <MonitorUpdatePasswordManagement />
        </v-tabs-window-item>

        <v-tabs-window-item value="exit">
          <LogoutUser />
        </v-tabs-window-item>
      </v-tabs-window>
    </v-card-text>
  </v-card>
</template>
<script setup>
import { useUserStore } from "@/stores/user.js";
import { computed, ref } from "vue";
import "vue3-toastify/dist/index.css";

import ListFoodManagement from "@/components/monitor/listFood/index.vue"; // 0
import MonitorFoodManagement from "@/components/monitor/foodManagement/index.vue"; // 1
import MonitorReportManagement from "@/components/monitor/reportManagement/index.vue"; // 2
import MonitorEmployeeManagement from "@/components/monitor/employeeManagement/index.vue"; // 3
import MonitorSalesManagement from "@/components/monitor/salesManagement/index.vue"; // 4
import MonitorCashRegisterManagement from "@/components/monitor/cashRegisterManagement/index.vue"; // 5
import MonitorInventoryManagement from "@/components/monitor/inventoryManagement/index.vue"; // 6
import MonitorAreaManagement from "@/components/monitor/areaManagement/index.vue"; // 7
import MonitorTaxFeesDiscountsManagement from "@/components/monitor/taxFeesDiscounts/index.vue"; // 8
import MonitorUpdatePasswordManagement from "@/components/monitor/updatePassword/index.vue"; // 9
import LogoutUser from "@/components/monitor/logoutUser/index.vue"; // 10

const userStore = useUserStore();
const tab = ref("main");

// Lấy thông tin người dùng từ store
const user = computed(() => userStore.user);
console.log("user", user.value);
</script>
