<template>
  <div class="foodManagement d-flex flex-column">
    <div class="foodManagement_search d-flex align-center">
      <!-- Thanh search -->
      <v-text-field
        class="user_debt_equipment-search"
        append-inner-icon="mdi-magnify"
        density="compact"
        label="Nhập món ăn cần tìm kiếm..."
        variant="outlined"
        hide-details
        single-line
        v-model="search"
        style="flex: 3"
      ></v-text-field>
      <!-- Tài khoản tương tác -->
      <v-combobox
        class="foodManagement_search-phone ms-2"
        placeholder="Tên hoặc số điện thoại..."
        v-model="searchPhoneNumbers"
        :items="filteredPhoneNumbers"
        item-title="display"
        item-value="phone"
        :return-object="true"
        hide-no-data
        hide-selected
        clearable
        variant="outlined"
        style="flex: 2"
        :search-input.sync="searchPhoneKeyword"
      />
      <!-- Mở/Kết ca -->
      <div>
        <v-btn
          v-if="swapButtonShift"
          class="ms-2 my_btn_custommer"
          style="background-color: #b00000 !important"
          @click="showOpenShift"
          >Mở ca</v-btn
        >
        <v-btn
          v-else
          class="ms-2 my_btn_custommer"
          style="background-color: #002051 !important"
          @click="showCloseShift"
          >Kết ca</v-btn
        >
      </div>

      <!-- Dialog mở ca -->
      <v-dialog
        v-model="isShowOpenShift"
        width="650px"
        persistent
        class="open_shift d-flex align-center justify-center"
      >
        <v-card>
          <v-card-title class="">Ghi nhận mở ca</v-card-title>
          <v-card-text
            class="text-center py-4"
            style="width: 90%; margin: 0 auto"
          >
            <div style="font-size: 16px; font-weight: 500; color: #555">
              Số tiền hiện có trong két là:
            </div>
            <div
              style="
                font-size: 28px;
                font-weight: bold;
                color: #01579b;
                margin-top: 8px;
              "
            >
              {{ formatCurrency(inputOpenCashAmount) }}
            </div>
          </v-card-text>

          <v-card-actions>
            <v-btn @click="cancelConfirmOpenShift" color="red">Đóng</v-btn>
            <v-btn @click="confirmOpenShift" color="green">Xác nhận</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <!-- Dialog kết ca -->
      <v-dialog v-model="isShowCloseShift" width="650px" persistent>
        <v-card>
          <v-card-title class="">Ghi nhận kết ca</v-card-title>
          <v-card-text style="padding: 0px 4px; width: 90%; margin: 0 auto">
            <v-text-field
              label="Số tiền trong két (nghìn VND)"
              v-model="inputCloseCashAmount"
            ></v-text-field>
            <v-text-field
              label="Số tiền phát sinh"
              v-model="inputAdjustmentAmount"
            ></v-text-field>
            <v-text-field
              label="Nguyên nhân sử dụng"
              v-model="inputAdjustmentReason"
            ></v-text-field>
          </v-card-text>

          <v-card-actions>
            <v-btn @click="cancelCofirmCloseShift" color="red">Đóng</v-btn>
            <v-btn @click="confirmCloseShift" color="primary">Xác nhận</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>

    <div
      class="foodManagement_listFoodOrder flex-1-0 mt-2 d-flex justify-space-between"
      style="margin-bottom: 52px"
    >
      <!-- Hiển thị danh sách món và các chức năng thao tác -->
      <div
        class="foodManagement_listFoodOrder_menu rounded w-66"
        :style="{ backgroundColor: 'var(--bg-color-item)' }"
        v-if="!loading && foodCategories && filteredFoodItems"
      >
        <!-- Danh mục -->
        <div
          class="foodManagement_listFoodOrder_menu_dishes d-flex justify-center flex-wrap mt-1"
        >
          <v-chip
            class="ma-1 foodManagement_listFoodOrder_menu_dishes_item"
            label
            v-for="foodCategory in foodCategories"
            :key="
              foodCategory.categoryId +
              foodCategory.categoryName +
              foodCategory.icon
            "
            variant="tonal"
            :type="foodCategory.categoryId"
            :class="{
              selected: listFoodCategorySelected.includes(foodCategory),
            }"
            @click="tonggleSelected(foodCategory)"
          >
            <v-icon
              :icon="foodCategory.icon"
              start
              style="margin-left: 2px"
            ></v-icon>
            {{ foodCategory.categoryName }}
          </v-chip>
        </div>

        <v-divider class="mt-1"></v-divider>

        <!-- Các món uống -->
        <div class="foodManagement_listFoodOrder_menu_foods">
          <v-container class="pa-2 mt-1">
            <!-- Hiển thị danh sách các món hiện có -->
            <v-row class="pa-1" gutters="16">
              <v-col
                v-for="foodItem in filteredFoodItems"
                :key="foodItem.foodItemId"
                :cols="12"
                :lg="3"
                :md="4"
                :sm="6"
                :xs="12"
                class="pa-1"
              >
                <div
                  class="foodManagement_listFoodOrder_menu_foods_item rounded-lg pa-2 position-relative"
                >
                  <img
                    :src="foodItem.imageUrl"
                    alt="Food Item"
                    class="foodManagement_listFoodOrder_menu_foods_item_img rounded-lg"
                    style="height: 140px; width: 98%"
                  />
                  <h4 class="mt-2 text-center hiddent-text-two-line">
                    {{ foodItem.foodName }}
                  </h4>
                  <h5>
                    {{ formatCurrency(foodItem.priceCustom) }}/{{
                      foodItem.unit
                    }}
                  </h5>
                  <h6>{{ formatPoint(foodItem.point) }}/{{ foodItem.unit }}</h6>
                  <v-btn
                    class="mt-2 foodManagement_listFoodOrder_menu_foods_item_addFood"
                    size="small"
                    style="height: 28px; font-size: 12px; min-width: 130px"
                    color="blue-darken-2"
                    variant="tonal"
                    @click="openDialogShowDetail(foodItem)"
                  >
                    Chọn món
                  </v-btn>
                  <div
                    v-if="
                      listFoodItemIdBestSelling.includes(foodItem.foodItemId)
                    "
                    class="foodManagement_listFoodOrder_menu_foods_item-bestseller"
                  >
                    Best seller
                  </div>
                </div>
              </v-col>
            </v-row>

            <!-- Dialog chọn món -->
            <v-dialog
              v-model="showDialogSelectedAdditionalFood"
              max-width="900px"
            >
              <v-card>
                <v-card-title class="headline d-flex align-center pb-0">
                  Thêm món phụ nào ...
                </v-card-title>

                <v-card-text class="px-4 py-2">
                  <!-- Thông tin món chính vừa chọn -->
                  <div class="d-flex justify-space-between align-center">
                    <div class="d-flex align-center">
                      <img
                        :src="currentOrderItem.Image"
                        alt=""
                        style="width: 64px; height: 50px"
                        class="rounded me-2"
                      />
                      <div class="d-flex flex-column">
                        <span>{{ currentOrderItem.FoodName }}</span>
                        <span
                          >{{ formatCurrency(currentOrderItem.Price) }}
                        </span>
                      </div>
                    </div>
                    <input
                      type="number"
                      v-model="currentOrderItem.Quantity"
                      style="width: 75px; height: 36px; border: 1px solid #333"
                      outlined
                      class="rounded pa-3"
                      min="0"
                    />
                  </div>

                  <v-divider class="my-3"></v-divider>

                  <!-- Danh sách món gọi thêm -->
                  <div class="w-100">
                    <h4>Các món gọi thêm</h4>
                    <div
                      class="AdditionalItems d-flex flex-wrap"
                      style="max-height: 220px; overflow-y: auto"
                    >
                      <v-checkbox
                        v-for="item in currentOrderItem.ListAdditionalFood"
                        :key="item.id"
                        :label="`${item.foodName} - ${formatCurrency(
                          item.priceCustom
                        )}`"
                        class="w-50"
                        :value="item"
                        v-model="resultOrderItem.ListAdditionalFood"
                        hide-details
                        style="position: relative"
                      >
                        <input
                          type="number"
                          v-model="item.quantity"
                          style="
                            width: 75px;
                            height: 36px;
                            border: 1px solid #333;
                            position: absolute;
                            left: 330px;
                          "
                          outlined
                          class="rounded pa-3"
                          min="0"
                        />
                      </v-checkbox>
                    </div>
                  </div>

                  <!-- Ghi chú cho món ăn vừa chọn -->
                  <div class="w-100 mt-1" id="nodefooditem">
                    <h4>Ghi chú</h4>
                    <v-textarea
                      variant="solo-filled"
                      v-model="currentOrderItem.Note"
                    ></v-textarea>
                  </div>
                </v-card-text>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="red darken-1"
                    text
                    @click="nonSelectedFoodItemToCart"
                    >Hủy</v-btn
                  >
                  <v-btn
                    color="green darken-1"
                    text
                    @click="selectedFoodItemToCart(currentOrderItem)"
                    >Thêm vào giỏ</v-btn
                  >
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-container>
        </div>
      </div>

      <!-- Loading nếu chưa tải xong danh sách món ăn hoặc category -->
      <div v-else class="pa-4" style="width: 66%">
        <v-row>
          <v-col cols="12">
            <div class="d-flex flex-wrap">
              <v-skeleton-loader
                v-for="n in 8"
                :key="n"
                type="chip"
                class="ma-1"
                min-width="110"
                :loading="loading"
              >
                <v-chip label class="ma-1">
                  <v-icon start>mdi-food</v-icon>
                  Loading...
                </v-chip>
              </v-skeleton-loader>
            </div>
          </v-col>

          <v-col
            v-for="n in 9"
            :key="n"
            cols="12"
            lg="4"
            md="4"
            sm="6"
            xs="12"
            class="pa-2"
          >
            <v-skeleton-loader
              type="image, list-item-two-line"
              height="200"
            ></v-skeleton-loader>
          </v-col>
        </v-row>
      </div>

      <!-- Danh sách các món trong Orders -->
      <div
        class="foodManagement_listFoodOrder_bill px-2 rounded"
        :style="{ backgroundColor: 'var(--bg-color-item)' }"
      >
        <h4 class="mt-2 mb-1">Các món đã chọn</h4>
        <v-divider></v-divider>
        <div class="foodManagement_listFoodOrder_bill_orderList">
          <!-- Thông tin danh sách các món đã chọn -->
          <v-container>
            <v-row>
              <v-col
                cols="12"
                class="foodManagement_listFoodOrder_bill_orderList_item"
                v-for="(item, index) in currentOrder.items"
                :key="item.FoodItemId"
                style="padding: 2px 0 !important"
              >
                <!-- Thông tin chính cần biết -->
                <div
                  class="d-flex align-center overflow-hidden"
                  style="width: 79%"
                >
                  <img
                    :src="item.Image"
                    alt=""
                    class="foodManagement_listFoodOrder_bill_orderList_item_img"
                  />
                  <div
                    class="foodManagement_listFoodOrder_bill_orderList_item_info ms-1 d-flex flex-column overflow-hidden"
                  >
                    <h5 class="hiddent-text-one-line">
                      {{ item.FoodName }}
                    </h5>
                    <div style="font-size: 11px; line-height: 12px">
                      <span> {{ formatCurrency(item.Price) }}</span>
                      <span> x{{ item.Quantity }} {{ item.Unit }}</span>
                    </div>

                    <div
                      v-if="item.ListAdditionalFood.length > 0"
                      class="hiddent-text-one-line"
                      style="line-height: 14px"
                    >
                      <span
                        style="font-size: smaller"
                        v-for="(foodName, index) in item.ListAdditionalFood"
                        :key="foodName.id"
                      >
                        {{ foodName.foodName }}+{{ foodName.quantity }}
                        <span v-if="index < item.ListAdditionalFood.length - 1"
                          >,
                        </span>
                      </span>
                    </div>
                    <div
                      v-if="item.Note"
                      class="hiddent-text-one-line"
                      style="line-height: 14px !important"
                    >
                      <span style="font-size: smaller">{{ item.Note }} </span>
                    </div>
                  </div>
                </div>

                <!-- Tổng tiền của món và các thao tác với món -->
                <div>
                  <div
                    class="foodManagement_listFoodOrder_bill_orderList_item_price"
                    style="font-size: smaller"
                  >
                    {{
                      formatCurrency(
                        item.Quantity *
                          (item.Price +
                            totalAmountAdditionalFoodItem(
                              item.ListAdditionalFood
                            ))
                      )
                    }}
                  </div>
                  <div class="d-flex justify-end align-center">
                    <v-icon
                      color="blue"
                      x-small
                      style="font-size: 20px"
                      class="cursor-pointer me-1"
                      @click="updateCurrentFoodSelected(item, index)"
                      >mdi-pencil</v-icon
                    >
                    <v-icon
                      color="red"
                      x-small
                      style="font-size: 20px"
                      class="cursor-pointer"
                      @click="deleteCurrentFoodSelected(index)"
                      >mdi-delete</v-icon
                    >
                  </div>
                </div>
              </v-col>
            </v-row>
          </v-container>

          <!-- Dialog chỉnh sửa món ăn được chọn, nằm trong danh sách món ăn đã chọn -->
          <v-dialog
            v-model="visibleUpdateCurrentFoodSelected"
            max-width="900px"
          >
            <v-card>
              <v-card-title class="headline pb-0"
                >Thông tin món đã gọi</v-card-title
              >

              <v-card-text class="px-4 py-2">
                <!-- Thông tin chính món được cập nhật -->
                <div class="d-flex justify-space-between align-center">
                  <div class="d-flex align-center">
                    <img
                      :src="updateOrderItem.Image"
                      alt=""
                      style="width: 64px; height: 50px"
                      class="rounded me-2"
                    />
                    <div class="d-flex flex-column">
                      <span>{{ updateOrderItem.FoodName }}</span>
                      <span>{{ formatCurrency(updateOrderItem.Price) }} </span>
                    </div>
                  </div>
                  <input
                    type="number"
                    v-model="updateOrderItem.Quantity"
                    style="width: 75px; height: 36px; border: 1px solid #333"
                    outlined
                    class="rounded pa-3"
                    min="0"
                  />
                </div>

                <v-divider class="my-2"></v-divider>

                <!-- Thông tin thay đổi về phần topping -->
                <div class="w-100">
                  <h4>Các món gọi thêm đã chọn</h4>
                  <div
                    class="AdditionalItems d-flex flex-wrap"
                    style="max-height: 220px; overflow-y: auto"
                  >
                    <v-checkbox
                      v-for="item in updateOrderItem.ListAdditionalFood"
                      :key="item.id"
                      :label="`${item.foodName} - ${formatCurrency(
                        item.priceCustom
                      )}`"
                      class="w-50"
                      :value="item"
                      v-model="updateOrderItem.ListAdditionalFoodSelected"
                      hide-details
                      style="min-height: 40px !important; position: relative"
                    >
                      <input
                        type="number"
                        v-model="item.quantity"
                        style="
                          width: 75px;
                          height: 36px;
                          border: 1px solid #333;
                          position: absolute;
                          left: 330px;
                        "
                        outlined
                        class="rounded pa-3"
                        min="0"
                      />
                    </v-checkbox>
                  </div>
                </div>

                <!-- Ghi chú thêm -->
                <div class="w-100" id="nodefooditem">
                  <h4>Ghi chú</h4>
                  <v-textarea
                    variant="solo-filled"
                    v-model="updateOrderItem.Note"
                  ></v-textarea>
                </div>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                  color="red darken-1"
                  text
                  @click="
                    visibleUpdateCurrentFoodSelected =
                      !visibleUpdateCurrentFoodSelected
                  "
                  >Hủy</v-btn
                >
                <v-btn
                  color="green darken-1"
                  text
                  @click="updateFoodItem(updateOrderItem)"
                  >Xác nhận thay đổi</v-btn
                >
              </v-card-actions>
            </v-card>
          </v-dialog>
        </div>

        <!-- Đơn giá danh sách các sản phẩm -->
        <div class="foodManagement_listFoodOrder_bill_payment rounded-lg mt-2">
          <div
            class="foodManagement_listFoodOrder_bill_payment_total d-flex justify-space-between my-1"
          >
            <span style="font-size: 12px">Tổng tiền</span>
            <span>
              {{ formatCurrency(currentOrder.total_amount) }} |
              {{ formatPoint(currentOrder.total_amount) }}</span
            >
          </div>
          <div
            class="foodManagement_listFoodOrder_bill_payment_discount d-flex justify-space-between mb-1"
          >
            <span style="font-size: 12px">Tiền nhận (nghìn VNĐ)</span>

            <input
              type="number"
              v-model="currentOrder.receivedAmount"
              class="discount-input my_input_custom"
              min="0"
            />
          </div>
          <div
            class="foodManagement_listFoodOrder_bill_payment_discount d-flex justify-space-between mb-1"
          >
            <span style="font-size: 12px">Tiền thừa</span>
            <span class="my_input_custom">{{
              formatCurrency(returnedAmountCurrentOrder)
            }}</span>
          </div>
          <div
            class="foodManagement_listFoodOrder_bill_payment_discount d-flex justify-space-between"
          >
            <span style="font-size: 12px">Giảm giá(%)</span>
            <input
              type="number"
              v-model="currentOrder.discount"
              class="discount-input my_input_custom"
              min="0"
            />
          </div>
          <div
            class="foodManagement_listFoodOrder_bill_payment_paymentMethod d-flex justify-space-between mt-1"
          >
            <v-radio-group v-model="currentOrder.paymentMethod" inline>
              <v-radio label="Tiền mặt" value="Tiền mặt"></v-radio>
              <v-radio label="Chuyển khoản" value="Chuyển khoản"></v-radio>
              <v-radio label="Điểm" value="Điểm"></v-radio>
            </v-radio-group>
          </div>

          <v-divider></v-divider>
          <div
            class="foodManagement_listFoodOrder_bill_payment_payment d-flex justify-space-between mt-2"
          >
            <h4 style="font-size: 14px">Tổng thanh toán</h4>
            <span
              >{{ formatCurrency(resultTotalAmount) }} |
              {{ formatPoint(resultTotalAmount) }}</span
            >
          </div>
        </div>

        <!-- Thực hiện thao tác -->
        <div class="d-flex justify-space-between align-center mt-2">
          <v-btn
            class="my_btn_custommer me-1"
            @click="callApiOrderFood()"
            color="orange-darken-2"
            >Thanh toán</v-btn
          >
          <v-btn
            class="my_btn_custommer ms-1"
            color="teal-darken-2"
            @click="isShowQRCode = !isShowQRCode"
            >Mã QR</v-btn
          >
        </div>
        <div class="d-flex justify-space-between align-center mt-2 mb-1">
          <v-btn
            class="my_btn_custommer w-100"
            color="blue-darken-4"
            @click="callApiOrderFoodAndAddTable"
            >Chọn bàn</v-btn
          >
          <!-- <v-btn
            class="mb-1 my_btn_custommer ms-1"
            style="background-color: #002051 !important"
            @click="isShowQRCode = !isShowQRCode"
            >Hóa đơn</v-btn
          > -->
        </div>

        <!-- Mã QR -->
        <v-dialog
          v-model="isShowQRCode"
          width="650px"
          height="600px"
          persistent
        >
          <v-card>
            <v-card-title class="">Tên tài khoản: LE THANH GIANG </v-card-title>
            <v-card-text style="padding: 0px 4px; width: 60%; margin: 0 auto">
              <v-img cover :src="momoQRCodeUrl"></v-img>
            </v-card-text>

            <v-card-actions>
              <v-btn @click="isShowQRCode = false" color="red">Đóng</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        <!-- Hóa đơn -->
        <!-- <v-dialog
          v-model="isShowQRCode"
          width="650px"
          height="600px"
          persistent
        >
          <v-card>
            <v-card-title class="">Tên tài khoản: LE THANH GIANG </v-card-title>
            <v-card-text style="padding: 0px 4px; width: 60%; margin: 0 auto">
              <v-img cover :src="momoQRCodeUrl"></v-img>
            </v-card-text>

            <v-card-actions>
              <v-btn @click="isShowQRCode = false" color="red">Đóng</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog> -->

        <!-- Xác thực thanh toán otp -->
        <v-dialog v-model="isShowOTPPoints" width="650px" persistent>
          <v-card class="confirm_otp_payment">
            <v-card-title>Xác thực thanh toán bằng điểm</v-card-title>
            <v-card-text
              style="padding: 0px 4px; width: 80%; margin: 0 auto"
              class="d-flex flex-column align-center"
            >
              <v-otp-input v-model="enteredOtp" focus-all focused></v-otp-input>
              <v-btn
                @click="verifyOTP"
                color="success"
                block
                v-if="!visibleResendOtp"
              >
                Xác minh OTP
              </v-btn>
              <v-btn @click="sendOTP" color="primary" block v-else>
                Gửi lại OTP
              </v-btn>
            </v-card-text>
            <v-card-actions>
              <v-btn @click="isShowOTPPoints = false" color="red">Đóng</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>

        <!-- Dialog nếu chọn bàn -->
        <v-dialog
          v-model="showComponentAreaManagement"
          max-width="1180px"
          persistent
        >
          <v-card style="width: 100%; height: 100%">
            <v-card-title class="headline">Chọn bàn</v-card-title>

            <v-card-text
              id="showComponentAreaManagement"
              style="padding: 0 4px"
            >
              <AreaManagement
                v-show="showComponentAreaManagement"
                @resetFoodsSelected="handleCloseAndReset"
              />
            </v-card-text>

            <v-card-actions>
              <v-btn @click="handleCloseComponentAreaManagement" color="red"
                >Đóng</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
      </div>
    </div>
  </div>
</template>

<script setup>
import useFoodManagement from "./listFood.js";
import AreaManagement from "@/components/monitor/areaManagement/index.vue";

const {
  // State variables
  foodCategories,
  search,
  showDialogSelectedAdditionalFood,
  visibleUpdateCurrentFoodSelected,
  showComponentAreaManagement,
  loading,
  isShowQRCode,

  // Computed properties
  filteredFoodItems,
  resultTotalAmount,

  // Data objects
  listFoodCategorySelected,
  currentOrderItem,
  resultOrderItem,
  updateOrderItem,
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
  tonggleSelected,
  selectedFoodItemToCart,
  nonSelectedFoodItemToCart,
  totalAmountAdditionalFoodItem,
  formatCurrency,
  formatPoint,
  openDialogShowDetail,
  handleCloseComponentAreaManagement,
  handleCloseAndReset,
  updateCurrentFoodSelected,
  updateFoodItem,
  deleteCurrentFoodSelected,
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
} = useFoodManagement();
</script>
