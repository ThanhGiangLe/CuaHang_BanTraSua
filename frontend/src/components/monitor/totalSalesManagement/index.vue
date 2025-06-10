<template>
  <div
    class="reportManagement_InventoryItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
      <v-card-title class="pa-0 mb-2 d-flex justify-center">
        <div class="d-flex align-center">
          <v-icon class="ma-1" size="large">mdi-warehouse</v-icon>
          <span style="font-size: 26px">Doanh thu các phiên làm việc</span>
        </div>
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
                  {{ selectedEmployee.fullName || "Nhân viên" }}
                </v-btn>
              </template>
              <v-list max-height="200px" style="overflow-y: auto">
                <v-list-item
                  v-for="(emp, index) in listEmployee"
                  :key="index"
                  :value="emp"
                  style="min-height: 36px !important"
                  @click="filerCashRegisterForEmployeeSelected(emp)"
                >
                  <v-list-item-title>{{ emp.fullName }}</v-list-item-title>
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
            :items="totalSalesFilter"
            height="calc(33vh - 2rem)"
            density="compact"
            fixed-footer
            fixed-header
          >
            <template v-slot:item.fullName="{ item }">
              <span
                style="font-weight: 500; color: rgba(var(--v-theme-primary), 1)"
                class="cursor-pointer"
              >
                {{ item.fullName ? item.fullName : "-" }}
              </span>
            </template>
            <template v-slot:item.date="{ item }">
              <span>
                {{ item.date ? formatDateFormApiToView(item.date) : "-" }}
              </span>
            </template>
            <template v-slot:item.shiftId="{ item }">
              <span> {{ shifs[item.shiftId] }}</span>
            </template>
            <template v-slot:item.cashAmount="{ item }">
              <span> {{ formatCurencyFromApiToView(item.cashAmount) }}</span>
            </template>
            <template v-slot:item.bankAmount="{ item }">
              <span> {{ formatCurencyFromApiToView(item.bankAmount) }}</span>
            </template>
            <template v-slot:item.closingCashAmount="{ item }">
              <span>
                {{ formatCurencyFromApiToView(item.closingCashAmount) }}</span
              >
            </template>
            <template v-slot:loading>
              <v-skeleton-loader type="table-row@5"></v-skeleton-loader>
            </template>
            <template v-slot:no-data>
              <div
                class="d-event-info-item d-emp-activity-item-content d-emp-activity-no-data pa-6"
                style="background: none"
              >
                <VIcon icon="mdi-robot-dead-outline"></VIcon>
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
import "underscore";
import "vue3-toastify/dist/index.css";
import { employeeManagementHandler } from "/src/composables/employeeManagement/employeeManagementHandler.js";
import { totalSalesManagementHandler } from "/src/composables/totalSalesManagement/totalSalesManagementHandler.js";

const { getTotalSaleAllEmployee, getTotalSalesEmployee } =
  totalSalesManagementHandler();
const { getAllEmployee } = employeeManagementHandler();
const loading = shallowRef(true);
const totalSales = ref([]);
const totalSalesFilter = ref([]);
const listEmployee = ref([]);
const selectedEmployee = ref("");
const selectedDay = ref("");

const header = ref([
  { title: "Họ tên", key: "fullName" },
  { title: "Ngày làm việc", key: "date" },
  { title: "Ca làm việc", key: "shiftId" },
  { title: "Tổng tiền mặt", key: "cashAmount" },
  { title: "Tổng chuyển khoản", key: "bankAmount" },
  { title: "Tổng", key: "closingCashAmount" },
]);
const shifs = ref({
  S: "Ca sáng",
  C: "Ca chiều",
  T: "Ca tối",
  C1: "Ca 1",
  C2: "Ca 2",
  O: "Nghỉ",
});
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
const currentMonth = currentDate.getMonth(); // Tháng hiện tại (cộng thêm 1 vì getMonth() trả về giá trị từ 0 đến 11)
const currentYear = currentDate.getFullYear(); // Năm hiện tại

const dateList = ref(generateDates(currentMonth + 1, currentYear));

async function init() {
  const response = await getTotalSaleAllEmployee();
  console.log("totalSales: ", totalSales.value);
  totalSales.value = JSON.parse(JSON.stringify(response));
  totalSalesFilter.value = JSON.parse(JSON.stringify(response));

  const responseEmp = await getAllEmployee();
  listEmployee.value = responseEmp.map((emp) => {
    return {
      userId: emp.userId,
      fullName: emp.fullName,
    };
  });

  loading.value = false;
}
init();
const formatDateFormApiToView = (inputDate) => {
  const date = new Date(inputDate);
  const formattedDate = `${date.getDate().toString().padStart(2, "0")}-${(
    date.getMonth() + 1
  )
    .toString()
    .padStart(2, "0")}-${date.getFullYear()}`;
  return formattedDate;
};
const formatCurencyFromApiToView = (money) => {
  return `${money.toLocaleString("vi-VN")} VND`;
};
const filerCashRegisterForEmployeeSelected = (emp) => {
  loading.value = true;
  selectedEmployee.value = emp;
  console.log("selectedEmployee: ", selectedEmployee.value);
  if (selectedDay.value) {
    totalSalesFilter.value = totalSales.value.filter(
      (item) =>
        item.fullName == selectedEmployee.value.fullName.trim() &&
        formatDateFormApiToView(item.date) == selectedDay.value
    );
  } else {
    totalSalesFilter.value = totalSales.value.filter(
      (item) => item.fullName == selectedEmployee.value.fullName.trim()
    );
  }
  loading.value = false;
};
const filerCashRegisterForDaySelected = (day) => {
  loading.value = true;
  selectedDay.value = day;
  if (selectedEmployee.value) {
    totalSalesFilter.value = totalSales.value.filter(
      (item) =>
        formatDateFormApiToView(item.date) == selectedDay.value &&
        item.fullName == selectedEmployee.value.fullName.trim()
    );
  } else {
    totalSalesFilter.value = totalSales.value.filter(
      (item) => formatDateFormApiToView(item.date) == selectedDay.value
    );
  }
  loading.value = false;
};
const resetFilterCashRegister = () => {
  selectedEmployee.value = "";
  selectedDay.value = "";
  init();
};
</script>
