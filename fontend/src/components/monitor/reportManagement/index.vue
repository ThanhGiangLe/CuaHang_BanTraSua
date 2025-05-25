<template>
  <div
    id="reportManagement"
    class="mt-1 d-flex flex-column"
    style="width: 100%; height: 100vh"
  >
    <div class="d-flex justify-center flex-wrap">
      <v-btn
        @click="valueVisiable = 1"
        variant="elevated"
        color="indigo-darken-4"
        class="mx-3 mb-1"
      >
        <v-icon class="ma-1">mdi-file-chart-outline</v-icon>
        Xem báo cáo doanh thu
      </v-btn>
      <v-btn
        @click="valueVisiable = 2"
        variant="elevated"
        color="indigo-darken-4"
        class="mx-3 mb-1"
      >
        <v-icon class="ma-1">mdi-chart-line</v-icon>
        Xem báo cáo các món bán chạy
      </v-btn>
      <v-btn
        @click="valueVisiable = 3"
        variant="elevated"
        color="indigo-darken-4"
        class="mx-3 mb-1"
      >
        <v-icon class="ma-1">mdi-file-document</v-icon>
        Xem báo cáo tổng hóa đơn
      </v-btn>
      <v-btn
        @click="valueVisiable = 4"
        variant="elevated"
        color="indigo-darken-4"
        class="mx-3 mb-1"
      >
        <v-icon class="ma-1">mdi-warehouse</v-icon>
        Xem báo cáo tồn kho
      </v-btn>
    </div>

    <div style="width: 100%; margin-bottom: 52px" class="flex-1-0">
      <ReportRevenue v-if="valueVisiable == 1" />
      <BestSellingItemsReport v-if="valueVisiable == 2" />
      <BillingReport v-if="valueVisiable == 3" />
      <InventoryReport v-if="valueVisiable == 4" />
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import ReportRevenue from "./reportRevenue.vue";
import BestSellingItemsReport from "./bestSellingItemsReport.vue";
import InventoryReport from "./inventoryReport.vue";
import BillingReport from "./billingReport.vue";
const valueVisiable = ref(1);
</script>
