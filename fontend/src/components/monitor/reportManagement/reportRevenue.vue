<template>
  <div
    class="reportManagement_totalAmount d-flex flex-wrap flex-column pa-2 rounded"
    style="height: 100%"
  >
    <v-card style="height: 100%" class="mb-2">
      <v-card-title class="pa-0 d-flex justify-center">
        <div class="d-flex align-center mb-1">
          <v-icon size="large" class="ma-1">mdi-file-chart-outline</v-icon>
          <span style="font-size: 26px"
            >BÁO CÁO DOANH THU THÁNG {{ currentMonth }}</span
          >
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
        class="px-2 h-100"
        :style="{ backgroundColor: 'var(--bg-color-item)' }"
      >
        <div class="d-flex flex-wrap justify-space-between h-100">
          <div class="" style="width: 59%">
            <!-- Báo cáo doanh thu theo nhân viên -->
            <div class="mt-1 d-flex flex-column" style="height: 60%">
              <h2 class="text-center w-100 opacity-50">Theo Nhân Viên</h2>
              <div
                class="reportManagement_chosseTime d-flex justify-md-space-between"
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
                      >
                        {{ selectedDayForEmp || "Ngày" }}
                      </v-btn>
                    </template>
                    <v-list max-height="200px" style="overflow-y: auto">
                      <v-list-item
                        v-for="day in dateList"
                        :key="day"
                        :value="day"
                        style="min-height: 36px !important"
                        @click="selectDayAndCallAPIForEmployee(day)"
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
                        {{ selectedMonthForEmp || "Tháng" }}
                      </v-btn>
                    </template>
                    <v-list max-height="200px" style="overflow-y: auto">
                      <v-list-item
                        v-for="(month, index) in monthList"
                        :key="index"
                        :value="month"
                        style="min-height: 36px !important"
                        @click="selectMonthAndCallAPIForEmployee(month)"
                      >
                        <v-list-item-title>{{ month }}</v-list-item-title>
                      </v-list-item>
                    </v-list>
                  </v-menu>
                </div>
                <v-btn
                  style="border: 1px solid #333; min-width: 60px"
                  size="small"
                  @click="resetTimeFillterRevenueOrderForEmployee"
                >
                  Làm mới
                </v-btn>
              </div>
              <!-- style="height: 405px; max-height: 405px; overflow-y: auto" -->
              <div
                class="reportManagement_totalAmount_salesSummary flex-1-0 d-flex mt-1 mb-1"
                style="overflow-y: auto"
              >
                <v-data-table
                  :headers="headersEmployee"
                  :loading="loading"
                  :items="filteredTotalRevenueForEmployee"
                  height="calc(33vh - 2rem)"
                  density="compact"
                  fixed-footer
                  fixed-header
                >
                  <template v-slot:item.fullName="{ item }">
                    <span
                      style="
                        font-weight: 500;
                        color: rgba(var(--v-theme-primary), 1);
                      "
                    >
                      {{ item.fullName ? item.fullName : "-" }}
                    </span>
                  </template>
                  <template v-slot:item.totalRevenue="{ item }">
                    <div class="d-user-contract-inventory-chart-progress-bar">
                      <div
                        class="d-user-contract-inventory-chart-progress-bar-container"
                      >
                        <div
                          class="d-user-contract-inventory-chart-progress-bar-indicator"
                          :style="{
                            width:
                              (item.totalRevenue / totalRevenueMax) * 100 + '%',
                          }"
                        ></div>
                      </div>
                      <div
                        class="d-user-contract-inventory-chart-progress-bar-text"
                      >
                        {{
                          item.totalRevenue
                            ? formatCurencyFromApiToView(item.totalRevenue)
                            : 0
                        }}
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
                      <VIcon class="ma-2" icon="mdi-robot-dead-outline"></VIcon>
                      <span>Hệ thống không tìm thấy thông tin</span>
                    </div>
                  </template>
                </v-data-table>
              </div>
            </div>
            <!-- Báo cáo doanh thu theo thời gian -->
            <div class="mb-2 mt-2">
              <h2 class="text-center w-100 opacity-50">
                Tổng doanh thu - Số lượng đơn hàng hôm nay
              </h2>
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
              <div class="reportManagement_totalAmount_salesSummary d-flex">
                <!-- Hiển thị tổng doanh thu -->
                <div
                  class="reportManagement_totalAmount_salesSummary_revenue w-50 pa-3 rounded-lg me-1"
                  style="
                    border: 1px solid #333;
                    background-color: rgb(32 32 32);
                  "
                >
                  <!-- Nếu có tháng được chọn -->
                  <div v-if="selectedMonth != ''">
                    <span>Doanh thu</span>
                    <h2>{{ formatCurencyFromApiToView(totalRevenue) }} vnđ</h2>
                    <div>
                      <v-icon
                        color="green"
                        v-if="totalRevenue > totalRevenueYesterday"
                        >mdi-trending-up</v-icon
                      >
                      <v-icon color="red" v-else>mdi-trending-down</v-icon>
                      <span
                        style="color: green"
                        v-if="totalRevenue > totalRevenueYesterday"
                        >{{ calculateRevenuePercentage.toFixed(1) }}%</span
                      >
                      <span style="color: red" v-else
                        >{{ calculateRevenuePercentage.toFixed(1) }}%</span
                      >
                      <span style="font-size: 12px; margin-left: 6px"
                        >so với {{ previousMonth }}</span
                      >
                    </div>
                  </div>
                  <!-- Nếu có ngày được chọn -->
                  <div v-else-if="selectedDay != ''">
                    <span>Doanh thu</span>
                    <h2>{{ formatCurencyFromApiToView(totalRevenue) }}</h2>
                    <div>
                      <v-icon
                        color="green"
                        v-if="totalRevenue > totalRevenueYesterday"
                        >mdi-trending-up</v-icon
                      >
                      <v-icon color="red" v-else>mdi-trending-down</v-icon>
                      <span
                        style="color: green"
                        v-if="totalRevenue > totalRevenueYesterday"
                        >{{ calculateRevenuePercentage.toFixed(1) }}%</span
                      >
                      <span style="color: red" v-else
                        >{{ calculateRevenuePercentage.toFixed(1) }}%</span
                      >
                      <span style="font-size: 12px; margin-left: 6px">
                        so với {{ previousDay }}
                      </span>
                    </div>
                  </div>
                  <!-- Nếu chọn ngày là hôm nay -->
                  <div v-else-if="selectedCurrentDay != ''">
                    <span>Doanh thu</span>
                    <h2>{{ formatCurencyFromApiToView(totalRevenue) }} vnđ</h2>
                    <div>
                      <v-icon
                        v-if="totalRevenue > totalRevenueYesterday"
                        color="green"
                        >mdi-trending-up</v-icon
                      >
                      <v-icon v-else color="red">mdi-trending-down</v-icon>
                      <span
                        style="color: green"
                        v-if="totalRevenue > totalRevenueYesterday"
                        >{{ calculateRevenuePercentage.toFixed(1) }}%</span
                      >
                      <span style="color: red" v-else
                        >{{ calculateRevenuePercentage.toFixed(1) }}%</span
                      >
                      <span style="font-size: 12px; margin-left: 6px"
                        >so với {{ previousCurrentDay }}</span
                      >
                    </div>
                  </div>
                  <!-- Nếu không có lựa chọn nào (Hiển thị mặc định) -->
                  <div v-else>
                    <span>Doanh thu</span>
                    <h2>999,999,999 vnđ</h2>
                    <div>
                      <v-icon color="green">mdi-trending-up</v-icon>
                      <span style="color: green">100%</span>
                      <span style="font-size: 12px; margin-left: 4px"
                        >từ dd/MM/yyyy - dd/MM/yyyy</span
                      >
                    </div>
                  </div>
                </div>
                <!-- Hiển thị tổng đơn hàng -->
                <div
                  class="reportManagement_totalAmount_salesSummary_order w-50 pa-3 rounded-lg ms-1"
                  style="
                    border: 1px solid #333;
                    background-color: rgb(32 32 32);
                  "
                >
                  <!-- Nếu có tháng được chọn -->
                  <div v-if="selectedMonth != ''">
                    <span>Đơn hàng</span>
                    <h2>{{ totalOrders }}</h2>
                    <div>
                      <v-icon
                        color="green"
                        v-if="totalOrders > totalOrdersYesterday"
                        >mdi-trending-up</v-icon
                      >
                      <v-icon color="red" v-else>mdi-trending-down</v-icon>
                      <span
                        style="color: green"
                        v-if="totalOrders > totalOrdersYesterday"
                        >{{ calculateOrderPercentage.toFixed(1) }}%</span
                      >
                      <span style="color: red" v-else
                        >{{ calculateOrderPercentage.toFixed(1) }}%</span
                      >
                      <span style="font-size: 12px; margin-left: 6px">
                        so với {{ previousMonth }}
                      </span>
                    </div>
                  </div>

                  <!-- Nếu có ngày được chọn -->
                  <div v-else-if="selectedDay != ''">
                    <span>Đơn hàng</span>
                    <h2>{{ totalOrders }}</h2>
                    <div>
                      <v-icon
                        color="green"
                        v-if="totalOrders > totalOrdersYesterday"
                        >mdi-trending-up</v-icon
                      >
                      <v-icon color="red" v-else>mdi-trending-down</v-icon>
                      <span
                        style="color: green"
                        v-if="totalOrders > totalOrdersYesterday"
                        >{{ calculateOrderPercentage.toFixed(1) }}%</span
                      >
                      <span style="color: red" v-else
                        >{{ calculateOrderPercentage.toFixed(1) }}%</span
                      >
                      <span style="font-size: 12px; margin-left: 6px">
                        so với {{ previousDay }}
                      </span>
                    </div>
                  </div>

                  <!-- Nếu chọn ngày là hôm nay -->
                  <div v-else-if="selectedCurrentDay != ''">
                    <span>Đơn hàng</span>
                    <h2>{{ totalOrders }}</h2>
                    <div>
                      <v-icon
                        color="green"
                        v-if="totalOrders > totalOrdersYesterday"
                        >mdi-trending-up</v-icon
                      >
                      <v-icon color="red" v-else>mdi-trending-down</v-icon>
                      <span
                        style="color: green"
                        v-if="totalOrders > totalOrdersYesterday"
                        >{{ calculateOrderPercentage.toFixed(1) }}%</span
                      >
                      <span style="color: red" v-else
                        >{{ calculateOrderPercentage.toFixed(1) }}%</span
                      >
                      <span style="font-size: 12px; margin-left: 6px">
                        so với {{ previousCurrentDay }}
                      </span>
                    </div>
                  </div>

                  <!-- Nếu không có lựa chọn nào (Hiển thị mặc định) -->
                  <div v-else>
                    <span>Đơn hàng</span>
                    <h2>9999</h2>
                    <div>
                      <v-icon color="green">mdi-trending-up</v-icon>
                      <span style="color: green">100%</span>
                      <span style="font-size: 12px; margin-left: 4px">
                        từ dd/MM/yyyy - dd/MM/yyyy
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="mt-1 mb-2" style="width: 40%">
            <h2 class="text-center w-100 opacity-50">Theo Danh Mục</h2>
            <div
              class="reportManagement_chosseTime d-flex justify-md-space-between"
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
                      {{ selectedDayForCate || "Ngày" }}
                    </v-btn>
                  </template>
                  <v-list max-height="200px" style="overflow-y: auto">
                    <v-list-item
                      v-for="day in dateList"
                      :key="day"
                      :value="day"
                      style="min-height: 36px !important"
                      @click="selectDayAndCallAPIForCategory(day)"
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
                      {{ selectedMonthForCate || "Tháng" }}
                    </v-btn>
                  </template>
                  <v-list max-height="200px" style="overflow-y: auto">
                    <v-list-item
                      v-for="(month, index) in monthList"
                      :key="index"
                      :value="month"
                      style="min-height: 36px !important"
                      @click="selectMonthAndCallAPIForCategory(month)"
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
                @click="resetTimeFillterRevenueOrderForCategory"
              >
                Làm mới
              </v-btn>
            </div>
            <Pie :chart-data="chartData" :chart-options="chartOptions" />
          </div>
        </div>
      </v-card-text>
    </v-card>
  </div>
</template>
<script setup>
import useReportRevenue from "./reportRevenue";
import { Pie } from "vue-chartjs";
import { Chart as ChartJS, Title, Tooltip, Legend, ArcElement } from "chart.js";

ChartJS.register(Title, Tooltip, Legend, ArcElement);
const {
  generateDates,
  generateMonths,
  currentDate,
  currentMonth,
  currentYear,
  dateList,
  monthList,
  selectedMonth,
  selectedDay,
  selectedCurrentDay,
  totalRevenueOrderCurrentDay,
  totalRevenueOrderCurrentMonth,
  totalRevenue,
  totalRevenueYesterday,
  totalOrders,
  totalOrdersYesterday,
  totalRevenueMax,
  totalRevenueForCategoryMax,
  loading,
  filteredTotalRevenueForEmployee,
  filteredTotalRevenueForCategory,
  tab,
  selectedMonthForEmp,
  selectedDayForEmp,
  selectedDayForCate,
  selectedMonthForCate,
  init,
  foodCategories,
  listLabels,
  listDatas,
  listColor,
  chartData,
  chartOptions,
  selectMonthAndCallAPI,
  selectDayAndCallAPI,
  selectCurrentDayAndCallAPI,
  previousDay,
  previousMonth,
  previousCurrentDay,
  calculateRevenuePercentage,
  calculateOrderPercentage,
  resetTimeFillterRevenueOrder,
  headersEmployee,
  headersCategory,
  callApi_GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH,
  callApi_GET_ALL_FOOD_CATEGORIES,
  callApi_GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH,
  selectDayAndCallAPIForEmployee,
  selectMonthAndCallAPIForEmployee,
  resetTimeFillterRevenueOrderForEmployee,
  selectDayAndCallAPIForCategory,
  selectMonthAndCallAPIForCategory,

  resetTimeFillterRevenueOrderForCategory,
  formatCurencyFromApiToView,
} = useReportRevenue();
</script>
