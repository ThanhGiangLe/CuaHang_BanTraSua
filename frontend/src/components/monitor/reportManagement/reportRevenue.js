import { ref } from "vue";
import { computed } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { Pie } from "vue-chartjs";
import { Chart as ChartJS, Title, Tooltip, Legend, ArcElement } from "chart.js";
import { reportManagementHandler } from "/src/composables/reportManagement/reportManagementHandler.js";
import { foodManagementHandler } from "/src/composables/foodManagement/foodManagementHandler.js";
ChartJS.register(Title, Tooltip, Legend, ArcElement);

export default function useReportRevenue() {
  const {
    getAllRevenueByMonth,
    gettAllRevenueByDay,
    gettAllRevenueWithEmployeeByMonth,
    gettAllRevenueWithEmployeeByDay,
    gettAllRevenueWithCategoryByMonth,
    gettAllRevenueWithCategoryByDay,
  } = reportManagementHandler();
  const { getAllCategory } = foodManagementHandler();
  const currentDate = new Date();
  const currentMonth = currentDate.getMonth() + 1; // Tháng hiện tại (cộng thêm 1 vì getMonth() trả về giá trị từ 0 đến 11)
  const currentYear = currentDate.getFullYear(); // Năm hiện tại
  const currentDay = currentDate.getDate();

  const selectedMonth = ref("");
  const selectedDay = ref("");
  const selectedCurrentDay = ref("");
  const totalRevenueOrderCurrentDay = ref([]);
  const totalRevenueOrderCurrentMonth = ref([]);
  const totalRevenue = ref(0);
  const totalRevenueYesterday = ref(0);
  const totalOrders = ref(0);
  const totalOrdersYesterday = ref(0);
  const totalRevenueMax = ref(0);
  const totalRevenueForCategoryMax = ref(0);
  const loading = shallowRef(true);
  const filteredTotalRevenueForEmployee = ref([]);
  const filteredTotalRevenueForCategory = ref([]);
  const selectedMonthForEmp = ref("");
  const selectedDayForEmp = ref("");
  const selectedDayForCate = ref("");
  const selectedMonthForCate = ref("");
  init();
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
  const isBeforeMonth = (dateString) => {
    const [month, year] = dateString.split("-");
    return month <= currentMonth;
  };
  const fullDateList = generateDates(currentMonth, currentYear);
  const dateList = ref(fullDateList.filter((item) => isBeforeToday(item)));
  const fullMonthList = generateMonths(currentYear);
  const monthList = ref(fullMonthList.filter((item) => isBeforeMonth(item)));
  const headersEmployee = ref([
    { title: "Tên Nhân Viên", key: "fullName", width: "35%" },
    { title: "Tổng doanh thu", key: "totalRevenue", width: "65%" },
  ]);
  const foodCategories = ref([]);
  const listLabels = ref([
    "Best thèm",
    "Món mới",
    "Đậm vị",
    "Trà sữa truyền thống",
    "Trà sữa mix",
    "Thèm nhai đã",
    "Siêu tiết kiệm / Combo hot",
    "Thơm béo ngất ngay",
    "Món thêm",
  ]);
  const listDatas = ref([1, 1, 1, 1, 1, 1, 1, 1, 1]);
  const listColor = ref([
    "#bf360c", // đỏ nâu
    "#00acc1", // xanh dương ngọc
    "#710808", // đỏ rượu
    "#388e3c", // xanh lá
    "#c2185b", // hồng đậm
    "#ffa000", // vàng cam
    "#6a1b9a", // tím đậm
    "#00796b", // xanh ngọc đậm
    "#f4511e", // cam cháy
  ]);
  const chartData = ref({
    labels: listLabels.value,
    datasets: [
      {
        label: "Dataset 1",
        data: listDatas.value,
        backgroundColor: listColor.value,
        hoverOffset: 4,
      },
    ],
  });
  const chartOptions = ref({
    responsive: true,
    maintainAspectRatio: false, // Cho phép thay đổi tỉ lệ
    plugins: {
      legend: {
        position: "top",
      },
      title: {
        display: true,
      },
    },
  });

  // <== Tổng doanh thu - Số lượng đơn hàng hôm nay ==>
  async function selectMonthAndCallAPI(month) {
    selectedMonth.value = month;
    selectedDay.value = "";
    selectedCurrentDay.value = "";
    let [monthf, yearf] = selectedMonth.value.split("-");
    monthf = monthf.padStart(2, "0");
    selectedMonth.value = `${monthf}-${yearf}`;

    const responseTotal1 = await getAllRevenueByMonth(selectedMonth.value);
    console.log("responseTotal1: ", responseTotal1);
    totalRevenueOrderCurrentMonth.value = responseTotal1;

    totalRevenue.value =
      totalRevenueOrderCurrentMonth.value.totalAmountCurrentMonth;
    totalRevenueYesterday.value =
      totalRevenueOrderCurrentMonth.value.totalAmountLastMonth;
    totalOrders.value =
      totalRevenueOrderCurrentMonth.value.totalOrdersCurrentMonth;
    totalOrdersYesterday.value =
      totalRevenueOrderCurrentMonth.value.totalOrdersLastMonth;
  }
  async function selectDayAndCallAPI(day) {
    selectedDay.value = day;
    selectedMonth.value = "";
    selectedCurrentDay.value = "";
    let [dayf, monthf, yearf] = selectedDay.value.split("-");
    dayf = dayf.padStart(2, "0");
    monthf = monthf.padStart(2, "0");
    selectedDay.value = `${dayf}-${monthf}-${yearf}`;
    const responseTotal1 = await gettAllRevenueByDay(selectedDay.value);
    totalRevenueOrderCurrentDay.value = responseTotal1;

    totalRevenue.value = totalRevenueOrderCurrentDay.value.totalAmount;
    totalRevenueYesterday.value =
      totalRevenueOrderCurrentDay.value.totalAmountYesterday;
    totalOrders.value = totalRevenueOrderCurrentDay.value.totalOrders;
    totalOrdersYesterday.value =
      totalRevenueOrderCurrentDay.value.totalOrdersYesterday;
  }
  async function selectCurrentDayAndCallAPI() {
    const day = currentDate.getDate();
    selectedMonth.value = "";
    selectedDay.value = "";
    const formattedDate = `${day < 10 ? "0" + day : day}-${
      currentMonth < 10 ? "0" + currentMonth : currentMonth
    }-${currentYear}`;
    selectedCurrentDay.value = formattedDate;

    const responseTotal1 = await gettAllRevenueByDay(selectedCurrentDay.value);
    totalRevenueOrderCurrentDay.value = responseTotal1;

    totalRevenue.value = totalRevenueOrderCurrentDay.value.totalAmount;
    totalRevenueYesterday.value =
      totalRevenueOrderCurrentDay.value.totalAmountYesterday;
    totalOrders.value = totalRevenueOrderCurrentDay.value.totalOrders;
    totalOrdersYesterday.value =
      totalRevenueOrderCurrentDay.value.totalOrdersYesterday;
  }
  const previousDay = computed(() => {
    if (!selectedDay.value) return "";
    const [day, month, year] = selectedDay.value.split("-").map(Number);
    const currentDate = new Date(year, month - 1, day); // Chuyển đổi thành đối tượng Date
    currentDate.setDate(currentDate.getDate() - 1); // Trừ một ngày
    const previousDay = currentDate.getDate().toString().padStart(2, "0"); // Đảm bảo có 2 chữ số
    const previousMonth = (currentDate.getMonth() + 1)
      .toString()
      .padStart(2, "0"); // Đảm bảo có 2 chữ số
    const previousYear = currentDate.getFullYear(); // Năm không cần định dạng
    return `${previousDay}-${previousMonth}-${previousYear}`;
  });
  const previousMonth = computed(() => {
    if (!selectedMonth.value) return "";
    const [month, year] = selectedMonth.value.split("-").map(Number);
    const currentDate = new Date(year, month - 1, 1); // Tạo đối tượng Date với ngày đầu tháng
    currentDate.setMonth(currentDate.getMonth() - 1); // Trừ một tháng

    const previousMonth = currentDate.getMonth() + 1; // Tháng (tăng 1 vì tháng bắt đầu từ 0)
    const previousYear = currentDate.getFullYear(); // Năm

    return `${previousMonth}-${previousYear}`;
  });
  const previousCurrentDay = computed(() => {
    if (!selectedCurrentDay.value) return "";
    const [day, month, year] = selectedCurrentDay.value.split("-").map(Number);
    const currentDate = new Date(year, month - 1, day); // Chuyển đổi thành đối tượng Date
    currentDate.setDate(currentDate.getDate() - 1); // Trừ một ngày
    const previousDay = currentDate.getDate().toString().padStart(2, "0"); // Đảm bảo có 2 chữ số
    const previousMonth = (currentDate.getMonth() + 1)
      .toString()
      .padStart(2, "0"); // Đảm bảo có 2 chữ số
    const previousYear = currentDate.getFullYear(); // Năm không cần định dạng
    return `${previousDay}-${previousMonth}-${previousYear}`;
  });
  const calculateRevenuePercentage = computed(() => {
    if (totalRevenue.value == 0 && totalRevenueYesterday.value == 0) {
      return 0;
    } else if (totalRevenue.value != 0 && totalRevenueYesterday.value == 0) {
      return 100;
    } else if (totalRevenue.value == 0 && totalRevenueYesterday.value != 0) {
      return 100;
    }
    if (totalRevenue.value > totalRevenueYesterday.value) {
      return (totalRevenue.value / totalRevenueYesterday.value) * 100;
    } else {
      return (totalRevenueYesterday.value / totalRevenue.value) * 100;
    }
  });
  const calculateOrderPercentage = computed(() => {
    if (totalOrders.value == 0 && totalOrdersYesterday.value == 0) {
      return 0;
    } else if (totalOrders.value != 0 && totalOrdersYesterday.value == 0) {
      return 100;
    } else if (totalOrders.value == 0 && totalOrdersYesterday.value != 0) {
      return 100;
    }
    if (totalOrders.value > totalOrdersYesterday.value) {
      return (totalOrders.value / totalOrdersYesterday.value) * 100;
    } else {
      return (totalOrdersYesterday.value / totalOrders.value) * 100;
    }
  });
  const resetTimeFillterRevenueOrder = () => {
    selectedDay.value = "";
    selectedMonth.value = "";
    selectedCurrentDay.value = "";
    selectCurrentDayAndCallAPI();
  };

  // <== Init ==>
  async function callApi_GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH() {
    const formattedMonth = currentMonth.toString().padStart(2, "0");
    const responseRevenue = await gettAllRevenueWithEmployeeByMonth(
      `${formattedMonth}-${currentYear}`
    );
    console.log("gettAllRevenueWithEmployeeByMonth: ", responseRevenue);
    if (responseRevenue) {
      filteredTotalRevenueForEmployee.value = responseRevenue.sort(
        (a, b) => b.totalRevenue - a.totalRevenue
      );
    }
    const totalRevenueMaxDataTable = filteredTotalRevenueForEmployee.value
      ? filteredTotalRevenueForEmployee.value.reduce((max, current) => {
          return current.totalRevenue > max.totalRevenue ? current : max;
        }, filteredTotalRevenueForEmployee.value[0])
      : null;
    totalRevenueMax.value = totalRevenueMaxDataTable
      ? totalRevenueMaxDataTable.totalRevenue
      : 0;
  }
  async function callApi_GET_ALL_FOOD_CATEGORIES() {
    const response = await getAllCategory();
    foodCategories.value = response.listCategory;
    listLabels.value = foodCategories.value.map((c) => c.categoryName);
  }
  async function callApi_GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH() {
    const formattedMonth = currentMonth.toString().padStart(2, "0");
    const responseRevenueForCategory = await gettAllRevenueWithCategoryByMonth(
      `${formattedMonth}-${currentYear}`
    );
    if (responseRevenueForCategory.length != 0) {
      const revenueMap = responseRevenueForCategory.reduce((acc, item) => {
        acc[item.categoryName] = item.totalRevenue;
        return acc;
      }, {});
      listDatas.value = listLabels.value.map((label) => revenueMap[label] || 0);
    } else {
      listDatas.value = [1, 1, 1, 1, 1, 1, 1, 1, 1];
    }

    chartData.value = {
      labels: listLabels.value,
      datasets: [
        {
          label: "Doanh thu theo loại món",
          data: listDatas.value,
          backgroundColor: listColor.value,
        },
      ],
    };
  }
  async function init() {
    loading.value = true;

    callApi_GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH();

    callApi_GET_ALL_FOOD_CATEGORIES();

    callApi_GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH();

    selectCurrentDayAndCallAPI();

    loading.value = false;
  }

  // <== Báo cáo doanh thu theo nhân viên ==>
  async function selectDayAndCallAPIForEmployee(day) {
    loading.value = true;
    selectedDayForEmp.value = day;
    selectedMonthForEmp.value = "";
    let [dayf, monthf, yearf] = selectedDayForEmp.value.split("-");
    dayf = dayf.padStart(2, "0");
    monthf = monthf.padStart(2, "0");
    selectedDayForEmp.value = `${dayf}-${monthf}-${yearf}`;
    const responseTotal1 = await gettAllRevenueWithEmployeeByDay(
      selectedDayForEmp.value
    );
    if (responseTotal1) {
      filteredTotalRevenueForEmployee.value = responseTotal1.sort(
        (a, b) => b.totalRevenue - a.totalRevenue
      );
    }
    const totalRevenueMaxDataTable = filteredTotalRevenueForEmployee.value
      ? filteredTotalRevenueForEmployee.value.reduce((max, current) => {
          return current.totalRevenue > max.totalRevenue ? current : max;
        }, filteredTotalRevenueForEmployee.value[0])
      : null;
    totalRevenueMax.value = totalRevenueMaxDataTable
      ? totalRevenueMaxDataTable.totalRevenue
      : 0;
    loading.value = false;
  }
  async function selectMonthAndCallAPIForEmployee(month) {
    loading.value = true;
    selectedMonthForEmp.value = month;
    selectedDayForEmp.value = "";
    let [monthf, yearf] = selectedMonthForEmp.value.split("-");
    monthf = monthf.padStart(2, "0");
    selectedMonthForEmp.value = `${monthf}-${yearf}`;

    const responseTotal1 = await gettAllRevenueWithEmployeeByMonth(
      selectedMonthForEmp.value
    );
    if (responseTotal1) {
      filteredTotalRevenueForEmployee.value = responseTotal1.sort(
        (a, b) => b.totalRevenue - a.totalRevenue
      );
    }
    const totalRevenueMaxDataTable = filteredTotalRevenueForEmployee.value
      ? filteredTotalRevenueForEmployee.value.reduce((max, current) => {
          return current.totalRevenue > max.totalRevenue ? current : max;
        }, filteredTotalRevenueForEmployee.value[0])
      : null;
    totalRevenueMax.value = totalRevenueMaxDataTable
      ? totalRevenueMaxDataTable.totalRevenue
      : 0;
    loading.value = false;
  }
  const resetTimeFillterRevenueOrderForEmployee = () => {
    selectedDayForEmp.value = "";
    selectedMonthForEmp.value = "";
    callApi_GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH();
  };

  // <== Báo cáo doanh thu theo danh mục ==>
  async function selectDayAndCallAPIForCategory(day) {
    loading.value = true;
    selectedDayForCate.value = day;
    selectedMonthForCate.value = "";
    let [dayf, monthf, yearf] = selectedDayForCate.value.split("-");
    dayf = dayf.padStart(2, "0");
    monthf = monthf.padStart(2, "0");
    selectedDayForCate.value = `${dayf}-${monthf}-${yearf}`;
    const responseTotal1 = await gettAllRevenueWithCategoryByDay(
      selectedDayForCate.value
    );
    if (responseTotal1.length != 0) {
      const revenueMap = responseTotal1.reduce((acc, item) => {
        acc[item.categoryName] = item.totalRevenue;
        return acc;
      }, {});

      listDatas.value = listLabels.value.map((label) => revenueMap[label] || 0);
    } else {
      listDatas.value = [1, 1, 1, 1, 1, 1, 1, 1, 1];
    }
    chartData.value = {
      labels: listLabels.value,
      datasets: [
        {
          label: "Doanh thu theo loại món",
          data: listDatas.value,
          backgroundColor: listColor.value,
        },
      ],
    };
    loading.value = false;
  }
  async function selectMonthAndCallAPIForCategory(month) {
    loading.value = true;
    selectedMonthForCate.value = month;
    selectedDayForCate.value = "";
    let [monthf, yearf] = selectedMonthForCate.value.split("-");
    monthf = monthf.padStart(2, "0");
    selectedMonthForCate.value = `${monthf}-${yearf}`;

    const responseTotal1 = await gettAllRevenueWithCategoryByMonth(
      selectedMonthForCate.value
    );
    if (responseTotal1.length != 0) {
      const revenueMap = responseTotal1.reduce((acc, item) => {
        acc[item.categoryName] = item.totalRevenue;
        return acc;
      }, {});

      listDatas.value = listLabels.value.map((label) => revenueMap[label] || 0);
    } else {
      listDatas.value = [1, 1, 1, 1, 1, 1, 1, 1, 1];
    }

    chartData.value = {
      labels: listLabels.value,
      datasets: [
        {
          label: "Doanh thu theo loại món",
          data: listDatas.value,
          backgroundColor: listColor.value,
        },
      ],
    };
    loading.value = false;
  }
  const resetTimeFillterRevenueOrderForCategory = () => {
    selectedDayForCate.value = "";
    selectedMonthForCate.value = "";
    callApi_GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH();
  };

  const formatCurencyFromApiToView = (time) => {
    return `${time.toLocaleString("vi-VN")} VNĐ`;
  };

  return {
    currentMonth,
    dateList,
    monthList,
    selectedMonth,
    selectedDay,
    selectedCurrentDay,
    totalRevenue,
    totalRevenueYesterday,
    totalOrders,
    totalOrdersYesterday,
    totalRevenueMax,
    loading,
    filteredTotalRevenueForEmployee,
    selectedMonthForEmp,
    selectedDayForEmp,
    selectedDayForCate,
    selectedMonthForCate,
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
    selectDayAndCallAPIForEmployee,
    selectMonthAndCallAPIForEmployee,
    resetTimeFillterRevenueOrderForEmployee,
    selectDayAndCallAPIForCategory,
    selectMonthAndCallAPIForCategory,

    resetTimeFillterRevenueOrderForCategory,
    formatCurencyFromApiToView,
  };
}
