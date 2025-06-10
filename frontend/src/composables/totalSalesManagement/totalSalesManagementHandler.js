import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";
export function totalSalesManagementHandler() {
  const getTotalSaleAllEmployee = async () => {
    try {
      const response = await axiosClient.get(
        API_ENDPOINTS.GET_TOTALSALES_ALL_EMPLOYEE
      );
      return response.data;
    } catch (err) {
      return err;
    }
  };
  const getTotalSalesEmployee = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_TOTALSALES_BY_DAY_OR_USERID,
        request
      );
      return response.data;
    } catch (err) {
      return err;
    }
  };
  return {
    getTotalSaleAllEmployee,
    getTotalSalesEmployee,
  };
}
