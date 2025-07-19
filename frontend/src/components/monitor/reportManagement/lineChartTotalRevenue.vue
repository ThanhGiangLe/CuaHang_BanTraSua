<template>
  <div
    class="reportManagement_bestsellingItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
      <v-card-text
        class="pa-3 rounded mb-2"
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
              @click="selectedCurrentDayAndCallAPI"
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
                  @click="selectedDayAndCallAPI(day)"
                >
                  <v-list-item-title style="font-size: 0.8rem">{{
                    day
                  }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
            <!-- Lọc theo tuần -->
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
                  {{ selectedWeek || "Tuần" }}
                </v-btn>
              </template>
              <v-list max-height="200px" style="overflow-y: auto">
                <v-list-item
                  v-for="week in ['Tuần 1', 'Tuần 2', 'Tuần 3', 'Tuần 4']"
                  :key="week"
                  :value="week"
                  style="min-height: 36px !important"
                  @click="selectedWeekAndCallAPI(week)"
                >
                  <v-list-item-title style="font-size: 0.8rem">{{
                    week
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
                  @click="selectedMonthAndCallAPI(month)"
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
        <div style="max-width: 1080px; margin: 0 auto">
          <canvas ref="lineChart"></canvas>
        </div>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup>
import { reportManagementHandler } from "/src/composables/reportManagement/reportManagementHandler.js";
import { ref, onMounted } from "vue";
import { Chart, registerables } from "chart.js";

const { GetRevenueOrderByDay, GetRevenueOrderByWeek, GetRevenueOrderByMonth } =
  reportManagementHandler();
const allItemBestSeling = ref([]);
const loading = shallowRef(true);
const currentDate = new Date();
const currentMonth = currentDate.getMonth() + 1; // Tháng hiện tại (cộng thêm 1 vì getMonth() trả về giá trị từ 0 đến 11)
const currentYear = currentDate.getFullYear(); // Năm hiện tại
const currentDay = currentDate.getDate();

const selectedMonth = ref("");
const selectedWeek = ref("");
const selectedDay = ref("");
const selectedCurrentDay = ref("");

Chart.register(...registerables);

const lineChart = ref(null);
let chartInstance = null;

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

const renderLineChart = (
  canvasRef,
  labels,
  dataChartOne,
  dataChartTwo,
  chartTitle = "DOANH THU VÀ SỐ LƯỢNG ĐƠN"
) => {
  const ctx = canvasRef.getContext("2d");

  if (chartInstance) {
    chartInstance.destroy();
  }

  chartInstance = new Chart(ctx, {
    type: "line",
    data: {
      labels: labels,
      datasets: [
        {
          label: "Doanh thu (nghìn VNĐ)",
          data: dataChartOne,
          borderColor: "rgba(54, 162, 235, 1)",
          backgroundColor: "rgba(54, 162, 235, 0.2)",
          fill: false,
          tension: 0.2,
          pointBackgroundColor: "white",
          pointBorderColor: "rgba(54,162,235,1)",
          pointRadius: 4,
        },
        {
          label: "Số lượng đơn",
          data: dataChartTwo,
          borderColor: "rgba(255, 99, 132, 1)",
          backgroundColor: "rgba(255, 99, 132, 0.2)",
          fill: false,
          tension: 0.2,
          pointBackgroundColor: "white",
          pointBorderColor: "rgba(255,99,132,1)",
          pointRadius: 4,
        },
      ],
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          display: true,
          position: "top",
        },
        title: {
          display: true,
          text: chartTitle,
        },
      },
      scales: {
        y: {
          beginAtZero: true,
          title: {
            display: true,
            text: "Giá trị",
          },
        },
        x: {
          title: {
            display: true,
            text: "Thời gian",
          },
        },
      },
    },
  });
};

async function init() {
  loading.value = true;
  selectedCurrentDayAndCallAPI();
  loading.value = false;
}
init();

async function selectedCurrentDayAndCallAPI() {
  const day = currentDate.getDate();
  selectedDay.value = "";
  selectedWeek.value = "";
  selectedMonth.value = "";
  const dayf = day.toString().padStart(2, "0");
  const dayRequest = `${dayf}-${currentMonth
    .toString()
    .padStart(2, "0")}-${currentYear}`;
  const response = await GetRevenueOrderByDay({
    Date: dayRequest,
  });
  const results = response.result;
  const labels = [];
  const dataChartOne = [];
  const dataChartTwo = [];
  for (const key in results) {
    const item = results[key];
    labels.push(item.timeRange);
    dataChartOne.push(item.totalAmount / 1000);
    dataChartTwo.push(item.totalOrder);
  }
  renderLineChart(lineChart.value, labels, dataChartOne, dataChartTwo);
}
async function selectedDayAndCallAPI(day) {
  selectedDay.value = day;
  selectedCurrentDay.value = "";
  selectedWeek.value = "";
  selectedMonth.value = "";
  let [dayf, monthf, yearf] = selectedDay.value.split("-");
  dayf = dayf.padStart(2, "0");
  monthf = monthf.padStart(2, "0");
  selectedDay.value = `${dayf}-${monthf}-${yearf}`;
  const response = await GetRevenueOrderByDay({
    Date: selectedDay.value,
  });
  const results = response.result;
  console.log("results: ", results);
  const labels = [];
  const dataChartOne = [];
  const dataChartTwo = [];
  for (const key in results) {
    const item = results[key];
    labels.push(item.timeRange);
    dataChartOne.push(item.totalAmount / 1000);
    dataChartTwo.push(item.totalOrder);
  }
  renderLineChart(lineChart.value, labels, dataChartOne, dataChartTwo);
}
async function selectedWeekAndCallAPI(week) {
  selectedWeek.value = week;
  selectedCurrentDay.value = "";
  selectedDay.value = "";
  selectedMonth.value = "";
  const weekNumber = selectedWeek.value.charAt(week.toString().length - 1);
  const response = await GetRevenueOrderByWeek({
    Date: weekNumber,
  });
  const results = response.result;
  const labels = [];
  const dataChartOne = [];
  const dataChartTwo = [];
  for (const key in results) {
    const item = results[key];
    labels.push(item.timeRange);
    dataChartOne.push(item.totalAmount / 1000);
    dataChartTwo.push(item.totalOrder);
  }
  renderLineChart(lineChart.value, labels, dataChartOne, dataChartTwo);
}
async function selectedMonthAndCallAPI(month) {
  selectedMonth.value = month;
  selectedCurrentDay.value = "";
  selectedDay.value = "";
  selectedWeek.value = "";
  let [monthf, yearf] = selectedMonth.value.split("-");
  monthf = monthf.padStart(2, "0");
  selectedMonth.value = `${monthf}-${yearf}`;
  const response = await GetRevenueOrderByMonth({
    Date: selectedMonth.value,
  });
  const results = response.result;
  const labels = [];
  const dataChartOne = [];
  const dataChartTwo = [];
  for (const key in results) {
    const item = results[key];
    labels.push(item.timeRange);
    dataChartOne.push(item.totalAmount / 1000);
    dataChartTwo.push(item.totalOrder);
  }
  renderLineChart(lineChart.value, labels, dataChartOne, dataChartTwo);
}
const resetTimeFillterRevenueOrder = () => {
  selectedDay.value = "";
  selectedMonth.value = "";
  selectedCurrentDay.value = "";
  selectedWeek.value = "";
  init();
};
</script>
