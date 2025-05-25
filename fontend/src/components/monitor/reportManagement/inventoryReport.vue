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
          class="reportManagement_chosseTime d-flex justify-md-space-between mb-2"
        >
          <div>
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
              size="small"
              class="me-2"
              @click="filterMaterialsOfAlmostOver()"
            >
              Sắp hết hạn
            </v-btn>
            <!-- Lọc theo Sắp hết hàng -->
            <v-btn
              style="border: 1px solid #333"
              size="small"
              @click="filterMaterialsOfSoldOut()"
            >
              Sắp hết hàng
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
                {{ item.quantity ? item.quantity.toFixed(2) : "-" }}
              </span>
            </template>
            <template v-slot:item.unit="{ item }">
              <span class="text-capitalize">
                {{ item.unit ? item.unit : "-" }}
              </span>
            </template>
            <template v-slot:item.importDate="{ item }">
              <span>
                {{
                  item.importDate
                    ? formatDateFormApiToView(item.importDate)
                    : "-"
                }}
              </span>
            </template>
            <template v-slot:item.expirationDate="{ item }">
              <span>
                {{
                  item.expirationDate
                    ? formatDateFormApiToView(item.expirationDate)
                    : "-"
                }}</span
              >
            </template>
            <!-- <template v-slot:item.added="{ item }">
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
            </template> -->

            <!-- <template v-slot:item.quantitySold="{ item }">
              <div class="d-user-contract-inventory-chart-progress-bar">
                <div
                  class="d-user-contract-inventory-chart-progress-bar-container"
                >
                  <div
                    class="d-user-contract-inventory-chart-progress-bar-indicator"
                    :style="{
                      width: (item.quantitySold / quantitySoldMax) * 100 + '%',
                    }"
                  ></div>
                </div>
                <div class="d-user-contract-inventory-chart-progress-bar-text">
                  {{ item.quantitySold ? item.quantitySold : 0 }}
                </div>
              </div>
            </template> -->
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
  </div>
</template>

<script setup>
import { ref } from "vue";
import axios from "axios";
import API_ENDPOINTS from "@/api/api.js";
import "underscore";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
const loading = shallowRef(true);
const materials = ref([]);
const materialsFilter = ref([]);

const header = ref([
  { title: "Tên gọi", key: "materialName" },
  { title: "Số lượng", key: "quantity" },
  { title: "Đơn vị tính", key: "unit" },
  { title: "Ngày nhập", key: "importDate" },
  { title: "Ngày hết hạn", key: "expirationDate" },
]);

async function init() {
  const response = await axios.get(API_ENDPOINTS.GET_ALL_MATERIALS);
  materials.value = response.data;
  materials.value = materials.value.map((item) => ({
    ...item,
    quantityAdded: 0,
    isEditing: false,
  }));
  materials.value.sort((a, b) => a.materialName.localeCompare(b.materialName));
  materialsFilter.value = materials.value;

  loading.value = false;
}
init();
const formatDateFormApiToView = (inputDate) => {
  const date = new Date(inputDate);
  const formattedDate = `${date.getDate().toString().padStart(2, "0")}/${(
    date.getMonth() + 1
  )
    .toString()
    .padStart(2, "0")}/${date.getFullYear()} ${date
    .getHours()
    .toString()
    .padStart(2, "0")}:${date.getMinutes().toString().padStart(2, "0")}:${date
    .getSeconds()
    .toString()
    .padStart(2, "0")}`;
  return formattedDate;
};

const filterMaterialsOfFood = () => {
  materialsFilter.value = materials.value.filter(
    (item) => item.materialType.toLowerCase() === "thực phẩm"
  );
};

const filterMaterialsOfIngredient = () => {
  materialsFilter.value = materials.value.filter(
    (item) => item.materialType.toLowerCase() === "nguyên liệu"
  );
};

const filterMaterialsOfSpice = () => {
  materialsFilter.value = materials.value.filter(
    (item) => item.materialType.toLowerCase() === "gia vị"
  );
};

const filterMaterialsOfAlmostOver = () => {
  materialsFilter.value = materials.value.filter((item) => {
    const importDate = new Date(item.importDate);
    const expirationDate = new Date(item.expirationDate);

    importDate.setDate(importDate.getDate() + 1); // Tăng số ngày nhập vào một ngày sẽ bằng ngày hết hạn
    // Hiện tại thời gian hết hạn là một ngày sau khi nhập

    return (
      importDate.toISOString().split("T")[0] ===
      expirationDate.toISOString().split("T")[0]
    );
  });
};

const filterMaterialsOfSoldOut = () => {
  materialsFilter.value = materials.value.filter(
    (item) => item.quantity <= item.minQuantity
  );
};

function showDisplayInputAddedValues(item) {
  item.isEditing = !item.isEditing;
}

async function addQuantityInItemAndCallAPIUpdate(item) {
  try {
    const response = await axios.post(API_ENDPOINTS.UPDATE_QUANTITY_MATERIAL, {
      MaterialId: item.materialId,
      AddedQuantity: item.quantityAdded,
    });

    if (response.data.message && response.data.message.trim() !== "") {
      item.quantity = item.quantity + item.quantityAdded;
      item.quantityAdded = 0;
      toast.success("Thêm thành công!", {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false, // Hiện thanh tiến trình
        closeOnClick: true, // Đóng khi nhấp vào thông báo
        pauseOnHover: true, // Dừng khi di chuột lên thông báo
        draggable: true, // Kéo thông báo
        progress: undefined, // Tiến độ (nếu có)
      });
    }
  } catch (error) {
    console.error("API Error:", error);
    toast.error("Thêm thất bại!", {
      position: "top-right",
      autoClose: 3000,
      hideProgressBar: false, // Hiện thanh tiến trình
      closeOnClick: true, // Đóng khi nhấp vào thông báo
      pauseOnHover: true, // Dừng khi di chuột lên thông báo
      draggable: true, // Kéo thông báo
      progress: undefined, // Tiến độ (nếu có)
    });
  }
  item.isEditing = !item.isEditing;
}

const resetTimeFillterRevenueOrder = () => {
  init();
};
</script>
