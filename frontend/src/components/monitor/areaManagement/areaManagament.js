import { computed, ref, defineEmits, watch } from "vue";
import axios from "axios";
import API_ENDPOINTS from "@/api/api.js";
import { useUserStore } from "@/stores/user.js";
import { userOrderStore } from "@/stores/orderStore.js";
import { toast } from "vue3-toastify";
import { showToast } from "@/styles/handmade";
import { areaManagementHandler } from "/src/composables/areaManagement/areaManagementHandler.js";
import { orderFoodHandler } from "/src/composables/listFood/orderFoodHandler.js";
import { employeeManagementHandler } from "/src/composables/employeeManagement/employeeManagementHandler.js";
import emailjs from "emailjs-com";

export default function useAreaManagement() {
  const userStore = useUserStore();
  const orderStore = userOrderStore();
  const { getAllTable } = areaManagementHandler();
  const { orderFood, orderDetail, getEmailByPhone } = orderFoodHandler();
  const { getAllEmployee } = employeeManagementHandler();
  const tables = ref([]);
  const user = computed(() => userStore.user);

  const confirmDialog = ref(false);
  const currentTableId = ref(0);
  const showListFoodOrderOfTableId = ref(false);
  const listFoodOrderOfTableId = ref([]);
  const isShowQRCode = ref(false);

  const searchPhoneNumbers = ref(null); // Dùng để chọn object
  const allPhoneNumbers = ref([]); // Dữ liệu gốc
  const filteredPhoneNumbers = ref([]); // Dữ liệu đã lọc
  const isShowOTPPoints = ref(false);
  const visibleResendOtp = ref(false);
  const enteredOtp = ref("");
  const otpCode = ref("");
  const serviceID = "service_cojqzzb";
  const templateID = "template_bql1hr5";
  const publicKey = "YVFyP3Zy91mr0Jc5W";
  emailjs.init(publicKey);
  const otpSendTime = ref(null); // Lưu thời gian lúc gửi OTP
  const otpTimeout = 1 * 60 * 1000; // Thời gian 1 phút

  const paymentInfo = ref({
    tax: 0,
    discount: 0,
    paymentMethod: "Tiền mặt",
    totalAmount: null,
    totalResult: null,
    status: "Paid",
    receivedAmount: null,
  });
  const returnedAmountCurrentOrder = computed(() => {
    const received = Number(paymentInfo.value.receivedAmount * 1000 || 0);
    const total = Number(paymentInfo.value.totalResult || 0);
    return received > total ? received - total : 0;
  });

  async function init() {
    const response = await getAllTable();
    tables.value = response.listTable.map((table) => ({
      ...table,
      isActive: false,
    }));
    const responseEmp = await getAllEmployee();
    allPhoneNumbers.value = responseEmp.map((item) => ({
      ...item,
      display: `${item.fullName == "" ? "/*name*/" : item.fullName} - ${
        item.phone
      } - ${formatPoint(item.point)}`,
    }));
    filteredPhoneNumbers.value = JSON.parse(
      JSON.stringify(allPhoneNumbers.value)
    );
  }

  init();
  const handleConfirmDialog = (table) => {
    confirmDialog.value = true;
    currentTableId.value = table.tableId;
  };
  function getTotal_totalAmount_totalResult(order) {
    const totalAmount = order.items.reduce((total, item) => {
      return (
        total +
        (item.Price + totalAmountAdditionalFoodItem(item.ListAdditionalFood)) *
          item.Quantity
      );
    }, 0);
    const t = totalAmount;
    const totalPayment =
      t - t * (order?.discount / 100) + t * (order?.tax / 100);
    return {
      totalAmount,
      totalPayment,
    };
  }
  function formatCurrency(value) {
    if (value === null || value === undefined) return "0 VNĐ";

    const numericValue = Number(value);
    if (isNaN(numericValue)) return "0 VNĐ";

    return numericValue.toLocaleString("vi-VN") + " VNĐ";
  }
  function totalAmountAdditionalFoodItem(ListAdditionalFood) {
    return ListAdditionalFood.reduce((total, item) => {
      return total + item.priceCustom * item.quantity;
    }, 0);
  }
  function checkFoodOrderByTableId(tableId) {
    return orderStore.getDishesByTable(tableId);
  }
  function getCurrentDateTimeForSQL() {
    const now = new Date();
    const localOffset = 7 * 60; // Phút (GMT+7)
    const localTime = new Date(now.getTime() + localOffset * 60 * 1000);
    return localTime.toISOString(); // Format: YYYY-MM-DDTHH:MM:SS.SSSZ
  }
  function formatDate(dateString) {
    const date = new Date(dateString);
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
  }
  function formatPoint(value) {
    if (value === null || value === undefined) return "0 Điểm";

    const numericValue = Number(value);
    if (isNaN(numericValue)) return "0 Điểm";

    return numericValue.toLocaleString("vi-VN") + " Điểm";
  }
  watch(searchPhoneNumbers, (val, oldVal) => {
    if (!val) {
      filteredPhoneNumbers.value = allPhoneNumbers.value;
      return;
    }
    if (typeof val === "string") {
      filteredPhoneNumbers.value = allPhoneNumbers.value.filter(
        (item) =>
          item.fullName.toLowerCase().includes(val.toLowerCase()) ||
          item.phone.toLowerCase().includes(val.toLowerCase())
      );
      if (val !== oldVal) {
        searchPhoneNumbers.value = val;
      }
    } else if (typeof val === "object" && val !== null) {
      const newVal = val.phone;
      if (newVal !== oldVal) {
        searchPhoneNumbers.value = newVal;
      }
    }
  });
  watch(
    () => paymentInfo.value.discount,
    (newDiscount) => {
      const discountPercent = parseFloat(newDiscount) || 0;
      const totalAmount = parseFloat(paymentInfo.value.totalAmount) || 0;
      paymentInfo.value.totalResult = totalAmount * (1 - discountPercent / 100);
    }
  );
  async function viewTableAndSetCurrentOrders() {
    showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;

    let resStore = orderStore.getDishesByTable(currentTableId.value);
    console.log("resStore: ", resStore);

    let total = getTotal_totalAmount_totalResult(resStore);
    console.log("total: ", total);

    listFoodOrderOfTableId.value = resStore.items;
    paymentInfo.value.totalAmount = total.totalAmount;
    paymentInfo.value.totalResult = total.totalPayment;

    paymentInfo.value.tax = resStore.tax;
    paymentInfo.value.discount = resStore.discount;
    paymentInfo.value.status = resStore.status;
    paymentInfo.value.paymentMethod = resStore.paymentMethod;
  }
  async function cancelTableAndSetCurrentOrders() {
    confirmDialog.value = !confirmDialog.value;
    // orderStore.clearAllOrders();
  }
  const processOrder = async () => {
    const orderTimeCurrent = getCurrentDateTimeForSQL();
    const request = {
      userId: user.value.userId,
      orderTime: orderTimeCurrent,
      tableId: currentTableId.value,
      totalAmount: paymentInfo.value.totalAmount,
      totalResult: paymentInfo.value.totalResult,
      status: paymentInfo.value.status,
      discount: paymentInfo.value.discount,
      tax: paymentInfo.value.tax,
      paymentMethod: paymentInfo.value.paymentMethod,
      phone: searchPhoneNumbers.value ?? "",
      receivedAmount: paymentInfo.value.receivedAmount * 1000 || 0,
      returnedAmount: returnedAmountCurrentOrder.value || 0,
    };

    try {
      const response = await orderFood(request);
      if (!response.responseOrder) {
        handleOrderHttpError(response);
        return;
      }

      const { success, data } = response.responseOrder;
      if (success == -1) {
        showToast("Thông tin đặt không hợp lệ!", "error");
      } else if (success === 1) {
        const orderId = data.orderId;
        await createOrderItems(orderId, orderTimeCurrent);
        showToast("Thanh toán thành công!", "success");
        searchPhoneNumbers.value = null;
        showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;
        confirmDialog.value = !confirmDialog.value;
        orderStore.clearTableOrder(currentTableId.value);
        setTimeout(() => {
          window.location.reload();
        }, 3200);
      } else if (success === -20) {
        showToast("Số điểm không đủ để thanh toán!", "warn");
      } else if (success === -26) {
        showToast(
          "Tài khoản không hợp lệ. Vui lòng thay đổi phương thức thanh toán!",
          "warn"
        );
      } else {
        showToast("Có lỗi trong quá trình tạo đơn hàng!", "error");
      }
    } catch (err) {
      console.error("Order error:", err);
      showToast("Không thể tạo đơn hàng!", "error");
    }
  };

  const handleOrderHttpError = (response) => {
    const status = response.response?.status;
    if (status === 404) {
      showToast(response.response.data, "warn");
    } else if (status === 403) {
      showToast("Đăng nhập lại để thực hiện thao tác!", "warn");
    } else {
      showToast("Xảy ra lỗi trong quá trình xử lý đơn hàng.", "error");
    }
  };

  const createOrderItems = async (orderId, orderTime) => {
    await Promise.all(
      listFoodOrderOfTableId.value.map(async (item) => {
        const mainRequest = {
          orderId,
          foodItemId: item.FoodItemId,
          foodName: item.FoodName,
          quantity: item.Quantity,
          price: item.Price,
          isMainItem: item.IsMain ?? 1,
          unit: item.Unit,
          note: item.Note,
          categoryId: item.CategoryId,
          orderTime,
        };
        await orderDetail(mainRequest);

        await Promise.all(
          item.ListAdditionalFood.map((addFood) => {
            const subRequest = {
              orderId,
              foodItemId: addFood.foodItemId,
              foodName: addFood.foodName,
              quantity: addFood.quantity,
              price: addFood.priceCustom,
              isMainItem: 0,
              unit: addFood.unit,
              note: "",
              categoryId: addFood.categoryId,
              orderTime,
            };
            return orderDetail(subRequest);
          })
        );
      })
    );
  };
  function generateOTP() {
    return Math.floor(100000 + Math.random() * 900000).toString(); // Mã OTP 6 chữ số
  }
  const sendOTP = async () => {
    visibleResendOtp.value = false;
    const request = {
      Phone: searchPhoneNumbers.value,
    };
    const responseEmail = await getEmailByPhone(request);
    if (responseEmail.result.success) {
      const emailResponse = responseEmail.result.email;

      try {
        otpSendTime.value = new Date().getTime(); // dùng để xác định thời gian gửi mã OTP đến người dùng
        otpCode.value = generateOTP();
        console.log("otpCode: ", otpCode.value);
        const templateParams = {
          email: emailResponse,
          otp_code: otpCode.value,
        };

        emailjs
          .send(serviceID, templateID, templateParams)
          .then((response) => {
            showToast("Hãy kiểm tra thông báo email!", "success");
          })
          .catch((err) => {
            showToast("Có lỗi trong quá trình gửi mail.", "warn");
          });
      } catch (error) {
        showToast("Lỗi trong quá trình xác minh tài khoản.", "error");
      }
    } else {
      if (responseEmail.result.response.status == 400) {
        showToast("Thông tin yêu cầu không hợp lệ!", "warn");
      } else if (responseEmail.result.response.status == 404) {
        showToast("Số điện thoại chưa đăng kí email để thực hiện xác nhận OTP");
      }
    }
  };

  const verifyOTP = async () => {
    if (!enteredOtp.value) {
      showToast("Nhập mã nhận được từ email!", "warn");
      return;
    }
    let currentTime = new Date().getTime();
    if (currentTime - otpSendTime.value > otpTimeout) {
      showToast("OTP đã hết thời gian sử dụng!", "error");
      visibleResendOtp.value = true;
      otpCode.value = "";
      enteredOtp.value = "";
      return;
    }
    if (enteredOtp.value === otpCode.value) {
      showToast("Xác nhận thành công!", "success");
      await processOrder();
      otpCode.value = ""; // Reset mã OTP
      isShowOTPPoints.value = false;
      enteredOtp.value = "";
    } else {
      showToast("OTP không chính xác!", "warn");
    }
  };

  async function ConfirmPayment() {
    if (paymentInfo.value.paymentMethod === "Điểm") {
      if (!searchPhoneNumbers.value) {
        showToast(
          "Vui lòng chọn số điện thoại để thanh toán bằng điểm!",
          "warn"
        );
        return;
      }
      await sendOTP();
      setTimeout(() => (isShowOTPPoints.value = true), 2000);
    } else {
      if (paymentInfo.value.paymentMethod === "Tiền mặt") {
        console.log("Có vào");
        if (
          paymentInfo.value.receivedAmount == 0 ||
          paymentInfo.value.receivedAmount == null
        ) {
          showToast("Nhập số tiền nhận từ khách!", "warn");
          return;
        }
      }
      await processOrder();
    }
  }
  function closeShowListFoodOrderOfTableId() {
    confirmDialog.value = !confirmDialog.value;
    showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;
    paymentInfo.value = {
      tax: 0,
      discount: 0,
      paymentMethod: "Tiền mặt",
      totalAmount: null,
      totalResult: null,
      status: "Paid",
      receivedAmount: null,
    };
    searchPhoneNumbers.value = null;
  }

  const momoQRCodeUrl = computed(() => {
    const amount = paymentInfo.value.totalResult || 0;
    return `https://img.vietqr.io/image/TCB-192602032003-compact2.png?amount=${amount}&addInfo=${encodeURIComponent(
      "TraSuaSunSun"
    )}`;
  });

  const timeNow = ref(new Date());

  onMounted(() => {
    const interval = setInterval(() => {
      timeNow.value = new Date();
    }, 60000); // mỗi 1 phút

    onUnmounted(() => {
      clearInterval(interval);
    });
  });

  function calculateDuration(orderTime) {
    const orderDate = new Date(orderTime);
    const diffMs = timeNow.value - orderDate;
    const diffMinutes = Math.floor(diffMs / 60000);

    if (diffMinutes < 60) {
      return `${diffMinutes} phút`;
    } else {
      const hour = Math.floor(diffMinutes / 60);
      const minute = diffMinutes % 60;
      return `${hour} giờ ${minute} phút`;
    }
  }
  return {
    tables,
    confirmDialog,
    showListFoodOrderOfTableId,
    listFoodOrderOfTableId,
    currentTableId,
    orderStore,
    paymentInfo,
    isShowQRCode,
    momoQRCodeUrl,
    returnedAmountCurrentOrder,
    searchPhoneNumbers,
    filteredPhoneNumbers,
    isShowOTPPoints,
    visibleResendOtp,
    enteredOtp,

    formatDate,
    formatCurrency,
    handleConfirmDialog,
    cancelTableAndSetCurrentOrders,
    viewTableAndSetCurrentOrders,
    getTotal_totalAmount_totalResult,
    totalAmountAdditionalFoodItem,
    closeShowListFoodOrderOfTableId,
    checkFoodOrderByTableId,
    ConfirmPayment,
    verifyOTP,
    sendOTP,
    calculateDuration,
  };
}
