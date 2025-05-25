from flask import Flask, request, jsonify
from chatbot import get_answer  # Hàm lấy câu trả lời từ chatbot
from flask_cors import CORS

app = Flask(__name__)
CORS(app, resources={r"/chatbot": {"origins": "*"}})  # Fix lỗi CORS

@app.route('/chatbot', methods=['POST'])
def chatbot():
    data = request.json
    question = data.get("question", "")
    answer = get_answer(question)  # Gọi chatbot xử lý câu hỏi
    return jsonify({"answer": answer})

# Fix lỗi favicon.ico
@app.route('/favicon.ico')
def favicon():
    return '', 204  # Trả về rỗng và mã 204 (No Content)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=1902, debug=True)