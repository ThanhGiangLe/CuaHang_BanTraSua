<template>
  <v-card style="height: 100vh; max-width: 1280px; margin: 0 auto">
    <v-tabs v-model="tab">
      <v-tab prepend-icon="mdi-menu" value="listFood">Danh sách món ăn</v-tab>
      <v-tab prepend-icon="mdi-food" value="food">Quản lý món ăn</v-tab>
      <v-tab prepend-icon="mdi-map-outline" value="map">Khu vực</v-tab>
      <v-tab prepend-icon="mdi-file-chart-outline" value="report"
        >Báo cáo</v-tab
      >
      <v-tab prepend-icon="mdi-account-group" value="users">Nhân viên</v-tab>
      <v-tab prepend-icon="mdi-currency-usd" value="currency">Két tiền</v-tab>
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

import ListFoodManagement from "@/components/monitor/listFood/index.vue";
import MonitorFoodManagement from "@/components/monitor/foodManagement/index.vue";
import MonitorReportManagement from "@/components/monitor/reportManagement/index.vue";
import MonitorEmployeeManagement from "@/components/monitor/employeeManagement/index.vue";
import MonitorSalesManagement from "@/components/monitor/salesManagement/index.vue";
import MonitorCashRegisterManagement from "@/components/monitor/cashRegisterManagement/index.vue";
import MonitorInventoryManagement from "@/components/monitor/inventoryManagement/index.vue";
import MonitorAreaManagement from "@/components/monitor/areaManagement/index.vue";
import MonitorTaxFeesDiscountsManagement from "@/components/monitor/taxFeesDiscounts/index.vue";
import MonitorUpdatePasswordManagement from "@/components/monitor/updatePassword/index.vue";
import LogoutUser from "@/components/monitor/logoutUser/index.vue";

const userStore = useUserStore();
const tab = ref("listFood");

const user = computed(() => userStore.user);
console.log("user", user.value);
</script>
