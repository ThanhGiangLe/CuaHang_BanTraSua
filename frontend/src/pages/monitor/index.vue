<template>
  <v-card style="height: 100vh; max-width: 1280px; margin: 0 auto">
    <v-tabs v-model="tab" class="my-tabs">
      <v-tab prepend-icon="mdi-menu" value="listFood" class="my-vtab-custommer"
        >Menu</v-tab
      >
      <v-tab prepend-icon="mdi-food" value="food" class="my-vtab-custommer"
        >Món ăn</v-tab
      >
      <v-tab
        prepend-icon="mdi-map-outline"
        value="map"
        class="my-vtab-custommer"
        >Bàn</v-tab
      >
      <v-tab
        prepend-icon="mdi-file-chart-outline"
        value="report"
        class="my-vtab-custommer"
        >Báo cáo</v-tab
      >
      <v-tab
        prepend-icon="mdi-account-group"
        value="users"
        class="my-vtab-custommer"
        >Tài khoản</v-tab
      >
      <v-tab
        prepend-icon="mdi-currency-usd"
        value="currency"
        class="my-vtab-custommer"
        >Két tiền</v-tab
      >
      <v-tab
        prepend-icon="mdi-store-clock-outline"
        value="scheduleHistory"
        class="my-vtab-custommer"
        >Lịch sử lịch làm việc</v-tab
      >
      <v-tab prepend-icon="mdi-cog" value="settings" class="my-vtab-custommer"
        >Thay đổi mật khẩu</v-tab
      >
      <v-tab prepend-icon="mdi-logout" value="exit" class="my-vtab-custommer"
        >Đăng xuất</v-tab
      >
    </v-tabs>

    <v-card-text style="height: 100%" class="px-2 py-1">
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
          <MonitorTotalSalesManagement />
        </v-tabs-window-item>

        <v-tabs-window-item value="scheduleHistory">
          <ScheduleHistoryManagement />
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
import MonitorTotalSalesManagement from "@/components/monitor/totalSalesManagement/index.vue";
import ScheduleHistoryManagement from "@/components/monitor/scheduleHistoryManagement/index.vue";
import MonitorAreaManagement from "@/components/monitor/areaManagement/index.vue";
import MonitorUpdatePasswordManagement from "@/components/monitor/updatePassword/index.vue";
import LogoutUser from "@/components/monitor/logoutUser/index.vue";

const userStore = useUserStore();
const tab = ref("listFood");

const user = computed(() => userStore.user);
console.log("user", user.value);
</script>
