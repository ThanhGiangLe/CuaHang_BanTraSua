import { computed, ref, defineEmits } from "vue";
import axios from "axios";
import API_ENDPOINTS from "@/api/api.js";
import { useUserStore } from "@/stores/user.js";
import { userOrderStore } from "@/stores/orderStore.js";
import { toast } from "vue3-toastify";
import { showToast } from "@/styles/handmade";
import { areaManagementHandler } from "/src/composables/areaManagement/areaManagementHandler.js";
import { orderFoodHandler } from "/src/composables/listFood/orderFoodHandler.js";

export default function useAreaManagement() {
  const userStore = useUserStore();
  const orderStore = userOrderStore();
  const { getAllTable } = areaManagementHandler();
  const { orderFood, orderDetail } = orderFoodHandler();
  const tables = ref([]);
  const user = computed(() => userStore.user);

  const confirmDialog = ref(false);
  const currentTableId = ref(0);
  const showListFoodOrderOfTableId = ref(false);
  const listFoodOrderOfTableId = ref([]);
  const isShowQRCode = ref(false);
  const paymentInfo = ref({
    tax: 0,
    discount: 0,
    paymentMethod: "Tiền mặt",
    resultTotalAmount: null,
    resultTotalPayment: null,
    status: "Paid",
  });

  async function init() {
    const response = await getAllTable();
    tables.value = response.listTable.map((table) => ({
      ...table,
      isActive: false,
    }));
  }

  init();
  const handleConfirmDialog = (table) => {
    confirmDialog.value = true;
    currentTableId.value = table.tableId;
  };
  function getTotal_ResultTotalAmount_ResultTotalPayment(order) {
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

  async function viewTableAndSetCurrentOrders() {
    showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;

    let resStore = orderStore.getDishesByTable(currentTableId.value);
    console.log("resStore: ", resStore);

    let total = getTotal_ResultTotalAmount_ResultTotalPayment(resStore);
    console.log("total: ", total);

    listFoodOrderOfTableId.value = resStore.items;
    paymentInfo.value.resultTotalAmount = total.totalAmount;
    paymentInfo.value.resultTotalPayment = total.totalPayment;

    paymentInfo.value.tax = resStore.tax;
    paymentInfo.value.discount = resStore.discount;
    paymentInfo.value.status = resStore.status;
    paymentInfo.value.paymentMethod = resStore.paymentMethod;
  }
  async function cancelTableAndSetCurrentOrders() {
    confirmDialog.value = !confirmDialog.value;
    // orderStore.clearAllOrders();
  }

  async function ConfirmPayment() {
    let orderTimeCurrent = getCurrentDateTimeForSQL();
    const request = {
      userId: user.value.userId,
      orderTime: orderTimeCurrent,
      tableId: currentTableId.value,
      totalAmount: paymentInfo.value.resultTotalAmount,
      totalResult: paymentInfo.value.resultTotalPayment,
      status: paymentInfo.value.status,
      discount: paymentInfo.value.discount,
      tax: paymentInfo.value.tax,
      paymentMethod: paymentInfo.value.paymentMethod ?? "Tiền mặt",
    };
    const orderResponse = await orderFood(request);
    if (orderResponse.responseOrder.success === -1) {
      showToast("Có lỗi trong quá trình tạo đơn hàng!", "error");
    } else if (orderResponse.responseOrder.success === 1) {
      const orderId = orderResponse.responseOrder.data.orderId;
      await Promise.all(
        listFoodOrderOfTableId.value.map(async (item) => {
          const mainRequest = {
            orderId: orderId,
            foodItemId: item.FoodItemId,
            foodName: item.FoodName,
            quantity: item.Quantity,
            price: item.Price,
            isMainItem: 1,
            unit: item.Unit,
            note: item.Note,
            categoryId: item.CategoryId,
            orderTime: orderTimeCurrent,
          };
          const mainItemResponse = await orderDetail(mainRequest);
          // Gửi các món phụ với parentItemId là mainItemId và các món phụ đi kèm món chính đó
          await Promise.all(
            item.ListAdditionalFood.map(async (addFood) => {
              const subRequest = {
                orderId: orderId,
                foodItemId: addFood.foodItemId,
                foodName: addFood.foodName,
                quantity: addFood.quantity, // Số lượng mặc định là 1 nếu không chọn khác
                price: addFood.priceCustom,
                isMainItem: 0,
                unit: addFood.unit,
                note: "",
                categoryId: addFood.categoryId,
                orderTime: orderTimeCurrent,
              };
              const subItemResponse = await orderDetail(subRequest);
            })
          );
        })
      );
      showToast("Thanh toán thành công!", "success");
      showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;
      confirmDialog.value = !confirmDialog.value;
      orderStore.clearTableOrder(currentTableId.value);
      setTimeout(() => {
        window.location.reload();
      }, 3200);
    } else {
      showToast("Có lỗi trong quá trình tạo đơn hàng!", "error");
    }
  }
  function closeShowListFoodOrderOfTableId() {
    confirmDialog.value = !confirmDialog.value;
    showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;
  }

  const momoQRCodeUrl = computed(() => {
    const amount = paymentInfo.value.resultTotalPayment || 0;
    return `https://img.vietqr.io/image/TCB-192602032003-compact2.png?amount=${amount}&addInfo=${encodeURIComponent(
      "TraSuaSunSun"
    )}`;
  });
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

    formatDate,
    formatCurrency,
    handleConfirmDialog,
    cancelTableAndSetCurrentOrders,
    viewTableAndSetCurrentOrders,
    getTotal_ResultTotalAmount_ResultTotalPayment,
    totalAmountAdditionalFoodItem,
    closeShowListFoodOrderOfTableId,
    checkFoodOrderByTableId,
    ConfirmPayment,
  };
}
