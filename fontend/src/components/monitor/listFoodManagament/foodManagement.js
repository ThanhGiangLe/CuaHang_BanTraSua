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
  const additionalFoods = ref([]);
  const search = ref("");
  const detailItem = ref(false);
  const currentFoodItem = ref({});
  const visibleUpdateCurrentFoodSelected = ref(false);
  const showComponentAreaManagement = ref(false);
  const loading = shallowRef(true);
  const currentOrderClone = ref({});
  const propertyTax = ref(0);
  const propertyDiscount = ref(0);

  // Lấy thông tin người dùng từ store
  const user = computed(() => userStore.user);

  // Danh sách categories được chọn
  const listDashSelected = ref([]);

  // Hàm chạy đầu tiên
  async function init() {
    const response = await axios.get(API_ENDPOINTS.GET_ALL_FOOD_CATEGORIES);
    foodCategories.value = response.data;

    const responseFoodItems = await axios.get(API_ENDPOINTS.GET_ALL_FOOD_ITEMS);
    foodItems.value = responseFoodItems.data;

    const responseAdditonalFoods = await axios.get(
      API_ENDPOINTS.GET_ALL_ADDITIONAL_FOODS
    );
    additionalFoods.value = responseAdditonalFoods.data;
    loading.value = false;
  }
  init();

  // Format CategoryId
  function format(str) {
    const index = str.indexOf("(");
    if (index !== -1) {
      return str.substring(0, index).trim(); // Trim để loại bỏ khoảng trắng thừa
    }
    return str.trim();
  }

  function tonggleSelected(foodCategory) {
    if (listDashSelected.value.includes(foodCategory)) {
      listDashSelected.value = listDashSelected.value.filter(
        (s) => s.categoryId !== foodCategory.categoryId
      );
    } else {
      listDashSelected.value.push(foodCategory);
    }
  }
  const filteredFoodItems = computed(() => {
    // Kiểm tra foodItems.value là mảng
    if (!Array.isArray(foodItems.value)) {
      console.error("foodItems.value is not an array:", foodItems.value);
      return []; // Trả về mảng rỗng nếu không phải mảng
    }

    return foodItems.value.filter((foodItem) => {
      const isCategoryMatch =
        listDashSelected.value.length === 0 ||
        listDashSelected.value.some(
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
  // Dùng để thao tác khi thêm món và0 trong Danh sách món đã chọn
  const currentOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Quantity: 0,
    Price: 0,
    Image: "",
    CustomItemId: null,
    Unit: "",
    Note: "",
    CategoryId: 0,
    ListAdditionalFood: [],
  });
  // Sử dụng ListAdditionalFood trong phần v-model của v-checkbox để lưu các checkbox được checked
  const resultOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Quantity: 0,
    Price: 0,
    Image: "",
    CustomItemId: null,
    Unit: "",
    Note: "",
    CategoryId: 0,
    ListAdditionalFood: [],
  });
  const updateOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Quantity: 0,
    Price: 0,
    Image: "",
    CustomItemId: null,
    Unit: "",
    Note: "",
    CategoryId: 0,
    ListAdditionalFood: [],
    ListAdditionalFoodSelected: [],
  });
  const resultUpdateOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Quantity: 0,
    Price: 0,
    Image: "",
    CustomItemId: null,
    Unit: "",
    Note: "",
    CategoryId: 0,
    ListAdditionalFood: [],
  });
  const currentOrder = ref({
    user_id: user.value.userId,
    order_time: getCurrentDateTimeForSQL(),
    table_id: 1,
    total_amount: 0,
    status: "Paid",
    discount: 12,
    tax: 6,
    items: [],
  });
  function resetOrderItem() {
    // Reset các giá trị cơ bản
    currentOrderItem.value = {
      FoodItemId: "",
      FoodName: "",
      Quantity: 0,
      Price: 0,
      Image: "",
      CustomItemId: null,
      Unit: "",
      Note: "",
      CategoryId: 0,
      ListAdditionalFood: [],
    };

    resultOrderItem.value = {
      FoodItemId: "",
      FoodName: "",
      Quantity: 0,
      Price: 0,
      Image: "",
      CustomItemId: null,
      Unit: "",
      Note: "",
      CategoryId: 0,
      ListAdditionalFood: [],
    };

    updateOrderItem.value = {
      FoodItemId: "",
      FoodName: "",
      Quantity: 0,
      Price: 0,
      Image: "",
      CustomItemId: null,
      Unit: "",
      Note: "",
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
      CustomItemId: null,
      Unit: "",
      Note: "",
      CategoryId: 0,
      ListAdditionalFood: [],
    };
  }
  function selectedFoodItem(currentOrderItem) {
    // Đem hết thông tin thao tác trên currentOrderItem đem qua resultOrderItem. Còn phần ListAdditionalFood đã được thao tác trước đó
    resultOrderItem.value.FoodItemId = currentOrderItem.FoodItemId;
    resultOrderItem.value.FoodName = currentOrderItem.FoodName;
    resultOrderItem.value.Quantity = currentOrderItem.Quantity;
    resultOrderItem.value.Price = currentOrderItem.Price;
    resultOrderItem.value.Image = currentOrderItem.Image;
    resultOrderItem.value.CustomItemId = currentOrderItem.CustomItemId;
    resultOrderItem.value.Unit = currentOrderItem.Unit;
    resultOrderItem.value.Note = currentOrderItem.Note;
    resultOrderItem.value.CategoryId = currentOrderItem.CategoryId;

    // Thêm món ăn vào danh sách order
    currentOrder.value.items.push({ ...resultOrderItem.value });

    // Cập nhật tổng tiền khi thêm hoặc tăng số lượng món
    updateTotalAmount();

    resetOrderItem();
    detailItem.value = false;
  }
  function nonSelectedFoodItem() {
    // Lý do gán lại resultOrderItem.value.ListAdditionalFood vì khi check sẽ thao tác trực tiếp với resultOrderItem.value.ListAdditionalFood
    resultOrderItem.value.ListAdditionalFood =
      resultOrderItem.value.ListAdditionalFood.map(() => false);
    resetOrderItem();
    detailItem.value = false;
  }
  function updateCurrentFoodSelected(item) {
    updateOrderItem.value.CategoryId = item.CategoryId;
    updateOrderItem.value.CustomItemId = item.CustomItemId;
    updateOrderItem.value.FoodItemId = item.FoodItemId;
    updateOrderItem.value.FoodName = item.FoodName;
    updateOrderItem.value.Quantity = item.Quantity;
    updateOrderItem.value.Image = item.Image;
    updateOrderItem.value.Unit = item.Unit;
    updateOrderItem.value.Price = item.Price;
    updateOrderItem.value.Note = item.Note;

    // Lấy tất cả các món ăn thêm có thể chọn cho món này
    const allAdditionalFoods = additionalFoods.value
      .filter((i) => i.categoryId === item.CategoryId)
      .map((food) => {
        // Tìm món ăn thêm tương ứng trong danh sách đã chọn
        const selectedFood = item.ListAdditionalFood.find(
          (selected) => selected.id === food.id
        );
        return {
          ...food,
          quantity: selectedFood ? selectedFood.quantity : 0, // Giữ lại số lượng nếu đã chọn trước đó
        };
      });

    updateOrderItem.value.ListAdditionalFood = allAdditionalFoods;

    // Cập nhật danh sách món đã được chọn
    updateOrderItem.value.ListAdditionalFoodSelected =
      allAdditionalFoods.filter((foodItem) =>
        item.ListAdditionalFood.some((selected) => selected.id === foodItem.id)
      );

    visibleUpdateCurrentFoodSelected.value = true;
  }
  function updateFoodItem(updateOrderItem) {
    // Tìm index của món cần update trong mảng items
    const itemIndex = currentOrder.value.items.findIndex(
      (item) =>
        item.FoodItemId === updateOrderItem.FoodItemId &&
        item.CustomItemId === updateOrderItem.CustomItemId
    );

    if (itemIndex !== -1) {
      // Cập nhật món ăn tại vị trí tìm được
      currentOrder.value.items[itemIndex] = {
        CategoryId: updateOrderItem.CategoryId,
        CustomItemId: updateOrderItem.CustomItemId,
        FoodItemId: updateOrderItem.FoodItemId,
        FoodName: updateOrderItem.FoodName,
        Image: updateOrderItem.Image,
        ListAdditionalFood: updateOrderItem.ListAdditionalFoodSelected,
        Note: updateOrderItem.Note,
        Price: updateOrderItem.Price,
        Quantity: updateOrderItem.Quantity,
        Unit: updateOrderItem.Unit,
      };

      // Cập nhật tổng tiền
      updateTotalAmount();
    }

    resetOrderItem();
    visibleUpdateCurrentFoodSelected.value = false;
  }
  function deleteCurrentFoodSelected(item) {
    // Tìm index của món cần xóa dựa vào cả FoodItemId và CustomItemId
    const itemIndex = currentOrder.value.items.findIndex(
      (orderItem) =>
        orderItem.FoodItemId === item.FoodItemId &&
        orderItem.CustomItemId === item.CustomItemId
    );

    if (itemIndex !== -1) {
      // Xóa món ăn tại vị trí tìm được
      currentOrder.value.items.splice(itemIndex, 1);

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
      return total + item.price * item.quantity;
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
    return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
  }
  const openDialogShowDetail = (foodItem) => {
    // Reset trước khi mở dialog mới
    resetOrderItem();

    // Lọc và map các món ăn thêm với quantity mặc định là 0
    const additionalFoodsWithQuantity = additionalFoods.value
      .filter((item) => item.categoryId === foodItem.categoryId)
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
      Note: "",
    };

    detailItem.value = true;
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
    console.log("currentOrder", currentOrder.value);
    try {
      // Call API để thêm vào bảng Orders về thông tin của một lần gọi món
      const orderResponse = await axios.post(API_ENDPOINTS.ADD_ORDER, {
        userId: currentOrder.value.user_id,
        orderTime: orderTimeCurrent,
        tableId: currentOrder.value.table_id,
        totalAmount: resultTotalAmount.value,
        status: currentOrder.value.status,
        discount: currentOrder.value.discount,
        tax: currentOrder.value.tax,
      });

      const orderId = orderResponse.data.data.orderId; // Nhận tại OrderId để khi thêm các món ăn đã gọi sẽ biết thuộc bill nào

      // Gửi cả món chính và món phụ trong sau khi đã nhận được orderId, nhầm xác định được các món ăn này thuộc bill nào
      await Promise.all(
        currentOrder.value.items.map(async (item) => {
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
                categoryId: 0,
                orderTime: orderTimeCurrent,
              });
            })
          );
        })
      );

      const [responseUpdateMaterial, responseUpdateTotalIncome] =
        await Promise.all([
          axios.post(API_ENDPOINTS.UPDATE_QUANTITY_MATERIALS_AFTER_ORDER, {
            Items: currentOrder.value.items,
          }),
          axios.post(API_ENDPOINTS.UPDATE_TOTALINCOME_CASH_REGISTER, {
            UserId: user.value.userId,
            TotalAmount: resultTotalAmount.value,
          }),
        ]);
      console.log("responseUpdateMaterial", responseUpdateMaterial.data);
      console.log("responseUpdateTotalIncome", responseUpdateTotalIncome.data);
      // Kiểm tra phản hồi từ server
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
        toast.success("Add order successful!", {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false, // Hiện thanh tiến trình
          closeOnClick: true, // Đóng khi nhấp vào thông báo
          pauseOnHover: true, // Dừng khi di chuột lên thông báo
          draggable: true, // Kéo thông báo
          progress: undefined, // Tiến độ (nếu có)
        });
        resetCurrentOrder();
        setTimeout(() => {
          window.location.reload();
        }, 3200);
      } else {
        console.error("Failed to add order", orderResponse.data.message);
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
  async function callApiOrderFoodAndAddTable() {
    showComponentAreaManagement.value = !showComponentAreaManagement.value;
    currentOrderClone.value = {
      ...currentOrder.value,
      total_amount: resultTotalAmount.value,
    };
    console.log("Danh sách món ăn đã gọi: ", currentOrderClone.value);
    propertyTax.value = currentOrderClone.value.tax;
    propertyDiscount.value = currentOrderClone.value.discount;
    console.log("Số 11 ", propertyTax.value);
    console.log("Số 11 ", propertyDiscount.value);

    orderStore.setSelectedDishes(currentOrderClone.value);
  }
  const handleShowComponentAreaManagement = () => {
    showComponentAreaManagement.value = !showComponentAreaManagement.value;
    orderStore.clearSelectedDishes();
    // resetCurrentOrder();
  };
  function handleCloseAndReset() {
    console.log("Có vào");
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
    additionalFoods,
    search,
    detailItem,
    currentFoodItem,
    visibleUpdateCurrentFoodSelected,
    showComponentAreaManagement,
    loading,

    // Computed properties
    user,
    filteredFoodItems,
    resultTotalAmount,

    // Data objects
    listDashSelected,
    currentOrderItem,
    resultOrderItem,
    updateOrderItem,
    resultUpdateOrderItem,
    currentOrder,

    // Methods
    init,
    format,
    tonggleSelected,
    getCurrentDateTimeForSQL,
    resetOrderItem,
    selectedFoodItem,
    nonSelectedFoodItem,
    updateTotalAmount,
    totalAmountAdditionalFoodItem,
    formatCurrency,
    openDialogShowDetail,
    updateCurrentFoodSelected,
    updateFoodItem,
    deleteCurrentFoodSelected,
    handleShowComponentAreaManagement,
    handleCloseAndReset,
    resetCurrentOrder,
    callApiOrderFood,
    callApiOrderFoodAndAddTable,
  };
}
