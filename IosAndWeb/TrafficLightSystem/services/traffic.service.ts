import { onValue, ref, Unsubscribe, update } from "firebase/database";
import { database } from "./firebase";

class TrafficService {
  private rootRef = ref(database);
  private arduinoRef = ref(database, "Arduino_Data");

  // trạng thái route đang chọn
  private selectedRoutes: Set<string> = new Set();

  /**
   * Gửi command chung
   */
  async sendCommand(command: string) {
    try {
      await update(this.rootRef, {
        Nut_Bam: command,
      });
    } catch (err) {
      console.error("Firebase send error:", err);
    }
  }

  // =========================
  // SYSTEM MODE
  // =========================

  powerOn() {
    return this.sendCommand("POWER_ON");
  }

  powerOff() {
    return this.sendCommand("POWER_OFF");
  }

  normal() {
    return this.sendCommand("NORMAL");
  }

  flash() {
    return this.sendCommand("FLASH");
  }

  // =========================
  // TIME CONFIG
  // =========================

  setTime(a: number, y: number, b: number) {
    return this.sendCommand(`SET,${a},${y},${b}`);
  }

  // =========================
  // 🚦 ROUTE LOGIC (QUAN TRỌNG)
  // =========================

  toggleRoute(route: string) {
    if (this.selectedRoutes.has(route)) {
      this.selectedRoutes.delete(route);
    } else {
      this.selectedRoutes.add(route);
    }

    const result = this.getRouteString();

    return this.sendCommand(`ROUTE,${result}`);
  }

  private getRouteString(): string {
    return Array.from(this.selectedRoutes).sort().join("");
  }

  resetRoute() {
    this.selectedRoutes.clear();
    return this.sendCommand("ROUTE,");
  }

  // =========================
  // LISTEN ARDUINO
  // =========================

  listenArduino(callback: (msg: string) => void): Unsubscribe {
    return onValue(this.arduinoRef, (snap) => {
      callback(snap.val() ?? "");
    });
  }
}

export default new TrafficService();