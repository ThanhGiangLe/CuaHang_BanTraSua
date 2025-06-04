import { defineStore } from "pinia";
import { toRaw } from "vue";

export const useUserStore = defineStore("user", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user")) || null, // Khôi phục thông tin từ localStorage
  }),
  actions: {
    setUser(user) {
      const plainUser = toRaw(user); // Lấy object thuần, loại bỏ reactive proxy
      this.user = plainUser;
      localStorage.setItem("user", JSON.stringify(plainUser));
    },
    // Dùng để đăng xuất
    clearUser() {
      this.user = null; // Xóa thông tin người dùng
      localStorage.removeItem("user"); // Xóa thông tin khỏi localStorage
    },
  },
});
