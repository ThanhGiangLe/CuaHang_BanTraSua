<template>
  <div
    class="reportManagement_billingItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
      <v-card-title class="pa-0 mb-2 d-flex justify-center">
        <div class="d-flex align-center">
          <v-icon class="ma-1" size="large">mdi-file-document</v-icon>
          <span style="font-size: 26px">Lịch sử thay đổi ca làm</span>
        </div>
      </v-card-title>
      <v-card-text
        class="pa-3 rounded"
        :style="{ backgroundColor: 'var(--bg-color-item)' }"
      >
        <div
          class="reportManagement_chosseTime d-flex justify-md-space-between align-center mb-2"
        >
          <div>
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
                  @click="filterScheduleHistoryBySelectedDay(day)"
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
                  @click="filterScheduleHistoryBySelectedMonth(month)"
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
                  @click="filterScheduleHistoryBySelectedEmp(emp)"
                >
                  <v-list-item-title>{{ emp }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
            <!-- Lọc theo ca làm -->
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
                  {{ selectedShift || "Ca làm" }}
                </v-btn>
              </template>
              <v-list max-height="200px" style="overflow-y: auto">
                <v-list-item
                  v-for="shift in shifts"
                  :key="shift"
                  :value="shift"
                  style="min-height: 36px !important"
                  @click="filterScheduleHistoryBySelectedShift(shift)"
                >
                  <v-list-item-title style="font-size: 0.8rem">{{
                    shift
                  }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
            <!-- Lọc theo người thực hiện -->
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
                  {{ selectedEmployeeChanged || "Người thực hiện" }}
                </v-btn>
              </template>
              <v-list max-height="200px" style="overflow-y: auto">
                <v-list-item
                  v-for="(emp, index) in employeeListFullName"
                  :key="index"
                  :value="emp"
                  style="min-height: 36px !important"
                  @click="filterScheduleHistoryBySelectedEmpChanged(emp)"
                >
                  <v-list-item-title>{{ emp }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>
          <div class="d-flex">
            <JsonExcel
              class="btn btn-default"
              :data="dataTable"
              :fields="datafieldExcel"
              worksheet="Lịch sử ca trực"
              type="xlsx"
              :name="nameFileExcel"
            >
              <v-btn
                class="text-none"
                size="small"
                prepend-icon="mdi-download"
                color="#8690A0"
              >
                Xuất Excel
              </v-btn>
            </JsonExcel>
            <v-btn
              style="border: 1px solid #333; min-width: 60px"
              size="small"
              class="ms-2"
              @click="resetFilterScheduleHistory"
            >
              Làm mới
            </v-btn>
          </div>
        </div>
        <div
          class="reportManagement_totalAmount_salesSummary_bestSellingItems d-flex"
          style="height: 450px; max-height: 450px; overflow-y: auto"
        >
          <v-data-table
            :headers="header"
            :loading="loading"
            :items="filterAllScheduleHistory"
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
            <template v-slot:item.date="{ item }">
              <span>
                {{ item.date ? formatDate(item.date) : "-" }}
              </span>
            </template>
            <template v-slot:item.oldShiftId="{ item }">
              <span>
                {{ item.oldShiftId ? shifts[item.oldShiftId] : "-" }}
              </span>
            </template>
            <template v-slot:item.newShiftId="{ item }">
              <span>
                {{ item.newShiftId ? shifts[item.newShiftId] : "-" }}
              </span>
            </template>
            <template v-slot:item.changedBy="{ item }">
              <span>
                {{ item.changedBy ? item.changedBy : "-" }}
              </span>
            </template>
            <template v-slot:item.changedAt="{ item }">
              <span>
                {{
                  item.changedAt ? formatDateFormApiToView(item.changedAt) : "-"
                }}
              </span>
            </template>
            <template v-slot:loading>
              <v-skeleton-loader type="table-row@10"></v-skeleton-loader>
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
import { scheduleManagementHandler } from "/src/composables/scheduleManagement/scheduleManagementHandler.js";
import { employeeManagementHandler } from "/src/composables/employeeManagement/employeeManagementHandler.js";
const { GetAllScheduleHistory } = scheduleManagementHandler();
const { getAllEmployee, getAllSchedule } = employeeManagementHandler();
const allScheduleHistory = ref([]);
const filterAllScheduleHistory = ref([]);
const loading = shallowRef(true);
const employeeList = ref([]);
const employeeListFullName = ref([]);
const header = ref([
  { title: "Nhân viên", key: "fullName" },
  { title: "Ngày làm việc", key: "date" },
  { title: "Ca ban đầu", key: "oldShiftId" },
  { title: "Ca chuyển đổi", key: "newShiftId" },
  { title: "Người thực hiện", key: "changedBy" },
  { title: "Thời gian thực hiện", key: "changedAt" },
]);
const selectedDay = ref("");
const selectedMonth = ref("");
const selectedEmployee = ref("");
const selectedEmployeeChanged = ref("");
const selectedShift = ref("");
const shifts = ref({
  S: "Ca sáng",
  C: "Ca chiều",
  T: "Ca tối",
  C1: "Ca 1",
  C2: "Ca 2",
  O: "Nghỉ",
});
const getShiftKeyByValue = (val) => {
  const entries = Object.entries(shifts.value); // return array có key và value [["S", "Ca sáng"], ["C", "Ca chiều"], ...., ["O", "Nghỉ"]];
  const found = entries.find(([key, value]) => value === val);
  return found ? found[0] : null;
};

const currentDate = new Date();
const currentMonth = currentDate.getMonth() + 1; // Tháng hiện tại (cộng thêm 1 vì getMonth() trả về giá trị từ 0 đến 11)
const currentYear = currentDate.getFullYear(); // Năm hiện tại
const currentDay = currentDate.getDate();
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
const isBeforeToday = (dateString) => {
  const [day, month, year] = dateString.split("-");
  return day <= currentDay;
};
const isBeforeToMonth = (dateString) => {
  const [month, year] = dateString.split("-");
  return month <= currentMonth;
};

const fullDateList = generateDates(currentMonth, currentYear);
const dateList = fullDateList; //ref(fullDateList.filter((item) => isBeforeToday(item)));
const fullMonthList = generateMonths(currentYear);
const monthList = fullMonthList; //ref(fullMonthList.filter((item) => isBeforeToMonth(item)));

async function init() {
  const response = await GetAllScheduleHistory();
  const resSchedules = await getAllSchedule();
  const schedules = resSchedules.data;
  console.log("schedules: ", schedules);

  const sortedHistories = response.result.data.sort((a, b) => {
    if (a.userId == b.userId) {
      return new Date(a.changedAt) - new Date(b.changedAt);
    }
    return String(a.userId).localeCompare(String(b.userId));
  });
  console.log("sortedHistories: ", sortedHistories);

  const results = [];
  for (let i = 0; i < sortedHistories.length; i++) {
    const currentScheduleHis = sortedHistories[i];
    let newShift = currentScheduleHis.oldShiftId;
    if (currentScheduleHis.changedBy != "Auto") {
      if (
        i + 1 < sortedHistories.length &&
        sortedHistories[i + 1].userId == currentScheduleHis.userId &&
        new Date(sortedHistories[i + 1].date).toDateString() ==
          new Date(currentScheduleHis.date).toDateString()
      ) {
        newShift = sortedHistories[i + 1].oldShiftId;
      } else {
        const currentSchedule = schedules.find(
          (item) =>
            item.userId == currentScheduleHis.userId &&
            new Date(item.date).toDateString() ==
              new Date(currentScheduleHis.date).toDateString()
        );
        newShift = currentSchedule ? currentSchedule.shiftId : "";
      }
    }
    results.push({
      fullName: currentScheduleHis.fullName,
      date: currentScheduleHis.date,
      oldShiftId: currentScheduleHis.oldShiftId,
      newShiftId: newShift,
      changedBy: currentScheduleHis.changedBy,
      changedAt: currentScheduleHis.changedAt,
    });
  }
  console.log("result: ", results);
  allScheduleHistory.value = JSON.parse(JSON.stringify(results));
  filterAllScheduleHistory.value = JSON.parse(JSON.stringify(results));

  const responseEmp = await getAllEmployee();
  employeeList.value = responseEmp;
  employeeListFullName.value = employeeList.value
    .filter((emp) => emp.role != "Khách hàng")
    .map((emp) => emp.fullName);
  loading.value = false;
}
init();
function formatDate(dateTime) {
  const date = new Date(dateTime);
  const formattedDate = `${date.getDate().toString().padStart(2, "0")}-${(
    date.getMonth() + 1
  )
    .toString()
    .padStart(2, "0")}-${date.getFullYear()}`;
  return formattedDate;
}
function formatDateMonth(dateTime) {
  const date = new Date(dateTime);
  const formattedDate = `${(date.getMonth() + 1)
    .toString()
    .padStart(2, "0")}-${date.getFullYear()}`;
  return formattedDate;
}
function filterScheduleHistoryBySelectedDay(day) {
  selectedDay.value = day;
  selectedMonth.value = "";

  filterScheduleShift();
}
function filterScheduleHistoryBySelectedMonth(month) {
  selectedMonth.value = month;
  selectedDay.value = "";

  filterScheduleShift();
}

function filterScheduleHistoryBySelectedEmp(emp) {
  selectedEmployee.value = emp;
  console.log("selectedEmployee: ", selectedEmployee.value);

  filterScheduleShift();
}
function filterScheduleHistoryBySelectedEmpChanged(emp) {
  selectedEmployeeChanged.value = emp;
  filterScheduleShift();
}
function filterScheduleHistoryBySelectedShift(shift) {
  selectedShift.value = shift;
  filterScheduleShift();
}
const filterScheduleShift = () => {
  loading.value = true;
  filterAllScheduleHistory.value = allScheduleHistory.value.filter((item) => {
    const matchSelectedDay =
      !selectedDay.value || formatDate(item.date) == selectedDay.value;
    const matchSelectedMonth =
      !selectedMonth.value || formatDateMonth(item.date) == selectedMonth.value;
    const matchEmployee =
      !selectedEmployee.value ||
      item.fullName.toLowerCase().trim() ==
        selectedEmployee.value.toLowerCase().trim();
    const matchEmployeeChanged =
      !selectedEmployeeChanged.value ||
      item.changedBy.toLowerCase().trim() ==
        selectedEmployeeChanged.value.toLowerCase().trim();
    const matchShift =
      !selectedShift.value ||
      item.newShiftId == getShiftKeyByValue(selectedShift.value);
    return (
      matchEmployee &&
      matchEmployeeChanged &&
      matchSelectedDay &&
      matchSelectedMonth &&
      matchShift
    );
  });
  loading.value = false;
};

const resetFilterScheduleHistory = () => {
  selectedDay.value = "";
  selectedMonth.value = "";
  selectedEmployee.value = "";
  selectedShift.value = "";
  selectedEmployeeChanged.value = "";
  init();
};
const formatDateFormApiToView = (inputDate) => {
  const date = new Date(inputDate);
  const formattedDate = `${date.getDate().toString().padStart(2, "0")}-${(
    date.getMonth() + 1
  )
    .toString()
    .padStart(2, "0")}-${date.getFullYear()} ${date
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
const dataTable = computed(() => {
  return filterAllScheduleHistory.value?.map((item) => ({
    fullName: item.fullName || "-",
    date: formatDate(item.date),
    oldShift: shifts.value[item.oldShiftId] || item.oldShiftId || "-",
    newShift: shifts.value[item.newShiftId] || item.newShiftId || "-",
    changedBy: item.changedBy || "-",
    changedAt: formatDateFormApiToView(item.changedAt),
  }));
});
const datafieldExcel = {
  "Nhân viên": "fullName",
  "Ngày làm việc": "date",
  "Ca ban đầu": "oldShift",
  "Ca chuyển đổi": "newShift",
  "Người thực hiện": "changedBy",
  "Thời gian thực hiện": "changedAt",
};

const nameFileExcel = computed(() => {
  return `lich_su_ca_truc_${currentDay}_${currentMonth}_${currentYear}.xlsx`;
});
</script>
