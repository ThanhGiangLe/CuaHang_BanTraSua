<template>
  <div
    class="reportManagement_bestsellingItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
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
        <div style="max-width: 1080px">
          <canvas ref="chartRef"></canvas>
        </div>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup>
import { reportManagementHandler } from "/src/composables/reportManagement/reportManagementHandler.js";
import { ref, onMounted } from "vue";
import { Chart, registerables } from "chart.js";

const { getAllBestSellingByMonth, getAllBestSellingByDay } =
  reportManagementHandler();
const allItemBestSeling = ref([]);
const loading = shallowRef(true);
const currentDate = new Date();
const currentMonth = currentDate.getMonth() + 1; // Tháng hiện tại (cộng thêm 1 vì getMonth() trả về giá trị từ 0 đến 11)
const currentYear = currentDate.getFullYear(); // Năm hiện tại
const currentDay = currentDate.getDate();
const selectedCurrentDay = ref("");
const selectedDay = ref("");
const selectedMonth = ref("");

Chart.register(...registerables);

const chartRef = ref(null);
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

function renderChart(dataItems) {
  if (chartInstance) chartInstance.destroy();

  const labels = dataItems.map((item) => item.foodName);
  const data = dataItems.map((item) => item.quantitySold);

  chartInstance = new Chart(chartRef.value, {
    type: "bar",
    data: {
      labels,
      datasets: [
        {
          label: "Số lượng bán ra",
          data,
          backgroundColor: "rgba(54, 162, 235, 0.7)",
          borderRadius: 6,
        },
      ],
    },
    options: {
      indexAxis: "y", // Horizontal
      responsive: true,
      plugins: {
        legend: { display: false },
        title: {
          display: true,
          text: "Top 10 món bán chạy",
          font: { size: 18 },
        },
        tooltip: {
          callbacks: {
            label: (context) => `${context.parsed.x} ly`,
          },
        },
      },
      scales: {
        x: {
          beginAtZero: true,
          ticks: { stepSize: 10 },
        },
      },
    },
  });
}

async function init() {
  const monthFormat = currentMonth.toString().padStart(2, "0"); // Đảm bảo tháng có 2 chữ số
  const response = await getAllBestSellingByMonth(
    `${monthFormat}-${currentYear}`
  );
  allItemBestSeling.value = response;

  allItemBestSeling.value = [...response]
    .sort((a, b) => b.quantitySold - a.quantitySold)
    .slice(0, 10);

  renderChart(allItemBestSeling.value);

  loading.value = false;
}
init();

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

  const response = await getAllBestSellingByDay(selectedCurrentDay.value);
  allItemBestSeling.value = response;
  allItemBestSeling.value = [...response]
    .sort((a, b) => b.quantitySold - a.quantitySold)
    .slice(0, 10);

  renderChart(allItemBestSeling.value);
}
async function selectDayAndCallAPI(day) {
  selectedMonth.value = "";
  selectedDay.value = day;
  let [dayf, monthf, yearf] = selectedDay.value.split("-");
  dayf = dayf.padStart(2, "0");
  monthf = monthf.padStart(2, "0");
  selectedDay.value = `${dayf}-${monthf}-${yearf}`;
  const responseTotal1 = await getAllBestSellingByDay(selectedDay.value);
  allItemBestSeling.value = responseTotal1;
  allItemBestSeling.value = [...responseTotal1]
    .sort((a, b) => b.quantitySold - a.quantitySold)
    .slice(0, 10);
  renderChart(allItemBestSeling.value);
}
async function selectMonthAndCallAPI(month) {
  selectedDay.value = "";
  selectedMonth.value = month;
  let [monthf, yearf] = selectedMonth.value.split("-");
  monthf = monthf.padStart(2, "0");
  selectedMonth.value = `${monthf}-${yearf}`;

  const response = await getAllBestSellingByMonth(selectedMonth.value);
  allItemBestSeling.value = response;
  allItemBestSeling.value = [...response]
    .sort((a, b) => b.quantitySold - a.quantitySold)
    .slice(0, 10);
  renderChart(allItemBestSeling.value);
}

const resetTimeFillterRevenueOrder = () => {
  selectedDay.value = "";
  selectedMonth.value = "";
  selectedCurrentDay.value = "";
  init();
};
</script>
