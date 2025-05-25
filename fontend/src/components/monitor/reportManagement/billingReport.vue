<template>
  <div
    class="reportManagement_billingItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
      <v-card-title class="pa-0 mb-2 d-flex justify-center">
        <div class="d-flex align-center">
          <v-icon class="ma-1" size="large">mdi-file-document</v-icon>
          <span style="font-size: 26px">Báo cáo tổng hóa đơn</span>
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
            <!-- Lọc theo nhân viên -->
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
                  {{ selectedEmployee || "Nhân viên" }}
                </v-btn>
              </template>
              <v-list max-height="200px" style="overflow-y: auto">
                <v-list-item
                  v-for="(emp, index) in employeeListFullName"
                  :key="index"
                  :value="emp"
                  style="min-height: 36px !important"
                  @click="selectEmployeeAndCallAPI(emp)"
                >
                  <v-list-item-title>{{ emp }}</v-list-item-title>
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
            :headers="header"
            :loading="loading"
            :items="allBilling"
            height="calc(33vh - 2rem)"
            density="compact"
            fixed-footer
            fixed-header
          >
            <template v-slot:item.fullName="{ item }">
              <span
                style="font-weight: 500; color: rgba(var(--v-theme-primary), 1)"
              >
                {{ item.fullName ? item.fullName : "-" }}
              </span>
            </template>
            <template v-slot:item.orderTime="{ item }">
              <span>
                {{
                  item.orderTime ? formatDateFormApiToView(item.orderTime) : "-"
                }}
              </span>
            </template>
            <template v-slot:item.totalAmount="{ item }">
              <span>
                {{
                  item.totalAmount
                    ? formatCurrencyFromApiToView(item.totalAmount)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.discount="{ item }">
              <span> {{ item.discount ? item.discount : "-" }}% </span>
            </template>
            <template v-slot:item.tax="{ item }">
              <span> {{ item.tax ? item.tax : "-" }}% </span>
            </template>

            <!-- <template v-slot:item.quantitySold="{ item }">
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
            </template> -->
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
const allBilling = ref([]);
const loading = shallowRef(true);
const employeeList = ref([]);
const employeeListFullName = ref([]);

const header = ref([
  { title: "Nhân viên đứng quầy", key: "fullName" },
  { title: "Thời gian bán", key: "orderTime" },
  { title: "Bàn ăn", key: "tableName" },
  { title: "Tổng thanh toán", key: "totalAmount" },
  { title: "Giảm giá", key: "discount" },
  { title: "Thuế", key: "tax" },
]);

const selectedCurrentDay = ref("");
const selectedDay = ref("");
const selectedMonth = ref("");
const selectedEmployee = ref("");

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

const currentDate = new Date();
const currentMonth = currentDate.getMonth() + 1; // Tháng hiện tại (cộng thêm 1 vì getMonth() trả về giá trị từ 0 đến 11)
const currentYear = currentDate.getFullYear(); // Năm hiện tại

const dateList = ref(generateDates(currentMonth, currentYear));
const monthList = ref(generateMonths(currentYear));

async function init() {
  const monthFormat = currentMonth.toString().padStart(2, "0"); // Đảm bảo tháng có 2 chữ số
  const response = await axios.post(API_ENDPOINTS.GET_ALL_ORDER_CURRENT_MONTH, {
    date: `${monthFormat}-${currentYear}`,
  });
  allBilling.value = response.data;

  // const quantitySoldMaxDataTable = allBilling.value
  //   ? allBilling.value.reduce((max, current) => {
  //       return current.quantitySold > max.quantitySold ? current : max;
  //     }, allBilling.value[0])
  //   : null;
  // quantitySoldMax.value = quantitySoldMaxDataTable
  //   ? quantitySoldMaxDataTable.quantitySold
  //   : 0;

  const responseEmp = await axios.get(API_ENDPOINTS.GET_ALL_EMPLOYEES);
  employeeList.value = responseEmp.data;
  employeeListFullName.value = employeeList.value.map((emp) => emp.fullName);
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

  const response = await axios.post(API_ENDPOINTS.GET_ALL_ORDER_CURRENT_DAY, {
    date: selectedCurrentDay.value,
  });
  allBilling.value = response.data;
}
async function selectDayAndCallAPI(day) {
  selectedDay.value = day;
  let [dayf, monthf, yearf] = selectedDay.value.split("-");
  dayf = dayf.padStart(2, "0");
  monthf = monthf.padStart(2, "0");
  selectedDay.value = `${dayf}-${monthf}-${yearf}`;
  const responseTotal1 = await axios.post(
    API_ENDPOINTS.GET_ALL_ORDER_CURRENT_DAY,
    {
      date: selectedDay.value,
    }
  );
  allBilling.value = responseTotal1.data;
}
async function selectMonthAndCallAPI(month) {
  selectedMonth.value = month;
  let [monthf, yearf] = selectedMonth.value.split("-");
  monthf = monthf.padStart(2, "0");
  selectedMonth.value = `${monthf}-${yearf}`;

  const response = await axios.post(API_ENDPOINTS.GET_ALL_ORDER_CURRENT_MONTH, {
    date: selectedMonth.value,
  });
  allBilling.value = response.data;
}
async function selectEmployeeAndCallAPI(emp) {
  selectedEmployee.value = emp;
  const response = await axios.post(API_ENDPOINTS.GET_ALL_ORDER_FULL_NAME, {
    date: selectedEmployee.value,
  });
  allBilling.value = response.data;
}

const resetTimeFillterRevenueOrder = () => {
  selectedDay.value = "";
  selectedMonth.value = "";
  selectedCurrentDay.value = "";
  selectedEmployee.value = "";
  init();
};
const formatDateFormApiToView = (inputDate) => {
  const date = new Date(inputDate);
  const formattedDate = `${date.getDate().toString().padStart(2, "0")}/${(
    date.getMonth() + 1
  )
    .toString()
    .padStart(2, "0")}/${date.getFullYear()} ${date
    .getHours()
    .toString()
    .padStart(2, "0")}:${date.getMinutes().toString().padStart(2, "0")}:${date
    .getSeconds()
    .toString()
    .padStart(2, "0")}`;
  return formattedDate;
};
const formatCurrencyFromApiToView = (currency) => {
  return `${currency.toLocaleString("vi-VN")} VND`;
};
</script>
