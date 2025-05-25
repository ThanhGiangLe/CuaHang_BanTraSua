import faiss
import numpy as np
from sentence_transformers import SentenceTransformer
from load_documents import load_documents  

# Load model và FAISS index
model = SentenceTransformer("sentence-transformers/all-mpnet-base-v2")
index = faiss.read_index("faiss_index.bin")
documents = load_documents("documents/")

def is_valid_question(question):
    if len(question) <= 9:  # Câu quá ngắn
        return False
    if all(char in "?.,!1234567890" for char in question):  # Chứa toàn ký tự đặc biệt
        return False
    return True

# Hàm lấy câu trả lời cho API
def get_answer(question):
    if not is_valid_question(question):
        return "😼❌ Vui lòng đặt câu hỏi rõ ràng hơn."

    query_vector = model.encode([question])
    distances, indices = index.search(np.array(query_vector), 1)

    best_match_idx = indices[0][0]
    best_distance = distances[0][0]

    # Debug: Xem chỉ số FAISS chọn
    print(f"Câu hỏi: {question}")
    print(f"Index câu trả lời: {best_match_idx}, Distance: {best_distance}")
    print(f"Kết quả: {documents[best_match_idx][:100]}...\n")

    # Kiểm tra nếu câu hỏi không đủ độ chính xác
    THRESHOLD = 0.748
    if best_distance > THRESHOLD:
        return (
            "🤖 Xin lỗi, tôi chưa có thông tin về câu hỏi này. "
            "Bạn có thể thử hỏi lại hoặc liên hệ hỗ trợ khách hàng."
        )

    if isinstance(documents[best_match_idx], str):
        return documents[best_match_idx].replace("==>", "😼✅").replace("?", ":")
    else:
        return documents[best_match_idx]  # Chuyển về chuỗi nếu cần
