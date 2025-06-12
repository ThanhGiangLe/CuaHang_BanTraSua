import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";

export function foodManagementHandler() {
  const getAllFood = async () => {
    try {
      const response = await axiosClient.get(API_ENDPOINTS.GET_ALL_FOOD_ITEMS);
      const listFood = response.data.data;
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
      const listCategory = response.data.data;
      return { listCategory };
    } catch (err) {
      return err;
    }
  };

  const createFood = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.CREATE_FOOD_ITEM,
        request,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      const responseCreate = response.data;
      return { responseCreate };
    } catch (err) {
      return err;
    }
  };

  const updateFood = async (request) => {
    try {
      const response = await axiosClient.put(
        `${API_ENDPOINTS.UPDATE_FOOD_ITEM}/${request.foodItemId}`,
        request,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      const responseUpdate = response.data;
      return { responseUpdate };
    } catch (err) {
      return err;
    }
  };

  const deleteFood = async (FoodItemCurrentId) => {
    try {
      const response = await axiosClient.delete(
        `${API_ENDPOINTS.DELETE_FOOD_ITEM}/${FoodItemCurrentId}`
      );
      const responseDelete = response.data;
      return { responseDelete };
    } catch (err) {
      return err;
    }
  };

  return {
    getAllFood,
    getAllCategory,
    createFood,
    updateFood,
    deleteFood,
  };
}
