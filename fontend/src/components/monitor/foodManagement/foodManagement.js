import { ref } from "vue";
import axios from "axios";
import { computed } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { useUserStore } from "@/stores/user.js";
import AreaManagement from "@/components/monitor/areaManagement/index.vue";

export default function useFoodManagement() {
  const userStore = useUserStore();
  const showDialogUpdate = ref(false);
  const showDialogAdd = ref(false);
  const isStaff = ref(true);
  const currentDish = ref({});
  const foodCategories = ref([]);
  const foodItems = ref([]);
  const additionalFoods = ref([]);
  const search = ref("");
  const currentFoodItem = ref({});
  const visibleUpdateCurrentFoodSelected = ref(false);
  const showComponentAreaManagement = ref(false);
  const modalConfirmDeleteFoodItem = ref(false);
  const modalUpdateFoodItem = ref(false);
  const foodItemCurrentUpdate = ref({});
  const loading = shallowRef(true);
  // Thêm biến để lưu trữ giá trị ban đầu của món ăn
  const originalFoodItem = ref(null);

  // Lấy thông tin người dùng từ store
  const user = computed(() => userStore.user);

  // Danh sách categories được chọn
  const listDashSelected = ref([]);

  // Thông tin lưu món được thêm vào
  const foodAdd = ref({
    categoryId: 0,
    foodName: "",
    priceCustom: "",
    priceListed: "",
    unit: "",
    imageUrl: "",
  });

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

  function formatDate(dateTime) {
    const date = new Date(dateTime); // Chuyển đổi chuỗi thời gian thành đối tượng Date
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0"); // Tháng bắt đầu từ 0
    const day = String(date.getDate()).padStart(2, "0");
    return `${day}-${month}-${year}`; // Định dạng: "yyyy-MM-dd"
  }

  // Hàm hỗ trợ chuyển đổi File thành base64 string
  function convertFileToBase64(file) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = (error) => reject(error);
    });
  }

  // Lưu thông tin xuống database
  async function saveFood() {
    try {
      // Format categoryId
      foodAdd.value.categoryId = format(foodAdd.value.categoryId);

      // Chuyển đổi file ảnh thành base64 string nếu có
      let imageString = null;
      if (foodAdd.value.imageUrl instanceof File) {
        imageString = await convertFileToBase64(foodAdd.value.imageUrl);
      }

      // Tạo object data với ảnh dạng string
      const requestData = {
        foodName: foodAdd.value.foodName,
        priceListed: foodAdd.value.priceListed,
        priceCustom: foodAdd.value.priceCustom,
        unit: foodAdd.value.unit,
        categoryId: foodAdd.value.categoryId,
        imageUrl: imageString,
      };

      const response = await axios.post(
        API_ENDPOINTS.ADD_FOOD_ITEM,
        requestData,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.data.success) {
        showDialogAdd.value = !showDialogAdd.value;

        // Tạo object món ăn mới với dữ liệu từ response và ảnh
        const food = {
          categoryId: response.data.categoryId,
          foodItemId: response.data.foodItemId,
          foodName: response.data.foodName,
          imageUrl: imageString || response.data.imageUrl, // Sử dụng ảnh mới hoặc từ response
          priceCustom: response.data.priceCustom,
          priceListed: response.data.priceListed,
          status: response.data.status,
          unit: response.data.unit,
          createDate: new Date(response.data.createDate),
          updateDate: new Date(response.data.updateDate),
        };

        // Thêm món ăn mới vào danh sách
        foodItems.value.push(food);

        // Reset form
        foodAdd.value = {
          categoryId: 0,
          foodName: "",
          priceCustom: "",
          priceListed: "",
          unit: "",
          imageUrl: "",
        };

        toast.success("Add food item successful!", {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
        });
      } else {
        toast.error("Add food item fail!", {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
        });
      }
    } catch (error) {
      console.error("Error adding food item:", error);
      toast.error(`Error adding food item: ${error.message}`, {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
      });
    }
  }

  const getCategoryName = (categoryId) => {
    const categories = {
      2: "Món khô",
      3: "Món nước",
      4: "Món ăn nhanh",
      5: "Tráng miệng",
      6: "Nước uống",
      7: "Món gọi thêm",
    };
    return categories[categoryId] || "Không xác định";
  };

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
  }
  function nonSelectedFoodItem() {
    // Lý do gán lại resultOrderItem.value.ListAdditionalFood vì khi check sẽ thao tác trực tiếp với resultOrderItem.value.ListAdditionalFood
    resultOrderItem.value.ListAdditionalFood =
      resultOrderItem.value.ListAdditionalFood.map(() => false);
    resetOrderItem();
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
    const discountAmount =
      (currentOrder.value.total_amount * (currentOrder.value.discount || 0)) /
      100;
    const taxAmount =
      (currentOrder.value.total_amount * (currentOrder.value.tax || 0)) / 100;
    return currentOrder.value.total_amount + taxAmount - discountAmount;
  });
  function formatCurrency(value) {
    return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
  }
  //Hàm xóa confirm xóa món
  function openDialogShowDeleteFoodItemSelected(foodItem) {
    currentOrderItem.value.FoodItemId = foodItem.foodItemId;
    currentOrderItem.value.FoodName = foodItem.foodName;
    currentOrderItem.value.Image = foodItem.imageUrl;
    currentOrderItem.value.Price = foodItem.priceCustom;
    modalConfirmDeleteFoodItem.value = true;
  }

  async function confimDeleteFoodItem(currentOrderItem) {
    const FoodItemCurrentId = currentOrderItem.FoodItemId;

    // Truyền FoodItemId qua query parameters

    const response = await axios.delete(
      `${API_ENDPOINTS.DELETE_FOOD_ITEM}/${FoodItemCurrentId}`
    );
    if (response.data.success) {
      modalConfirmDeleteFoodItem.value = !modalConfirmDeleteFoodItem.value;
      toast.success("Delete food item successful!", {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
    }

    foodItems.value = foodItems.value.filter(
      (item) => item.foodItemId != FoodItemCurrentId
    );
  }

  function openDialogShowUpdateFoodItemSelected(foodItem) {
    // Lưu lại giá trị ban đầu
    originalFoodItem.value = { ...foodItem };
    foodItemCurrentUpdate.value = { ...foodItem };
    modalUpdateFoodItem.value = true;
  }

  function cancelConfirmUpdateFoodItem() {
    // Khôi phục lại giá trị ban đầu từ originalFoodItem
    if (originalFoodItem.value) {
      foodItemCurrentUpdate.value = { ...originalFoodItem.value };
    }
    modalUpdateFoodItem.value = false;
  }

  async function confirmUpdateFoodItem(foodItemCurrentUpdate) {
    const FoodItemUpdateId = foodItemCurrentUpdate.foodItemId;

    try {
      // Nếu có file ảnh mới, chuyển đổi thành base64 string
      let imageString = foodItemCurrentUpdate.imageUrl;
      if (foodItemCurrentUpdate.imageUrl instanceof File) {
        imageString = await convertFileToBase64(foodItemCurrentUpdate.imageUrl);
      }

      // Tạo object data với ảnh dạng string
      const requestData = {
        foodItemId: foodItemCurrentUpdate.foodItemId,
        foodName: foodItemCurrentUpdate.foodName,
        priceListed: foodItemCurrentUpdate.priceListed,
        priceCustom: foodItemCurrentUpdate.priceCustom,
        unit: foodItemCurrentUpdate.unit,
        categoryId: foodItemCurrentUpdate.categoryId,
        status: foodItemCurrentUpdate.status,
        imageUrl: imageString,
      };

      const response = await axios.put(
        `${API_ENDPOINTS.UPDATE_FOOD_ITEM}/${FoodItemUpdateId}`,
        requestData,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.data.success) {
        const updatedFoodIndex = foodItems.value.findIndex(
          (item) => item.foodItemId === FoodItemUpdateId
        );

        if (updatedFoodIndex !== -1) {
          // Cập nhật ngay lập tức dữ liệu trong foodItems
          foodItems.value[updatedFoodIndex] = {
            ...foodItems.value[updatedFoodIndex],
            ...response.data.data, // Giả sử server trả về object với thông tin đã cập nhật
            imageUrl: imageString, // Cập nhật URL ảnh mới
          };
        }

        modalUpdateFoodItem.value = false;
        toast.success("Update food item successful!", {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
        });
      } else {
        toast.error("Update failed. Please try again.", {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
        });
      }
    } catch (error) {
      console.error("Error updating food item:", error);
      toast.error(`Error updating food item: ${error.message}`, {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
      });
    }
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
      const orderResponse = await axios.post(API_ENDPOINTS.ADD_ORDER, {
        userId: currentOrder.value.user_id,
        orderTime: orderTimeCurrent,
        tableId: currentOrder.value.table_id,
        totalAmount: resultTotalAmount.value,
        status: currentOrder.value.status,
        discount: currentOrder.value.discount,
        tax: currentOrder.value.tax,
      });

      const orderId = orderResponse.data.data.orderId;

      // Gửi cả món chính và món phụ trong cùng một request
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
          // Gửi các món phụ với parentItemId là mainItemId
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
      } else {
        console.error("Failed to add order", orderResponse.data.message);
      }
    } catch (error) {
      toast.error(`Error adding employee: ${error}`, {
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
    storeOrder.setSelectedDishes(currentOrder.value);
  }
  const handleShowComponentAreaManagement = () => {
    showComponentAreaManagement.value = !showComponentAreaManagement.value;
  };
  // Thêm hàm xử lý event từ AreaManagement
  function handleCloseAndReset() {
    showComponentAreaManagement.value = false;
    resetCurrentOrder();
  }

  return {
    // State variables
    showDialogUpdate,
    showDialogAdd,
    isStaff,
    currentDish,
    foodCategories,
    foodItems,
    additionalFoods,
    search,
    currentFoodItem,
    visibleUpdateCurrentFoodSelected,
    showComponentAreaManagement,
    modalConfirmDeleteFoodItem,
    modalUpdateFoodItem,
    foodItemCurrentUpdate,
    loading,
    originalFoodItem,

    // Computed properties
    user,
    filteredFoodItems,
    resultTotalAmount,

    // Data objects
    listDashSelected,
    foodAdd,
    currentOrderItem,
    resultOrderItem,
    updateOrderItem,
    resultUpdateOrderItem,
    currentOrder,

    // Methods
    init,
    format,
    formatDate,
    saveFood,
    getCategoryName,
    tonggleSelected,
    getCurrentDateTimeForSQL,
    resetOrderItem,
    updateTotalAmount,
    totalAmountAdditionalFoodItem,
    formatCurrency,
    openDialogShowDeleteFoodItemSelected,
    confimDeleteFoodItem,
    openDialogShowUpdateFoodItemSelected,
    handleShowComponentAreaManagement,
    handleCloseAndReset,
    confirmUpdateFoodItem,
    cancelConfirmUpdateFoodItem,
    updateCurrentFoodSelected,
    updateFoodItem,
    deleteCurrentFoodSelected,
    resetCurrentOrder,
    callApiOrderFood,
    callApiOrderFoodAndAddTable,
  };
}
