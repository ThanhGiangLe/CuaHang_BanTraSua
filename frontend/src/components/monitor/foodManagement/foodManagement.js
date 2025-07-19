import { ref } from "vue";
import axios from "axios";
import { computed } from "vue";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import { useUserStore } from "@/stores/user.js";
import _ from "underscore";
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

  const categoriesDataDefault = ref([]);
  const isMainDefault = {
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
    point: "",
  });
  function getIdByNameCategory(categoryName) {
    const category = categoriesDataDefault.value.find(
      (item) =>
        item.categoryName.toLowerCase().trim() ==
        categoryName.toLowerCase().trim()
    );
    return category ? category.categoryId : null;
  }
  function getIdByNameIsMain(name) {
    const cleanedName = name.trim();
    for (const [id, value] of Object.entries(isMainDefault)) {
      if (value === cleanedName) {
        return parseInt(id);
      }
    }
    return null;
  }
  const getNameByIdCategory = (categoryId) => {
    const cate = categoriesDataDefault.value.find(
      (item) => item.categoryId == categoryId
    );
    return cate ? cate.categoryName : "Không xác định";
  };
  const getNameByIdMain = (isMain) => {
    return isMainDefault[isMain] || "Không xác định";
  };
  function formatDate(dateTime) {
    const date = new Date(dateTime);
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
    foodItems.value = responseFood.listFood;

    const responseCate = await getAllCategory();
    foodCategories.value = responseCate.listCategory;
    categoriesDataDefault.value = foodCategories.value.map((item) => {
      return {
        categoryId: item.categoryId,
        categoryName: item.categoryName,
      };
    });
    loading.value = false;
  }
  init();

  function tonggleSelected(foodCategory) {
    if (listDashSelected.value.includes(foodCategory)) {
      listDashSelected.value = listDashSelected.value.filter(
        (s) => s.categoryId !== foodCategory.categoryId
      );
    } else {
      listDashSelected.value.push(foodCategory);
    }
  }
  function removeVietnameseTones(str) {
    return str
      .normalize("NFD") // tách dấu khỏi chữ
      .replace(/[\u0300-\u036f]/g, "") // xóa các dấu
      .replace(/đ/g, "d")
      .replace(/Đ/g, "D");
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

      const isSearchMatch = removeVietnameseTones(
        foodItem.foodName.toLowerCase()
      ).includes(removeVietnameseTones(search.value.toLowerCase()));

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
  function formatPoint(value) {
    if (value === null || value === undefined) return "0 Điểm";

    const numericValue = Number(value);
    if (isNaN(numericValue)) return "0 Điểm";

    return numericValue.toLocaleString("vi-VN") + " Điểm";
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
      point: "",
    };
  }
  async function saveFood() {
    foodAdd.value.categoryId = getIdByNameCategory(
      foodAdd.value.categoryIdString
    );
    foodAdd.value.isMain = getIdByNameIsMain(foodAdd.value.isMainString);

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
      point: foodAdd.value.point,
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
          createDate: response.responseCreate.createDate,
          createBy: response.responseCreate.createBy,
          updateDate: response.responseCreate.updateDate,
          updateBy: response.responseCreate.updateBy,
          isMain: response.responseCreate.isMain,
          point: response.responseCreate.point,
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
      } else if (response.response.status == 400) {
        showToast(`Món ăn thêm không hợp lệ!`, "warn");
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
      } else if (response.response.status == 404) {
        showToast(`Không tìm thấy món ăn cần xóa!`, "warn");
        modalConfirmDeleteFoodItem.value = !modalConfirmDeleteFoodItem.value;
      } else if (response.response.status == 500) {
        showToast(`Lỗi trong quá trình xử lý!`, "warn");
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
      categoryIdString: getNameByIdCategory(foodItem.categoryId),
      isMainString: getNameByIdMain(foodItem.isMain),
    };
    console.log("foodItemCurrentUpdate:", foodItemCurrentUpdate.value);
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
    foodItemCurrentUpdate.categoryId = getIdByNameCategory(
      foodItemCurrentUpdate.categoryIdString
    );
    foodItemCurrentUpdate.isMain = getIdByNameIsMain(
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
      point: foodItemCurrentUpdate.point,
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
      if (response.response.status == 400) {
        showToast("Dữ liễu gửi đi không hợp lệ!", "error");
      } else if (response.response.status == 403) {
        showToast(`Bạn không có quyền thao tác chức năng này!`, "warn");
        modalUpdateFoodItem.value = false;
      } else if (response.response.status == 404) {
        showToast(`Không tìm thấy món ăn!`, "warn");
        modalUpdateFoodItem.value = false;
      } else if (response.response.status == 500) {
        showToast(`Lỗi trong quá trình xử lý!`, "warn");
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
    categoriesDataDefault,

    // Methods
    formatDate,
    cancelSaveFood,
    saveFood,
    getNameByIdCategory,
    getNameByIdMain,
    tonggleSelected,
    formatCurrency,
    formatPoint,
    openDialogShowDeleteFoodItemSelected,
    confimDeleteFoodItem,
    openDialogShowUpdateFoodItemSelected,
    confirmUpdateFoodItem,
    cancelConfirmUpdateFoodItem,
  };
}
