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
  GET_ORDERDETAILS_BY_ORDERID: "/report/get-orderdetails-by-orderid",

  GET_REVENUE_ORDER_BY_DAY: "/report/get-revenue-order-quantity-by-day",
  GET_REVENUE_ORDER_BY_WEEK: "/report/get-revenue-order-quantity-by-week",
  GET_REVENUE_ORDER_BY_MONTH: "/report/get-revenue-order-quantity-by-month",

  // Total sales
  GET_TOTALSALES_ALL_EMPLOYEE: "/totalsales/get-total-sales",
  GET_TOTALSALES_BY_DAY_OR_USERID:
    "/totalsales/get-total-sales-by-day-or-userid",
  GET_DETAILS_TOTALSALE_SCHEDULE: "/totalsales/get-detail-total-sale-schedule",

  // Employee management
  GET_ALL_EMPLOYEES: `/user`,
  ADD_USER: `/user/add`,
  UPDATE_USER: `/user/update-user`,
  DELETE_USER: `/user/delete-user`,
  GET_EMAIL_BY_PHONE: "/user/get-email-by-phone",

  // Schedule
  GET_ALL_SCHEDULE: "/schedule/get-all-schedule",
  GET_SCHEDULE_BY_USERID: "/schedule/get-schedule-by-userid",
  UPDATE_SCHEDULE_BY_USERID: "/schedule/update-schedule-by-userid",
  SWAP_SCHEDULE_SHIFT: "/schedule/swap-schedule-shift",
  OPEN_SHIFT: "/schedule/open-shift",
  CLOSE_SHIFT: "/schedule/close-shift",
  GET_SCHEDULE_TODAY: "/schedule/get-schedule-by-userid-today",
  GET_ALL_SCHEDULE_HISTORY: "/schedule/get-all-schedule-history",
};

export default API_ENDPOINTS;
