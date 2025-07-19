// src/firebase.js
import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";

const firebaseConfig = {
  apiKey: "AIzaSyDNiYUJK4636C2b_ikO-Tb8PZ1uayJ95Zk",
  authDomain: "sunsun-tea-milk.firebaseapp.com",
  projectId: "sunsun-tea-milk",
  storageBucket: "sunsun-tea-milk.appspot.com", // ✅ sửa lại ".app" → ".com"
  messagingSenderId: "971256403977",
  appId: "1:971256403977:web:c93ce82b116b1884a940d8",
  measurementId: "G-CFKDJ34MXD",
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app); // ✅ getAuth sau khi app đã init

export { app, auth };
