import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";

export function reportManagementHandler() {
  const getAllRevenueByMonth = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_REVENUE_BY_TIME_MONTH,
        {
          date: date,
        }
      );
      const total = response.data;
      return total;
    } catch (err) {
      return err;
    }
  };
  const gettAllRevenueByDay = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_REVENUE_BY_TIME,
        {
          date: date,
        }
      );
      const total = response.data;
      return total;
    } catch (err) {
      return err;
    }
  };
  const gettAllRevenueWithEmployeeByMonth = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME_MONTH,
        {
          date: date,
        }
      );
      const total = response.data;
      return total;
    } catch (err) {
      return err;
    }
  };
  const gettAllRevenueWithEmployeeByDay = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_REVENUE_BY_EMPLOYEE_AND_TIME,
        {
          date: date,
        }
      );
      const total = response.data;
      return total;
    } catch (err) {
      return err;
    }
  };
  const gettAllRevenueWithCategoryByMonth = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_REVENUE_BY_CATEGORY_AND_TIME_MONTH,
        {
          date: date,
        }
      );
      const total = response.data;
      return total;
    } catch (err) {
      return err;
    }
  };
  const gettAllRevenueWithCategoryByDay = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_REVENUE_BY_CATEGORY_AND_TIME,
        {
          date: date,
        }
      );
      const total = response.data;
      return total;
    } catch (err) {
      return err;
    }
  };
  const getAllBestSellingByMonth = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_ORDERITEM_BESTSELING_CURRENTMONTH,
        {
          date: date,
        }
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const getAllBestSellingByDay = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_ORDERITEM_BESTSELING_CURRENTDAY,
        {
          date: date,
        }
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const getAllOrderByMonth = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_ORDER_CURRENT_MONTH,
        {
          date: date,
        }
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const getAllOrderByDay = async (date) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_ORDER_CURRENT_DAY,
        {
          date: date,
        }
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const getAllOrderByFullName = async (selectedEmployee) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_ALL_ORDER_FULL_NAME,
        {
          date: selectedEmployee,
        }
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  return {
    getAllRevenueByMonth,
    gettAllRevenueByDay,
    gettAllRevenueWithEmployeeByMonth,
    gettAllRevenueWithEmployeeByDay,
    gettAllRevenueWithCategoryByMonth,
    gettAllRevenueWithCategoryByDay,
    getAllBestSellingByMonth,
    getAllBestSellingByDay,
    getAllOrderByMonth,
    getAllOrderByDay,
    getAllOrderByFullName,
  };
}
