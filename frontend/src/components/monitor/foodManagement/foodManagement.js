import { ref } from "vue";
import axios from "axios";
import { computed } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { useUserStore } from "@/stores/user.js";
import { showToast } from "@/styles/handmade";

export default function useFoodManagement() {
  const userStore = useUserStore();
  const showDialogAdd = ref(false);
  const foodCategories = ref([]);
  const foodItems = ref([]);
  const search = ref("");
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

  const categoriesDataDefault = {
    8: "Best thèm",
    9: "Món mới",
    10: "Đậm vị",
    11: "Trà sữa truyền thống",
    12: "Trà sữa mix",
    13: "Thèm nhai đã",
    14: "Siêu tiết kiệm / Combo hot",
    15: "Thơm béo ngất ngây",
    16: "Món thêm",

    1: "Món chính",
    0: "Món thêm",
  };
  // Thông tin lưu món được thêm vào
  const foodAdd = ref({
    foodName: "",
    priceListed: "",
    priceCustom: "",
    imageUrl: "",
    unit: "",
    categoryId: -1,
    categoryIdString: "",
    isMain: -1,
    isMainString: "",
  });

  // Hàm chạy đầu tiên
  async function init() {
    const response = await axios.get(API_ENDPOINTS.GET_ALL_FOOD_CATEGORIES);
    foodCategories.value = response.data.data;

    const responseFoodItems = await axios.get(API_ENDPOINTS.GET_ALL_FOOD_ITEMS);
    foodItems.value = responseFoodItems.data.data;
    loading.value = false;
  }
  init();

  // Format CategoryId
  function getIdByName(name) {
    const cleanedName = name.trim();
    for (const [id, value] of Object.entries(categoriesDataDefault)) {
      if (value === cleanedName) {
        return parseInt(id);
      }
    }
    return null;
  }
  function formatDate(dateTime) {
    const isoString = dateTime.toString();

    return isoString
      .replace("T", " ")
      .replace(/\.\d+Z$/, "")
      .replace(/\.\d+/, "");
  }
  function convertFileToBase64(file) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = (error) => reject(error);
    });
  }
  function cancelSaveFood() {
    showDialogAdd.value = !showDialogAdd.value;
    foodAdd.value = {
      foodName: "",
      priceListed: "",
      priceCustom: "",
      imageUrl: "",
      unit: "",
      categoryIdString: "",
      categoryId: -1,
      isMainString: "",
      isMain: -1,
    };
  }
  // Lưu thông tin xuống database
  async function saveFood() {
    try {
      // Format categoryId
      foodAdd.value.categoryId = getIdByName(foodAdd.value.categoryIdString);
      foodAdd.value.isMain = getIdByName(foodAdd.value.isMainString);

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
        imageUrl: imageString,
        unit: foodAdd.value.unit,
        categoryId: foodAdd.value.categoryId,
        createBy: user.value.fullName,
        updateBy: user.value.fullName,
        isMain: foodAdd.value.isMain,
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
          createBy: response.data.createBy,
          updateDate: new Date(response.data.updateDate),
          updateBy: response.data.updateBy,
          isMain: response.data.isMain,
        };

        // Thêm món ăn mới vào danh sách
        foodItems.value.push(food);

        // Reset form
        foodAdd.value = {
          foodName: "",
          priceListed: "",
          priceCustom: "",
          imageUrl: "",
          unit: "",
          categoryIdString: "",
          categoryId: -1,
          isMainString: "",
          isMain: -1,
        };
        showToast("Thêm món thành công!", "success");
      } else {
        showToast("Thêm món thất bại!", "error");
      }
    } catch (error) {
      showToast(`Lỗi trong quá trình thêm món ${error.message}!`, "error");
    }
  }

  const getCategoryName = (categoryId) => {
    return categoriesDataDefault[categoryId] || "Không xác định";
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
  // Dùng để thao tác khi thêm món và0 trong Danh sách món đã chọn
  const currentOrderItem = ref({
    FoodItemId: "",
    FoodName: "",
    Price: 0,
    Image: "",
    Unit: "",
    CategoryId: 0,
  });
  function formatCurrency(value) {
    if (value === null || value === undefined) return "0 VNĐ";

    const numericValue = Number(value);
    if (isNaN(numericValue)) return "0 VNĐ";

    return numericValue.toLocaleString("vi-VN") + " VNĐ";
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

    const response = await axios.delete(
      `${API_ENDPOINTS.DELETE_FOOD_ITEM}/${FoodItemCurrentId}`
    );
    if (response.data.success) {
      modalConfirmDeleteFoodItem.value = !modalConfirmDeleteFoodItem.value;
      showToast("Xóa món thành công!", "success");
      foodItems.value = foodItems.value.filter(
        (item) => item.foodItemId != FoodItemCurrentId
      );
    } else {
      showToast("Xóa món thất bại!", "error");
    }
  }

  function openDialogShowUpdateFoodItemSelected(foodItem) {
    // Lưu lại giá trị ban đầu
    originalFoodItem.value = { ...foodItem };
    foodItemCurrentUpdate.value = {
      ...foodItem,
      categoryIdString: getCategoryName(foodItem.categoryId),
      isMainString: getCategoryName(foodItem.isMain),
    };
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
    foodItemCurrentUpdate.categoryId = getIdByName(
      foodItemCurrentUpdate.categoryIdString
    );
    foodItemCurrentUpdate.isMain = getIdByName(
      foodItemCurrentUpdate.isMainString
    );
    console.log("foodItemCurrentUpdate: ", foodItemCurrentUpdate);
    let FoodItemUpdateId = foodItemCurrentUpdate.foodItemId;
    try {
      // Nếu có file ảnh mới, chuyển đổi thành base64 string
      let imageString = foodItemCurrentUpdate.imageUrl;
      if (foodItemCurrentUpdate.imageUrl instanceof File) {
        imageString = await convertFileToBase64(foodItemCurrentUpdate.imageUrl);
      }

      // Tạo object data với ảnh dạng string
      const requestData = {
        foodItemId: FoodItemUpdateId,
        foodName: foodItemCurrentUpdate.foodName,
        priceListed: foodItemCurrentUpdate.priceListed,
        priceCustom: foodItemCurrentUpdate.priceCustom,
        imageUrl: imageString,
        unit: foodItemCurrentUpdate.unit,
        categoryId: foodItemCurrentUpdate.categoryId,
        status: foodItemCurrentUpdate.status,
        updateBy: user.value.fullName,
        isMain: foodItemCurrentUpdate.isMain,
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
        console.log("response: ", response.data.data);
        if (updatedFoodIndex !== -1) {
          // Cập nhật ngay lập tức dữ liệu trong foodItems
          foodItems.value[updatedFoodIndex] = {
            ...foodItems.value[updatedFoodIndex],
            ...response.data.data, // Giả sử server trả về object với thông tin đã cập nhật
            updateDate: response.data.data.updateDate,
            imageUrl: imageString, // Cập nhật URL ảnh mới
          };
          console.log("Sau cập nhật: ", response.data.data);
        }

        modalUpdateFoodItem.value = false;
        showToast("Cập nhật thành công!", "success");
      } else {
        showToast("Cập nhật thất bại!", "error");
      }
    } catch (error) {
      showToast(`Lỗi trong quá trình cập nhật ${error.message}`, "error");
    }
  }

  return {
    // State variables
    showDialogAdd,
    foodCategories,
    search,
    modalConfirmDeleteFoodItem,
    modalUpdateFoodItem,
    foodItemCurrentUpdate,
    loading,

    // Computed properties
    user,
    filteredFoodItems,

    // Data objects
    listDashSelected,
    foodAdd,
    currentOrderItem,

    // Methods
    formatDate,
    cancelSaveFood,
    saveFood,
    getCategoryName,
    tonggleSelected,
    formatCurrency,
    openDialogShowDeleteFoodItemSelected,
    confimDeleteFoodItem,
    openDialogShowUpdateFoodItemSelected,
    confirmUpdateFoodItem,
    cancelConfirmUpdateFoodItem,
  };
}
