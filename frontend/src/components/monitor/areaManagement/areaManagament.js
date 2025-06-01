import { computed, ref, defineEmits } from "vue";
import axios from "axios";
import API_ENDPOINTS from "@/api/api.js";
import { useUserStore } from "@/stores/user.js";
import { userOrderStore } from "@/stores/orderStore.js";
import { toast } from "vue3-toastify";
import { showToast } from "@/styles/handmade";

export default function useAreaManagement() {
  const userStore = useUserStore();
  const orderStore = userOrderStore();

  const tables = ref([]);
  const user = computed(() => userStore.user);

  const confirmDialog = ref(false);
  const currentTableId = ref(0);
  const showListFoodOrderOfTableId = ref(false);
  const listFoodOrderOfTableId = ref([]);
  const paymentInfo = ref({
    tax: 0,
    discount: 0,
    totalAmount: 0,
    resultTotalAmount: 0,
    paymentMethod: "Tiền mặt",
    status: "Paid",
  });

  async function init() {
    const response = await axios.get(API_ENDPOINTS.GET_ALL_TABLE);
    tables.value = response.data.map((table) => ({
      ...table,
      isActive: false,
    }));
  }

  init();
  const handleConfirmDialog = (table) => {
    confirmDialog.value = true;
    currentTableId.value = table.tableId;
  };

  async function cancelTableAndSetCurrentOrders() {
    confirmDialog.value = !confirmDialog.value;
  }
  async function viewTableAndSetCurrentOrders() {
    showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;

    let resStore = orderStore.getDishesByTable(currentTableId.value);

    listFoodOrderOfTableId.value = resStore.items;

    paymentInfo.value.tax = resStore.tax;
    paymentInfo.value.discount = resStore.discount;
    paymentInfo.value.totalAmount = resStore.total_amount;
    paymentInfo.value.resultTotalAmount = resStore.result_total_amount;
    paymentInfo.value.status = resStore.status;
    paymentInfo.value.paymentMethod = resStore.paymentMethod;
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
  function closeShowListFoodOrderOfTableId() {
    confirmDialog.value = !confirmDialog.value;
    showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;
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
    const isoString = dateString.toString();

    return isoString
      .replace("T", " ")
      .replace(/\.\d+Z$/, "")
      .replace(/\.\d+/, "");
  }
  async function ConfirmPayment() {
    let orderTimeCurrent = getCurrentDateTimeForSQL();
    try {
      const orderResponse = await axios.post(API_ENDPOINTS.ADD_ORDER, {
        userId: user.value.userId,
        orderTime: orderTimeCurrent,
        tableId: currentTableId.value,
        totalAmount: paymentInfo.value.totalAmount,
        totalResult: paymentInfo.value.resultTotalAmount,
        status: paymentInfo.value.status,
        discount: paymentInfo.value.discount,
        tax: paymentInfo.value.tax,
        paymentMethod: paymentInfo.value.paymentMethod ?? "Tiền mặt",
      });

      const orderId = orderResponse.data.data.orderId;

      await Promise.all(
        listFoodOrderOfTableId.value.map(async (item) => {
          // Gửi món chính
          const mainItemResponse = await axios.post(
            API_ENDPOINTS.ADD_ORDER_DETAIL,
            {
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
            }
          );
          // Gửi các món phụ với parentItemId là mainItemId và các món phụ đi kèm món chính đó
          await Promise.all(
            item.ListAdditionalFood.map(async (addFood) => {
              await axios.post(API_ENDPOINTS.ADD_ORDER_DETAIL, {
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
              });
            })
          );
        })
      );

      if (orderResponse.data.success === -1) {
        showToast("Please provide all required information!", "warn");
      } else if (orderResponse.data.success === 1) {
        showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;
        confirmDialog.value = !confirmDialog.value;
        showToast("Thanh toán thành công!", "success");
        orderStore.clearTableOrder(currentTableId.value);
        setTimeout(() => {
          window.location.reload();
        }, 3200);
      }
    } catch (error) {
      showToast(`Error adding order: ${error}`, "error");
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

    formatDate,
    formatCurrency,
    handleConfirmDialog,
    cancelTableAndSetCurrentOrders,
    viewTableAndSetCurrentOrders,
    totalAmountAdditionalFoodItem,
    closeShowListFoodOrderOfTableId,
    checkFoodOrderByTableId,
    ConfirmPayment,
  };
}
