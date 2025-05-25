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
            checkFoodOrderByTableId(table.tableId).length != 0 ? 'vang' : 'lam'
          "
          @click="handleConfirmDialog(table)"
        >
          <div class="d-flex justify-space-between">
            <h4>{{ table.tableName }}</h4>
            <span>hh:MM:ss</span>
          </div>
          <div class="mt-5">
            {{
              checkFoodOrderByTableId(table.tableId).length != 0
                ? "Đã gọi " +
                  checkFoodOrderByTableId(table.tableId).length +
                  " món"
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
            v-if="checkFoodOrderByTableId(currentTableId).length != 0"
            >Xem món ăn đã gọi</v-btn
          >
          <v-btn color="error" @click="cancelTableAndSetCurrentOrders"
            >Hủy</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="showListFoodOrderOfTableId" max-width="800" persistent>
      <v-card class="pa-4">
        <v-card-title class="text-h6 font-weight-bold pa-0">
          Danh sách món ăn đã gọi
        </v-card-title>
        <v-divider></v-divider>

        <v-container
          class="pt-4 pa-2"
          style="max-height: 500px; overflow-y: auto"
        >
          <v-row dense style="min-height: 100px">
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
                  class="rounded-l-lg"
                  cover
                  style="min-width: 40%; height: 100px"
                ></v-img>

                <div class="pa-3" style="width: 60%">
                  <div
                    class="text-subtitle-1 font-weight-medium mb-1"
                    style="
                      max-height: 24px;
                      line-height: 24px;
                      overflow: hidden;
                      display: -webkit-box;
                      -webkit-box-orient: vertical;
                      -webkit-line-clamp: 1;
                    "
                  >
                    {{ foodItem.FoodName }}
                  </div>
                  <div class="text-caption text-grey-darken-1">
                    Đơn giá: {{ formatCurrency(foodItem.Price) }} VNĐ x
                    {{ foodItem.Quantity }}
                  </div>
                  <div class="mt-1 text-body-2 text-caption">
                    <span class="text-grey">
                      Tổng + món thêm:
                      {{
                        formatCurrency(
                          foodItem.Price * foodItem.Quantity +
                            foodItem.Quantity *
                              totalAmountAdditionalFoodItem(
                                foodItem.ListAdditionalFood
                              )
                        )
                      }}
                      VNĐ
                    </span>
                  </div>
                </div>
              </v-card>
            </v-col>
          </v-row>
          <v-row dense class="d-flex justify-space-between align-end">
            <div class="w-33">
              <div
                class="foodManagement_listFoodOrder_bill_payment_tax d-flex justify-space-between mb-2"
              >
                <span style="font-size: 14px">Thuế(%)</span>
                <span>{{ tax }}%</span>
              </div>
              <div
                class="foodManagement_listFoodOrder_bill_payment_discount d-flex justify-space-between align-center mb-2"
              >
                <span style="font-size: 14px">Giảm giá(%)</span>
                <input
                  type="number"
                  v-model="discount"
                  class="discount-input px-2 py-1"
                  min="0"
                  style="width: 65px; border: 1px solid rgb(52, 52, 52)"
                />
              </div>
            </div>
            <div class="w-33 d-flex flex-column">
              <div class="d-flex justify-space-between">
                <span class="mb-2">Tổng tiền: </span>
                <span>{{ formatCurrency(totalAmount) }} VNĐ</span>
              </div>
              <div class="d-flex justify-space-between">
                <span class="mb-2">Số tiền thanh toán: </span>
                <span>{{ formatCurrency(resultTotalAmount) }} VNĐ</span>
              </div>
            </div>
          </v-row>
        </v-container>

        <v-divider class="my-2"></v-divider>
        <v-card-actions class="justify-end">
          <v-btn color="primary" @click="ConfirmPayment">Thanh toán</v-btn>
          <v-btn color="red" @click="closeShowListFoodOrderOfTableId"
            >Đóng</v-btn
          >
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { defineEmits } from "vue";
import useAreaManagement from "./areaManagament";
const emit = defineEmits(["resetFoodsSelected"]);
async function chooseTableAndSetCurrentOrders() {
  // let orderTimeCurrent = getCurrentDateTimeForSQL();
  // currentOrder.value = storeOrder.getSelectedDishes();
  // try {
  //   const orderResponse = await axios.post(API_ENDPOINTS.ADD_ORDER, {
  //     userId: currentOrder.value.user_id,
  //     orderTime: orderTimeCurrent,
  //     tableId: currentTableId.value,
  //     totalAmount: resultTotalAmount.value,
  //     status: currentOrder.value.status,
  //     discount: currentOrder.value.discount,
  //     tax: currentOrder.value.tax,
  //   });

  //   const orderId = orderResponse.data.data.orderId;

  //   // Gửi cả món chính và món phụ trong cùng một request
  //   await Promise.all(
  //     currentOrder.value.items.map(async (item) => {
  //       // Gửi món chính
  //       const mainItemResponse = await axios.post(
  //         API_ENDPOINTS.ADD_ORDER_ITEM,
  //         {
  //           orderId: orderId,
  //           foodItemId: item.FoodItemId,
  //           foodName: item.FoodName,
  //           quantity: item.Quantity,
  //           price: item.Price,
  //           isMainItem: 1,
  //           unit: item.Unit,
  //           note: item.Note,
  //           categoryId: item.CategoryId,
  //           orderTime: orderTimeCurrent,
  //         }
  //       );
  //       // Gửi các món phụ với parentItemId là mainItemId
  //       await Promise.all(
  //         item.ListAdditionalFood.map(async (addFood) => {
  //           await axios.post(API_ENDPOINTS.ADD_ORDER_ITEM, {
  //             orderId: orderId,
  //             foodItemId: addFood.id,
  //             foodName: addFood.foodName,
  //             quantity: addFood.quantity, // Số lượng mặc định là 1 nếu không chọn khác
  //             price: addFood.price,
  //             isMainItem: 0,
  //             unit: addFood.unit,
  //             note: "",
  //             categoryId: 0,
  //             orderTime: orderTimeCurrent,
  //           });
  //         })
  //       );
  //     })
  //   );

  //   // Kiểm tra phản hồi từ server
  //   if (orderResponse.data.success === -1) {
  //     toast.warn("Please provide all required information!", {
  //       position: "top-right",
  //       autoClose: 3000,
  //       hideProgressBar: false, // Hiện thanh tiến trình
  //       closeOnClick: true, // Đóng khi nhấp vào thông báo
  //       pauseOnHover: true, // Dừng khi di chuột lên thông báo
  //       draggable: true, // Kéo thông báo
  //       progress: undefined, // Tiến độ (nếu có)
  //     });
  //   } else if (orderResponse.data.success === 1) {
  //     confirmDialog.value = false;
  //     tables.value = tables.value.map((table) => ({
  //       ...table,
  //       isActive: table.tableId == currentTableId.value,
  //     }));
  //     toast.success("Add order successful!", {
  //       position: "top-right",
  //       autoClose: 3000,
  //       hideProgressBar: false, // Hiện thanh tiến trình
  //       closeOnClick: true, // Đóng khi nhấp vào thông báo
  //       pauseOnHover: true, // Dừng khi di chuột lên thông báo
  //       draggable: true, // Kéo thông báo
  //       progress: undefined, // Tiến độ (nếu có)
  //     });
  //     // Emit event để đóng component và reset
  //     emit("closeAndReset");
  //   } else {
  //     console.error("Failed to add order", orderResponse.data.message);
  //   }
  // } catch (error) {
  //   toast.error(`Error adding employee: ${error}`, {
  //     position: "top-right",
  //     autoClose: 3000,
  //     hideProgressBar: false, // Hiện thanh tiến trình
  //     closeOnClick: true, // Đóng khi nhấp vào thông báo
  //     pauseOnHover: true, // Dừng khi di chuột lên thông báo
  //     draggable: true, // Kéo thông báo
  //     progress: undefined, // Tiến độ (nếu có)
  //   });
  // }
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
  totalAmount,
  resultTotalAmount,
  tax,
  discount,

  formatCurrency,
  handleConfirmDialog,
  cancelTableAndSetCurrentOrders,
  viewTableAndSetCurrentOrders,
  totalAmountAdditionalFoodItem,
  closeShowListFoodOrderOfTableId,
  checkFoodOrderByTableId,
  ConfirmPayment,
} = useAreaManagement();
</script>
