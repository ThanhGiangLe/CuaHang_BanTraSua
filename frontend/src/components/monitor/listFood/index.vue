<template>
  <div class="foodManagement mt-1 d-flex flex-column">
    <!-- Chức năng tìm kiếm -->
    <div class="foodManagement_search d-flex align-center">
      <v-text-field
        class="user_debt_equipment-search"
        append-inner-icon="mdi-magnify"
        density="compact"
        label="Nhập món ăn cần tìm kiếm..."
        variant="outlined"
        hide-details
        single-line
        v-model="search"
      ></v-text-field>
    </div>

    <div
      class="foodManagement_listFoodOrder flex-1-0 mt-3 d-flex justify-space-between"
      style="margin-bottom: 52px"
    >
      <!-- Hiển thị MENU để thao tác -->
      <div
        class="foodManagement_listFoodOrder_menu rounded w-66"
        :style="{ backgroundColor: 'var(--bg-color-item)' }"
        v-if="!loading && foodCategories && filteredFoodItems"
      >
        <!-- Categories -->
        <div
          class="foodManagement_listFoodOrder_menu_dishes d-flex justify-center flex-wrap mt-2"
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

        <v-divider class="mt-2"></v-divider>

        <!-- Food Items -->
        <div class="foodManagement_listFoodOrder_menu_foods">
          <v-container class="pa-2">
            <!-- Hiển thị danh sách các món hiện có -->
            <v-row class="pa-2" gutters="16">
              <v-col
                v-for="foodItem in filteredFoodItems"
                :key="foodItem.foodItemId"
                :cols="12"
                :lg="4"
                :md="4"
                :sm="6"
                :xs="12"
                class="pa-2"
              >
                <div
                  class="foodManagement_listFoodOrder_menu_foods_item rounded-lg pa-3 position-relative"
                >
                  <img
                    :src="foodItem.imageUrl"
                    alt="Food Item"
                    class="foodManagement_listFoodOrder_menu_foods_item_img rounded-lg"
                    style="height: 140px; width: 95%"
                  />
                  <span
                    class="mt-3 hiddent-text-one-line"
                    style="max-height: 24px; line-height: 24px"
                  >
                    {{ foodItem.foodName }}
                  </span>
                  <h6 class="text-decoration-line-through">
                    {{ formatCurrency(foodItem.priceListed) }} /{{
                      foodItem.unit
                    }}
                  </h6>
                  <h5>
                    {{ formatCurrency(foodItem.priceCustom) }} /{{
                      foodItem.unit
                    }}
                  </h5>
                  <v-btn
                    class="mt-3 foodManagement_listFoodOrder_menu_foods_item_addFood"
                    size="x-small"
                    style="height: 28px"
                    color="orange-darken-2"
                    @click="openDialogShowDetail(foodItem)"
                  >
                    Chọn món
                  </v-btn>
                </div>
              </v-col>
            </v-row>

            <!-- Dialog chọn món -->
            <v-dialog
              v-model="showDialogSelectedAdditionalFood"
              max-width="900px"
            >
              <v-card>
                <v-card-title class="headline d-flex align-center">
                  Chi tiết món được chọn
                  <v-icon class="ml-2" color="" size="small"
                    >mdi-check-circle-outline</v-icon
                  >
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

                  <v-divider class="my-4"></v-divider>

                  <!-- Danh sách món gọi thêm -->
                  <div class="w-100">
                    <h4>Các món gọi thêm</h4>
                    <div
                      class="AdditionalItems d-flex flex-wrap"
                      style="max-height: 200px; overflow-y: auto"
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
                    color="green darken-1"
                    text
                    @click="selectedFoodItemToCart(currentOrderItem)"
                    >Thêm vào giỏ</v-btn
                  >
                  <v-btn
                    color="red darken-1"
                    text
                    @click="nonSelectedFoodItemToCart"
                    >Hủy</v-btn
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
                v-for="n in 6"
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
        <h4 class="my-3">Các món đã chọn</h4>
        <div class="foodManagement_listFoodOrder_bill_orderList">
          <!-- Thông tin danh sách các món đã chọn -->
          <v-container>
            <v-row>
              <v-col
                cols="12"
                class="foodManagement_listFoodOrder_bill_orderList_item"
                v-for="(item, index) in currentOrder.items"
                :key="item.FoodItemId"
                style="padding: 4px !important"
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
                    <span style="font-size: smaller"
                      >x{{ item.Quantity }} {{ item.Unit }}</span
                    >
                    <div
                      v-if="item.ListAdditionalFood.length > 0"
                      class="hiddent-text-one-line"
                    >
                      <span
                        style="font-size: smaller"
                        v-for="(foodName, index) in item.ListAdditionalFood"
                        :key="foodName.id"
                      >
                        {{ foodName.foodName }}x{{ foodName.quantity }}
                        <span v-if="index < item.ListAdditionalFood.length - 1"
                          >,
                        </span>
                      </span>
                    </div>
                    <div v-if="item.Note" class="hiddent-text-one-line">
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
              <v-card-title class="headline">Thông tin món đã gọi</v-card-title>

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

                <v-divider class="my-4"></v-divider>

                <!-- Thông tin thay đổi về phần topping -->
                <div class="w-100">
                  <h4>Các món gọi thêm đã chọn</h4>
                  <div
                    class="AdditionalItems d-flex flex-wrap"
                    style="max-height: 175px; overflow-y: auto"
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
                  color="green darken-1"
                  text
                  @click="updateFoodItem(updateOrderItem)"
                  >Xác nhận thay đổi</v-btn
                >
                <v-btn
                  color="red darken-1"
                  text
                  @click="
                    visibleUpdateCurrentFoodSelected =
                      !visibleUpdateCurrentFoodSelected
                  "
                  >Hủy</v-btn
                >
              </v-card-actions>
            </v-card>
          </v-dialog>
        </div>

        <!-- Đơn giá danh sách các sản phẩm -->
        <div class="foodManagement_listFoodOrder_bill_payment rounded-lg mt-5">
          <h4 class="mb-3">Tổng hóa đơn</h4>
          <div
            class="foodManagement_listFoodOrder_bill_payment_total d-flex justify-space-between mb-2"
          >
            <span style="font-size: 14px">Tổng tiền</span>
            <span> {{ formatCurrency(currentOrder.total_amount) }}</span>
          </div>
          <div
            class="foodManagement_listFoodOrder_bill_payment_tax d-flex justify-space-between mb-2"
          >
            <span style="font-size: 14px">Thuế(%)</span>
            <span>{{ currentOrder.tax }}</span>
          </div>
          <div
            class="foodManagement_listFoodOrder_bill_payment_discount d-flex justify-space-between mb-2"
          >
            <span style="font-size: 14px">Giảm giá(%)</span>
            <input
              type="number"
              v-model="currentOrder.discount"
              class="discount-input"
              min="0"
              style="width: 65px"
            />
          </div>
          <div
            class="foodManagement_listFoodOrder_bill_payment_paymentMethod d-flex justify-space-between mb-2"
          >
            <v-radio-group v-model="currentOrder.paymentMethod">
              <v-radio
                label="Thanh toán bằng tiền mặt"
                value="Tiền mặt"
              ></v-radio>
              <v-radio
                label="Thanh toán bằng chuyển khoản"
                value="Chuyển khoản"
              ></v-radio>
            </v-radio-group>
          </div>

          <v-divider></v-divider>
          <div
            class="foodManagement_listFoodOrder_bill_payment_payment d-flex justify-space-between mt-2"
          >
            <span style="font-size: 14px">Tổng thanh toán</span>
            <span>{{ formatCurrency(resultTotalAmount) }}</span>
          </div>
        </div>

        <!-- Thực hiện thao tác -->
        <v-btn
          class="w-100 mb-2 mt-2"
          @click="callApiOrderFood()"
          color="orange-darken-2"
          >Đặt món</v-btn
        >
        <v-btn
          class="w-100"
          style="margin-bottom: 20px"
          color="blue-darken-4"
          @click="callApiOrderFoodAndAddTable"
          >Đặt món và chọn bàn</v-btn
        >

        <!-- <v-dialog
          v-model="isShowQRCode"
          width="500px"
          height="500px"
          persistent
        >
          <v-card>
            <v-card-text
              style="padding: 16px 4px 0px; width: 88%; margin: 0 auto"
            >
              <v-img cover src="/public/qr.jpg"></v-img>
            </v-card-text>

            <v-card-actions>
              <v-btn @click="isShowQRCode = false" color="red">Đóng</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog> -->

        <!-- Dialog nếu chọn bàn -->
        <v-dialog
          v-model="showComponentAreaManagement"
          max-width="1080px"
          max-height="900px"
          persistent
        >
          <v-card>
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

  // Computed properties
  filteredFoodItems,
  resultTotalAmount,

  // Data objects
  listFoodCategorySelected,
  currentOrderItem,
  resultOrderItem,
  updateOrderItem,
  currentOrder,

  // Methods
  tonggleSelected,
  selectedFoodItemToCart,
  nonSelectedFoodItemToCart,
  totalAmountAdditionalFoodItem,
  formatCurrency,
  openDialogShowDetail,
  handleCloseComponentAreaManagement,
  handleCloseAndReset,
  updateCurrentFoodSelected,
  updateFoodItem,
  deleteCurrentFoodSelected,
  callApiOrderFood,
  callApiOrderFoodAndAddTable,
} = useFoodManagement();
</script>
