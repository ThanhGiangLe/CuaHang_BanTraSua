import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";

export function scheduleManagementHandler() {
  const GetAllScheduleHistory = async (request) => {
    try {
      const response = await axiosClient.get(
        API_ENDPOINTS.GET_ALL_SCHEDULE_HISTORY,
        request
      );
      const result = response.data;
      return { result };
    } catch (err) {
      return err;
    }
  };
  return {
    GetAllScheduleHistory,
  };
}
