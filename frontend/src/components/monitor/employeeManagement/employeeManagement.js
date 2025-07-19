import { computed, onMounted, ref } from "vue";
import "vue3-toastify/dist/index.css";
import { showToast } from "@/styles/handmade";
import { useUserStore } from "@/stores/user.js";
import { employeeManagementHandler } from "/src/composables/employeeManagement/employeeManagementHandler.js";
import emailjs from "emailjs-com";

export default function useEmployeeManagement() {
  const {
    getAllEmployee,
    addEmployee,
    deleteEmployee,
    updateEmployee,
    getScheduleByUserId,
    updateScheduleByUserId,
    swapScheduleShift,
  } = employeeManagementHandler();
  const userStore = useUserStore();
  const user = computed(() => userStore.user);
  const search = ref("");
  const showDialogAddEmployee = ref(false);
  const employeeList = ref([]);

  const displayMonitorUpdateUser = ref(false);
  const displayMonitorSchedule = ref(false);
  const displayMonitorDeleteUser = ref(false);
  const employeeCurrentChoose = ref();
  const employeeIdCurrentChoose = ref(-1);
  const employeeNameCurrentChoose = ref("");
  const scheduleOfUser = ref([]);
  const scheduleOfUserBackup = ref([]);
  const currentDay = ref(null);
  const selectedRole = ref("");
  const listRoleDefault = ref([
    "Chủ cửa hàng",
    "Quản lý",
    "Nhân viên",
    "Khách hàng",
    "Tất cả",
  ]);
  const shifs = ref({
    S: "Ca sáng",
    C: "Ca chiều",
    T: "Ca tối",
    C1: "Ca 1",
    C2: "Ca 2",
    O: "Nghỉ",
  });
  const employeeInfo = ref({
    FullName: "",
    Phone: "",
    Email: "",
    Address: "",
    Password: "",
    Role: "",
    ImageUrl: "",
  });
  const swapModeSchedule = ref(true);
  const selectedDay = ref(-1);
  const selectedShift = ref(null);

  const isShowListEmployeeSwapSchedule = ref(false);
  const isConfirmSelectedEmployeeSwapSchedule = ref(false);
  const employeeSelectedSwapSchedule = ref(null);

  // Thông tin tài khoản EmailJS
  const serviceID = "service_cojqzzb";
  const templateID = "template_yykkt9a";
  const publicKey = "YVFyP3Zy91mr0Jc5W";
  emailjs.init(publicKey);

  async function init() {
    const response = await getAllEmployee();
    if (
      user.value.role.toLowerCase().trim() == "chủ cửa hàng" ||
      user.value.role.toLowerCase().trim() == "quản lý"
    ) {
      employeeList.value = response;
    } else {
      employeeList.value = response.filter(
        (item) => item.userId == user.value.userId
      );
    }
  }
  init();
  const filterEmployeeList = computed(() => {
    if (selectedRole.value.toLowerCase() == "tất cả") {
      return employeeList.value;
    }
    return employeeList.value.filter((e) => {
      const isSearchMatch = e.fullName
        .toLowerCase()
        .includes(search.value.toLowerCase());
      const isMatchRole =
        !selectedRole.value ||
        selectedRole.value.toLowerCase() == e.role.toLowerCase();
      return isSearchMatch && isMatchRole;
    });
  });
  const formatDay = (day) =>
    day != null ? day.toString().padStart(2, "0") : "";
  const emailRules = [
    (v) => !!v || "Email là bắt buộc",
    (v) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v) || "Email không hợp lệ",
  ];
  function filterAccountByRole(role) {
    console.log("role: ", role);
    selectedRole.value = role;
  }
  // <== Thêm nhân viên ==>
  function cancelAddEmployee() {
    showDialogAddEmployee.value = false;
    employeeInfo.value = {
      FullName: "",
      Phone: "",
      Email: "",
      Address: "",
      Password: "",
      Role: "",
      ImageUrl: "",
    };
  }
  function convertFileToBase64(file) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = (error) => reject(error);
    });
  }

  async function sendMailRegisterAccount(email) {
    try {
      const templateParams = {
        email: email,
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
      showToast("Lỗi trong quá trình tạo tài khoản.", "error");
    }
  }
  async function handleAddEmployee() {
    let imageString = employeeInfo.value.ImageUrl;
    if (employeeInfo.value.ImageUrl instanceof File) {
      imageString = await convertFileToBase64(employeeInfo.value.ImageUrl);
    }
    const request = {
      FullName: employeeInfo.value.FullName,
      Phone: employeeInfo.value.Phone,
      Email: employeeInfo.value.Email,
      Address: employeeInfo.value.Address,
      Password: employeeInfo.value.Password,
      Role: employeeInfo.value.Role,
      Avatar: imageString,
      CreateBy: user.value.fullName,
      UpdateBy: user.value.fullName,
    };
    const response = await addEmployee(request);
    console.log("Response: ", response);
    if (
      response.success == -1 ||
      response.success == 0 ||
      response.success == 1
    ) {
      if (response.success == -1) {
        showToast("Thông tin không hợp lệ!", "warn");
      } else if (response.success == 0) {
        showToast("Số điện thoại hoặc email đã được dùng!", "warn");
      } else if (response.success == 1) {
        showToast("Thêm thành công!", "success");
        sendMailRegisterAccount(response.data.email);
        console.log("response: ", response);
        if (
          !employeeList.value.some((e) => e.userId === response.data.userId)
        ) {
          employeeList.value.push(response.data);
        }
        showDialogAddEmployee.value = false;
        employeeInfo.value = {
          FullName: "",
          Phone: "",
          Email: "",
          Address: "",
          Password: "",
          Role: "",
          ImageUrl: "",
        };
      }
    } else {
      if (response.response.status == 403) {
        showToast(`Bạn không có quyền thao tác chức năng này!`, "warn");
        showDialogAddEmployee.value = false;
      } else if (response.response.status == 500) {
        showToast("Thêm thất bại!", "error");
      }
    }
  }

  // <== Chọn các thao tác khác ==>
  async function handleExtensionOfAccount(index, employee, indexEmp) {
    console.log("Employee: ", employee);
    if (index === 0) {
      if (employee.role.toLowerCase().trim() == "khách hàng") {
        showToast("Tính năng này không áp dụng cho tài khoản khách hàng");
        return;
      }
      displayMonitorSchedule.value = true;
      employeeIdCurrentChoose.value = employee.userId;
      employeeNameCurrentChoose.value = employee.fullName;
      const request = {
        UserId: employee.userId,
      };
      const response = await getScheduleByUserId(request);
      scheduleOfUser.value = JSON.parse(JSON.stringify(response));
      scheduleOfUserBackup.value = JSON.parse(JSON.stringify(response));
    } else if (index === 1) {
      displayMonitorUpdateUser.value = true;
      employeeCurrentChoose.value = {
        ...employee,
        passwordOfUser: "",
        imageUrl: employee.avatar,
      };
    } else if (index === 2) {
      displayMonitorDeleteUser.value = true;
      employeeIdCurrentChoose.value = employee.userId;
    }
  }

  // <== Cập nhật thông tin nhân viên ==>
  async function updateInfoUserByManager() {
    let imageString = employeeCurrentChoose.value.imageUrl;
    if (employeeCurrentChoose.value.imageUrl instanceof File) {
      imageString = await convertFileToBase64(
        employeeCurrentChoose.value.imageUrl
      );
    }
    const request = {
      UserId: employeeCurrentChoose.value.userId,
      FullName: employeeCurrentChoose.value.fullName,
      Email: employeeCurrentChoose.value.email,
      Address: employeeCurrentChoose.value.address,
      Phone: employeeCurrentChoose.value.phone,
      Role: employeeCurrentChoose.value.role,
      NewPassword: employeeCurrentChoose.value.passwordOfUser,
      Avatar: imageString,
      UpdateBy: user.value.fullName,
    };
    console.log("request: ", request);
    const response = await updateEmployee(request);
    console.log("response: ", response);
    if (response.success === 1) {
      showToast("Cập nhật thành công!", "success");
      displayMonitorUpdateUser.value = !displayMonitorUpdateUser.value;
      const updatedUserIndex = employeeList.value.findIndex(
        (item) => item.userId === employeeCurrentChoose.value.userId
      );
      if (updatedUserIndex !== -1) {
        employeeList.value[updatedUserIndex] = {
          ...employeeList.value[updatedUserIndex],
          ...response.data,
        };
      }
    } else {
      if (response.response.status === 400) {
        showToast("Thông tin cập nhật không hợp lệ!", "warn");
      } else if (response.response.status == 403) {
        showToast(`Bạn không có quyền thao tác chức năng này!`, "warn");
        displayMonitorUpdateUser.value = false;
      } else if (response.response.status === 404) {
        showToast("Không tìm thấy thông tin nhân viên!", "warn");
      } else if (response.response.status == 500) {
        showToast("Cập nhật thất bại!", "error");
      }
    }
  }

  // <== Xóa nhân viên ==>
  async function deleteUserByManager() {
    const responseDel = await deleteEmployee(employeeIdCurrentChoose.value);
    if (responseDel.message == "Succes") {
      showToast(`Đã xóa nhân viên: ${responseDel.userRomeve.fullName}`);
      employeeList.value = employeeList.value.filter(
        (item) => item.userId != employeeIdCurrentChoose.value
      );
      displayMonitorDeleteUser.value = false;
      employeeIdCurrentChoose.value = -1;
    } else {
      if (responseDel.response.status == 403) {
        showToast("Bạn không có quyền thao tác chức năng này!", "warn");
      } else if (responseDel.response.status == 404) {
        showToast("Không tìm thấy nhân viên cần xóa", "error");
      } else {
        showToast("Có lỗi trong quá trình xử lý!", "error");
      }
    }
  }

  // <== Phần lịch trực nhân viên ==>
  const timeNow = new Date();
  const currentYear = timeNow.getFullYear();
  const currentMonth = timeNow.getMonth(); // Tính từ tháng 0 -> 11
  currentDay.value = timeNow.getDate();
  const daysInCurrentMonth = new Date(
    currentYear,
    currentMonth + 1,
    0
  ).getDate();

  const weeks = computed(() => {
    const result = [];
    let week = new Array(7).fill(null);

    const startDay = new Date(currentYear, currentMonth, 1).getDay(); // Ngày đầu tiên sẽ thuộc thứ mấy
    const startIndex = startDay === 0 ? 6 : startDay - 1;

    let dayCounter = 1;

    // Fill in the first week
    for (let i = startIndex; i < 7 && dayCounter <= daysInCurrentMonth; i++) {
      week[i] = dayCounter++;
    }
    result.push([...week]);

    // Fill the rest of the weeks
    while (dayCounter <= daysInCurrentMonth) {
      week = new Array(7).fill(null);
      for (let i = 0; i < 7 && dayCounter <= daysInCurrentMonth; i++) {
        week[i] = dayCounter++;
      }
      result.push([...week]);
    }
    return result;
  });

  function handleClickItemScheduleTable(day, index) {
    selectedDay.value = day;
    selectedShift.value = scheduleOfUser.value[selectedDay.value - 1].shiftCode;
  }

  function handleClickItemShift(shiftCode, shift) {
    selectedShift.value = shiftCode;
    scheduleOfUser.value[selectedDay.value - 1].shiftCode = shiftCode;
  }

  function isShiftChanged(day) {
    return (
      JSON.stringify(scheduleOfUserBackup.value[day - 1]) !=
      JSON.stringify(scheduleOfUser.value[day - 1])
    );
  }

  function closeDisplayMonitorSchedule() {
    displayMonitorSchedule.value = false;
    selectedDay.value = -1;
    selectedShift.value = null;
    swapModeSchedule.value = true;
    isShowListEmployeeSwapSchedule.value = false;
  }

  function cancelUpdateSchedule() {
    swapModeSchedule.value = true;
    scheduleOfUser.value = JSON.parse(
      JSON.stringify(scheduleOfUserBackup.value)
    );
    selectedDay.value = -1;
    selectedShift.value = null;
    isShowListEmployeeSwapSchedule.value = false;
  }

  async function handleUpdateSchedule() {
    // const cleanSchedule = scheduleOfUser.value.map((item) => {
    //   return {
    //     day: item.day.toString().padStart(2, "0"),
    //     shiftCode: item.shiftCode,
    //   };
    // });
    var request = {
      UserId: employeeIdCurrentChoose.value,
      UpdateBy: user.value.fullName,
      Year: currentYear,
      Month: currentMonth + 1,
      Schedules: scheduleOfUser.value,
    };
    const response = await updateScheduleByUserId(request);
    if (response.message == "Cập nhật lịch làm việc thành công") {
      showToast(`${response.message}`, "success");
      swapModeSchedule.value = true;
      selectedDay.value = -1;
      selectedShift.value = null;
      scheduleOfUserBackup.value = JSON.parse(
        JSON.stringify(scheduleOfUser.value)
      );
    }
  }

  function showListEmployee() {
    isShowListEmployeeSwapSchedule.value = true;
  }

  function confirmSelectedEmployee(emp) {
    console.log("emp: ", emp);
    isConfirmSelectedEmployeeSwapSchedule.value = true;
    employeeSelectedSwapSchedule.value = emp;
  }
  async function swapSchedule() {
    const request = {
      FromUserId: employeeIdCurrentChoose.value,
      ToUserId: employeeSelectedSwapSchedule.value.userId,
      Year: currentYear,
      Month: currentMonth + 1,
      Day: selectedDay.value,
      UpdateBy: user.value.fullName,
    };
    const response = await swapScheduleShift(request);
    if (response.success) {
      showToast("Thay đổi lịch làm việc thành công!", "success");
      // Ẩn form xác nhận và listEmployee
      isConfirmSelectedEmployeeSwapSchedule.value = false;
      isShowListEmployeeSwapSchedule.value = false;
      employeeSelectedSwapSchedule.value = null;
      // Cập nhật hiển thị
      scheduleOfUser.value[selectedDay.value - 1].shiftCode =
        response.data.newShiftCodeFromUser;
      scheduleOfUserBackup.value = JSON.parse(
        JSON.stringify(scheduleOfUser.value)
      );
      // Reset
      selectedDay.value = -1;
      selectedShift.value = null;
    } else {
      if (response.response.status == 403) {
        showToast("Bạn không có quyền thực hiện thao tác này!", "warn");
      } else if (response.response.status == 400) {
        showToast("Thông tin gửi đi không hợp lệ!", "error");
      } else if (response.response.status == 404) {
        showToast(`${response.response.data}`, "error");
      } else {
        showToast("Có lỗi trong quá trình xử lý!", "error");
      }
    }
  }
  function cancelSwapSchedule() {
    isConfirmSelectedEmployeeSwapSchedule.value = false;
    isShowListEmployeeSwapSchedule.value = false;
    employeeSelectedSwapSchedule.value = null;
  }

  return {
    user,
    search,
    showDialogAddEmployee,
    employeeList,
    employeeInfo,
    displayMonitorUpdateUser,
    displayMonitorSchedule,
    displayMonitorDeleteUser,
    employeeCurrentChoose,
    employeeNameCurrentChoose,
    filterEmployeeList,
    weeks,
    scheduleOfUser,
    shifs,
    swapModeSchedule,
    selectedDay,
    selectedShift,
    currentDay,
    isShowListEmployeeSwapSchedule,
    employeeSelectedSwapSchedule,
    isConfirmSelectedEmployeeSwapSchedule,
    emailRules,
    selectedRole,
    listRoleDefault,

    formatDay,
    cancelAddEmployee,
    handleAddEmployee,
    deleteUserByManager,
    handleExtensionOfAccount,
    updateInfoUserByManager,
    cancelUpdateSchedule,
    handleUpdateSchedule,
    handleClickItemScheduleTable,
    handleClickItemShift,
    isShiftChanged,
    closeDisplayMonitorSchedule,
    showListEmployee,
    confirmSelectedEmployee,
    swapSchedule,
    cancelSwapSchedule,
    filterAccountByRole,
  };
}
