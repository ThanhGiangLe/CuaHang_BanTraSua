<script setup>
import API_ENDPOINTS from "@/api/api";
import askChatbot from "@/services/api";
import axios from "axios";

const chatbotVisible = ref(false); // Hiển thị chatbot
const message = ref(""); // Câu hỏi được gửi đi
const messages = ref([]); // Danh sách câu hỏi và câu trả lời
const toggleChatbot = () => {
  chatbotVisible.value = !chatbotVisible.value;
  messages.value = [];
};
const sendMessage = async () => {
  if (message.value.trim()) {
    // Thêm câu hỏi vào danh sách
    messages.value.push({
      type: "request",
      text: message.value,
    });

    const userMessage = message.value; // Tạo biến để gán giá trị tạm thời
    message.value = ""; // Gán giá trị ở v-text-field bằng chuỗi rỗng

    // Thêm vào hiển thị: Đang đợi tìm kiếm câu trả lời
    messages.value.push({
      type: "response",
      text: "Đang tìm kiếm...",
    });

    const response = await askChatbot(userMessage); // Nhận kết quả trả về
    // Kiểm tra nếu phản hồi có danh sách thì chuyển thành mảng
    let formattedResponse = response; // type: Array
    if (response.includes("1.")) {
      formattedResponse = response
        .split(/(?=\d+\.)/)
        .map((item) => item.trim());
    }

    // Thay thế text: "Đang tìm kiếm..." bằng câu trả lời nhận được
    messages.value[messages.value.length - 1].text = formattedResponse;
    if (response) {
      const res = await axios.post(API_ENDPOINTS.IMPORT_DATA_TRAINING, {
        Content: userMessage,
        Response: response,
      });
    }
  }
};
</script>

<template>
  <div class="chatbot-button-container">
    <!-- Icon để mở khung chat -->
    <button class="chatbot-button" @click="toggleChatbot">
      <v-icon>mdi-chat</v-icon>
    </button>
    <!-- Khung chat -->
    <div v-if="chatbotVisible" class="chatbot-dialog d-flex flex-column">
      <!-- Phần header -->
      <div
        class="d-flex justify-space-between align-center"
        style="padding: 5px; background-color: #007bff; color: white"
      >
        <div class="d-flex justify-center align-center">
          <v-icon size="large">mdi-cat</v-icon>
          <span class="ms-1" style="font-size: 14px">Méo Mập</span>
        </div>
        <v-icon size="small" class="close-icon" @click="toggleChatbot"
          >mdi-close</v-icon
        >
      </div>
      <!-- Phần khung chat -->
      <div
        class="chatbot-dialog-body d-flex flex-column justify-space-between px-2 h-100 overflow-hidden"
      >
        <!-- Phần danh sách tin nhắn -->
        <div class="chatbot-dialog-body-listmessage mb-2 overflow-y-auto">
          <!-- Phần để chứa 1 câu hỏi và 1 câu trả lời -->
          <div
            class="chatbot-dialog-body-chats d-flex flex-column"
            v-for="(msg, index) in messages"
            :key="index"
          >
            <!-- Gửi câu hỏi đi -->
            <div
              v-if="msg.type === 'request'"
              class="chatbot-dialog-body-chats-request px-2 py-1 rounded-xl align-self-end mt-2"
              style="
                background-color: rgba(97, 97, 97, 1);
                font-size: 12px;
                max-width: 90%;
              "
            >
              <span> {{ msg.text }}</span>
            </div>
            <!-- Nhận về câu trả lời -->
            <div
              v-if="msg.type === 'response'"
              class="chatbot-dialog-body-chats-response px-2 py-1 rounded-xl align-self-start mt-2"
              style="
                background-color: rgba(66, 66, 66, 1);
                font-size: 12px;
                max-width: 90%;
              "
            >
              <span v-if="Array.isArray(msg.text)">
                <ul style="padding-left: 4px; margin: 0">
                  <li
                    v-for="(item, idx) in msg.text"
                    :key="idx"
                    style="list-style-type: none"
                  >
                    {{ item }}
                  </li>
                </ul>
              </span>
              <span v-else>{{ msg.text }}</span>
            </div>
          </div>
        </div>
        <!-- Phần nhập tin nhắn -->
        <div class="chatbot-dialog-body-sendmessage d-flex align-center mb-2">
          <v-textarea
            variant="outlined"
            label="Input message..."
            v-model="message"
            rows="1"
            auto-grow
            autofocus
            density="compact"
            style="max-height: 250px; overflow-y: auto; font: 12px"
          ></v-textarea>
          <v-icon
            class="cursor-pointer ms-2"
            style="font-size: 32px"
            @click="sendMessage"
            >mdi-arrow-up-circle</v-icon
          >
        </div>
      </div>
    </div>
  </div>
</template>
