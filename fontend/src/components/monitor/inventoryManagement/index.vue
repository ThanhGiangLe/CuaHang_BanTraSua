<template>
  <div
    class="reportManagement_InventoryItem d-flex flex-wrap flex-column pa-2 rounded"
  >
    <v-card style="height: 100%">
      <v-card-title class="pa-0 mb-2 d-flex justify-center">
        <div class="d-flex align-center">
          <v-icon class="ma-1" size="large">mdi-warehouse</v-icon>
          <span style="font-size: 26px">Các mặt hàng còn lại trong kho</span>
        </div>
        <!-- <JsonExcel class="btn btn-default" 
                    :data="dataTable" 
                    :fields="datafieldExcel" 
                    worksheet="My Worksheet" type="xlsx"
                    :name="nameFileExcel">
                    <VBtn class="text-none" size="small" prependIcon="mdi-crop" color="#8690A0">
                    Xuất Excel
                    </VBtn>
                </JsonExcel> -->
      </v-card-title>
      <v-card-text
        class="pa-3 rounded"
        :style="{ backgroundColor: 'var(--bg-color-item)' }"
      >
        <div
          class="reportManagement_chosseTime d-flex justify-md-space-between align-center flex-wrap mb-2"
        >
          <v-btn
            style="border: 1px solid #333; min-width: 60px"
            size="small"
            v-if="!isFilter"
            @click="isFilter = !isFilter"
          >
            Chọn điều kiện lọc
          </v-btn>
          <div v-else>
            <!-- Lọc theo Thực phẩm -->
            <v-btn
              style="border: 1px solid #333"
              size="small"
              class="me-2"
              @click="filterMaterialsOfFood()"
            >
              Thực phẩm
            </v-btn>
            <!-- Lọc theo Nguyên liệu -->
            <v-btn
              style="border: 1px solid #333"
              size="small"
              class="me-2"
              @click="filterMaterialsOfIngredient()"
            >
              Nguyên liệu
            </v-btn>
            <!-- Lọc theo Gia vị -->
            <v-btn
              style="border: 1px solid #333"
              size="small"
              class="me-2"
              @click="filterMaterialsOfSpice()"
            >
              Gia vị
            </v-btn>
            <!-- Lọc theo Sắp hến hạn -->
            <v-btn
              style="border: 1px solid #333"
              title="Còn sử dụng được trong vòng 3 ngày"
              size="small"
              class="me-2"
              @click="filterMaterialsOfAlmostOver()"
            >
              Sắp hết hạn
            </v-btn>
            <!-- Lọc theo Đã hến hạn -->
            <v-btn
              style="border: 1px solid #333"
              size="small"
              class="me-2"
              @click="filterMaterialsOfOver()"
            >
              Hết hạn sử dụng
            </v-btn>
            <!-- Lọc theo Sắp hết hàng -->
            <v-btn
              style="border: 1px solid #333"
              size="small"
              class="me-2"
              @click="filterMaterialsOfAlmostSoldOut()"
            >
              Sắp hết hàng
            </v-btn>
            <!-- Lọc theo Đã hết hàng -->
            <v-btn
              style="border: 1px solid #333"
              size="small"
              @click="filterMaterialsOfSoldOut()"
            >
              Hết hàng
            </v-btn>
          </div>
          <v-btn
            style="border: 1px solid #333; min-width: 60px"
            size="small"
            class="ms-5"
            @click="resetTimeFillterRevenueOrder"
          >
            Làm mới
          </v-btn>
        </div>
        <div
          class="reportManagement_totalAmount_salesSummary_bestSellingItems d-flex"
          style="height: 450px; max-height: 450px; overflow-y: auto"
        >
          <v-data-table
            :headers="header"
            :loading="loading"
            :items="materialsFilter"
            height="calc(33vh - 2rem)"
            density="compact"
            fixed-footer
            fixed-header
          >
            <template v-slot:item.materialName="{ item }">
              <span
                style="font-weight: 500; color: rgba(var(--v-theme-primary), 1)"
                class="cursor-pointer"
                :class="{ 'opacity-50': item.quantity <= 1 }"
                @click="showDetailMaterialHistory(item)"
              >
                {{ item.materialName ? item.materialName : "-" }}
              </span>
            </template>
            <template v-slot:item.quantity="{ item }">
              <span
                style="padding: 4px 12px; border-radius: 25px"
                :class="{
                  'bg-green-lighten-1': item.quantity > item.minQuantity,
                  'bg-yellow-lighten-1':
                    item.quantity <= item.minQuantity && item.quantity > 1,
                  'bg-red-lighten-1': item.quantity <= 1,
                  'opacity-50': item.quantity <= 1,
                }"
              >
                {{ item.quantity ? item.quantity.toFixed(3) : "0" }}
              </span>
            </template>
            <template v-slot:item.unit="{ item }">
              <span
                class="text-capitalize"
                :class="{ 'opacity-50': item.quantity <= 1 }"
              >
                {{ item.unit ? item.unit : "-" }}
              </span>
            </template>
            <template v-slot:item.importDate="{ item }">
              <span :class="{ 'opacity-50': item.quantity <= 1 }">
                {{
                  item.importDate
                    ? formatDateFormApiToView(item.importDate)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.expirationDate="{ item }">
              <span :class="{ 'opacity-50': item.quantity <= 1 }">
                {{
                  item.expirationDate
                    ? formatDateFormApiToView(item.expirationDate)
                    : "-"
                }}</span
              >
            </template>
            <template v-slot:item.added="{ item }" class="d-flex align-center">
              <v-btn
                style="border: 1px solid #333"
                size="small"
                color="light-blue-accent-4"
                v-if="!item.isEditing"
                @click="showDisplayInputAddedValues(item)"
                >Nhập thêm</v-btn
              >
              <div class="d-flex align-center" v-else>
                <input
                  type="number"
                  style="border: 1px solid #333; max-width: 100px"
                  class="me-2 px-2"
                  placeholder="Số lượng"
                  v-model="item.quantityAdded"
                />
                <v-btn
                  style="border: 1px solid #333"
                  size="small"
                  color="light-blue-accent-4"
                  @click="addQuantityInItemAndCallAPIUpdate(item)"
                  >Nhập</v-btn
                >
              </div>
            </template>
            <template v-slot:item.cancel="{ item }">
              <v-btn
                style="border: 1px solid #333"
                size="small"
                color="red-accent-3"
                v-if="checkExpirationDate(item)"
                @click="cancelAllGoods(item)"
                >Hủy hàng</v-btn
              >
            </template>

            <template v-slot:loading>
              <v-skeleton-loader type="table-row@5"></v-skeleton-loader>
            </template>
            <template v-slot:no-data>
              <div
                class="d-event-info-item d-emp-activity-item-content d-emp-activity-no-data pa-6"
                style="background: none"
              >
                <!-- <VIcon icon="mdi-robot-dead-outline"></VIcon> -->
                <span>Hệ thống không tìm thấy thông tin</span>
              </div>
            </template>
          </v-data-table>
        </div>
      </v-card-text>

      <v-dialog v-model="displayWarehouseHistoryOfMaterial" max-width="1280px">
        <v-card>
          <v-card-title class="d-flex justify-space-between align-center">
            <span class="">Lịch Sử Nhập Kho</span>
            <v-btn icon @click="displayWarehouseHistoryOfMaterial = false">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-card-title>

          <v-card-text>
            <div
              class="reportManagement_totalAmount_salesSummary_bestSellingItems d-flex"
              style="height: 450px; max-height: 450px; overflow-y: auto"
            >
              <v-data-table
                :headers="headerDetail"
                :loading="loading"
                :items="warehouseHistoryOfMaterialIsChoose"
                height="calc(33vh - 2rem)"
                density="compact"
                fixed-footer
                fixed-header
              >
                <template v-slot:item.materialName="{ item }">
                  <span
                    style="
                      font-weight: 500;
                      color: rgba(var(--v-theme-primary), 1);
                    "
                    class="cursor-pointer"
                    :class="{ 'opacity-50': item.currentQuantity <= 1 }"
                    @click="showDetailMaterialHistory(item)"
                  >
                    {{ item.materialName ? item.materialName : "-" }}
                  </span>
                </template>
                <template v-slot:item.quantity="{ item }">
                  <span :class="{ 'opacity-50': item.currentQuantity <= 1 }">
                    {{ item.quantity ? item.quantity.toFixed(3) : "0" }}
                  </span>
                </template>
                <template v-slot:item.currentQuantity="{ item }">
                  <span :class="{ 'opacity-50': item.currentQuantity <= 1 }">
                    {{
                      item.currentQuantity
                        ? item.currentQuantity.toFixed(3)
                        : "0"
                    }}
                  </span>
                </template>
                <template v-slot:item.unit="{ item }">
                  <span
                    class="text-capitalize"
                    :class="{ 'opacity-50': item.currentQuantity <= 1 }"
                  >
                    {{ item.unit ? item.unit : "-" }}
                  </span>
                </template>
                <template v-slot:item.importPrice="{ item }">
                  <span
                    class="text-capitalize"
                    :class="{ 'opacity-50': item.currentQuantity <= 1 }"
                  >
                    {{ formatCurrency(item.importPrice) }} VNĐ
                  </span>
                </template>
                <template v-slot:item.money="{ item }">
                  <span
                    class="text-capitalize"
                    :class="{ 'opacity-50': item.currentQuantity <= 1 }"
                  >
                    {{ formatCurrency(item.quantity * item.importPrice) }} VNĐ
                  </span>
                </template>
                <template v-slot:item.importDate="{ item }">
                  <span :class="{ 'opacity-50': item.currentQuantity <= 1 }">
                    {{
                      item.importDate
                        ? formatDateFormApiToView(item.importDate)
                        : "-"
                    }}
                  </span>
                </template>
                <template v-slot:item.expirationDate="{ item }">
                  <span :class="{ 'opacity-50': item.currentQuantity <= 1 }">
                    {{
                      item.expirationDate
                        ? formatDateFormApiToView(item.expirationDate)
                        : "-"
                    }}</span
                  >
                </template>
                <template v-slot:item.cancel="{ item }">
                  <v-btn
                    style="border: 1px solid #333"
                    size="small"
                    color="red-accent-3"
                    v-if="checkExpirationDate(item)"
                    @click="cancelAllGoodsDetail(item)"
                    >Hủy hàng</v-btn
                  >
                </template>
                <template v-slot:loading>
                  <v-skeleton-loader type="table-row@5"></v-skeleton-loader>
                </template>
                <template v-slot:no-data>
                  <div
                    class="d-event-info-item d-emp-activity-item-content d-emp-activity-no-data pa-6"
                    style="background: none"
                  >
                    <!-- <VIcon icon="mdi-robot-dead-outline"></VIcon> -->
                    <span>Hệ thống không tìm thấy thông tin</span>
                  </div>
                </template>
              </v-data-table>
            </div>
          </v-card-text>
        </v-card>
      </v-dialog>
    </v-card>
  </div>
</template>

<script setup>
import useInventoryManagement from "./inventoryManagement";

const {
  loading,
  materials,
  materialsFilter,
  header,
  headerDetail,
  isFilter,
  displayWarehouseHistoryOfMaterial,
  warehouseHistoryOfMaterialIsChoose,

  formatDateFormApiToView,
  filterMaterialsOfFood,
  filterMaterialsOfIngredient,
  filterMaterialsOfSpice,
  filterMaterialsOfOver,
  filterMaterialsOfAlmostOver,
  filterMaterialsOfAlmostSoldOut,
  filterMaterialsOfSoldOut,
  showDisplayInputAddedValues,
  addQuantityInItemAndCallAPIUpdate,
  resetTimeFillterRevenueOrder,
  checkExpirationDate,
  cancelAllGoods,
  cancelAllGoodsDetail,
  showDetailMaterialHistory,
  formatCurrency,
} = useInventoryManagement();
</script>
