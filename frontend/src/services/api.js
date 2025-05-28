import axios from "axios";

const API_URL = "http://127.0.0.1:1902"; // Địa chỉ backend

const askChatbot = async (question) => {
  try {
    const response = await axios.post(`${API_URL}/chatbot`, { question });
    return response.data.answer;
  } catch (error) {
    console.error("Lỗi khi gọi API chatbot:", error);
    return "Xin lỗi, có lỗi xảy ra!";
  }
};

export default askChatbot;
