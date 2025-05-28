import { ref } from "vue";
import axios from "axios";
import { computed } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { Pie } from "vue-chartjs";
import { Chart as ChartJS, Title, Tooltip, Legend, ArcElement } from "chart.js";

ChartJS.register(Title, Tooltip, Legend, ArcElement);

export default function useReportRevenue() {
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

  const tab = ref(null);

  const selectedMonthForEmp = ref("");
  const selectedDayForEmp = ref("");

  const selectedDayForCate = ref("");
  const selectedMonthForCate = ref("");
  init();

  const foodCategories = ref([]);
  const listLabels = ref([
    "Món khô",
    "Món nước",
    "Món ăn nhanh",
    "Tráng miệng",
    "Nước uống",
    "Món gọi thêm",
  ]);

  const listDatas = ref([1, 1, 1, 1, 1, 1]);
  const listColor = ref([
    "#bf360c",
    "#00acc1",
    "#710808",
    "#388e3c",
    "#c2185b",
    "#ffa000",
  ]);
  // Khai báo dữ liệu biểu đồ
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

  async function selectMonthAndCallAPI(month) {
    selectedMonth.value = month;
    let [monthf, yearf] = selectedMonth.value.split("-");
    monthf = monthf.padStart(2, "0");
    selectedMonth.value = `${monthf}-${yearf}`;

    const responseTotal1 = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_TIME_MONTH,
      {
        date: selectedMonth.value,
      }
    );
    totalRevenueOrderCurrentMonth.value = responseTotal1.data;

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
    let [dayf, monthf, yearf] = selectedDay.value.split("-");
    dayf = dayf.padStart(2, "0");
    monthf = monthf.padStart(2, "0");
    selectedDay.value = `${dayf}-${monthf}-${yearf}`;
    const responseTotal1 = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_TIME,
      {
        date: selectedDay.value,
      }
    );
    totalRevenueOrderCurrentDay.value = responseTotal1.data;

    totalRevenue.value = totalRevenueOrderCurrentDay.value.totalAmount;
    totalRevenueYesterday.value =
      totalRevenueOrderCurrentDay.value.totalAmountYesterday;
    totalOrders.value = totalRevenueOrderCurrentDay.value.totalOrders;
    totalOrdersYesterday.value =
      totalRevenueOrderCurrentDay.value.totalOrdersYesterday;
  }
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

    const responseTotal1 = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_TIME,
      {
        date: selectedCurrentDay.value,
      }
    );
    totalRevenueOrderCurrentDay.value = responseTotal1.data;

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
      return totalRevenue.value / totalRevenueYesterday.value;
    } else {
      return totalRevenueYesterday.value / totalRevenue.value;
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
      return totalOrders.value / totalOrdersYesterday.value;
    } else {
      return totalOrdersYesterday.value / totalOrders.value;
    }
  });
  const resetTimeFillterRevenueOrder = () => {
    selectedDay.value = "";
    selectedMonth.value = "";
    selectedCurrentDay.value = "";
    selectCurrentDayAndCallAPI();
  };

  const headersEmployee = ref([
    { title: "Tên Nhân Viên", key: "fullName", width: "35%" },
    { title: "Tổng doanh thu", key: "totalRevenue", width: "65%" },
  ]);
  const headersCategory = ref([
    { title: "Tên Danh mục", key: "categoryName", width: "35%" },
    { title: "Tổng doanh thu", key: "totalRevenue", width: "65%" },
  ]);

  async function callApi_GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH() {
    const formattedMonth = currentMonth.toString().padStart(2, "0");
    // Lọc danh thu theo nhân viên
    const responseRevenue = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH,
      {
        date: `${formattedMonth}-${currentYear}`,
      }
    );
    if (responseRevenue) {
      filteredTotalRevenueForEmployee.value = responseRevenue.data.sort(
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
    const response = await axios.get(API_ENDPOINTS.GET_ALL_FOOD_CATEGORIES);
    foodCategories.value = response.data;
    listLabels.value = foodCategories.value.map((c) => c.categoryName);
  }
  async function callApi_GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH() {
    const formattedMonth = currentMonth.toString().padStart(2, "0");
    const responseRevenueForCategory = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH,
      {
        date: `${formattedMonth}-${currentYear}`,
      }
    );

    const revenueMap = responseRevenueForCategory.data.reduce((acc, item) => {
      acc[item.categoryName] = item.totalRevenue;
      return acc;
    }, {});

    listDatas.value = listLabels.value.map((label) => revenueMap[label] || 0);

    // listDatas.value = responseRevenueForCategory.data.map(
    //   (i) => i.totalRevenue
    // );
    // while (listDatas.value.length < listLabels.value.length) {
    //   listDatas.value.push(1);
    // }

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
    callApi_GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH();

    callApi_GET_ALL_FOOD_CATEGORIES();

    callApi_GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH();

    selectCurrentDayAndCallAPI();

    loading.value = false;
  }

  // Xử lý ở phần NHÂN VIÊN
  async function selectDayAndCallAPIForEmployee(day) {
    loading.value = true;
    selectedDayForEmp.value = day;
    let [dayf, monthf, yearf] = selectedDayForEmp.value.split("-");
    dayf = dayf.padStart(2, "0");
    monthf = monthf.padStart(2, "0");
    selectedDayForEmp.value = `${dayf}-${monthf}-${yearf}`;
    const responseTotal1 = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME,
      {
        date: selectedDayForEmp.value,
      }
    );
    if (responseTotal1) {
      filteredTotalRevenueForEmployee.value = responseTotal1.data.sort(
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
    let [monthf, yearf] = selectedMonthForEmp.value.split("-");
    monthf = monthf.padStart(2, "0");
    selectedMonthForEmp.value = `${monthf}-${yearf}`;

    const responseTotal1 = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH,
      {
        date: selectedMonthForEmp.value,
      }
    );
    if (responseTotal1) {
      filteredTotalRevenueForEmployee.value = responseTotal1.data.sort(
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

  // Xử lý ở nhần DANH MỤC
  async function selectDayAndCallAPIForCategory(day) {
    loading.value = true;
    selectedDayForCate.value = day;
    let [dayf, monthf, yearf] = selectedDayForCate.value.split("-");
    dayf = dayf.padStart(2, "0");
    monthf = monthf.padStart(2, "0");
    selectedDayForCate.value = `${dayf}-${monthf}-${yearf}`;
    const responseTotal1 = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_CATEGORY_AND_TIME,
      {
        date: selectedDayForCate.value,
      }
    );

    const revenueMap = responseTotal1.data.reduce((acc, item) => {
      acc[item.categoryName] = item.totalRevenue;
      return acc;
    }, {});

    listDatas.value = listLabels.value.map((label) => revenueMap[label] || 0);

    // listDatas.value = responseRevenueForCategory.data.map(
    //   (i) => i.totalRevenue
    // );
    // while (listDatas.value.length < listLabels.value.length) {
    //   listDatas.value.push(1);
    // }

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
    let [monthf, yearf] = selectedMonthForCate.value.split("-");
    monthf = monthf.padStart(2, "0");
    selectedMonthForCate.value = `${monthf}-${yearf}`;

    const responseTotal1 = await axios.post(
      API_ENDPOINTS.GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH,
      {
        date: selectedMonthForCate.value,
      }
    );
    const revenueMap = responseTotal1.data.reduce((acc, item) => {
      acc[item.categoryName] = item.totalRevenue;
      return acc;
    }, {});

    listDatas.value = listLabels.value.map((label) => revenueMap[label] || 0);

    // listDatas.value = responseRevenueForCategory.data.map(
    //   (i) => i.totalRevenue
    // );
    // while (listDatas.value.length < listLabels.value.length) {
    //   listDatas.value.push(1);
    // }

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
    return `${time.toLocaleString("vi-VN")} VND`;
  };

  return {
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
  };
}
