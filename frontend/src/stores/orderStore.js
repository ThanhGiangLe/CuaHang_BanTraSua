import { defineStore } from "pinia";
const STORAGE_KEY = "tableOrders";
export const userOrderStore = defineStore("order", {
  state: () => ({
    selectedDishes: {
      user_id: null,
      order_time: null,
      table_id: null,
      status: "",
      discount: 0,
      tax: 0,
      items: [],
    },
    tableOrders: JSON.parse(localStorage.getItem(STORAGE_KEY)) || {},
  }),

  actions: {
    setSelectedDishes(order) {
      this.selectedDishes = { ...order };
    },
    assignDishesToTable(tableId) {
      if (!tableId || this.selectedDishes.items.length == 0) {
        console.warn("Chưa chọn món ăn");
        return;
      }
      this.selectedDishes.table_id = tableId;

      const currentTableOrder = this.tableOrders[tableId];

      if (currentTableOrder) {
        this.selectedDishes.items.forEach((newDish) => {
          const existing = currentTableOrder.items.find(
            (d) =>
              d.FoodItemId === newDish.FoodItemId &&
              d.Note === newDish.Note &&
              JSON.stringify(d.ListAdditionalFood) ===
                JSON.stringify(newDish.ListAdditionalFood)
          );
          if (existing) {
            existing.Quantity += newDish.Quantity || 1;
          } else {
            currentTableOrder.items.push({ ...newDish });
          }
        });

        // Gộp các thông tin khác (tuỳ bạn muốn merge hay ghi đè)
        this.tableOrders[tableId] = {
          ...currentTableOrder,
          ...this.selectedDishes,
          order_time: this.selectedDishes.order_time,
          items: currentTableOrder.items,
        };
      } else {
        // Nếu chưa có thì lưu toàn bộ
        this.tableOrders[tableId] = {
          ...this.selectedDishes,
        };
      }
      this.persistTableOrders();
      // this.clearSelectedDishes();
    },

    getDishesByTable(tableId) {
      return (
        this.tableOrders[tableId] || {
          user_id: null,
          order_time: null,
          table_id: tableId,
          status: "",
          discount: 0,
          tax: 0,
          items: [],
        }
      );
    },

    removeDishFromTable(tableId, dishId) {
      if (!this.tableOrders[tableId]) return;

      this.tableOrders[tableId].items = this.tableOrders[tableId].items.filter(
        (d) => d.FoodItemId !== dishId
      );
      this.persistTableOrders();
    },

    updateDishQuantity(tableId, dishId, newQty) {
      if (!this.tableOrders[tableId]) return;
      const dish = this.tableOrders[tableId].items.find(
        (d) => d.FoodItemId === dishId
      );
      if (dish) {
        dish.Quantity = newQty;
        this.persistTableOrders();
      }
    },

    clearSelectedDishes() {
      this.selectedDishes = {
        user_id: null,
        order_time: null,
        table_id: null,
        total_amount: 0,
        result_total_amount: 0,
        status: "",
        discount: 0,
        tax: 0,
        items: [],
      };
    },

    clearTableOrder(tableId) {
      if (this.tableOrders[tableId]) {
        delete this.tableOrders[tableId];
        this.persistTableOrders();
      }
    },

    // ✅ Gọi mỗi khi thay đổi order để lưu vào localStorage
    persistTableOrders() {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(this.tableOrders));
    },

    // ✅ Clear toàn bộ localStorage nếu cần reset hệ thống
    clearAllOrders() {
      this.tableOrders = {};
      localStorage.removeItem(STORAGE_KEY);
    },
  },
});
