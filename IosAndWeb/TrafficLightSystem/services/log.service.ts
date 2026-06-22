import {
    collection,
    onSnapshot,
    orderBy,
    query,
    Timestamp,
} from "firebase/firestore";
import { db } from "./firebase";

export type TrafficLog = {
  message: string;
  timestamp: Timestamp;
};

class LogService {
  listenLogs(callback: (logs: TrafficLog[]) => void) {
    const q = query(
      collection(db, "logs"),
      orderBy("timestamp", "desc")
    );

    return onSnapshot(q, (snapshot) => {
      const logs: TrafficLog[] = snapshot.docs.map((doc) => {
        const data = doc.data();
        return {
          message: data.message ?? "",
          timestamp: data.timestamp,
        };
      });

      callback(logs);
    });
  }
}

export default new LogService();