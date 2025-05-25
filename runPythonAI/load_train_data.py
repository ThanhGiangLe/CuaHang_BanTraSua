import json

# Đọc file JSON
with open("documents/train_data.json", "r", encoding="utf-8") as file:
    train_data = json.load(file)

# In ra dữ liệu để kiểm tra
for item in train_data["train_data"]:
    print(f"📌 Câu hỏi: {item['question']}")
    print("✅ Các bước hướng dẫn:")
    for step in item["steps"]:
        print(f"  - {step}")
    print("\n" + "="*50 + "\n")
