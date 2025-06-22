<template>
  <div class="areaManagement pa-3">
    <div class="areaManagement_item d-flex align-center rounded">
      <div
        class="areaManagement_item_tables d-flex flex-wrap py-2 justify-center"
        style="width: 100%"
      >
        <div
          v-for="table in tables"
          :key="table.tableId"
          size="large"
          class="areaManagement_item_tables_table d-flex flex-column ma-2 pa-2 rounded cursor-pointer justify-space-between"
          style="width: 250px; min-height: 100px"
          :type="
            checkFoodOrderByTableId(table.tableId).items.length != 0
              ? 'ban'
              : 'trong'
          "
          @click="handleConfirmDialog(table)"
        >
          <div class="d-flex justify-space-between">
            <h4>{{ table.tableName }}</h4>
            <span>{{
              checkFoodOrderByTableId(table.tableId).items.length != 0
                ? formatDate(checkFoodOrderByTableId(table.tableId).order_time)
                : "dd:MM:yyyy hh:mm:ss"
            }}</span>
          </div>
          <div class="mt-5">
            {{
              checkFoodOrderByTableId(table.tableId).items.length != 0
                ? "Đã gọi " +
                  checkFoodOrderByTableId(table.tableId).items.length +
                  " món: " +
                  formatCurrency(
                    getTotal_totalAmount_totalResult(
                      checkFoodOrderByTableId(table.tableId)
                    ).totalPayment
                  )
                : "Chưa gọi món"
            }}
          </div>
        </div>
      </div>
    </div>
    <v-dialog
      v-model="confirmDialog"
      max-width="400px"
      class="text-center"
      persistent
    >
      <v-card>
        <v-card-title class="text-h5 text-center"> Xác nhận </v-card-title>
        <v-card-actions class="justify-center" style="gap: 1.2rem">
          <v-btn color="green" @click="chooseTableAndSetCurrentOrders"
            >Đồng ý</v-btn
          >
          <v-btn
            color="primary"
            @click="viewTableAndSetCurrentOrders"
            v-if="checkFoodOrderByTableId(currentTableId).items.length != 0"
            >Xem món ăn đã gọi</v-btn
          >
          <v-btn color="error" @click="cancelTableAndSetCurrentOrders"
            >Hủy</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="showListFoodOrderOfTableId" max-width="1180" persistent>
      <v-card class="pa-4">
        <v-card-title class="text-h6 font-weight-bold pa-0">
          Danh sách món ăn đã gọi
        </v-card-title>
        <v-divider></v-divider>

        <v-container class="pt-4 pa-2">
          <v-row
            dense
            style="min-height: 420px; max-height: 420px; overflow-y: auto"
          >
            <v-col
              v-for="foodItem in listFoodOrderOfTableId"
              :key="foodItem.FoodItemId"
              cols="12"
              md="6"
              class="mb-4"
            >
              <v-card
                class="d-flex flex-wrap align-center rounded-lg elevation-2"
              >
                <v-img
                  :src="foodItem.Image"
                  alt="Hình ảnh món ăn"
                  class="rounded-l-lg rounded"
                  cover
                  style="min-width: 40%; height: 130px"
                ></v-img>

                <div class="pa-1" style="width: 60%">
                  <div
                    class="text-subtitle-1 font-weight-medium mb-1 hiddent-text-one-line"
                    style="max-height: 24px; line-height: 24px"
                  >
                    {{ foodItem.FoodName }}
                  </div>
                  <div class="text-caption mb-1 hiddent-text-two-line">
                    Gọi thêm:
                    {{
                      foodItem.ListAdditionalFood.length > 0
                        ? _.map(foodItem.ListAdditionalFood, "foodName").join(
                            ","
                          )
                        : "..."
                    }}
                  </div>
                  <div class="text-caption mb-1 hiddent-text-one-line">
                    Ghi chú:
                    {{ foodItem.Note ? foodItem.Note : "..." }}
                  </div>
                  <div class="text-caption text-grey-darken-1">
                    Đơn giá: {{ formatCurrency(foodItem.Price) }} x
                    {{ foodItem.Quantity }}
                  </div>
                  <div class="mt-1 text-body-2 text-caption">
                    <span class="text-grey">
                      Tổng + món thêm:
                      {{
                        formatCurrency(
                          foodItem.Quantity *
                            (foodItem.Price +
                              totalAmountAdditionalFoodItem(
                                foodItem.ListAdditionalFood
                              ))
                        )
                      }}
                    </span>
                  </div>
                </div>
              </v-card>
            </v-col>
          </v-row>
          <v-row dense class="d-flex justify-space-between align-end mt-2">
            <div class="w-1/3">
              <div
                class="foodManagement_listFoodOrder_bill_payment_discount d-flex align-center justify-space-between mb-1"
              >
                <span style="font-size: 12px; margin-right: 4px"
                  >Tiền nhận</span
                >
                <input
                  type="number"
                  v-model="paymentInfo.receivedAmount"
                  class="discount-input my_input_custom"
                  min="0"
                />
              </div>
              <div
                class="foodManagement_listFoodOrder_bill_payment_discount d-flex align-center justify-space-between mb-1"
              >
                <span style="font-size: 12px; margin-right: 4px"
                  >Tiền thừa</span
                >
                <span class="my_input_custom">{{
                  formatCurrency(returnedAmountCurrentOrder)
                }}</span>
              </div>
              <div
                class="foodManagement_listFoodOrder_bill_payment_discount d-flex align-center justify-space-between"
              >
                <span style="font-size: 12px; margin-right: 4px"
                  >Giảm giá(%)</span
                >
                <input
                  type="number"
                  v-model="paymentInfo.discount"
                  class="discount-input my_input_custom"
                  min="0"
                />
              </div>
            </div>
            <div>
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
              <div
                class="foodManagement_listFoodOrder_bill_payment_paymentMethod d-flex justify-space-between mt-1"
              >
                <v-radio-group v-model="paymentInfo.paymentMethod" inline>
                  <v-radio label="Tiền mặt" value="Tiền mặt"></v-radio>
                  <v-radio label="Chuyển khoản" value="Chuyển khoản"></v-radio>
                  <v-radio label="Điểm" value="Điểm"></v-radio>
                </v-radio-group>
              </div>
            </div>
            <div class="w-33 d-flex flex-column">
              <div class="d-flex justify-space-between">
                <span class="mb-2">Tổng tiền: </span>
                <span>{{ formatCurrency(paymentInfo.totalAmount) }}</span>
              </div>
              <div class="d-flex justify-space-between">
                <span class="mb-2">Số tiền thanh toán: </span>
                <span>{{ formatCurrency(paymentInfo.totalResult) }}</span>
              </div>
            </div>
          </v-row>
        </v-container>

        <v-divider class="my-2"></v-divider>

        <v-card-actions class="d-flex justify-space-between">
          <v-btn class="" color="primary" @click="isShowQRCode = !isShowQRCode"
            >Mã thanh toán</v-btn
          >
          <div>
            <v-btn color="primary" @click="ConfirmPayment">Thanh toán</v-btn>
            <v-btn color="red" @click="closeShowListFoodOrderOfTableId"
              >Đóng</v-btn
            >
          </div>
        </v-card-actions>
      </v-card>
    </v-dialog>
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
    <!-- QR code -->
    <v-dialog v-model="isShowQRCode" width="650px" height="600px" persistent>
      <v-card>
        <v-card-title class="">Tên tài khoản: LE THANH GIANG </v-card-title>
        <v-card-text style="padding: 0px 4px; width: 60%; margin: 0 auto">
          <v-skeleton-loader
            type="card"
            v-if="!momoQRCodeUrl"
          ></v-skeleton-loader>
          <v-img cover :src="momoQRCodeUrl" v-else></v-img>
        </v-card-text>

        <v-card-actions>
          <v-btn @click="isShowQRCode = false" color="red">Đóng</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { defineEmits } from "vue";
import _ from "underscore";
import useAreaManagement from "./areaManagament";
const emit = defineEmits(["resetFoodsSelected"]);
async function chooseTableAndSetCurrentOrders() {
  console.log("Table được chọn là: ", currentTableId.value);
  orderStore.assignDishesToTable(currentTableId.value);
  confirmDialog.value = !confirmDialog.value;
  emit("resetFoodsSelected");
}

const {
  tables,
  confirmDialog,
  showListFoodOrderOfTableId,
  listFoodOrderOfTableId,
  currentTableId,
  orderStore,
  paymentInfo,
  isShowQRCode,
  momoQRCodeUrl,
  returnedAmountCurrentOrder,
  searchPhoneNumbers,
  filteredPhoneNumbers,
  isShowOTPPoints,
  visibleResendOtp,
  enteredOtp,

  formatDate,
  formatCurrency,
  handleConfirmDialog,
  cancelTableAndSetCurrentOrders,
  viewTableAndSetCurrentOrders,
  getTotal_totalAmount_totalResult,
  totalAmountAdditionalFoodItem,
  closeShowListFoodOrderOfTableId,
  checkFoodOrderByTableId,
  ConfirmPayment,
  verifyOTP,
  sendOTP,
} = useAreaManagement();
</script>
