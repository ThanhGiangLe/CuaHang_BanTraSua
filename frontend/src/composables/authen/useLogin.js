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

  return {
    login,
  };
}
