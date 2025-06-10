const API_ENDPOINTS = {
  LOGIN: `/user/login`,
  CHECK_EMAIL_EXISTS: `/email/check`,
  UPDATE_PASSWORD: `/user/update-password`,
  GET_ALL_FOOD_CATEGORIES: `/food/get-all-category`,
  GET_ALL_FOOD_ITEMS: `/food/get-all-food-items`,
  GET_ALL_ADDITIONAL_FOODS: `/food/get-all-additional-food`,
  ADD_ORDER: `/food/add-order`,
  ADD_ORDER_DETAIL: `/food/add-order-detail`,
  GET_ALL_EMPLOYEES: `/user`, // Lấy danh sách nhân viên từ database
  ADD_USER: `/user/add`,
  DELETE_USER: `/user/delete-user`,
  UPDATE_USER: `/user/update-user`,
  GET_ALL_TABLE: `/area/get-all-table`,
  GET_ALL_REVENUE_BY_EMPLOYEE: `/report/total-revenue-employee`,
  GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME: `/report/total-revenue-employee-time`,
  GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH: `/report/total-revenue-employee-time-month`,
  GET_ALL_REVENUE_BY_CATEGORY: `/report/total-revenue-category`,
  GET_ALL_REVENUE_BY_CATEGORY_AND_TIME: `/report/total-revenue-category-time`,
  GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH: `/report/total-revenue-category-time-month`,
  GET_ALL_REVENUE_BY_TIME: `/report/total-revenue-time`,
  GET_ALL_REVENUE_BY_TIME_MONTH: `/report/total-revenue-time-month`,
  GET_ALL_ORDERITEM_BESTSELING: `/report/get-all-orderitem-bestseling`,
  GET_ALL_ORDERITEM_BESTSELING_CURRENTDAY: `/report/get-all-orderitem-bestseling-currentday`,
  GET_ALL_ORDERITEM_BESTSELING_CURRENTMONTH: `/report/get-all-orderitem-bestseling-currentmonth`,
  GET_ALL_ORDER: `/report/get-all-order`,
  GET_ALL_ORDER_CURRENT_DAY: `/report/get-all-order-currentday`,
  GET_ALL_ORDER_CURRENT_MONTH: `/report/get-all-order-currentmonth`,
  GET_ALL_ORDER_FULL_NAME: `/report/get-all-order-fullname`,
  ADD_FOOD_ITEM: `/food/add-food-item`,
  DELETE_FOOD_ITEM: `/food/delete-food-item`,
  UPDATE_FOOD_ITEM: `/food/update-food-item`,

  GET_ALL_MATERIALS: `/Inventory/get-all-material`,
  UPDATE_QUANTITY_MATERIAL: `/Inventory/update-quantity-material`,
  CANCEL_ALL_GOODS: `/Inventory/cancel-all-goods`,
  CANCEL_ALL_GOODS_DETAIL: `/Inventory/cancel-all-goods-detail`,
  IMPORT_WAREHOUSE_HISTORY: `/Inventory/import-warehouse-history`,
  GET_WAREHOUSE_HISTORY_BY_MATERIALID: `/Inventory/get-warehouse-history`,

  UPDATE_QUANTITY_MATERIALS_AFTER_ORDER: `/Material/update-quantity-materials-after-order`,

  IMPORT_DATA_TRAINING: `/chatbot/import-data-training`,

  GET_ALL_CASH_REGISTER: `/CashRegister/get-all-cash-register`,
  IMPORT_CASH_REGISTER: `/CashRegister/insert-cash-register`,
  UPDATE_ENDTIME_CASH_REGISTER: `/CashRegister/update-endtime-cash-register`,
  UPDATE_TOTALINCOME_CASH_REGISTER: `/CashRegister/update-totalimcome-cash-register`,

  GET_SCHEDULE_BY_USERID: "/schedule/get-schedule-by-userid",
  UPDATE_SCHEDULE_BY_USERID: "/schedule/update-schedule-by-userid",
};

export default API_ENDPOINTS;
