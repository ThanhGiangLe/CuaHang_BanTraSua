import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";

export function areaManagementHandler() {
  const getAllTable = async () => {
    try {
      const response = await axiosClient.get(API_ENDPOINTS.GET_ALL_TABLE);
      const listTable = response.data;
      return { listTable };
    } catch (err) {
      return err;
    }
  };

  return {
    getAllTable,
  };
}
