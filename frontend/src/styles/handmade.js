import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

export function showToast(message, type = "info") {
  toast[type](message, {
    position: "top-right",
    autoClose: 3000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: undefined,
  });
}
