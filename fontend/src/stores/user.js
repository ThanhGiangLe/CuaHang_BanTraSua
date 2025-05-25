import { defineStore } from 'pinia';

export const useUserStore = defineStore('user', {
  state: () => ({
    user: JSON.parse(localStorage.getItem('user')) || null, // Khôi phục thông tin từ localStorage
  }),
  actions: {
    setUser(user) {
      this.user = user; // Cập nhật thông tin người dùng
      localStorage.setItem('user', JSON.stringify(user)); // Lưu thông tin vào localStorage
    },
    // Dùng để đăng xuất
    clearUser() {
      this.user = null; // Xóa thông tin người dùng
      localStorage.removeItem('user'); // Xóa thông tin khỏi localStorage
    },
  },
});
