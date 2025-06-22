import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";
export function totalSalesManagementHandler() {
  const getTotalSales = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_TOTALSALES_ALL_EMPLOYEE,
        request
      );
      return response.data;
    } catch (err) {
      return err;
    }
  };
  const getDetailTotalSaleSchedule = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_DETAILS_TOTALSALE_SCHEDULE,
        request
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  return {
    getTotalSales,
    getDetailTotalSaleSchedule,
  };
}
