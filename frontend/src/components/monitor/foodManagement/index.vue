<template>
  <div class="foodManagement mt-1 d-flex flex-column">
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
      <v-btn
        color="blue-lighten-1"
        class="ms-5"
        @click="showDialogAdd = !showDialogAdd"
        >Thêm món mới</v-btn
      >

      <!-- Dialog thêm món ăn mới -->
      <v-dialog v-model="showDialogAdd" max-width="960px">
        <v-card class="pa-2">
          <v-card-title> Thông tin món mới </v-card-title>
          <v-card-text class="py-0">
            <v-text-field
              label="Tên món"
              v-model="foodAdd.foodName"
              :rules="[(v) => !!v || 'Trường này là bắt buộc']"
            ></v-text-field>
            <div class="d-flex justify-space-between ga-3">
              <v-text-field
                label="Giá niêm yết"
                v-model="foodAdd.priceListed"
                :rules="[(v) => !!v || 'Trường này là bắt buộc']"
              ></v-text-field>
              <v-text-field
                label="Giá bán ra"
                v-model="foodAdd.priceCustom"
                :rules="[(v) => !!v || 'Trường này là bắt buộc']"
              ></v-text-field>
            </div>
            <div class="d-flex justify-space-between ga-3">
              <v-text-field
                label="Điểm quy đổi"
                v-model="foodAdd.point"
                :rules="[(v) => !!v || 'Trường này là bắt buộc']"
              ></v-text-field>
              <v-text-field
                label="Đơn vị tính"
                v-model="foodAdd.unit"
                :rules="[(v) => !!v || 'Trường này là bắt buộc']"
              ></v-text-field>
            </div>
            <div class="d-flex justify-space-between ga-3">
              <v-select
                label="Loại danh mục"
                :items="_.pluck(categoriesDataDefault, 'categoryName')"
                v-model="foodAdd.categoryIdString"
                :rules="[(v) => !!v || 'Trường này là bắt buộc']"
              />
              <v-select
                label="Phân loại"
                :items="['Món chính', 'Món thêm']"
                v-model="foodAdd.isMainString"
                :rules="[(v) => !!v || 'Trường này là bắt buộc']"
              />
            </div>
            <v-file-input
              label="Hình ảnh món"
              accept="image/*"
              v-model="foodAdd.imageUrl"
              prepend-icon="mdi-camera"
              :show-size="true"
              :rules="[(v) => !!v || 'Trường này là bắt buộc']"
            ></v-file-input>
          </v-card-text>
          <v-card-actions>
            <v-btn color="red darken-1" text @click="cancelSaveFood">Hủy</v-btn>
            <v-btn color="green darken-1" text @click="saveFood">Thêm</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>
    <div
      class="foodManagement_listFoodOrder flex-1-0 mt-3 d-flex justify-space-between"
      style="margin-bottom: 52px"
    >
      <div
        class="foodManagement_listFoodOrder_menu rounded"
        :style="{ backgroundColor: 'var(--bg-color-item)' }"
        v-if="!loading && foodCategories && filteredFoodItems"
      >
        <div
          class="foodManagement_listFoodOrder_menu_dishes d-flex justify-center flex-wrap mt-2"
        >
          <v-chip
            class="ma-1 foodManagement_listFoodOrder_menu_dishes_item"
            label
            v-for="foodCategory in foodCategories"
            :key="foodCategory.categoryId"
            :type="foodCategory.categoryId"
            :class="{ selected: listDashSelected.includes(foodCategory) }"
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

        <div class="foodManagement_listFoodOrder_menu_foods">
          <v-container>
            <v-row class="pa-2" gutters="16">
              <v-col
                v-for="foodItem in filteredFoodItems"
                :key="foodItem.foodItemId"
                :cols="12"
                :lg="4"
                :md="6"
                :sm="6"
                :xs="12"
                class="pa-2"
              >
                <div
                  class="foodManagement_listFoodOrder_menu_foods_item rounded-lg pa-4 position-relative align-start"
                >
                  <!-- Ảnh món ăn -->
                  <img
                    :src="foodItem.imageUrl"
                    alt="Food Item"
                    class="foodManagement_listFoodOrder_menu_foods_item_img rounded-lg"
                    style="width: 75%; height: 160px; margin: 0 auto"
                  />
                  <!-- Thông tin món ăn -->
                  <div class="d-flex flex-column mt-3" style="gap: 8px">
                    <!-- <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Mã món ăn:
                      </div>
                      <div>{{ foodItem.foodItemId }}</div>
                    </div> -->

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Tên món ăn:
                      </div>
                      <div
                        style="height: 16.56px; line-height: 16.56px"
                        class="hiddent-text-one-line"
                      >
                        {{ foodItem.foodName }}
                      </div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Giá niêm yết:
                      </div>
                      <div
                        style="height: 19.94px; max-height: 19.94px"
                        class="hiddent-text-one-line"
                      >
                        {{ formatCurrency(foodItem.priceListed) }}/{{
                          foodItem.unit
                        }}
                      </div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Giá bán ra:
                      </div>
                      <div
                        style="height: 19.94px; max-height: 19.94px"
                        class="hiddent-text-one-line"
                      >
                        {{ formatCurrency(foodItem.priceCustom) }}/{{
                          foodItem.unit
                        }}
                      </div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Điểm quy đổi:
                      </div>
                      <div
                        style="height: 19.94px; max-height: 19.94px"
                        class="hiddent-text-one-line"
                      >
                        {{ formatPoint(foodItem.point) }}/{{ foodItem.unit }}
                      </div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Loại danh mục:
                      </div>
                      <div>{{ getNameByIdCategory(foodItem.categoryId) }}</div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Tình trạng món:
                      </div>
                      <div>{{ foodItem.status }}</div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">Món:</div>
                      <div>
                        {{ getNameByIdMain(foodItem.isMain) }}
                      </div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Thời gian tạo:
                      </div>
                      <div>{{ formatDate(foodItem.createDate) }}</div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Người tạo:
                      </div>
                      <div>{{ foodItem.createBy }}</div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Thời gian cập nhật:
                      </div>
                      <div>{{ formatDate(foodItem.updateDate) }}</div>
                    </div>

                    <div class="d-flex align-center">
                      <div style="min-width: 130px; color: #666">
                        Người cập nhật:
                      </div>
                      <div>{{ foodItem.updateBy }}</div>
                    </div>
                  </div>
                  <!-- Buttons -->
                  <div
                    class="d-flex flex-wrap justify-center align-center w-100 mt-2"
                  >
                    <v-btn
                      class="foodManagement_listFoodOrder_menu_foods_item_addFood me-2"
                      size="x-small"
                      style="height: 28px; min-width: 100px"
                      color="blue-darken-4"
                      @click="openDialogShowUpdateFoodItemSelected(foodItem)"
                    >
                      Thay đổi
                    </v-btn>

                    <v-btn
                      class="foodManagement_listFoodOrder_menu_foods_item_addFood ms-2"
                      size="x-small"
                      style="height: 28px; min-width: 80px"
                      color="red-darken-4"
                      @click="openDialogShowDeleteFoodItemSelected(foodItem)"
                    >
                      Xóa
                    </v-btn>
                  </div>
                </div>
              </v-col>
            </v-row>

            <!-- Dialog xóa món -->
            <v-dialog v-model="modalConfirmDeleteFoodItem" max-width="800px">
              <v-card>
                <v-card-title class="headline">Thông tin món</v-card-title>

                <v-card-text class="px-4 py-2">
                  <div class="d-flex justify-space-between align-center">
                    <div class="d-flex align-center w-100">
                      <img
                        :src="currentOrderItem.Image"
                        alt=""
                        style="width: 100px; height: 100px"
                        class="rounded me-2"
                      />
                      <div
                        class="ml-4 d-flex flex-column ustify-space-between w-100"
                      >
                        <span>{{ currentOrderItem.FoodName }}</span>
                        <span>
                          {{ formatCurrency(currentOrderItem.Price) }}
                        </span>
                      </div>
                    </div>
                  </div>

                  <v-divider class="my-4"></v-divider>

                  <div class="w-100 text-center">
                    <h4>Bạn có chắc chắn muốn xóa món ăn này?</h4>
                    <div
                      class="AdditionalItems d-flex flex-wrap"
                      style="max-height: 175px; overflow-y: auto"
                    ></div>
                  </div>
                </v-card-text>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="red darken-1"
                    text
                    @click="
                      modalConfirmDeleteFoodItem = !modalConfirmDeleteFoodItem
                    "
                    >Hủy</v-btn
                  >
                  <v-btn
                    color="green darken-1"
                    text
                    @click="confimDeleteFoodItem(currentOrderItem)"
                    >Xác Nhận</v-btn
                  >
                </v-card-actions>
              </v-card>
            </v-dialog>

            <!-- Dialog cập nhật món -->
            <v-dialog v-model="modalUpdateFoodItem" max-width="800px">
              <v-card>
                <v-card class="pa-2">
                  <v-card-title> Thông tin món ăn </v-card-title>
                  <v-card-text>
                    <v-text-field
                      label="Tên món"
                      v-model="foodItemCurrentUpdate.foodName"
                    ></v-text-field>
                    <div class="d-flex justify-space-between ga-3">
                      <v-text-field
                        label="Giá mặc định"
                        v-model="foodItemCurrentUpdate.priceListed"
                      ></v-text-field>
                      <v-text-field
                        label="Giá bán"
                        v-model="foodItemCurrentUpdate.priceCustom"
                      ></v-text-field>
                    </div>
                    <div class="d-flex justify-space-between ga-3">
                      <v-text-field
                        label="Điểm quy đổi"
                        v-model="foodItemCurrentUpdate.point"
                      ></v-text-field>
                      <v-text-field
                        label="Đơn vị"
                        v-model="foodItemCurrentUpdate.unit"
                      ></v-text-field>
                    </div>
                    <div class="d-flex justify-space-between ga-3">
                      <v-select
                        label="Loại danh mục"
                        :items="_.pluck(categoriesDataDefault, 'categoryName')"
                        v-model="foodItemCurrentUpdate.categoryIdString"
                      />
                      <v-select
                        label="Phân loại"
                        :items="['Món chính', 'Món thêm']"
                        v-model="foodItemCurrentUpdate.isMainString"
                      />
                    </div>
                    <v-file-input
                      label="Hình ảnh món"
                      accept="image/*"
                      v-model="foodItemCurrentUpdate.imageUrl"
                      prepend-icon="mdi-camera"
                      :show-size="true"
                    ></v-file-input>
                  </v-card-text>
                </v-card>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="red darken-1"
                    text
                    @click="cancelConfirmUpdateFoodItem"
                    >Hủy</v-btn
                  >
                  <v-btn
                    color="green darken-1"
                    text
                    @click="confirmUpdateFoodItem(foodItemCurrentUpdate)"
                    >Xác Nhận</v-btn
                  >
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-container>
        </div>
      </div>
      <div v-else class="pa-4" style="width: 100%">
        <v-row>
          <!-- Skeleton cho food categories -->
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

          <!-- Skeleton cho food items -->
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
    </div>
  </div>
</template>

<script setup>
import useFoodManagement from "./foodManagement.js";
import AreaManagement from "@/components/monitor/areaManagement/index.vue";
import _ from "underscore";

const {
  // State variables
  showDialogAdd,
  foodCategories,
  search,
  modalConfirmDeleteFoodItem,
  modalUpdateFoodItem,
  foodItemCurrentUpdate,
  loading,

  // Computed properties
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
} = useFoodManagement();
</script>
