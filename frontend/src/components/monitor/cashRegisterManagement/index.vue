<template>
  <div
    class="reportManagement_InventoryItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
      <v-card-title class="pa-0 mb-2 d-flex justify-center">
        <div class="d-flex align-center">
          <v-icon class="ma-1" size="large">mdi-warehouse</v-icon>
          <span style="font-size: 26px">Danh sách đăng nhập/xuất</span>
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
                  @click="filerCashRegisterForEmployeeSelected(emp)"
                >
                  <v-list-item-title>{{ emp }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
            <v-menu>
              <template v-slot:activator="{ props }">
                <v-btn
                  v-bind="props"
                  append-icon="mdi-chevron-down"
                  min-width="90px"
                  style="border: 1px solid #333"
                  size="small"
                  class="ms-5"
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
                  @click="filerCashRegisterForDaySelected(day)"
                >
                  <v-list-item-title style="font-size: 0.8rem">{{
                    day
                  }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>
          <v-btn
            style="border: 1px solid #333; min-width: 60px"
            size="small"
            class="ms-5"
            @click="resetFilterCashRegister"
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
            :items="cashRegistersFilter"
            height="calc(33vh - 2rem)"
            density="compact"
            fixed-footer
            fixed-header
          >
            <template v-slot:item.userName="{ item }">
              <span
                style="font-weight: 500; color: rgba(var(--v-theme-primary), 1)"
                class="cursor-pointer"
              >
                {{ item.userName ? item.userName : "-" }}
              </span>
            </template>
            <template v-slot:item.startTime="{ item }">
              <span>
                {{
                  item.startTime ? formatDateFormApiToView(item.startTime) : "-"
                }}
              </span>
            </template>
            <template v-slot:item.endTime="{ item }">
              <span>
                {{
                  item.endTime != item.startTime
                    ? formatDateFormApiToView(item.endTime)
                    : "- - -"
                }}</span
              >
            </template>
            <template v-slot:item.totalIncome="{ item }">
              <span> {{ formatCurencyFromApiToView(item.totalIncome) }}</span>
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
import API_ENDPOINTS from "@/api/api.js";
import "underscore";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import dayjs from "dayjs";
const loading = shallowRef(true);
const cashRegisters = ref([]);
const cashRegistersFilter = ref([]);
const employeeListFullName = ref([]);
const selectedEmployee = ref("");
const selectedDay = ref("");

const header = ref([
  { title: "Tên tài khoản", key: "userName" },
  { title: "Thời gian đăng nhập", key: "startTime" },
  { title: "Thời gian đăng xuất", key: "endTime" },
  { title: "Tổng doanh thu", key: "totalIncome" },
]);
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
const currentDate = new Date();
const currentMonth = currentDate.getMonth() + 1; // Tháng hiện tại (cộng thêm 1 vì getMonth() trả về giá trị từ 0 đến 11)
const currentYear = currentDate.getFullYear(); // Năm hiện tại

const dateList = ref(generateDates(currentMonth, currentYear));

async function init() {
  const response = await axios.get(API_ENDPOINTS.GET_ALL_CASH_REGISTER);

  cashRegisters.value = response.data;
  console.log("cashRegisters.value: ", cashRegisters.value);
  cashRegisters.value.sort((a, b) => b.cashRegisterId - a.cashRegisterId);

  cashRegistersFilter.value = cashRegisters.value;

  const responseEmp = await axios.get(API_ENDPOINTS.GET_ALL_EMPLOYEES);
  employeeListFullName.value = responseEmp.data.map((emp) => emp.fullName);

  loading.value = false;
}
init();
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
const formatCurencyFromApiToView = (money) => {
  return `${money.toLocaleString("vi-VN")} VND`;
};
const filerCashRegisterForEmployeeSelected = (emp) => {
  selectedEmployee.value = emp;
  selectedDay.value = "";
  cashRegistersFilter.value = cashRegisters.value.filter(
    (item) => item.userName == selectedEmployee.value.trim()
  );
};
const filerCashRegisterForDaySelected = (day) => {
  selectedDay.value = day;
  selectedEmployee.value = "";
  const formattedSelectedDay = dayjs(selectedDay.value).format("YYYY-DD-MM");
  cashRegistersFilter.value = cashRegisters.value.filter((item) => {
    const itemDate = dayjs(item.startTime).format("YYYY-MM-DD");
    return itemDate === formattedSelectedDay;
  });
};
const resetFilterCashRegister = () => {
  selectedEmployee.value = "";
  selectedDay.value = "";
  init();
};
</script>
