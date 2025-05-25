import faiss
import numpy as np
from sentence_transformers import SentenceTransformer
from load_documents import load_documents   # Cập nhật hàm đọc file

# Chọn mô hình ngôn ngữ
model = SentenceTransformer("sentence-transformers/all-mpnet-base-v2")

def create_faiss_index(texts):
    """ Chuyển đổi văn bản thành vector và lưu vào FAISS """
    vectors = model.encode(texts, normalize_embeddings=True)  # Encode từng đoạn quy trình
    
    # Sử dụng FAISS với HNSW để tối ưu hiệu suất tìm kiếm
    d = vectors.shape[1]
    index = faiss.IndexHNSWFlat(d, 32)  # 32 là số neighbor (có thể điều chỉnh)
    index.add(np.array(vectors))

    return index, texts

# Đọc tài liệu từ file quy trình
documents = load_documents("documents/")  # Chỉ load file này

# Tạo FAISS index từ nội dung
index, doc_texts = create_faiss_index(documents)

# Lưu index để sử dụng sau
faiss.write_index(index, "faiss_index.bin")

# Cần được chạy lại để cập nhật dữ liệu sau khi thêm