// src/stores/app.js
import { defineStore } from "pinia";

export const useGlobalStore = defineStore("global", {
  state: () => ({
    globalVariableCurrentPage: "0", // Đảm bảo giá trị này là chuỗi
  }),
  actions: {
    updateGlobalVariableCurrentPage(newValue) {
      this.globalVariableCurrentPage = newValue; // Cập nhật giá trị
    },
  },
});
