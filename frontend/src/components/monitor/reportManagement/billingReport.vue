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
            <!-- Lọc theo phương thức thanh toán -->
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
                  {{ selectedPaymentMethod || "Phương thức thanh toán" }}
                </v-btn>
              </template>
              <v-list max-height="200px" style="overflow-y: auto">
                <v-list-item
                  v-for="(method, index) in paymentMethods"
                  :key="index + method"
                  :value="method.paymentName"
                  style="min-height: 36px !important"
                  @click="filterBillByPaymentMethod(method)"
                >
                  <v-list-item-title>{{
                    method.paymentName
                  }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>
          <div class="d-flex align-center">
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
              @click="resetTimeFillterRevenueOrder"
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
            :items="filterAllBilling"
            height="calc(33vh - 2rem)"
            density="compact"
            fixed-footer
            fixed-header
            @click:row="onRowClick"
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
            <template v-slot:item.receivedAmount="{ item }">
              <span>
                {{
                  item.receivedAmount
                    ? formatCurrencyFromApiToView(item.receivedAmount)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.returnedAmount="{ item }">
              <span>
                {{
                  item.returnedAmount
                    ? formatCurrencyFromApiToView(item.returnedAmount)
                    : "-"
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
              <span> {{ item.discount }}% </span>
            </template>
            <template v-slot:item.paymentMethod="{ item }">
              <span> {{ item.paymentMethod }} </span>
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
    <v-dialog
      v-model="showDialogOrderDetails"
      max-width="1080px"
      class="reportManagement_totalAmount_salesSummary_bestSellingItems"
    >
      <v-card>
        <v-card-title class="headline d-flex align-center pb-0">
          Chi tiết hóa đơn
        </v-card-title>

        <v-card-text class="px-4 py-0">
          <v-data-table
            :headers="headerDetail"
            :loading="loading"
            :items="orderDetails"
            height="calc(60vh -2rem)"
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
            <template v-slot:item.price="{ item }">
              <span>
                {{ item.price ? formatCurrencyFromApiToView(item.price) : "-" }}
              </span>
            </template>
            <template v-slot:item.quantity="{ item }">
              <span>
                {{ item.quantity ? item.quantity : "-" }}
              </span>
            </template>
            <template v-slot:item.note="{ item }">
              <span>
                {{ item.note }}
              </span>
            </template>
            <template v-slot:item.isMainItem="{ item }">
              <span>
                {{ item.isMainItem == 1 ? "Món chính" : "Gọi thêm" }}
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
          <v-btn
            color="red darken-1"
            text
            @click="showDialogOrderDetails = false"
            >Đóng</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { reportManagementHandler } from "/src/composables/reportManagement/reportManagementHandler.js";
import { employeeManagementHandler } from "/src/composables/employeeManagement/employeeManagementHandler.js";

const {
  getAllOrderByMonth,
  getAllOrderByDay,
  getAllOrderByFullName,
  getAllOrderByPaymentMethod,
  GetOrderDetailsByOrderId,
} = reportManagementHandler();
const { getAllEmployee } = employeeManagementHandler();
const allBilling = ref([]);
const filterAllBilling = ref([]);
const loading = shallowRef(true);
const employeeList = ref([]);
const employeeListFullName = ref([]);
const selectedBill = ref(null);
const orderDetails = ref([]);
const showDialogOrderDetails = ref(false);

const header = ref([
  { title: "Nhân viên", key: "fullName" },
  { title: "Thời gian", key: "orderTime" },
  { title: "Vị trí", key: "tableName" },
  { title: "Tiền nhận", key: "receivedAmount" },
  { title: "Tiền thừa", key: "returnedAmount" },
  { title: "Tổng thanh toán", key: "totalAmount" },
  { title: "Giảm giá", key: "discount" },
  { title: "Phương thức thanh toán", key: "paymentMethod" },
]);
const headerDetail = ref([
  { title: "Tên món", key: "foodName" },
  { title: "Gía bán", key: "price" },
  { title: "Số lượng", key: "quantity" },
  { title: "Ghi chú", key: "note" },
  { title: "Chính/Phụ", key: "isMainItem" },
]);
const selectedCurrentDay = ref("");
const selectedDay = ref("");
const selectedMonth = ref("");
const selectedEmployee = ref("");
const selectedPaymentMethod = ref("");
const paymentMethods = ref([
  {
    paymentId: 1,
    paymentName: "Tiền mặt",
  },
  {
    paymentId: 2,
    paymentName: "Chuyển khoản",
  },
  {
    paymentId: 3,
    paymentName: "Điểm",
  },
]);

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
const dateList = ref(fullDateList.filter((item) => isBeforeToday(item)));
const fullMonthList = generateMonths(currentYear);
const monthList = ref(fullMonthList.filter((item) => isBeforeToMonth(item)));

async function init() {
  const monthFormat = currentMonth.toString().padStart(2, "0"); // Đảm bảo tháng có 2 chữ số
  const response = await getAllOrderByMonth(`${monthFormat}-${currentYear}`);
  allBilling.value = JSON.parse(JSON.stringify(response));
  filterAllBilling.value = JSON.parse(JSON.stringify(response));

  const responseEmp = await getAllEmployee();
  employeeList.value = responseEmp;
  employeeListFullName.value = employeeList.value.map((emp) => emp.fullName);
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
async function selectCurrentDayAndCallAPI() {
  selectedDay.value = "";
  selectedMonth.value = "";
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

  const response = await getAllOrderByDay(selectedCurrentDay.value);
  console.log("Hôm nay: ", response);
  let result = response;
  if (selectedEmployee.value != "") {
    result = result.filter(
      (item) =>
        item.fullName.trim().toLowerCase() ==
        selectedEmployee.value.trim().toLowerCase()
    );
  }
  if (selectedPaymentMethod.value != "") {
    result = result.filter(
      (item) =>
        item.paymentMethod.trim().toLowerCase() ==
        selectedPaymentMethod.value.trim().toLowerCase()
    );
  }
  filterAllBilling.value = result;
}
async function selectDayAndCallAPI(day) {
  selectedCurrentDay.value = "";
  selectedMonth.value = "";
  selectedDay.value = day;
  let [dayf, monthf, yearf] = selectedDay.value.split("-");
  dayf = dayf.padStart(2, "0");
  monthf = monthf.padStart(2, "0");
  selectedDay.value = `${dayf}-${monthf}-${yearf}`;
  const responseTotal1 = await getAllOrderByDay(selectedDay.value);
  console.log("Chọn ngày: ", responseTotal1);
  let result = responseTotal1;
  if (selectedEmployee.value != "") {
    result = result.filter(
      (item) =>
        item.fullName.trim().toLowerCase() ==
        selectedEmployee.value.trim().toLowerCase()
    );
  }
  if (selectedPaymentMethod.value != "") {
    result = result.filter(
      (item) =>
        item.paymentMethod.trim().toLowerCase() ==
        selectedPaymentMethod.value.trim().toLowerCase()
    );
  }
  filterAllBilling.value = result;
}
async function selectMonthAndCallAPI(month) {
  selectedCurrentDay.value = "";
  selectedDay.value = "";
  selectedMonth.value = month;
  let [monthf, yearf] = selectedMonth.value.split("-");
  monthf = monthf.padStart(2, "0");
  selectedMonth.value = `${monthf}-${yearf}`;

  const response = await getAllOrderByMonth(selectedMonth.value);
  let result = response;
  if (selectedEmployee.value != "") {
    result = result.filter(
      (item) =>
        item.fullName.trim().toLowerCase() ==
        selectedEmployee.value.trim().toLowerCase()
    );
  }
  if (selectedPaymentMethod.value != "") {
    result = result.filter(
      (item) =>
        item.paymentMethod.trim().toLowerCase() ==
        selectedPaymentMethod.value.trim().toLowerCase()
    );
  }
  allBilling.value = result;
  filterAllBilling.value = allBilling.value.sort(
    (a, b) => b.orderTime - a.orderTime
  );
}

async function selectEmployeeAndCallAPI(emp) {
  selectedDay.value = "";
  selectedMonth.value = "";
  selectedCurrentDay.value = "";
  selectedEmployee.value = emp;
  const response = await getAllOrderByFullName(selectedEmployee.value);
  filterAllBilling.value = response;
}
async function filterBillByPaymentMethod(method) {
  selectedDay.value = "";
  selectedMonth.value = "";
  selectedCurrentDay.value = "";
  selectedEmployee.value = "";
  selectedPaymentMethod.value = method.paymentName;
  console.log("selectedPaymentMethod: ", selectedPaymentMethod.value);
  const response = await getAllOrderByPaymentMethod(
    selectedPaymentMethod.value
  );
  console.log("res: ", response);
  filterAllBilling.value = response;
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
async function onRowClick(event, item) {
  selectedBill.value = item.item;
  showDialogOrderDetails.value = true;
  const request = {
    OrderId: selectedBill.value.orderId,
  };
  const response = await GetOrderDetailsByOrderId(request);
  orderDetails.value = response.result;
}
const dataTable = computed(() => {
  return filterAllBilling.value?.map((item) => ({
    fullName: item.fullName || "-",
    orderTime: formatDateFormApiToView(item.orderTime),
    tableName: item.tableName,
    receivedAmount: formatCurrencyFromApiToView(item.receivedAmount),
    returnedAmount: formatCurrencyFromApiToView(item.returnedAmount),
    totalAmount: formatCurrencyFromApiToView(item.totalAmount),
    discount: item.discount != null ? item.discount + "%" : "-",
    paymentMethod: item.paymentMethod,
  }));
});
const datafieldExcel = {
  "Họ tên": "fullName",
  "Thời gian gọi": "orderTime",
  Bàn: "tableName",
  "Tiền nhận": "receivedAmount",
  "Tiền thừa": "returnedAmount",
  "Tổng thanh toán": "totalAmount",
  "Giảm giá": "discount",
  "Phương thức thanh toán": "paymentMethod",
};
const nameFileExcel = computed(() => {
  return `bao_cao_hoa_don_${currentDay}_${currentMonth}_${currentYear}.xlsx`;
});
</script>
