import firebaseConfig from "@/config/firebase.config";
import { getApp, getApps, initializeApp } from "firebase/app";
import { getDatabase } from "firebase/database";
import { getFirestore } from "firebase/firestore";

const app = getApps().length ? getApp() : initializeApp(firebaseConfig);

export const database = getDatabase(app);

export const db = getFirestore(app);