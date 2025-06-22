<template>
  <div
    class="reportManagement_InventoryItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
      <v-card-title class="pa-0 mb-2 d-flex justify-center">
        <!-- <div class="d-flex align-center">
          <v-icon class="ma-1" size="large">mdi-warehouse</v-icon>
          <span style="font-size: 26px">Doanh thu các phiên làm việc</span>
        </div> -->
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
                  @click="filterTotalSaleForEmployeeSelected(emp)"
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
                  @click="filterTotalSaleForDaySelected(day)"
                >
                  <v-list-item-title style="font-size: 0.8rem">{{
                    day
                  }}</v-list-item-title>
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
                  @click="filterTotalSaleForShiftSelected(shift)"
                >
                  <v-list-item-title style="font-size: 0.8rem">{{
                    shift
                  }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>
          <div class="d-flex">
            <JsonExcel
              v-if="dataTable.length"
              class="btn btn-default"
              :data="dataTable"
              :fields="datafieldExcel"
              worksheet="My Worksheet"
              type="xlsx"
              :name="nameFileExcel"
            >
              <VBtn
                class="text-none"
                size="small"
                prependIcon="mdi-crop"
                color="#8690A0"
              >
                Xuất Excel
              </VBtn>
            </JsonExcel>
            <v-btn
              style="border: 1px solid #333; min-width: 60px"
              size="small"
              class="ms-2"
              @click="resetFilterCashRegister"
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
            :items="totalSalesFilter"
            height="calc(33vh - 2rem)"
            density="compact"
            fixed-footer
            fixed-header
            @click:row="onRowClick"
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
              <span> {{ shifts[item.shiftId] }}</span>
            </template>
            <template v-slot:item.totalOpeningCashAmount="{ item }">
              <span>
                {{
                  formatCurencyFromApiToView(item.totalOpeningCashAmount)
                }}</span
              >
            </template>
            <template v-slot:item.receivedTotalAmount="{ item }">
              <span>
                {{ formatCurencyFromApiToView(item.receivedTotalAmount) }}</span
              >
            </template>
            <template v-slot:item.returnedTotalAmount="{ item }">
              <span>
                {{ formatCurencyFromApiToView(item.returnedTotalAmount) }}</span
              >
            </template>
            <template v-slot:item.adjustmentAmount="{ item }">
              <span>
                {{ formatCurencyFromApiToView(item.adjustmentAmount) }}</span
              >
            </template>
            <template v-slot:item.adjustmentReason="{ item }">
              <span> {{ item.adjustmentReason }}</span>
            </template>
            <template v-slot:item.totalClosingCashAmount="{ item }">
              <span>{{
                formatCurencyFromApiToView(item.totalClosingCashAmount)
              }}</span>
            </template>
            <template v-slot:item.actualCash="{ item }">
              <span>{{ formatCurencyFromApiToView(item.actualCash) }}</span>
            </template>
            <template v-slot:item.difference="{ item }">
              <span>{{ formatCurencyFromApiToView(item.difference) }}</span>
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
    <v-dialog
      v-model="showDetailSchedule"
      max-width="1080px"
      class="reportManagement_totalAmount_salesSummary_bestSellingItems"
    >
      <v-card>
        <v-card-title class="headline d-flex align-center pb-0">
          Chi tiết các lần mở/kết ca
        </v-card-title>

        <v-card-text class="px-4 py-0">
          <v-data-table
            :headers="headerDetail"
            :loading="loading"
            :items="detailSchedule"
            height="calc(60vh - 2rem)"
            density="compact"
            fixed-footer
            fixed-header
          >
            <template v-slot:item.startTime="{ item }">
              <span>
                {{
                  item.startTime
                    ? formatDateFormApiToViewDetail(item.startTime)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.endTime="{ item }">
              <span>
                {{
                  item.endTime
                    ? formatDateFormApiToViewDetail(item.endTime)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.openingCashAmount="{ item }">
              <span>
                {{
                  item.openingCashAmount
                    ? formatCurencyFromApiToView(item.openingCashAmount)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.receivedTotalAmount="{ item }">
              <span>
                {{
                  item.receivedTotalAmount
                    ? formatCurencyFromApiToView(item.receivedTotalAmount)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.returnedTotalAmount="{ item }">
              <span>
                {{
                  item.returnedTotalAmount
                    ? formatCurencyFromApiToView(item.returnedTotalAmount)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.adjustmentAmount="{ item }">
              <span>
                {{
                  item.adjustmentAmount
                    ? formatCurencyFromApiToView(item.adjustmentAmount)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.closingCashAmount="{ item }">
              <span>
                {{
                  item.closingCashAmount
                    ? formatCurencyFromApiToView(item.closingCashAmount)
                    : "-"
                }}
              </span>
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
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="red darken-1" text @click="showDetailSchedule = false"
            >Đóng</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { ref } from "vue";
import "underscore";
import "vue3-toastify/dist/index.css";
import { employeeManagementHandler } from "/src/composables/employeeManagement/employeeManagementHandler.js";
import { totalSalesManagementHandler } from "/src/composables/totalSalesManagement/totalSalesManagementHandler.js";
import { showToast } from "@/styles/handmade";
import JsonExcel from "vue-json-excel3";
const { getTotalSales, getDetailTotalSaleSchedule } =
  totalSalesManagementHandler();
const { getAllEmployee } = employeeManagementHandler();
const loading = shallowRef(true);
const totalSales = ref([]);
const totalSalesFilter = ref([]);
const listEmployee = ref([]);
const selectedEmployee = ref("");
const selectedDay = ref("");
const selectedShift = ref("");
const scheduleSelected = ref(null);
const detailSchedule = ref([]);
const showDetailSchedule = ref(false);
const nameFileExcel = ref("");

const header = ref([
  { title: "Họ tên", key: "fullName" },
  { title: "Ngày làm", key: "date" },
  { title: "Ca làm", key: "shiftId" },
  { title: "Tổng mở ca", key: "totalOpeningCashAmount" },
  { title: "Tổng nhận", key: "receivedTotalAmount" },
  { title: "Tổng thối", key: "returnedTotalAmount" },
  { title: "Tổng phát sinh", key: "totalAdjustmentAmount" },
  { title: "Tổng kết ca", key: "totalClosingCashAmount" },
  { title: "Chênh lệch", key: "difference" },
]);
const headerDetail = ref([
  { title: "Thời gian mở", key: "startTime" },
  { title: "Thời gian kết", key: "endTime" },
  { title: "Mở ca", key: "openingCashAmount" },
  { title: "Tổng nhận", key: "receivedTotalAmount" },
  { title: "Tổng thối", key: "returnedTotalAmount" },
  { title: "Phát sinh", key: "adjustmentAmount" },
  { title: "Kết ca", key: "closingCashAmount" },
]);
const shifts = ref({
  S: "Ca sáng",
  C: "Ca chiều",
  T: "Ca tối",
  C1: "Ca 1",
  C2: "Ca 2",
  O: "Nghỉ",
});
const getShiftKeyByValue = (val) => {
  const entries = Object.entries(shifts.value); // return array có key và value ["S", "Ca sáng"]
  const found = entries.find(([key, value]) => value === val);
  return found ? found[0] : null;
};
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
const currentDay = currentDate.getDate();
nameFileExcel.value = `Bao_Cao_Ket_Tien_Ngay_${currentDay}-${(currentMonth + 1)
  .toString()
  .padStart(2, "0")}-${currentYear}.xlsx`;
const isBeforeToday = (dateString) => {
  const [day, month, year] = dateString.split("-"); // 19-06-2025
  return day <= currentDay;
};

const fullDateList = generateDates(currentMonth + 1, currentYear);
const dateList = ref(fullDateList.filter((d) => isBeforeToday(d)));

async function init() {
  const response = await getTotalSales({});
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
const formatDateFormApiToViewDetail = (inputDate) => {
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
const formatCurencyFromApiToView = (money) => {
  if (money == null || isNaN(money)) return "-";
  return `${Number(money).toLocaleString("vi-VN")} VND`;
};
const filterTotalSaleForEmployeeSelected = (emp) => {
  selectedEmployee.value = emp;
  filterTotalSales();
};
const filterTotalSaleForDaySelected = (day) => {
  selectedDay.value = day;
  filterTotalSales();
};
const filterTotalSaleForShiftSelected = (shift) => {
  selectedShift.value = shift;
  filterTotalSales();
};
const filterTotalSales = () => {
  loading.value = true;
  totalSalesFilter.value = totalSales.value.filter((item) => {
    const matchFullName =
      !selectedEmployee.value ||
      item.fullName.toLowerCase().trim() ==
        selectedEmployee.value.fullName.toLowerCase().trim();

    const matchDate =
      !selectedDay.value ||
      formatDateFormApiToView(item.date) == selectedDay.value;

    const matchShift =
      !selectedShift.value ||
      getShiftKeyByValue(selectedShift.value) == item.shiftId;
    return matchFullName && matchDate && matchShift;
  });
  loading.value = false;
};

const resetFilterCashRegister = () => {
  selectedEmployee.value = "";
  selectedDay.value = "";
  selectedShift.value = "";
  init();
};
async function onRowClick(event, item) {
  scheduleSelected.value = item.item;
  showDetailSchedule.value = true;
  const request = {
    ScheduleId: scheduleSelected.value.scheduleId,
  };
  const response = await getDetailTotalSaleSchedule(request);
  console.log("response: ", response);
  if (response.length > 0) {
    detailSchedule.value = response;
  } else {
    if (response.response.status == 400) {
      showToast(response.response.data, "error");
    } else {
      showToast("Có lỗi trong quá trình xử lý!", "error");
    }
  }
}

// const dataTable = computed(() =>
//   totalSalesFilter.value.map((item) => ({
//     Họ_tên: item.fullName || "-",
//     Ngày_làm: formatDateFormApiToView(item.date),
//     Ca_làm: shifts.value[item.shiftId],
//     Tổng_mở_ca: formatCurencyFromApiToView(item.totalOpeningCashAmount),
//     Tổng_nhận: formatCurencyFromApiToView(item.receivedTotalAmount),
//     Tổng_thối: formatCurencyFromApiToView(item.returnedTotalAmount),
//     Phát_sinh: formatCurencyFromApiToView(item.adjustmentAmount || 0),
//     Lý_do_phát_sinh: item.adjustmentReason || "-",
//     Tổng_kết_ca: formatCurencyFromApiToView(item.totalClosingCashAmount || 0),
//     Thực_tế: formatCurencyFromApiToView(item.actualCash || 0),
//     Chênh_lệch: formatCurencyFromApiToView(item.difference || 0),
//   }))
// );
const dataTable = [
  {
    ho_ten: "Nguyễn Văn A",
    ngay_lam: "19-06-2025",
    ca_lam: "Ca sáng",
    tong_mo_ca: "500000 VND",
    tong_nhan: "1000000 VND",
    tong_thoi: "50000 VND",
    phat_sinh: "0 VND",
    ly_do_phat_sinh: "-",
    tong_ket_ca: "1450000 VND",
    thuc_te: "1450000 VND",
    chenh_lech: "0 VND",
  },
];

const datafieldExcel = {
  ho_ten: "Họ tên",
  ngay_lam: "Ngày làm",
  ca_lam: "Ca làm",
  tong_mo_ca: "Tổng mở ca",
  tong_nhan: "Tổng nhận",
  tong_thoi: "Tổng thối",
  phat_sinh: "Phát sinh",
  ly_do_phat_sinh: "Lý do phát sinh",
  tong_ket_ca: "Tổng kết ca",
  thuc_te: "Tiền thực tế",
  chenh_lech: "Chênh lệch",
};
</script>
