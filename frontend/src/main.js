// Plugins
import { registerPlugins } from "@/plugins";

// Components
import App from "./App.vue";
import JsonExcel from "vue-json-excel3";

// Composables
import { createApp } from "vue";
import { createPinia } from "pinia";

// Import file main.css
import "./assets/main.css"; // Sử dụng đường dẫn tương đối từ thư mục src

const app = createApp(App);
app.component("JsonExcel", JsonExcel);

// Khởi tạo Pinia
const pinia = createPinia();

// Đăng ký các plugin
registerPlugins(app);

// Đăng ký Pinia
app.use(pinia);

// Mount ứng dụng
app.mount("#app");
