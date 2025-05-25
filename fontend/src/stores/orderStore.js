import { defineStore } from "pinia";
const STORAGE_KEY = "tableOrders";
export const userOrderStore = defineStore("order", {
  state: () => ({
    selectedDishes: {
      user_id: null,
      order_time: null,
      table_id: null,
      total_amount: 0,
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
      if (!this.tableOrders[tableId]) {
        this.tableOrders[tableId] = [];
      }
      this.selectedDishes.items.forEach((newDish) => {
        const existing = this.tableOrders[tableId].find(
          (d) => d.FoodItemId == newDish.FoodItemId
        );
        if (existing) {
          existing.Quantity += newDish.Quantity || 1;
        } else {
          this.tableOrders[tableId].push({ ...newDish });
        }
      });
      this.persistTableOrders();
      // this.clearSelectedDishes();
    },

    getDishesByTable(tableId) {
      return this.tableOrders[tableId] || [];
    },

    removeDishFromTable(tableId, dishId) {
      if (!this.tableOrders[tableId]) return;

      this.tableOrders[tableId] = this.tableOrders[tableId].filter(
        (d) => d.id !== dishId
      );
      this.persistTableOrders();
    },

    updateDishQuantity(tableId, dishId, newQty) {
      if (!this.tableOrders[tableId]) return;
      const dish = this.tableOrders[tableId].find((d) => d.id === dishId);
      if (dish) {
        dish.quantity = newQty;
        this.persistTableOrders();
      }
    },

    clearSelectedDishes() {
      this.selectedDishes = {
        user_id: null,
        order_time: null,
        table_id: null,
        total_amount: 0,
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
