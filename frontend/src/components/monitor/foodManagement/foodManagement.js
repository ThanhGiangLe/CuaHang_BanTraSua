import { ref } from "vue";
import axios from "axios";
import { computed } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { useUserStore } from "@/stores/user.js";
import { showToast } from "@/styles/handmade";
import { foodManagementHandler } from "/src/composables/foodManagement/foodManagementHandler.js";

export default function useFoodManagement() {
  const userStore = useUserStore();
  const { getAllFood, getAllCategory, createFood, updateFood, deleteFood } =
    foodManagementHandler();
  const showDialogAdd = ref(false);
  const foodCategories = ref([]);
  const foodItems = ref([]);
  const search = ref("");
  const modalConfirmDeleteFoodItem = ref(false);
  const modalUpdateFoodItem = ref(false);
  const foodItemCurrentUpdate = ref({});
  const loading = shallowRef(true);
  const originalFoodItem = ref(null);

  const user = computed(() => userStore.user);

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
  // Hàm chạy đầu tiên
  async function init() {
    const responseFood = await getAllFood();
    foodItems.value = responseFood.listFood.data;

    const responseCate = await getAllCategory();
    foodCategories.value = responseCate.listCategory.data;
    loading.value = false;
  }
  init();
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

  // <=== CHỨC NĂNG CREATE ===>
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
  async function saveFood() {
    foodAdd.value.categoryId = getIdByName(foodAdd.value.categoryIdString);
    foodAdd.value.isMain = getIdByName(foodAdd.value.isMainString);

    let imageString = null;
    if (foodAdd.value.imageUrl instanceof File) {
      imageString = await convertFileToBase64(foodAdd.value.imageUrl);
    }

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

    const response = await createFood(requestData);

    if (response.responseCreate) {
      if (response.responseCreate.success) {
        const food = {
          categoryId: response.responseCreate.categoryId,
          foodItemId: response.responseCreate.foodItemId,
          foodName: response.responseCreate.foodName,
          imageUrl: imageString || response.responseCreate.imageUrl, // Sử dụng ảnh mới hoặc từ response
          priceCustom: response.responseCreate.priceCustom,
          priceListed: response.responseCreate.priceListed,
          status: response.responseCreate.status,
          unit: response.responseCreate.unit,
          createDate: formatDate(response.responseCreate.createDate),
          createBy: response.responseCreate.createBy,
          updateDate: formatDate(response.responseCreate.updateDate),
          updateBy: response.responseCreate.updateBy,
          isMain: response.responseCreate.isMain,
        };

        foodItems.value.push(food);
        cancelSaveFood();

        showToast("Thêm món thành công!", "success");
      } else {
        showToast("Thêm món thất bại!", "error");
      }
    } else {
      if (response.response.status == 403) {
        showToast(`Bạn không có quyền thao tác chức năng này!`, "warn");
        cancelSaveFood();
      }
    }
  }

  // <=== CHỨC NĂNG DELETE ===>
  function openDialogShowDeleteFoodItemSelected(foodItem) {
    currentOrderItem.value.FoodItemId = foodItem.foodItemId;
    currentOrderItem.value.FoodName = foodItem.foodName;
    currentOrderItem.value.Image = foodItem.imageUrl;
    currentOrderItem.value.Price = foodItem.priceCustom;
    modalConfirmDeleteFoodItem.value = true;
  }
  async function confimDeleteFoodItem(currentOrderItem) {
    const FoodItemCurrentId = currentOrderItem.FoodItemId;
    const response = await deleteFood(FoodItemCurrentId);
    if (response.responseDelete) {
      if (response.responseDelete.success) {
        modalConfirmDeleteFoodItem.value = !modalConfirmDeleteFoodItem.value;
        showToast("Xóa món thành công!", "success");
        foodItems.value = foodItems.value.filter(
          (item) => item.foodItemId != FoodItemCurrentId
        );
      } else {
        showToast("Xóa món thất bại!", "error");
      }
    } else {
      if (response.response.status == 403) {
        showToast(`Bạn không có quyền thao tác chức năng này!`, "warn");
        modalConfirmDeleteFoodItem.value = !modalConfirmDeleteFoodItem.value;
      }
    }
  }

  // <=== CHỨC NĂNG UPDATE ===>
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

    let FoodItemUpdateId = foodItemCurrentUpdate.foodItemId;

    let imageString = foodItemCurrentUpdate.imageUrl;
    if (foodItemCurrentUpdate.imageUrl instanceof File) {
      imageString = await convertFileToBase64(foodItemCurrentUpdate.imageUrl);
    }

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

    const response = await updateFood(requestData);

    if (response.responseUpdate) {
      if (response.responseUpdate.success) {
        const updatedFoodIndex = foodItems.value.findIndex(
          (item) => item.foodItemId === FoodItemUpdateId
        );
        if (updatedFoodIndex !== -1) {
          foodItems.value[updatedFoodIndex] = {
            ...foodItems.value[updatedFoodIndex],
            ...response.responseUpdate.data, // Giả sử server trả về object với thông tin đã cập nhật
            updateDate: response.responseUpdate.data.updateDate,
            imageUrl: imageString, // Cập nhật URL ảnh mới
          };
        }

        modalUpdateFoodItem.value = false;
        showToast("Cập nhật thành công!", "success");
      } else {
        showToast("Cập nhật thất bại!", "error");
      }
    } else {
      if (response.response.status == 403) {
        showToast(`Bạn không có quyền thao tác chức năng này!`, "warn");
        modalUpdateFoodItem.value = false;
      }
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
