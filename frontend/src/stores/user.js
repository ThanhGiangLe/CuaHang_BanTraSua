import { defineStore } from "pinia";
import { toRaw } from "vue";

export const useUserStore = defineStore("user", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user")) || null,
  }),
  actions: {
    setUser(user) {
      const plainUser = toRaw(user);
      this.user = plainUser;
      localStorage.setItem("user", JSON.stringify(plainUser));
    },
    clearUser() {
      this.user = null;
      localStorage.removeItem("user");
    },
  },
});
