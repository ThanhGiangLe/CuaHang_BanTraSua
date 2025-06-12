// src/composables/useAuth.js
import API_ENDPOINTS from "@/api/api.js";
import axiosClient from "@/services/utils/axiosClient";

export function useLogin() {
  const login = async ({ phone, password }) => {
    try {
      const response = await axiosClient.post(API_ENDPOINTS.LOGIN, {
        Phone: phone,
        Password: password,
      });
      const { token, data } = response.data;
      return { token, data };
    } catch (err) {
      return err;
    }
  };
  const checkEmail = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.CHECK_EMAIL_EXISTS,
        request
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };
  const updatePassword = async (request) => {
    try {
      const response = await axiosClient.post(
        API_ENDPOINTS.UPDATE_PASSWORD,
        request
      );
      const result = response.data;
      return result;
    } catch (err) {
      return err;
    }
  };

  return {
    login,
    checkEmail,
    updatePassword,
  };
}
