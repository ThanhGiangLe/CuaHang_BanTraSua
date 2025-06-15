const API_ENDPOINTS = {
  // Login
  LOGIN: `/user/login`,

  // Forgot password
  CHECK_EMAIL_EXISTS: `/authen/check`,
  UPDATE_PASSWORD: `/user/update-password`,

  // List food
  GET_ALL_FOOD_ITEMS: `/food/get-all-food-items`,
  GET_ALL_FOOD_CATEGORIES: `/food/get-all-category`,
  ADD_ORDER: `/food/add-order`,
  ADD_ORDER_DETAIL: `/food/add-order-detail`,
  TOP_BEST_SELLING: "/food/get-all-orderitem-top-bestselling",
  ADDITION_POINT: "/food/addition-point-by-userid",

  // Food managament
  CREATE_FOOD_ITEM: `/food/add-food-item`,
  DELETE_FOOD_ITEM: `/food/delete-food-item`,
  UPDATE_FOOD_ITEM: `/food/update-food-item`,

  // Area management
  GET_ALL_TABLE: `/area/get-all-table`,

  // Report management
  GET_ALL_REVENUE_BY_TIME_MONTH: `/report/total-revenue-time-month`,
  GET_ALL_REVENUE_BY_TIME: `/report/total-revenue-time`,
  GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH: `/report/total-revenue-employee-time-month`,
  GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH: `/report/total-revenue-category-time-month`,
  GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME: `/report/total-revenue-employee-time`,
  GET_ALL_REVENUE_BY_CATEGORY_AND_TIME: `/report/total-revenue-category-time`,
  GET_ALL_ORDERITEM_BESTSELING_CURRENTDAY: `/report/get-all-orderitem-bestseling-currentday`,
  GET_ALL_ORDERITEM_BESTSELING_CURRENTMONTH: `/report/get-all-orderitem-bestseling-currentmonth`,
  GET_ALL_ORDER_CURRENT_MONTH: `/report/get-all-order-currentmonth`,
  GET_ALL_ORDER_CURRENT_DAY: `/report/get-all-order-currentday`,
  GET_ALL_ORDER_FULL_NAME: `/report/get-all-order-fullname`,
  GET_ALL_REVENUE_BY_EMPLOYEE: `/report/total-revenue-employee`, // Không dùng
  GET_ALL_REVENUE_BY_CATEGORY: `/report/total-revenue-category`, // Không dùng
  GET_ALL_ORDERITEM_BESTSELING: `/report/get-all-orderitem-bestseling`, // Không dùng
  GET_ALL_ORDER: `/report/get-all-order`, // Không dùng

  // Total sales
  GET_TOTALSALES_ALL_EMPLOYEE: "/totalsales/get-total-sales-all-employee",
  GET_TOTALSALES_BY_DAY_OR_USERID:
    "/totalsales/get-total-sales-by-day-or-userid",

  // Employee management
  GET_ALL_EMPLOYEES: `/user`,
  ADD_USER: `/user/add`,
  GET_SCHEDULE_BY_USERID: "/schedule/get-schedule-by-userid",
  UPDATE_USER: `/user/update-user`,
  DELETE_USER: `/user/delete-user`,
  UPDATE_SCHEDULE_BY_USERID: "/schedule/update-schedule-by-userid",
};

export default API_ENDPOINTS;
