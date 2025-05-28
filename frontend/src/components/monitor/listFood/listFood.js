import { ref } from "vue";
import axios from "axios";
import { computed } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { useUserStore } from "@/stores/user.js";
import { userOrderStore } from "@/stores/orderStore.js";
import { showToast } from "@/styles/handmade";

export default function useFoodManagement() {
  const userStore = useUserStore();
  const orderStore = userOrderStore();
  const showDialogUpdate = ref(false);
  const isStaff = ref(true);
  const currentDish = ref({});
  const foodCategories = ref([]);
  const foodItems = ref([]);
  const search = ref("");
  const showDialogSelectedAdditionalFood = ref(false);
  const currentFoodItem = ref({});
  const visibleUpdateCurrentFoodSelected = ref(false);
  const showComponentAreaManagement = ref(false);
  const loading = shallowRef(true);

  const propertyTax = ref(0);
  const propertyDiscount = ref(0);
  const currentItemIsUpdate = ref(-1);

  // Lấy thông tin người dùng từ store
  const user = computed(() => userStore.user);

  // Danh sách categories được chọn
  const listFoodCategorySelected = ref([]);
  const currentOrderClone = ref({
    user_id: user.value.userId,
    order_time: getCurrentDateTimeForSQL(),
    table_id: null,
    total_amount: 0,
    result_total_amount: 0,
    status: "Paid",
    discount: 12,
    tax: 6,
    paymentMethod: "Tiền mặt",
    items: [],
  });
  // Hàm chạy đầu tiên
  async function init() {
    const response = await axios.get(API_ENDPOINTS.GET_ALL_FOOD_CATEGORIES);
    foodCategories.value = response.data.data;

    const responseFoodItems = await axios.get(API_ENDPOINTS.GET_ALL_FOOD_ITEMS);
    foodItems.value = responseFoodItems.data;
    loading.value = false;
  }
  init();

  function tonggleSelected(foodCategory) {
    if (listFoodCategorySelected.value.includes(foodCategory)) {
      listFoodCategorySelected.value = listFoodCategorySelected.value.filter(
        (s) => s.categoryId !== foodCategory.categoryId
      );
    } else {
      listFoodCategorySelected.value.push(foodCategory);
    }
  }
  const filteredFoodItems = computed(() => {
    if (!Array.isArray(foodItems.value.data)) {
      console.error("FoodItems is not an array:", foodItems.value);
      return [];
    }

    return foodItems.value.data.filter((foodItem) => {
      const isCategoryMatch =
        listFoodCategorySelected.value.length === 0 ||
        listFoodCategorySelected.value.some(
          (selected) => selected.categoryId === foodItem.categoryId
        );

      const isSearchMatch = foodItem.foodName
        .toLowerCase()
        .includes(search.value.toLowerCase());

      return isCategoryMatch && isSearchMatch;
    });
  });
  function getCurrentDateTimeForSQL() {
    const now = new Date();
    const localOffset = 7 * 60; // Phút (GMT+7)
    const localTime = new Date(now.getTime() + localOffset * 60 * 1000);
    return localTime.toISOString(); // Format: YYYY-MM-DDTHH:MM:SS.SSSZ
  }
  const currentOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Price: 0,
    Image: "",
    Unit: "",
    CategoryId: 0,
    IsMain: 1,
    // Các biến map thêm vào
    Quantity: 1,
    Note: "",
    ListAdditionalFood: [],
  });
  const resultOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Price: 0,
    Image: "",
    Unit: "",
    CategoryId: 0,
    IsMain: 1,
    Quantity: 0,
    Note: "",
    ListAdditionalFood: [],
  });
  const updateOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Price: 0,
    Image: "",
    Unit: "",
    CategoryId: 0,
    IsMain: 1,
    Quantity: 0,
    Note: "",
    ListAdditionalFood: [],
    ListAdditionalFoodSelected: [],
  });
  const resultUpdateOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Price: 0,
    Image: "",
    Unit: "",
    CategoryId: 0,
    IsMain: 1,
    Quantity: 0,
    Note: "",
    ListAdditionalFood: [],
  });
  const currentOrder = ref({
    user_id: user.value.userId,
    order_time: getCurrentDateTimeForSQL(),
    table_id: null,
    total_amount: 0,
    status: "Paid",
    discount: 12,
    tax: 6,
    paymentMethod: "Tiền mặt",
    items: [],
  });
  function resetOrderItem() {
    // Reset các giá trị cơ bản
    currentOrderItem.value = {
      FoodItemId: "",
      FoodName: "",
      Price: 0,
      Image: "",
      Unit: "",
      CategoryId: 0,
      IsMain: 1,
      // Các biến map thêm vào
      Quantity: 1,
      Note: "",
      ListAdditionalFood: [],
    };

    resultOrderItem.value = {
      FoodItemId: "",
      FoodName: "",
      Price: 0,
      Image: "",
      Unit: "",
      CategoryId: 0,
      IsMain: 1,
      // Các biến map thêm vào
      Quantity: 1,
      Note: "",
      ListAdditionalFood: [],
    };

    updateOrderItem.value = {
      FoodItemId: "",
      FoodName: "",
      Quantity: 0,
      Price: 0,
      Image: "",
      Unit: "",
      Note: "",
      IsMain: 1,
      CategoryId: 0,
      ListAdditionalFood: [],
      ListAdditionalFoodSelected: [],
    };

    resultUpdateOrderItem.value = {
      FoodItemId: "",
      FoodName: "",
      Quantity: 0,
      Price: 0,
      Image: "",
      Unit: "",
      Note: "",
      IsMain: 1,
      CategoryId: 0,
      ListAdditionalFood: [],
    };
  }
  function selectedFoodItemToCart(currentOrderItem) {
    // Đem hết thông tin thao tác trên currentOrderItem đem qua resultOrderItem. Còn phần ListAdditionalFood đã được thao tác trước đó
    resultOrderItem.value.FoodItemId = currentOrderItem.FoodItemId;
    resultOrderItem.value.FoodName = currentOrderItem.FoodName;
    resultOrderItem.value.Price = currentOrderItem.Price;
    resultOrderItem.value.Image = currentOrderItem.Image;
    resultOrderItem.value.Unit = currentOrderItem.Unit;
    resultOrderItem.value.CategoryId = currentOrderItem.CategoryId;
    resultOrderItem.value.IsMain = currentOrderItem.IsMain;
    resultOrderItem.value.Quantity = currentOrderItem.Quantity;
    resultOrderItem.value.Note = currentOrderItem.Note;

    // Thêm món ăn vào danh sách order
    currentOrder.value.items.push({ ...resultOrderItem.value });
    // Cập nhật tổng tiền khi thêm hoặc tăng số lượng món
    updateTotalAmount();

    resetOrderItem();
    showDialogSelectedAdditionalFood.value = false;
  }
  function nonSelectedFoodItemToCart() {
    // Lý do gán lại resultOrderItem.value.ListAdditionalFood vì khi check sẽ thao tác trực tiếp với resultOrderItem.value.ListAdditionalFood
    resultOrderItem.value.ListAdditionalFood =
      resultOrderItem.value.ListAdditionalFood.map(() => false);
    resetOrderItem();
    showDialogSelectedAdditionalFood.value = false;
  }
  function updateCurrentFoodSelected(item, index) {
    updateOrderItem.value.CategoryId = item.CategoryId;
    updateOrderItem.value.FoodItemId = item.FoodItemId;
    updateOrderItem.value.FoodName = item.FoodName;
    updateOrderItem.value.Image = item.Image;
    updateOrderItem.value.IsMain = item.IsMain;
    updateOrderItem.value.Note = item.Note;
    updateOrderItem.value.Price = item.Price;
    updateOrderItem.value.Quantity = item.Quantity;
    updateOrderItem.value.Unit = item.Unit;
    currentItemIsUpdate.value = index;

    // Lấy ra danh sách các món thêm
    const allAdditionalFoods = foodItems.value.data
      .filter((i) => i.isMain === 0)
      .map((food) => {
        // Tìm món ăn thêm tương ứng trong danh sách đã chọn để 'tick'
        const selectedFood = item.ListAdditionalFood.find(
          (selected) => selected.foodItemId === food.foodItemId
        );
        return {
          ...food,
          quantity: selectedFood ? selectedFood.quantity : 1, // Giữ lại số lượng nếu đã chọn trước đó
        };
      });

    updateOrderItem.value.ListAdditionalFood = allAdditionalFoods;

    // Cập nhật danh sách món đã được chọn
    updateOrderItem.value.ListAdditionalFoodSelected =
      allAdditionalFoods.filter((foodItem) =>
        item.ListAdditionalFood.some(
          (selected) => selected.foodItemId === foodItem.foodItemId
        )
      );

    visibleUpdateCurrentFoodSelected.value = true;
  }
  function updateFoodItem(updateOrderItem) {
    if (currentItemIsUpdate.value !== -1) {
      // Cập nhật món ăn tại vị trí tìm được
      currentOrder.value.items[currentItemIsUpdate.value] = {
        CategoryId: updateOrderItem.CategoryId,
        FoodItemId: updateOrderItem.FoodItemId,
        FoodName: updateOrderItem.FoodName,
        Image: updateOrderItem.Image,
        ListAdditionalFood: updateOrderItem.ListAdditionalFoodSelected,
        Note: updateOrderItem.Note,
        IsMain: updateOrderItem.IsMain,
        Price: updateOrderItem.Price,
        Quantity: updateOrderItem.Quantity,
        Unit: updateOrderItem.Unit,
      };

      // Cập nhật tổng tiền
      updateTotalAmount();
    }

    resetOrderItem();
    currentItemIsUpdate.value = -1;
    visibleUpdateCurrentFoodSelected.value = false;
  }
  function deleteCurrentFoodSelected(index) {
    if (index !== -1) {
      // Xóa món ăn tại vị trí tìm được
      currentOrder.value.items.splice(index, 1);

      // Cập nhật lại tổng tiền
      updateTotalAmount();
    }
  }
  // Hàm cập nhật tổng số tiền
  function updateTotalAmount() {
    currentOrder.value.total_amount = currentOrder.value.items.reduce(
      (total, item) => {
        const mainItemTotal = item.Price * item.Quantity;

        const additionalFoodTotal =
          item.Quantity *
          totalAmountAdditionalFoodItem(item.ListAdditionalFood);

        return total + mainItemTotal + additionalFoodTotal;
      },
      0
    );
  }
  function totalAmountAdditionalFoodItem(ListAdditionalFood) {
    return ListAdditionalFood.reduce((total, item) => {
      return total + item.priceCustom * item.quantity;
    }, 0);
  }
  const resultTotalAmount = computed(() => {
    const totalAmount = currentOrder.value.total_amount;
    const discountAmount =
      (totalAmount * (currentOrder.value.discount || 0)) / 100;
    const taxAmount = (totalAmount * (currentOrder.value.tax || 0)) / 100;
    return totalAmount + taxAmount - discountAmount;
  });
  function formatCurrency(value) {
    if (value === null || value === undefined) return "0 VNĐ";

    const numericValue = Number(value);
    if (isNaN(numericValue)) return "0 VNĐ";

    return numericValue.toLocaleString("vi-VN") + " VNĐ";
  }
  const openDialogShowDetail = (foodItem) => {
    resetOrderItem();

    // Lọc và map các món ăn thêm với quantity mặc định là 0
    const additionalFoodsWithQuantity = foodItems.value.data
      .filter((item) => item.isMain === 0)
      .map((item) => ({
        ...item,
        quantity: 1, // Set quantity mặc định là 0
      }));

    currentOrderItem.value = {
      FoodItemId: foodItem.foodItemId,
      FoodName: foodItem.foodName,
      Quantity: 1,
      Image: foodItem.imageUrl,
      Unit: foodItem.unit,
      Price: foodItem.priceCustom,
      CategoryId: foodItem.categoryId,
      ListAdditionalFood: additionalFoodsWithQuantity,
      Note: "",
      IsMain: foodItem.isMain,
    };

    updateOrderItem.value = {
      FoodItemId: foodItem.foodItemId,
      FoodName: foodItem.foodName,
      Quantity: 1,
      Image: foodItem.imageUrl,
      Unit: foodItem.unit,
      Price: foodItem.priceCustom,
      CategoryId: foodItem.categoryId,
      ListAdditionalFood: additionalFoodsWithQuantity,
      ListAdditionalFoodSelected: [],
      IsMain: foodItem.IsMain,
      Note: "",
    };

    showDialogSelectedAdditionalFood.value = true;
  };

  const resetCurrentOrder = () => {
    currentOrder.value = {
      user_id: user.value.userId,
      order_time: getCurrentDateTimeForSQL(),
      table_id: 1,
      total_amount: 0, // Reset lại tổng thanh toán
      status: "Paid", // Trạng thái mặc định
      discount: 12, // Reset giảm giá
      tax: 6, // Reset thuế
      items: [], // Reset danh sách các món ăn
    };
  };
  async function callApiOrderFood() {
    let orderTimeCurrent = getCurrentDateTimeForSQL();
    try {
      // Call API để thêm vào bảng Orders về thông tin của một lần gọi món
      const orderResponse = await axios.post(API_ENDPOINTS.ADD_ORDER, {
        userId: currentOrder.value.user_id,
        orderTime: orderTimeCurrent,
        tableId: currentOrder.value.table_id,
        totalAmount: currentOrder.value.total_amount,
        totalResult: resultTotalAmount.value,
        status: currentOrder.value.status,
        discount: currentOrder.value.discount,
        tax: currentOrder.value.tax,
        paymentMethod: currentOrder.value.paymentMethod,
      });

      const orderId = orderResponse.data.data.orderId; // Nhận tại OrderId để khi thêm các món ăn đã gọi sẽ biết thuộc bill nào
      // Gửi cả món chính và món phụ trong sau khi đã nhận được orderId, nhầm xác định được các món ăn này thuộc bill nào
      await Promise.all(
        currentOrder.value.items.map(async (item) => {
          const mainItemResponse = await axios.post(
            API_ENDPOINTS.ADD_ORDER_DETAIL,
            {
              orderId: orderId,
              foodItemId: item.FoodItemId,
              foodName: item.FoodName,
              quantity: item.Quantity,
              price: item.Price,
              isMainItem: item.IsMain ?? 1,
              unit: item.Unit,
              note: item.Note,
              categoryId: item.CategoryId,
              orderTime: orderTimeCurrent,
            }
          );
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

      // const [responseUpdateMaterial, responseUpdateTotalIncome] =
      //   await Promise.all([
      //     axios.post(API_ENDPOINTS.UPDATE_QUANTITY_MATERIALS_AFTER_ORDER, {
      //       Items: currentOrder.value.items,
      //     }),
      //     axios.post(API_ENDPOINTS.UPDATE_TOTALINCOME_CASH_REGISTER, {
      //       UserId: user.value.userId,
      //       TotalAmount: resultTotalAmount.value,
      //     }),
      //   ]);
      // Kiểm tra phản hồi từ server
      if (orderResponse.data.success === -1) {
        showToast("Please provide all required information!", "warn");
      } else if (orderResponse.data.success === 1) {
        showToast("Add order successful!", "success");
        resetCurrentOrder();
        // setTimeout(() => {
        //   window.location.reload();
        // }, 3200);
      } else {
        console.error("Failed to add order", orderResponse.data.message);
      }
    } catch (error) {
      showToast(`Error adding order: ${error}`, "error");
    }
  }

  async function callApiOrderFoodAndAddTable() {
    showComponentAreaManagement.value = !showComponentAreaManagement.value;
    currentOrderClone.value = {
      ...currentOrder.value,
      result_total_amount: resultTotalAmount.value,
    };

    orderStore.setSelectedDishes(currentOrderClone.value);
  }
  const handleCloseComponentAreaManagement = () => {
    showComponentAreaManagement.value = !showComponentAreaManagement.value;
    orderStore.clearSelectedDishes();
    // resetCurrentOrder();
  };
  function handleCloseAndReset() {
    resetCurrentOrder();
    showComponentAreaManagement.value = !showComponentAreaManagement.value;
  }

  return {
    // State variables
    showDialogUpdate,
    isStaff,
    currentDish,
    foodCategories,
    foodItems,
    search,
    showDialogSelectedAdditionalFood,
    currentFoodItem,
    visibleUpdateCurrentFoodSelected,
    showComponentAreaManagement,
    loading,

    // Computed properties
    user,
    filteredFoodItems,
    resultTotalAmount,

    // Data objects
    listFoodCategorySelected,
    currentOrderItem,
    resultOrderItem,
    updateOrderItem,
    resultUpdateOrderItem,
    currentOrder,

    // Methods
    init,
    tonggleSelected,
    getCurrentDateTimeForSQL,
    resetOrderItem,
    selectedFoodItemToCart,
    nonSelectedFoodItemToCart,
    updateTotalAmount,
    totalAmountAdditionalFoodItem,
    formatCurrency,
    openDialogShowDetail,
    updateCurrentFoodSelected,
    updateFoodItem,
    deleteCurrentFoodSelected,
    handleCloseComponentAreaManagement,
    handleCloseAndReset,
    resetCurrentOrder,
    callApiOrderFood,
    callApiOrderFoodAndAddTable,
  };
}
