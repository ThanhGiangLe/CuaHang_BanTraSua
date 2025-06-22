<template>
  <div class="employeeManagement py-2 px-4 d-flex flex-column">
    <div class="employeeManagement_search d-flex align-center">
      <v-text-field class="me-6" append-inner-icon="mdi-magnify" density="compact"
        label="Nhập tên tài khoản hoặc số điện thoại cần tìm kiếm..." variant="solo" hide-details single-line
        v-model="search"></v-text-field>
      <v-avatar color="info" style="width: 60px; height: 60px;">
        <v-img :src="user.avatar"></v-img>
      </v-avatar>
    </div>
    <div class="employeeManagement_heading mt-4 d-flex justify-space-between align-center">
      <div class="employeeManagement_heading_countEmp d-flex">
        <h2 class="me-2">Số lượng tài khoản: {{ employeeList.length }}</h2>
        <v-menu>
          <template v-slot:activator="{ props }">
            <v-btn v-bind="props" append-icon="mdi-chevron-down" min-width="150px" style="border: 1px solid #333"
              size="small" class="ms-2">
              {{ selectedRole || "Vai trò" }}
            </v-btn>
          </template>
          <v-list max-height="200px" style="overflow-y: auto">
            <v-list-item v-for="role in listRoleDefault" :key="role" :value="role" style="min-height: 36px !important"
              @click="filterAccountByRole(role)">
              <v-list-item-title>{{
                role
              }}</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </div>
      <div class="employeeManagement_heading_filter-addEmp d-flex align-center">
        <v-btn class="bg-orange-accent-4" @click="showDialogAddEmployee = !showDialogAddEmployee">
          + Thêm tài khoản
        </v-btn>
        <!-- Form thêm nhân viên -->
        <v-dialog v-model="showDialogAddEmployee" max-width="600px" max-height="660px" style="overflow-y: auto">
          <v-card class="pa-2">
            <v-card-title> Thông tin tài khoản </v-card-title>
            <v-card-text style="padding: 8px 16px 0 16px" class="form_register_account">
              <v-text-field label="Tên tài khoản" v-model="employeeInfo.FullName" :rules="[
                v => !!v || 'Tên tài khoản buộc nhập',
                v => v.length >= 3 || 'Ít nhất 3 ký tự'
              ]"></v-text-field>
              <v-text-field label="Số điện thoại" v-model="employeeInfo.Phone" :rules="[
                v => !!v || 'Số điện thoại buộc nhập',
                v => v.length >= 3 || 'Ít nhất 10 ký tự'
              ]"></v-text-field>
              <v-text-field label="Mật khẩu" v-model="employeeInfo.Password"></v-text-field>
              <!-- <v-radio-group v-model="employeeInfo.Role" inline>
                <v-radio label="Chủ cửa hàng" value="Chủ cửa hàng"></v-radio>
                <v-radio label="Quản lý" value="Quản lý"></v-radio>
                <v-radio label="Nhân viên" value="Nhân viên"></v-radio>
                <v-radio label="Khách hàng" value="Khách hàng"></v-radio>
              </v-radio-group> -->
              <v-combobox label="Vị trí" :items="['Chủ cửa hàng', 'Quản lý', 'Nhân viên', 'Khách hàng']"
                v-model="employeeInfo.Role" class="mb-6 form_register_account-combobox"></v-combobox>
              <v-text-field label="Email" v-model="employeeInfo.Email" :rules="emailRules"></v-text-field>
              <v-text-field label="Địa chỉ" v-model="employeeInfo.Address"></v-text-field>
              <v-file-input label="Ảnh tài khoản" accept="image/*" v-model="employeeInfo.ImageUrl"
                prepend-icon="mdi-camera" :show-size="true"></v-file-input>
            </v-card-text>
            <v-card-actions>
              <v-btn color="red darken-1" text @click="cancelAddEmployee()">Hủy</v-btn>
              <v-btn color="green darken-1" text @click="handleAddEmployee()">Thêm</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </div>
    </div>

    <!-- Show danh sách nhân viên hiện tại -->
    <div class="employeeManagement_list flex-1-0 overflow-y-auto mt-2" style="max-height: 80%">
      <v-container>
        <v-row>
          <v-col class="employeeManagement_list_item rounded-lg" cols="12" lg="6" md="6" sm="6" xs="12"
            v-for="(employee, indexEmp) in filterEmployeeList" :key="JSON.stringify(employee) + index">
            <v-sheet class="pa-4 rounded-lg me-3" style="border: 1px solid #333; margin-left: -12px">
              <div class="employeeManagement_list_item_title d-flex justify-space-between">
                <div class="employeeManagement_list_item_title_avt d-flex align-center">
                  <!-- v-if="scheduleAllUser[indexEmp] && scheduleAllUser[indexEmp][currentDay - 1]"
                    :type="scheduleAllUser[indexEmp][currentDay - 1].shiftCode != 'O' ? 'ON' : 'OFF'" -->
                  <div class="mb-1 employeeManagement_list_item_title_avt_img" type="ON">
                    <v-img :src="employee.avatar" style="border-radius: 50%; width: 100%; height: 100%;"></v-img>
                  </div>
                  <div class="d-flex flex-column ms-2">
                    <span>Họ tên: <strong>{{ employee.fullName }}</strong></span>
                    <span>Vị trí: {{ employee.role }}</span>
                  </div>
                </div>
                <v-menu>
                  <template v-slot:activator="{ props }">
                    <div class="employeeManagement_list_item_title_other text-center cursor-pointer"
                      style="font-size: 24px; font-weight: bold" v-bind="props">
                      ...
                    </div>
                  </template>
                  <v-list>
                    <v-list-item v-for="(item, index) in [
                      { title: 'Xem lịch làm việc' },
                      { title: 'Thay đổi thông tin' },
                      { title: 'Xóa nhân viên' },
                    ]" :key="index" :value="index" class="rounded-lg"
                      @click="handleExtensionOfAccount(index, employee, indexEmp)">
                      <v-list-item-title>{{ item.title }}</v-list-item-title>
                    </v-list-item>
                  </v-list>
                </v-menu>
              </div>
              <div class="employeeManagement_list_item_info mt-2 pa-3 rounded-lg">
                <div class="employeeManagement_list_item_info_phone font-weight-regular">
                  <v-icon class="me-2">mdi-phone-in-talk</v-icon>
                  <span>{{ employee.phone }}</span>
                </div>
                <div class="employeeManagement_list_item_info_mail font-weight-regular">
                  <v-icon class="me-2">mdi-email-outline</v-icon>
                  <span>{{ employee.email }}</span>
                </div>
                <div class="employeeManagement_list_item_info_address font-weight-regular hiddent-text-one-line">
                  <v-icon class="me-2">mdi-home-map-marker</v-icon>
                  <span>{{ employee.address }}</span>
                </div>
              </div>
            </v-sheet>
          </v-col>
        </v-row>
      </v-container>
    </div>

    <!-- Form delete nhân viên -->
    <div>
      <v-dialog persistent max-width="500" v-model="displayMonitorDeleteUser">
        <v-card class="pa-2">
          <v-card-title class="pb-0 mt-1">
            Bạn có chắc chắn muốn xóa nhân viên này?
          </v-card-title>
          <v-card-actions class="mt-4">
            <v-spacer></v-spacer>
            <v-btn text="Hủy" @click="displayMonitorDeleteUser = false"></v-btn>
            <v-btn text="Chắn chắn" color="red darken-1" @click="deleteUserByManager"></v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>

    <!-- Form update nhân viên -->
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
                      <v-combobox :items="['Chủ cửa hàng', 'Quản lý', 'Nhân viên', 'Khách hàng']"
                        v-model="employeeCurrentChoose.role" outlined></v-combobox>
                    </v-col>
                  </v-row>
                </v-col>
              </v-row>
              <v-row class="d-flex align-center">
                <v-col cols="2" class="font-weight-medium text-grey-darken-1 pa-1">
                  Ảnh mới
                </v-col>
                <v-col cols="9" class="pa-1 image-tag">
                  <v-file-input accept="image/*" v-model="employeeCurrentChoose.imageUrl" prepend-icon="mdi-camera"
                    :show-size="true"></v-file-input>
                </v-col>
              </v-row>
              <v-row class="d-flex align-center">
                <v-col cols="2" class="font-weight-medium text-grey-darken-1 pa-1">
                  Mật khẩu mới:
                </v-col>
                <v-col cols="9" class="pa-1">
                  <input type="text" v-model="employeeCurrentChoose.passwordOfUser" class="w-100 px-3 py-2"
                    placeholder="..."></input>
                </v-col>
              </v-row>
            </v-container>
          </v-card-text>

          <v-card-actions class="">
            <v-spacer></v-spacer>
            <v-btn text="Hủy" @click="displayMonitorUpdateUser = false"></v-btn>
            <v-btn text="Xác nhận" class="bg-orange-accent-3" @click="updateInfoUserByManager"></v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>

    <!-- Form lịch làm việc -->
    <div>
      <v-dialog persistent max-width="800" v-model="displayMonitorSchedule">
        <v-card class="pa-2">
          <v-card-title class="pb-0">
            Lịch làm việc của: {{ employeeNameCurrentChoose }}
          </v-card-title>
          <v-card-text class="pa-3 pb-0">
            <v-container class="pa-0 d-flex flex-column justify-center">
              <div>
                <table class="scheduleTable" border="1" cellpadding="5" cellspacing="0"
                  style="width: 100%; text-align: center;">
                  <thead>
                    <tr>
                      <th>Thứ hai</th>
                      <th>Thứ ba</th>
                      <th>Thứ tư</th>
                      <th>Thứ năm</th>
                      <th>Thứ sáu</th>
                      <th>Thứ bảy</th>
                      <th>Chủ nhật</th>
                    </tr>
                  </thead>
                  <tbody>
                    <template v-if="swapModeSchedule">
                      <tr v-for="(week, index) in weeks" :key="'update-' + index">
                        <td v-for="(day, idx) in week" :key="idx" class="scheduleTable_item">
                          <div class="d-flex flex-column justify-center"
                            :class="{ 'disable-day': day < currentDay, 'current-day': day == currentDay }">
                            <span style="font-size: 14px; opacity: 0.6;">{{ formatDay(day) || '' }}</span>
                            <span>{{ (scheduleOfUser[day - 1] && shifs[scheduleOfUser[day - 1].shiftCode]) || '_'
                            }}</span>
                          </div>
                        </td>
                      </tr>
                    </template>

                    <template v-else>
                      <tr v-for="(week, index) in weeks" :key="'view-' + index">
                        <td v-for="(day, idx) in week" :key="idx" class="scheduleTable_item"
                          @click="day >= currentDay ? handleClickItemScheduleTable(day, idx) : null"
                          :class="{ 'selected-day': selectedDay === day && day >= currentDay, 'change-shift-with-before': isShiftChanged(day) && day >= currentDay, 'cursor-pointer': day >= currentDay, 'current-day': day == currentDay }">
                          <div class="d-flex flex-column justify-center" :class="{ 'disable-day': day < currentDay }">
                            <span style="font-size: 14px; opacity: 0.6;">{{ formatDay(day) || '' }}</span>
                            <span>{{ (scheduleOfUser[day - 1] && shifs[scheduleOfUser[day - 1].shiftCode]) || '_'
                            }}</span>
                          </div>
                        </td>
                      </tr>
                    </template>
                  </tbody>
                </table>
              </div>
              <div class="mt-2" v-if="swapModeSchedule" style="margin: 0 auto;">
                <v-btn color="info mt-2" @click="swapModeSchedule = false">Cập nhật lịch làm việc</v-btn>
              </div>
              <div class="d-flex flex-column justify-center align-center mt-2" v-else>
                <v-chip-group>
                  <v-chip v-for="(shif, shifCode) in shifs" :class="{ 'text-primary': selectedShift === shifCode }"
                    :key="JSON.stringify(shif)" :text="shif" @click="handleClickItemShift(shifCode, shif)"></v-chip>
                </v-chip-group>

                <div class="mt-1">
                  <v-btn variant="outlined" color="info" style="min-width: 150px;" class="me-3"
                    @click="cancelUpdateSchedule">Hủy</v-btn>
                  <v-btn color="info" style="min-width: 150px;" class="me-3" @click="handleUpdateSchedule">Cập
                    nhật</v-btn>
                  <v-btn color="info" style="min-width: 150px;" @click="showListEmployee">
                    Thay đổi với nhân sự khác</v-btn>
                </div>
                <div class="mt-3" v-if="isShowListEmployeeSwapSchedule">
                  <v-menu>
                    <template v-slot:activator="{ props }">
                      <v-btn v-bind="props" append-icon="mdi-chevron-down" min-width="200px"
                        style="border: 1px solid #333; height: 34px;" size="small" class="ms-2">
                        {{ employeeSelectedSwapSchedule?.fullName || "Nhân viên" }}
                      </v-btn>
                    </template>
                    <v-list max-height="200px" style="overflow-y: auto">
                      <v-list-item v-for="(emp, index) in employeeList" :key="index" :value="emp"
                        style="min-height: 40px !important" @click="confirmSelectedEmployee(emp)">
                        <v-list-item-title>{{ emp.fullName }}</v-list-item-title>
                      </v-list-item>
                    </v-list>
                  </v-menu>
                </div>
              </div>
            </v-container>
          </v-card-text>

          <v-card-actions class="">
            <v-spacer></v-spacer>
            <v-btn text="Đóng" color="info" @click="closeDisplayMonitorSchedule"></v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-dialog v-model="isConfirmSelectedEmployeeSwapSchedule" max-width="400px" class="text-center" persistent>
        <v-card>
          <v-card-title class="text-h5 text-center"> Xác nhận </v-card-title>
          <v-card-actions class="justify-center" style="gap: 1.2rem">
            <v-btn color="green" @click="swapSchedule">Đồng ý</v-btn>
            <v-btn color="error" @click="cancelSwapSchedule">Hủy</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>
  </div>
</template>

<script setup>
import useEmployeeManagement from "./employeeManagement.js";
import _ from "underscore";
const {
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
} = useEmployeeManagement();

</script>
