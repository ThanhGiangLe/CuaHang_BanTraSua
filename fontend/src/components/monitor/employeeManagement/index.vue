<template>
  <div class="employeeManagement py-2 px-4 d-flex flex-column">
    <div class="employeeManagement_search d-flex align-center">
      <v-text-field
        class="me-12"
        append-inner-icon="mdi-magnify"
        density="compact"
        label="Nhập tên nhân viên cần tìm kiếm..."
        variant="solo"
        hide-details
        single-line
        v-model="search"
      ></v-text-field>
      <v-avatar color="info">
        <v-img src="/public/meo.jpg"></v-img>
      </v-avatar>
    </div>
    <div class="employeeManagement_heading mt-4 d-flex justify-space-between">
      <div class="employeeManagement_heading_countEmp">
        <h2>Số lượng nhân viên: {{ employeeList.length }}</h2>
      </div>
      <div class="employeeManagement_heading_filter-addEmp d-flex align-center">
        <!-- <v-btn class="bg-blue-lighten-3 me-6">
          <v-icon>mdi-filter-plus</v-icon>
          <span>Filter</span>
        </v-btn> -->
        <v-btn
          class="bg-orange-accent-4"
          @click="showDialogAddEmployee = !showDialogAddEmployee"
        >
          + Add Candidate
        </v-btn>
        <v-dialog
          v-model="showDialogAddEmployee"
          max-width="600px"
          max-height="600px"
          style="overflow-y: auto"
        >
          <v-card class="pa-2">
            <v-card-title> Thông tin nhân viên </v-card-title>
            <v-card-text style="padding: 8px 16px 0 16px">
              <v-text-field
                label="Tên nhân viên"
                v-model="employeeInfo.FullName"
              ></v-text-field>
              <v-text-field
                label="Số điện thoại"
                v-model="employeeInfo.Phone"
              ></v-text-field>
              <v-text-field
                label="Mật khẩu"
                v-model="employeeInfo.Password"
              ></v-text-field>
              <v-combobox
                label="Vị trí"
                :items="['Owner', 'Manager', 'Staff']"
                v-model="employeeInfo.Role"
                class="mb-6"
              ></v-combobox>
              <v-text-field
                label="Email"
                v-model="employeeInfo.Email"
              ></v-text-field>
              <v-text-field
                label="Địa chỉ"
                v-model="employeeInfo.Address"
              ></v-text-field>
            </v-card-text>
            <v-card-actions>
              <v-btn
                color="red darken-1"
                text
                @click="showDialogAddEmployee = !showDialogAddEmployee"
                >Hủy</v-btn
              >
              <v-btn color="green darken-1" text @click="addEmployee()"
                >Thêm</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
      </div>
    </div>
    <div
      class="employeeManagement_list flex-1-0 overflow-y-auto mt-2"
      style="max-height: 80%"
    >
      <v-container>
        <v-row>
          <v-col
            class="employeeManagement_list_item rounded-lg"
            cols="12"
            lg="6"
            md="6"
            sm="6"
            xs="12"
            v-for="employee in filterEmployeeList"
            :key="employee.userId"
          >
            <v-sheet
              class="pa-4 rounded-lg me-3"
              style="border: 1px solid #333; margin-left: -12px"
            >
              <div
                class="employeeManagement_list_item_title d-flex justify-space-between"
              >
                <div
                  class="employeeManagement_list_item_title_avt d-flex flex-column"
                >
                  <div
                    class="mb-1 employeeManagement_list_item_title_avt_img"
                    type="ON"
                  >
                    <v-img
                      :src="employee.avatar"
                      style="border-radius: 50%; margin: 2px"
                    ></v-img>
                  </div>
                  <span>Name: {{ employee.fullName }}</span>
                  <span>Position: {{ employee.role }}</span>
                </div>
                <v-menu>
                  <template v-slot:activator="{ props }">
                    <div
                      class="employeeManagement_list_item_title_other text-center cursor-pointer"
                      style="font-size: 24px; font-weight: bold"
                      v-bind="props"
                    >
                      ...
                    </div>
                  </template>
                  <v-list>
                    <v-list-item
                      v-for="(item, index) in [
                        { title: 'Thay đổi thông tin' },
                        { title: 'Xóa nhân viên' },
                      ]"
                      :key="index"
                      :value="index"
                      class="rounded-lg"
                      @click="handleExtensionOfAccount(index, employee)"
                    >
                      <v-list-item-title>{{ item.title }}</v-list-item-title>
                    </v-list-item>
                  </v-list>
                </v-menu>
              </div>
              <div
                class="employeeManagement_list_item_info mt-2 pa-3 rounded-lg"
              >
                <div
                  class="employeeManagement_list_item_info_phone font-weight-regular"
                >
                  <v-icon class="me-2">mdi-phone-in-talk</v-icon>
                  <span>{{ employee.phone }}</span>
                </div>
                <div
                  class="employeeManagement_list_item_info_mail font-weight-regular"
                >
                  <v-icon class="me-2">mdi-email-outline</v-icon>
                  <span>{{ employee.email }}</span>
                </div>
                <div
                  class="employeeManagement_list_item_info_address font-weight-regular"
                >
                  <v-icon class="me-2">mdi-home-map-marker</v-icon>
                  <span>{{ employee.address }}</span>
                </div>
              </div>
            </v-sheet>
          </v-col>
        </v-row>
      </v-container>
      <div>
        <v-dialog persistent max-width="800" v-model="displayMonitorUpdateUser">
          <v-card class="pa-2">
            <v-card-title class="pb-0">
              Thông tin chi tiết
            </v-card-title>
            <v-card-text class="pa-3">
              <v-container>
                <v-row class="d-flex align-center">
                  <v-col cols="2" class="font-weight-medium text-grey-darken-1 pa-1">
                    Họ tên:
                  </v-col>
                  <v-col cols="9" class="pa-1">
                    <input type="text" v-model="employeeCurrentChoose.fullName" class="w-100 px-3 py-2"></input>
                  </v-col>
                </v-row>
                <v-row class="d-flex align-center">
                  <v-col cols="2" class="font-weight-medium text-grey-darken-1 pa-1">
                    Email:
                  </v-col>
                  <v-col cols="9" class="pa-1">
                    <input type="text" v-model="employeeCurrentChoose.email" class="w-100 px-3 py-2"></input>
                  </v-col>
                </v-row>
                <v-row class="d-flex align-center">
                  <v-col cols="2" class="font-weight-medium text-grey-darken-1 pa-1">
                    Địa chỉ:
                  </v-col>
                  <v-col cols="9" class="pa-1">
                    <input type="text" v-model="employeeCurrentChoose.address" class="w-100 px-3 py-2"></input>
                  </v-col>
                </v-row>
                <v-row class="d-flex align-center">
                  <v-col cols="2" class="font-weight-medium text-grey-darken-1 pa-1">
                    Số điện thoại:
                  </v-col>
                  <v-col cols="9" class="pa-1">
                    <input type="text" v-model="employeeCurrentChoose.phone" class="w-100 px-3 py-2"></input>
                  </v-col>
                </v-row>
                <v-row class="d-flex align-center">
                  <v-col>
                    <v-row class="d-flex align-center">
                      <v-col cols="2" class="font-weight-medium text-grey-darken-1 pa-1">
                        Vai trò:
                      </v-col>
                      <v-col cols="9" class="pa-1">
                        <v-combobox
                          :items="['Owner', 'Manager', 'Staff']"
                          v-model="employeeCurrentChoose.role"
                          outlined
                        ></v-combobox>
                      </v-col>
                    </v-row>
                  </v-col>
                </v-row>
                <v-row class="d-flex align-center">
                  <v-col cols="2" class="font-weight-medium text-grey-darken-1 pa-1">
                    Mật khẩu mới:
                  </v-col>
                  <v-col cols="9" class="pa-1">
                    <input type="text" v-model="employeeCurrentChoose.passwordOfUser" class="w-100 px-3 py-2" placeholder="..."></input>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions class="">
              <v-spacer></v-spacer>
              <v-btn text="Hủy" @click="displayMonitorUpdateUser = !displayMonitorUpdateUser"></v-btn>
              <v-btn text="Xác nhận" class="bg-orange-accent-3" @click="updateInfoUserByManager"></v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref } from "vue";
import axios from "axios";
import API_ENDPOINTS from "@/api/api.js";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

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
});
const displayMonitorUpdateUser = ref(false);
const employeeCurrentChoose = ref();

async function init() {
  const response = await axios.get(API_ENDPOINTS.GET_ALL_EMPLOYEES);
  employeeList.value = response.data;
  console.log("Danh sách nhân viên:", employeeList.value);
}

init();

async function addEmployee() {
  try {
    // Gửi yêu cầu lên server với FormData
    const response = await axios.post(API_ENDPOINTS.ADD_USER, {
      FullName: employeeInfo.value.FullName,
      Phone: employeeInfo.value.Phone,
      Email: employeeInfo.value.Email,
      Address: employeeInfo.value.Address,
      Password: employeeInfo.value.Password,
      Role: employeeInfo.value.Role,
    });

    // Kiểm tra phản hồi từ server
    if (response.data.success === -1) {
      toast.warn("Please provide all required information!", {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
    } else if (response.data.success === 0) {
      toast.error(
        "The user with the provided email or phone number already exists!",
        {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false, // Hiện thanh tiến trình
          closeOnClick: true, // Đóng khi nhấp vào thông báo
          pauseOnHover: true, // Dừng khi di chuột lên thông báo
          draggable: true, // Kéo thông báo
          progress: undefined, // Tiến độ (nếu có)
        }
      );
    } else if (response.data.success === 1) {
      toast.success("Add user successful!", {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
      const user = {
        address: response.data.address,
        avatar: response.data.avatar,
        email: response.data.email,
        fullName: response.data.fullName,
        password: response.data.password,
        phone: response.data.phone,
        role: response.data.role,
        userId: response.data.userId,
      };
      employeeList.value.push(user); // Thêm user mới vào danh sách hiển thị
      showDialogAddEmployee.value = false; // Đóng dialog
    } else {
      console.error("Failed to add user", response.data.message);
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

async function handleExtensionOfAccount(index, employee) {
  if (index === 0) {
    displayMonitorUpdateUser.value = !displayMonitorUpdateUser.value;
    employeeCurrentChoose.value = {
      ...employee,
      passwordOfUser: "",
    };
  } else if (index === 1) {
    // Xóa nhân viên
    try {
      const responseDel = await axios.delete(
        `${API_ENDPOINTS.DELETE_USER}/${employee.userId}`
      );
      if (responseDel.data.message === "Succes") {
        toast.success(
          `Đã xóa nhân viên: ${responseDel.data.userRomeve.fullName}`,
          {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: false, // Hiện thanh tiến trình
            closeOnClick: true, // Đóng khi nhấp vào thông báo
            pauseOnHover: true, // Dừng khi di chuột lên thông báo
            draggable: true, // Kéo thông báo
            progress: undefined, // Tiến độ (nếu có)
          }
        );
      }
      employeeList.value = employeeList.value.filter(
        (item) => item.userId != employee.userId
      );
    } catch (error) {
      toast.error(`Lỗi khi xóa nhân viên, ${error}`, {
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
}

async function updateInfoUserByManager() {
  console.log("Thông tin mới nè: ", employeeCurrentChoose.value);
  try{
    const response = await axios.post(API_ENDPOINTS.UPDATE_USER, {
      UserId: employeeCurrentChoose.value.userId,
      FullName: employeeCurrentChoose.value.fullName,
      Email: employeeCurrentChoose.value.email,
      Address: employeeCurrentChoose.value.address,
      Phone: employeeCurrentChoose.value.phone,
      Role: employeeCurrentChoose.value.role,
      NewPassword: employeeCurrentChoose.value.passwordOfUser
    });
    if(response.data.success === 1) {
      toast.success(`Cập nhật thành công`, {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
      displayMonitorUpdateUser.value = !displayMonitorUpdateUser.value;

      const updatedUserIndex = employeeList.value.findIndex((item) => item.userId === employeeCurrentChoose.value.userId);
      if (updatedUserIndex !== -1) {
          employeeList.value[updatedUserIndex] = {
            ...employeeList.value[updatedUserIndex],
            ...response.data.data, 
          };
        }
    }else {
      toast.error(`Lỗi API`, {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
    }
  }catch(error) {
    toast.error(`Lỗi cập nhật thông tin, ${error}`, {
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
</script>
