import { ref, watch } from "vue";
import { computed } from "vue";
import "vue3-toastify/dist/index.css";
import { useUserStore } from "@/stores/user.js";
import { userOrderStore } from "@/stores/orderStore.js";
import { useShiftStore } from "/src/stores/useShiftStore.js";
import { showToast } from "@/styles/handmade";
import { orderFoodHandler } from "/src/composables/listFood/orderFoodHandler.js";
import { employeeManagementHandler } from "/src/composables/employeeManagement/employeeManagementHandler.js";
import _ from "underscore";
import emailjs from "emailjs-com";

export default function useFoodManagement() {
  const userStore = useUserStore();
  const {
    orderFood,
    orderDetail,
    getAllFood,
    getAllCategory,
    topBestSelling,
    additionPoint,
    getEmailByPhone,
  } = orderFoodHandler();
  const { getAllEmployee, OpenShift, CloseShift, getScheduleToDay } =
    employeeManagementHandler();
  const orderStore = userOrderStore();
  const shiftStore = useShiftStore();

  // Lấy dữ liệu
  const foodItems = ref([]);
  const foodCategories = ref([]);
  const listItemBestSelling = ref([]);
  const listFoodItemIdBestSelling = ref([]);

  const showDialogUpdate = ref(false);
  const currentDish = ref({});
  const search = ref("");
  const showDialogSelectedAdditionalFood = ref(false);
  const currentFoodItem = ref({});
  const visibleUpdateCurrentFoodSelected = ref(false);
  const showComponentAreaManagement = ref(false);
  const loading = shallowRef(true);
  const isShowQRCode = ref(false);
  const currentItemIsUpdate = ref(-1);

  // Các thông tin về mở/kết ca
  const swapButtonShift = ref(true);
  const isShowOpenShift = ref(false);
  const isShowCloseShift = ref(false);
  const inputOpenCashAmount = ref();
  const inputCloseCashAmount = ref();
  const inputAdjustmentAmount = ref();
  const inputAdjustmentReason = ref("");

  // Lấy thông tin người dùng từ store
  const user = computed(() => userStore.user);

  // Danh sách categories được chọn
  const listFoodCategorySelected = ref([]);
  const currentOrderClone = ref({});

  // Số điện thoại để cộng điểm
  const searchPhoneNumbers = ref(null);
  const allPhoneNumbers = ref([]);
  const filteredPhoneNumbers = ref([]);
  const searchPhoneKeyword = ref("");

  // Xác thực sử dụng điểm
  const isShowOTPPoints = ref(false); // Hiển thị form nhập mã OTP
  const visibleResendOtp = ref(false);
  const otpCode = ref(""); // Lưu OTP dùng để xác thực
  const enteredOtp = ref(""); // Lưu OTP dùng để xác thực
  const serviceID = "service_cojqzzb";
  const templateID = "template_bql1hr5";
  const publicKey = "YVFyP3Zy91mr0Jc5W";
  emailjs.init(publicKey);
  const otpSendTime = ref(null); // Lưu thời gian lúc gửi OTP
  const otpTimeout = 1 * 60 * 1000; // Thời gian 1 phút

  // Hàm chạy đầu tiên
  async function init() {
    loading.value = true;

    const responseFood = await getAllFood();
    foodItems.value = responseFood.listFood.data;

    const responseCate = await getAllCategory();
    foodCategories.value = responseCate.listCategory.data;

    const responseEmp = await getAllEmployee();
    allPhoneNumbers.value = responseEmp.map((item) => ({
      ...item,
      display: `${item.fullName == "" ? "/*Khách hàng*/" : item.fullName} - ${
        item.phone
      } - ${formatPoint(item.point)}`,
    }));
    filteredPhoneNumbers.value = JSON.parse(
      JSON.stringify(allPhoneNumbers.value)
    );

    const responseBestselling = await topBestSelling({
      Top: 5,
    });
    listItemBestSelling.value = responseBestselling.top;
    listFoodItemIdBestSelling.value = _.pluck(
      listItemBestSelling.value,
      "foodItemId"
    );

    const request = {
      UserId: user.value.userId,
    };
    const response = await getScheduleToDay(request);
    if (response?.result?.success) {
      inputOpenCashAmount.value = response?.result?.data?.openingCashAmount;
    }

    loading.value = false;
  }
  init();
  onMounted(() => {
    shiftStore.restoreFromStorage();
    swapButtonShift.value = !shiftStore.isShiftOpened;
  });

  function tonggleSelected(foodCategory) {
    if (listFoodCategorySelected.value.includes(foodCategory)) {
      listFoodCategorySelected.value = listFoodCategorySelected.value.filter(
        (s) => s.categoryId !== foodCategory.categoryId
      );
    } else {
      listFoodCategorySelected.value.push(foodCategory);
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
    const items = foodItems.value;
    if (!Array.isArray(foodItems.value)) {
      console.error("FoodItems is not an array:", foodItems.value);
      return [];
    }

    const filtered = items.filter((foodItem) => {
      const isMainFood = foodItem.isMain == 1;
      const isCategoryMatch =
        listFoodCategorySelected.value.length === 0 ||
        listFoodCategorySelected.value.some(
          (selected) => selected.categoryId === foodItem.categoryId
        );

      const isSearchMatch = removeVietnameseTones(
        foodItem.foodName.toLowerCase()
      ).includes(removeVietnameseTones(search.value.toLowerCase()));

      return isCategoryMatch && isSearchMatch && isMainFood;
    });

    return filtered.sort((a, b) => {
      const aIsBest = listFoodItemIdBestSelling.value.includes(a.foodItemId)
        ? 0
        : 1;
      const bIsBest = listFoodItemIdBestSelling.value.includes(b.foodItemId)
        ? 0
        : 1;
      return aIsBest - bIsBest;
    });
  });
  function getCurrentDateTimeForSQL() {
    return new Date().toISOString(); // Đã là giờ UTC chuẩn
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
    table_id: 1,
    total_amount: 0,
    status: "Paid",
    discount: 0,
    tax: 0,
    receivedAmount: 0,
    paymentMethod: "Tiền mặt",
    paymentType: "Tiền",
    items: [],
  });
  const returnedAmountCurrentOrder = computed(() => {
    const received = Number(currentOrder.value.receivedAmount * 1000 || 0);
    const total = Number(currentOrder.value.total_amount || 0);
    return received > total ? received - total : 0;
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
  const momoQRCodeUrl = computed(() => {
    const amount = resultTotalAmount.value || 0;
    return `https://img.vietqr.io/image/TCB-192602032003-compact2.png?amount=${amount}&addInfo=${encodeURIComponent(
      "TraSuaSunSun"
    )}`;
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
  const resetCurrentOrder = () => {
    currentOrder.value = {
      user_id: user.value.userId,
      order_time: getCurrentDateTimeForSQL(),
      table_id: 1,
      total_amount: 0, // Reset lại tổng thanh toán
      status: "Paid", // Trạng thái mặc định
      discount: 0, // Reset giảm giá
      tax: 0, // Reset thuế
      receivedAmount: 0,
      items: [], // Reset danh sách các món ăn
    };
  };

  // <== DIALOG ĐỂ CHỌN MÓN ==>
  const openDialogShowDetail = (foodItem) => {
    resetOrderItem();

    const additionalFoods = foodItems.value
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
      ListAdditionalFood: additionalFoods,
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
      ListAdditionalFood: additionalFoods,
      ListAdditionalFoodSelected: [],
      IsMain: foodItem.IsMain,
      Note: "",
    };

    showDialogSelectedAdditionalFood.value = true;
  };

  // <== THÊM MÓN VÀO GIỎ ==>
  function isEqualsListAdditionalFood(list1, list2) {
    if (list1.length != list2.length) return false;
    const sort1 = [...list1].sort((a, b) => a.foodItemId - b.foodItemId);
    const sort2 = [...list2].sort((a, b) => a.foodItemId - b.foodItemId);

    return sort1.every(
      (item, index) => item.foodItemId === sort2[index].foodItemId
    );
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

    const existingItem = currentOrder.value.items.find(
      (item) =>
        item.FoodItemId == resultOrderItem.value.FoodItemId &&
        item.FoodName == resultOrderItem.value.FoodName &&
        item.Note == resultOrderItem.value.Note &&
        isEqualsListAdditionalFood(
          item.ListAdditionalFood,
          resultOrderItem.value.ListAdditionalFood
        )
    );

    if (existingItem) {
      existingItem.Quantity += resultOrderItem.value.Quantity;
    } else {
      currentOrder.value.items.push({ ...resultOrderItem.value });
    }

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

  // <== DELETE MÓN ĐÃ CHỌN ==>
  function deleteCurrentFoodSelected(index) {
    if (index !== -1) {
      // Xóa món ăn tại vị trí tìm được
      currentOrder.value.items.splice(index, 1);

      // Cập nhật lại tổng tiền
      updateTotalAmount();
    }
  }

  // <== UPDATE MÓN ĐÃ CHỌN ==>
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
    const allAdditionalFoods = foodItems.value
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

  // <== ĐẶT MÓN ==>
  const processOrder = async () => {
    if (currentOrder.value.items.length === 0) {
      showToast("Hãy chọn món cần phục vụ!", "warn");
      return;
    }

    const orderTimeCurrent = getCurrentDateTimeForSQL();
    const request = {
      userId: currentOrder.value.user_id,
      orderTime: orderTimeCurrent,
      tableId: currentOrder.value.table_id,
      totalAmount: currentOrder.value.total_amount,
      totalResult: resultTotalAmount.value,
      status: currentOrder.value.status,
      discount: currentOrder.value.discount,
      tax: currentOrder.value.tax,
      paymentMethod: currentOrder.value.paymentMethod,
      phone: searchPhoneNumbers.value ?? "",
      receivedAmount: currentOrder.value.receivedAmount * 1000 || 0,
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
        showToast("Đặt món thành công!", "success");
        searchPhoneNumbers.value = null;
        resetCurrentOrder();
        setTimeout(() => window.location.reload(), 3200);
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
      currentOrder.value.items.map(async (item) => {
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

  async function callApiOrderFood() {
    if (currentOrder.value.paymentMethod === "Điểm") {
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
      if (currentOrder.value.paymentMethod === "Tiền mặt") {
        if (currentOrder.value.receivedAmount == 0) {
          showToast("Nhập số tiền nhận từ khách!", "warn");
          return;
        }
      }
      await processOrder();
    }
  }

  // <== ĐẶT MÓN VÀ CHỌN BÀN ==>
  async function callApiOrderFoodAndAddTable() {
    if (currentOrder.value.items.length == 0) {
      showToast("Hãy chọn món cần phục vụ!");
    } else {
      showComponentAreaManagement.value = !showComponentAreaManagement.value;
      currentOrderClone.value = {
        ...currentOrder.value,
      };

      orderStore.setSelectedDishes(currentOrderClone.value);
    }
  }
  const handleCloseComponentAreaManagement = () => {
    showComponentAreaManagement.value = !showComponentAreaManagement.value;
    orderStore.clearSelectedDishes();
  };
  function handleCloseAndReset() {
    resetCurrentOrder();
    showComponentAreaManagement.value = !showComponentAreaManagement.value;
  }

  // <== MỞ/KẾT CA ==>
  function showOpenShift() {
    swapButtonShift.value = false;
    isShowOpenShift.value = true;
  }
  function showCloseShift() {
    swapButtonShift.value = true;
    isShowCloseShift.value = true;
  }
  function cancelConfirmOpenShift() {
    isShowOpenShift.value = false;
    swapButtonShift.value = true;
  }
  function cancelCofirmCloseShift() {
    isShowCloseShift.value = false;
    inputCloseCashAmount.value = 0;
    swapButtonShift.value = false;
  }
  async function confirmOpenShift() {
    const request = {
      UserId: user.value.userId,
      OpeningCashAmount: inputOpenCashAmount.value,
    };
    const response = await OpenShift(request);
    if (response.result) {
      if (response.result.success) {
        shiftStore.openShiftState(user.value.userId, inputOpenCashAmount.value);
        showToast("Mở ca thành công!", "success");
        isShowOpenShift.value = false;
        swapButtonShift.value = false;
      }
    } else {
      if (response.response.status == 400) {
        showToast("Có lỗi trong dữ liệu gửi đi!", "error");
      } else if ((response.response.status = 404)) {
        showToast(`${response.response.data}`, "warn");
      } else {
        showToast("Lỗi hệ thống!", "error");
      }
    }
  }
  async function confirmCloseShift() {
    if (!inputCloseCashAmount.value) {
      showToast("Vui lòng nhập số tiền kết ca!", "warn");
      return;
    }
    const request = {
      UserId: user.value.userId,
      ClosingCashAmount: inputCloseCashAmount.value * 1000,
      AdjustmentAmount: inputAdjustmentAmount.value,
      AdjustmentReason: inputAdjustmentReason.value,
    };
    const response = await CloseShift(request);
    if (
      response.result.success == 1 ||
      response.result.success == -1 ||
      response.result.success == 0
    ) {
      if (response.result.success == 0) {
        showToast("Kết ca thành công!", "success");
        isShowCloseShift.value = false;
        swapButtonShift.value = true;
      } else if (response.result.success == -1) {
        showToast(
          `Số tiền kết ca ít hơn hệ thống ghi nhận là: ${formatCurrency(
            response.result.difference
          )}`,
          "warn"
        );
        setTimeout(() => showToast("Kết ca thành công!", "success"), 3100);
        isShowCloseShift.value = false;
        swapButtonShift.value = true;
      } else {
        showToast(
          `Hình như: ${formatCurrency(
            response.result.difference
          )} là tiền tip của bạn đó...`
        );
        setTimeout(() => showToast("Kết ca thành công!", "success"), 3100);
        isShowCloseShift.value = false;
        swapButtonShift.value = true;
      }
      shiftStore.closeShiftState();
      setTimeout(() => window.location.reload(), 3100);
    } else {
      if (response.response.status == 400) {
        showToast("Có lỗi trong dữ liệu gửi đi!", "error");
      } else if ((response.response.status = 404)) {
        showToast(`${response.response.data}`, "warn");
      } else {
        showToast("Lỗi hệ thống!", "error");
      }
    }
  }

  return {
    // State variables
    showDialogUpdate,
    currentDish,
    foodCategories,
    foodItems,
    search,
    showDialogSelectedAdditionalFood,
    currentFoodItem,
    visibleUpdateCurrentFoodSelected,
    showComponentAreaManagement,
    loading,
    isShowQRCode,

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
    returnedAmountCurrentOrder,
    momoQRCodeUrl,
    listFoodItemIdBestSelling,
    searchPhoneNumbers,
    filteredPhoneNumbers,
    searchPhoneKeyword,
    isShowOTPPoints,
    enteredOtp,
    visibleResendOtp,
    swapButtonShift,
    isShowOpenShift,
    isShowCloseShift,
    inputOpenCashAmount,
    inputCloseCashAmount,
    inputAdjustmentAmount,
    inputAdjustmentReason,

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
    formatPoint,
    openDialogShowDetail,
    updateCurrentFoodSelected,
    updateFoodItem,
    deleteCurrentFoodSelected,
    handleCloseComponentAreaManagement,
    handleCloseAndReset,
    resetCurrentOrder,
    callApiOrderFood,
    callApiOrderFoodAndAddTable,
    sendOTP,
    verifyOTP,
    showOpenShift,
    showCloseShift,
    cancelConfirmOpenShift,
    cancelCofirmCloseShift,
    confirmOpenShift,
    confirmCloseShift,
  };
}
