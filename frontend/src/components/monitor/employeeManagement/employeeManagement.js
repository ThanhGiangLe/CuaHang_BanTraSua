import { computed, onMounted, ref } from "vue";
import "vue3-toastify/dist/index.css";
import { showToast } from "@/styles/handmade";
import { useUserStore } from "@/stores/user.js";
import { employeeManagementHandler } from "/src/composables/employeeManagement/employeeManagementHandler.js";

export default function useEmployeeManagement() {
  const {
    getAllEmployee,
    addEmployee,
    deleteEmployee,
    updateEmployee,
    getScheduleByUserId,
    updateScheduleByUserId,
  } = employeeManagementHandler();
  const userStore = useUserStore();
  const user = computed(() => userStore.user);
  const search = ref("");
  const showDialogAddEmployee = ref(false);
  const employeeList = ref([]);
  const employeeInfo = ref({
    FullName: "",
    Phone: "",
    Email: "",
    Address: "",
    Password: "",
    Role: "",
    ImageUrl: "",
  });
  const displayMonitorUpdateUser = ref(false);
  const displayMonitorSchedule = ref(false);
  const displayMonitorDeleteUser = ref(false);
  const employeeCurrentChoose = ref();
  const employeeIdCurrentChoose = ref(-1);
  const scheduleOfUser = ref([]);
  const scheduleOfUserBackup = ref([]);
  const currentDay = ref(null);
  const shifs = ref({
    S: "Ca sáng",
    C: "Ca chiều",
    T: "Ca tối",
    C1: "Ca 1",
    C2: "Ca 2",
    O: "Nghỉ",
  });
  const swapModeSchedule = ref(true);
  const selectedDay = ref(-1);
  const selectedShift = ref(null);

  async function init() {
    const response = await getAllEmployee();
    employeeList.value = response;
  }

  onMounted(async () => {
    await init();
    const timeNow = new Date();
    currentDay.value = timeNow.getDate();
  });
  // :type="scheduleAllUser[index][currentDay -1].shiftCode != 'O' ? 'ON' : 'OFF'"
  const filterEmployeeList = computed(() => {
    if (!employeeList.value) {
      return [];
    }
    if (search.value.trim() == "") {
      return employeeList.value;
    }
    return employeeList.value.filter((e) => {
      return e.fullName?.toUpperCase().includes(search.value.toUpperCase());
    });
  });
  const formatDay = (day) =>
    day != null ? day.toString().padStart(2, "0") : "";

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
        employeeList.value.push(response.data); // Thêm user mới vào danh sách hiển thị
        showDialogAddEmployee.value = false; // Đóng dialog
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
    if (index === 0) {
      displayMonitorSchedule.value = true;
      employeeIdCurrentChoose.value = employee.userId;
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
  }

  function cancelUpdateSchedule() {
    swapModeSchedule.value = true;
    scheduleOfUser.value = JSON.parse(
      JSON.stringify(scheduleOfUserBackup.value)
    );
    selectedDay.value = -1;
    selectedShift.value = null;
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

  return {
    search,
    showDialogAddEmployee,
    employeeList,
    employeeInfo,
    displayMonitorUpdateUser,
    displayMonitorSchedule,
    displayMonitorDeleteUser,
    employeeCurrentChoose,
    filterEmployeeList,
    weeks,
    scheduleOfUser,
    shifs,
    swapModeSchedule,
    selectedDay,
    selectedShift,
    currentDay,

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
  };
}
