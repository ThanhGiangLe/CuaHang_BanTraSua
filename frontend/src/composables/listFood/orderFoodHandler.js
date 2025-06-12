// src/composables/useAuth.js
import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";

export function orderFoodHandler() {
  const getAllFood = async () => {
    try {
      const response = await axiosClient.get(API_ENDPOINTS.GET_ALL_FOOD_ITEMS);
      const listFood = response.data;
      return { listFood };
    } catch (err) {
      return err;
    }
  };

  const getAllCategory = async () => {
    try {
      const response = await axiosClient.get(
        API_ENDPOINTS.GET_ALL_FOOD_CATEGORIES
      );
      const listCategory = response.data;
      return { listCategory };
    } catch (err) {
      return err;
    }
  };

  const orderFood = async (request) => {
    try {
      const response = await axiosClient.post(API_ENDPOINTS.ADD_ORDER, request);
      const responseOrder = response.data;
      return { responseOrder };
    } catch (err) {
      return err;
    }
  };

  const orderDetail = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.ADD_ORDER_DETAIL,
        request
      );
      const responseDetail = response.data;
      return { responseDetail };
    } catch (err) {
      return err;
    }
  };

  const topBestSelling = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.TOP_BEST_SELLING,
        request
      );
      const top = response.data;
      return { top };
    } catch (err) {
      return err;
    }
  };

  return {
    getAllFood,
    getAllCategory,
    orderFood,
    orderDetail,
    topBestSelling,
  };
}
