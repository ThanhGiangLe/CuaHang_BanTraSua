<template>
  <div
    class="reportManagement_bestsellingItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
      <v-card-title class="pa-0 mb-2 d-flex justify-center">
        <div class="d-flex align-center">
          <v-icon class="ma-1" size="large">mdi-chart-line</v-icon>
          <span style="font-size: 26px">Báo cáo mặt hàng bán chạy</span>
        </div>
        <!-- <JsonExcel class="btn btn-default" 
                  :data="dataTable" 
                  :fields="datafieldExcel" 
                  worksheet="My Worksheet" type="xlsx"
                  :name="nameFileExcel">
                  <VBtn class="text-none" size="small" prependIcon="mdi-crop" color="#8690A0">
                  Xuất Excel
                  </VBtn>
              </JsonExcel> -->
      </v-card-title>
      <v-card-text
        class="pa-3 rounded"
        :style="{ backgroundColor: 'var(--bg-color-item)' }"
      >
        <div
          class="reportManagement_chosseTime d-flex justify-md-space-between mb-2"
        >
          <div>
            <!-- Lọc cho hôm nay -->
            <v-btn
              style="border: 1px solid #333"
              size="small"
              @click="selectCurrentDayAndCallAPI"
            >
              Hôm nay
            </v-btn>
            <!-- Lọc theo ngày -->
            <v-menu>
              <template v-slot:activator="{ props }">
                <v-btn
                  v-bind="props"
                  append-icon="mdi-chevron-down"
                  min-width="90px"
                  style="border: 1px solid #333"
                  size="small"
                  class="ms-2"
                >
                  {{ selectedDay || "Ngày" }}
                </v-btn>
              </template>
              <v-list max-height="200px" style="overflow-y: auto">
                <v-list-item
                  v-for="day in dateList"
                  :key="day"
                  :value="day"
                  style="min-height: 36px !important"
                  @click="selectDayAndCallAPI(day)"
                >
                  <v-list-item-title style="font-size: 0.8rem">{{
                    day
                  }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
            <!-- Lọc theo tháng -->
            <v-menu>
              <template v-slot:activator="{ props }">
                <v-btn
                  v-bind="props"
                  append-icon="mdi-chevron-down"
                  min-width="90px"
                  style="border: 1px solid #333"
                  size="small"
                  class="ms-2"
                >
                  {{ selectedMonth || "Tháng" }}
                </v-btn>
              </template>
              <v-list max-height="200px" style="overflow-y: auto">
                <v-list-item
                  v-for="(month, index) in monthList"
                  :key="index"
                  :value="month"
                  style="min-height: 36px !important"
                  @click="selectMonthAndCallAPI(month)"
                >
                  <v-list-item-title>{{ month }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>
          <v-btn
            style="border: 1px solid #333; min-width: 60px"
            size="small"
            class="ms-5"
            @click="resetTimeFillterRevenueOrder"
          >
            Làm mới
          </v-btn>
        </div>
        <div
          class="reportManagement_totalAmount_salesSummary_bestSellingItems d-flex"
          style="height: 450px; max-height: 450px; overflow-y: auto"
        >
          <v-data-table
            :headers="headersBestSeling"
            :loading="loading"
            :items="allItemBestSeling"
            height="calc(33vh - 2rem)"
            density="compact"
            fixed-footer
            fixed-header
          >
            <template v-slot:item.foodName="{ item }">
              <span
                style="font-weight: 500; color: rgba(var(--v-theme-primary), 1)"
              >
                {{ item.foodName ? item.foodName : "-" }}
              </span>
            </template>
            <template v-slot:item.quantitySold="{ item }">
              <div class="d-user-contract-inventory-chart-progress-bar">
                <div
                  class="d-user-contract-inventory-chart-progress-bar-container"
                >
                  <div
                    class="d-user-contract-inventory-chart-progress-bar-indicator"
                    :style="{
                      width: (item.quantitySold / quantitySoldMax) * 100 + '%',
                    }"
                  ></div>
                </div>
                <div class="d-user-contract-inventory-chart-progress-bar-text">
                  {{ item.quantitySold ? item.quantitySold : 0 }}
                </div>
              </div>
            </template>
            <template v-slot:loading>
              <v-skeleton-loader type="table-row@5"></v-skeleton-loader>
            </template>
            <template v-slot:no-data>
              <div
                class="d-event-info-item d-emp-activity-item-content d-emp-activity-no-data pa-6"
                style="background: none"
              >
                <!-- <VIcon icon="mdi-robot-dead-outline"></VIcon> -->
                <span>Hệ thống không tìm thấy thông tin</span>
              </div>
            </template>
          </v-data-table>
        </div>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup>
import { ref } from "vue";
import axios from "axios";
import { computed } from "vue";
import API_ENDPOINTS from "@/api/api.js";
const allItemBestSeling = ref([]);
const loading = shallowRef(true);
const quantitySoldMax = ref(0);
const currentDate = new Date();
const currentMonth = currentDate.getMonth() + 1; // Tháng hiện tại (cộng thêm 1 vì getMonth() trả về giá trị từ 0 đến 11)
const currentYear = currentDate.getFullYear(); // Năm hiện tại
const selectedCurrentDay = ref("");
const selectedDay = ref("");
const selectedMonth = ref("");

// Cho biết số ngày trong tháng và năm hiện tại
const generateDates = (month, year) => {
  const daysInMonth = new Date(year, month, 0).getDate(); // Lấy số ngày trong tháng
  const dates = [];
  for (let day = 1; day <= daysInMonth; day++) {
    let formatDay = day.toString().padStart(2, "0");
    let formatMonth = month.toString().padStart(2, "0");
    dates.push(`${formatDay}-${formatMonth}-${year}`);
  }
  return dates;
};
// Cho biết số tháng trong năm
const generateMonths = (year) => {
  const months = [];
  for (let month = 1; month <= 12; month++) {
    let formatMonth = month.toString().padStart(2, "0");
    months.push(`${formatMonth}-${year}`);
  }
  return months;
};
const dateList = ref(generateDates(currentMonth, currentYear));
const monthList = ref(generateMonths(currentYear));

const headersBestSeling = ref([
  { title: "Tên món ăn", key: "foodName", width: "35%" },
  { title: "Số lượng bán ra", key: "quantitySold", width: "65%" },
]);

async function init() {
  const monthFormat = currentMonth.toString().padStart(2, "0"); // Đảm bảo tháng có 2 chữ số
  const response = await axios.post(
    API_ENDPOINTS.GET_ALL_ORDERITEM_BESTSELING_CURRENTMONTH,
    {
      date: `${monthFormat}-${currentYear}`,
    }
  );
  allItemBestSeling.value = response.data;

  const quantitySoldMaxDataTable = allItemBestSeling.value
    ? allItemBestSeling.value.reduce((max, current) => {
        return current.quantitySold > max.quantitySold ? current : max;
      }, allItemBestSeling.value[0])
    : null;
  quantitySoldMax.value = quantitySoldMaxDataTable
    ? quantitySoldMaxDataTable.quantitySold
    : 0;

  loading.value = false;
}
init();

async function selectCurrentDayAndCallAPI() {
  const day = currentDate.getDate();
  // Định dạng lại thành "ngày/tháng/năm"
  const formattedDate = `${day < 10 ? "0" + day : day}-${
    currentMonth < 10 ? "0" + currentMonth : currentMonth
  }-${currentYear}`;
  selectedCurrentDay.value = formattedDate;
  let [dayf, monthf, yearf] = selectedCurrentDay.value.split("-");
  dayf = dayf.padStart(2, "0");
  monthf = monthf.padStart(2, "0");
  selectedCurrentDay.value = `${dayf}-${monthf}-${yearf}`;

  const response = await axios.post(
    API_ENDPOINTS.GET_ALL_ORDERITEM_BESTSELING_CURRENTDAY,
    {
      date: selectedCurrentDay.value,
    }
  );
  allItemBestSeling.value = response.data;

  const quantitySoldMaxDataTable = allItemBestSeling.value
    ? allItemBestSeling.value.reduce((max, current) => {
        return current.quantitySold > max.quantitySold ? current : max;
      }, allItemBestSeling.value[0])
    : null;
  quantitySoldMax.value = quantitySoldMaxDataTable
    ? quantitySoldMaxDataTable.quantitySold
    : 0;
}
async function selectDayAndCallAPI(day) {
  selectedDay.value = day;
  let [dayf, monthf, yearf] = selectedDay.value.split("-");
  dayf = dayf.padStart(2, "0");
  monthf = monthf.padStart(2, "0");
  selectedDay.value = `${dayf}-${monthf}-${yearf}`;
  const responseTotal1 = await axios.post(
    API_ENDPOINTS.GET_ALL_ORDERITEM_BESTSELING_CURRENTDAY,
    {
      date: selectedDay.value,
    }
  );
  allItemBestSeling.value = responseTotal1.data;

  const quantitySoldMaxDataTable = allItemBestSeling.value
    ? allItemBestSeling.value.reduce((max, current) => {
        return current.quantitySold > max.quantitySold ? current : max;
      }, allItemBestSeling.value[0])
    : null;
  quantitySoldMax.value = quantitySoldMaxDataTable
    ? quantitySoldMaxDataTable.quantitySold
    : 0;
}
async function selectMonthAndCallAPI(month) {
  selectedMonth.value = month;
  let [monthf, yearf] = selectedMonth.value.split("-");
  monthf = monthf.padStart(2, "0");
  selectedMonth.value = `${monthf}-${yearf}`;

  const response = await axios.post(
    API_ENDPOINTS.GET_ALL_ORDERITEM_BESTSELING_CURRENTMONTH,
    {
      date: selectedMonth.value,
    }
  );
  allItemBestSeling.value = response.data;

  const quantitySoldMaxDataTable = allItemBestSeling.value
    ? allItemBestSeling.value.reduce((max, current) => {
        return current.quantitySold > max.quantitySold ? current : max;
      }, allItemBestSeling.value[0])
    : null;
  quantitySoldMax.value = quantitySoldMaxDataTable
    ? quantitySoldMaxDataTable.quantitySold
    : 0;
}

const resetTimeFillterRevenueOrder = () => {
  selectedDay.value = "";
  selectedMonth.value = "";
  selectedCurrentDay.value = "";
  init();
};
</script>
