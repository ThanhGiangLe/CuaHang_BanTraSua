import { defineStore } from "pinia";
export const useShiftStore = defineStore("shift", {
  state: () => ({
    isShiftOpened: false,
    openingCashAmount: 0,
    userId: null,
  }),
  actions: {
    openShiftState(userId, amount) {
      this.isShiftOpened = true;
      this.openingCashAmount = amount;
      this.userId = userId;
      this.saveToStorage();
    },
    closeShiftState() {
      this.isShiftOpened = false;
      this.openingCashAmount = 0;
      this.userId = null;
      this.saveToStorage();
    },
    saveToStorage() {
      localStorage.setItem(
        "shift",
        JSON.stringify({
          isShiftOpened: this.isShiftOpened,
          openingCashAmount: this.openingCashAmount,
          userId: this.userId,
        })
      );
    },
    restoreFromStorage() {
      const saved = localStorage.getItem("shift");
      if (saved) {
        const parse = JSON.parse(saved);
        this.isShiftOpened = parse.isShiftOpened;
        this.openingCashAmount = parse.openingCashAmount;
        this.userId = parse.userId;
      }
    },
  },
});
