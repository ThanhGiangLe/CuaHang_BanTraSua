import { computed, ref, defineEmits } from "vue";
import axios from "axios";
import API_ENDPOINTS from "@/api/api.js";
import { useUserStore } from "@/stores/user.js";
import { userOrderStore } from "@/stores/orderStore.js";
import { toast } from "vue3-toastify";

export default function useAreaManagement() {
  const userStore = useUserStore();
  const orderStore = userOrderStore();

  const tables = ref([]);
  const user = computed(() => userStore.user);

  const confirmDialog = ref(false);
  const currentTableId = ref(0);
  const showListFoodOrderOfTableId = ref(false);
  const listFoodOrderOfTableId = ref([]);
  const tax = ref(6);
  const discount = ref(0);

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

    listFoodOrderOfTableId.value = orderStore.getDishesByTable(
      currentTableId.value
    );
    console.log("Kết quả đạt được là: ", listFoodOrderOfTableId.value);
  }
  function formatCurrency(value) {
    return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
  }
  function totalAmountAdditionalFoodItem(ListAdditionalFood) {
    return ListAdditionalFood.reduce((total, item) => {
      return total + item.price * item.quantity;
    }, 0);
  }
  function closeShowListFoodOrderOfTableId() {
    confirmDialog.value = !confirmDialog.value;
    showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;
  }
  function checkFoodOrderByTableId(tableId) {
    return orderStore.getDishesByTable(tableId);
  }
  const totalAmount = computed(() => {
    const result = listFoodOrderOfTableId.value.reduce((total, item) => {
      let mainItemTotal = item.Price * item.Quantity;
      let additionalFoodTotal =
        item.Quantity * totalAmountAdditionalFoodItem(item.ListAdditionalFood);
      return total + mainItemTotal + additionalFoodTotal;
    }, 0);
    return result;
  });
  const resultTotalAmount = computed(() => {
    const taxAmount = (totalAmount.value * tax.value) / 100;
    const discountAmount = (totalAmount.value * discount.value) / 100;
    return totalAmount.value + taxAmount - discountAmount;
  });
  function getCurrentDateTimeForSQL() {
    const now = new Date();
    const localOffset = 7 * 60; // Phút (GMT+7)
    const localTime = new Date(now.getTime() + localOffset * 60 * 1000);
    return localTime.toISOString(); // Format: YYYY-MM-DDTHH:MM:SS.SSSZ
  }
  async function ConfirmPayment() {
    let orderTimeCurrent = getCurrentDateTimeForSQL();
    console.log("Full order", listFoodOrderOfTableId.value);
    console.log("user", user.value);
    console.log("table", currentTableId.value);
    try {
      const orderResponse = await axios.post(API_ENDPOINTS.ADD_ORDER, {
        userId: user.value.userId,
        orderTime: orderTimeCurrent,
        tableId: currentTableId.value,
        totalAmount: resultTotalAmount.value,
        status: "Paid",
        discount: discount.value,
        tax: tax.value,
      });

      const orderId = orderResponse.data.data.orderId;

      await Promise.all(
        listFoodOrderOfTableId.value.map(async (item) => {
          // Gửi món chính
          const mainItemResponse = await axios.post(
            API_ENDPOINTS.ADD_ORDER_ITEM,
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
              await axios.post(API_ENDPOINTS.ADD_ORDER_ITEM, {
                orderId: orderId,
                foodItemId: addFood.id,
                foodName: addFood.foodName,
                quantity: addFood.quantity, // Số lượng mặc định là 1 nếu không chọn khác
                price: addFood.price,
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

      const [responseUpdateMaterial, responseUpdateTotalIncome] =
        await Promise.all([
          await axios.post(
            API_ENDPOINTS.UPDATE_QUANTITY_MATERIALS_AFTER_ORDER,
            {
              Items: listFoodOrderOfTableId.value,
            }
          ),
          await axios.post(API_ENDPOINTS.UPDATE_TOTALINCOME_CASH_REGISTER, {
            UserId: user.value.userId,
            TotalAmount: resultTotalAmount.value,
          }),
        ]);

      if (orderResponse.data.success === -1) {
        toast.warn("Please provide all required information!", {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false, // Hiện thanh tiến trình
          closeOnClick: true, // Đóng khi nhấp vào thông báo
          pauseOnHover: true, // Dừng khi di chuột lên thông báo
          draggable: true, // Kéo thông báo
          progress: undefined, // Tiến độ (nếu có)
        });
      } else if (orderResponse.data.success === 1) {
        showListFoodOrderOfTableId.value = !showListFoodOrderOfTableId.value;
        confirmDialog.value = !confirmDialog.value;
        toast.success("Thanh toán thành công!", {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false, // Hiện thanh tiến trình
          closeOnClick: true, // Đóng khi nhấp vào thông báo
          pauseOnHover: true, // Dừng khi di chuột lên thông báo
          draggable: true, // Kéo thông báo
          progress: undefined, // Tiến độ (nếu có)
        });
        orderStore.clearTableOrder(currentTableId.value);
        setTimeout(() => {
          window.location.reload();
        }, 3200);
      }
    } catch (error) {
      toast.error(`Error adding order: ${error}`, {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
    }
  }

  return {
    tables,
    confirmDialog,
    showListFoodOrderOfTableId,
    listFoodOrderOfTableId,
    currentTableId,
    orderStore,
    totalAmount,
    resultTotalAmount,
    tax,
    discount,

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
