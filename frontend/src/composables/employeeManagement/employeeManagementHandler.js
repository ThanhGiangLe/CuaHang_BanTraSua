import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";

export function employeeManagementHandler() {
  const getAllEmployee = async () => {
    try {
      const response = await axiosClient.get(API_ENDPOINTS.GET_ALL_EMPLOYEES);
      const listEmployee = response.data.data;
      return listEmployee;
    } catch (err) {
      return err;
    }
  };
  const addEmployee = async (request) => {
    try {
      const response = await axiosClient.post(API_ENDPOINTS.ADD_USER, request);
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const deleteEmployee = async (userId) => {
    try {
      const response = await axiosClient.delete(
        `${API_ENDPOINTS.DELETE_USER}/${userId}`
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const updateEmployee = async (request) => {
    try {
      const response = await axiosClient.post(
        `${API_ENDPOINTS.UPDATE_USER}`,
        request
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const getAllSchedule = async () => {
    try {
      const response = await axiosClient.get(
        `${API_ENDPOINTS.GET_ALL_SCHEDULE}`
      );
      console.log("GET_ALL_SCHEDULE: ", response);

      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const getScheduleByUserId = async (request) => {
    try {
      const response = await axiosClient.post(
        `${API_ENDPOINTS.GET_SCHEDULE_BY_USERID}`,
        request
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const updateScheduleByUserId = async (request) => {
    try {
      const response = await axiosClient.post(
        `${API_ENDPOINTS.UPDATE_SCHEDULE_BY_USERID}`,
        request
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const swapScheduleShift = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.SWAP_SCHEDULE_SHIFT,
        request
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const OpenShift = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.OPEN_SHIFT,
        request
      );
      const result = response.data;
      return { result };
    } catch (err) {
      return err;
    }
  };
  const CloseShift = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.CLOSE_SHIFT,
        request
      );
      const result = response.data;
      return { result };
    } catch (err) {
      return err;
    }
  };
  const getScheduleToDay = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.GET_SCHEDULE_TODAY,
        request
      );
      const result = response.data;
      return { result };
    } catch (err) {
      return err;
    }
  };
  return {
    getAllEmployee,
    addEmployee,
    deleteEmployee,
    updateEmployee,
    getAllSchedule,
    getScheduleByUserId,
    updateScheduleByUserId,
    swapScheduleShift,
    OpenShift,
    CloseShift,
    getScheduleToDay,
  };
}
