import time
import threading
import serial
import firebase_admin
import socket
from firebase_admin import credentials, db, firestore
from datetime import datetime
# python firebase_bridge.py


# SOCKET SERVER (WinForms)
HOST = "127.0.0.1"
PORT = 5000

server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server.bind((HOST, PORT))
server.listen(1)

client = None
print("Waiting WinForms...")


# COM gửi sang Proteus
try:
    proteus = serial.Serial(
        port='COM1',
        baudrate=9600,
        timeout=1,
        rtscts=False,
        dsrdtr=False,
        xonxoff=False
    )
    proteus.setDTR(False)
    proteus.setRTS(False)

    print("Proteus COM1 OK")
except Exception as e:
    print(f"Không mở được COM1: {e}")
    exit()



# COM Bluetooth thật
try:
    bluetooth = serial.Serial(
        port='COM3',
        baudrate=19200,
        timeout=0,
        write_timeout=0.5
    )
    print("Bluetooth COM3 OK")
except Exception as e:
    print(f"Không mở được COM3: {e}")
    bluetooth = None



# Firebase
DATABASE_URL = "https://trafficlightcontrol-86ea4-default-rtdb.asia-southeast1.firebasedatabase.app/"

cred = credentials.Certificate("serviceAccountKey.json")

firebase_admin.initialize_app(
    cred,
    {
    'databaseURL': DATABASE_URL
    }
)
firestore_db = firestore.client()

print("Firebase OK")



# Hàm gửi chung
def send_to_proteus(command):
    command = str(command).strip()

    print(
    "waiting=",
    proteus.in_waiting
    )
    if command and proteus.is_open:
        proteus.write((command + '\n').encode('utf-8'))
        print(f"Sent: {command}")

# HÀM NHẬN
def proteus_receive_thread():
    print("Thread Proteus đang chạy...")
    rx_buffer = ""

    while True:
        try:
            while proteus.in_waiting:

                chunk = proteus.read(
                    proteus.in_waiting
                ).decode(
                    "utf-8",
                    errors="ignore"
                )

                rx_buffer += chunk
                print(f"rx_buffer:{rx_buffer}")

                # Xử lý từng dòng hoàn chỉnh
                while "\n" in rx_buffer:

                    data, rx_buffer = \
                        rx_buffer.split("\n", 1)

                    data = data.strip()
                    if not data:
                        continue

                    print(f"📥 Proteus: {data}")

                    send_to_winforms(data)

                    # Log Firestore
                    if data.startswith("LOG,"):

                        try:
                            firestore_db.collection(
                                "logs"
                            ).add({
                                "message": data,
                                "timestamp": datetime.now()
                            })

                            print(
                                f"Log saved: {data}"
                            )

                        except Exception as e:
                            print(
                                "Firestore error:",
                                e
                            )

                    # Firebase + Bluetooth
                    else:

                        try:
                            db.reference(
                                "/Arduino_Data"
                            ).set(data)

                        except Exception as e:
                            print(
                                "Firebase error:",
                                e
                            )

                        if bluetooth and bluetooth.is_open:

                            try:
                                print(f"BT before write: {data}")
                                bluetooth.write(
                                    (data + "\n")
                                    .encode("utf-8")
                                )

                                print(
                                    f"BT after write: {data}"
                                )

                            except serial.SerialTimeoutException:
                                print("!!!!!!!Bluetooth write timeout!!!!!!!")

        except Exception as e:

            print(
                "Proteus RX error:",e
            )

        time.sleep(0.05)



# Firebase listener
def firebase_listener(event):

    command = str(event.data).strip()

    if command:
        print(f"Firebase: {command}")
        send_to_proteus(command)



# Bluetooth listener
def bluetooth_thread():

    if bluetooth is None:
        return

    while True:
        try:

            if bluetooth.in_waiting:

                data = bluetooth.readline().decode(
                    'utf-8',
                    errors='ignore'
                ).strip()

                if data:
                    print(f"Bluetooth: {data}")

                    send_to_proteus(data)

        except Exception as e:
            print("Bluetooth error:", e)

        time.sleep(0.05)

# Hàm gửi Winfroms
def send_to_winforms(message):
    global client
    if client is None:
        return
    try:
        client.send((message + "\n").encode())
    except:
        print("Socket send:", e)
    
# SOCKET RECEIVE
def socket_receive_thread():

    global client

    while client:

        try:

            data = client.recv(1024)

            if not data:
                break

            data = data.decode().strip()

            if data:

                print("WinForms:", data)

                send_to_proteus(data)

        except Exception as e:

            print(e)
            break

    client = None


# Đợi WINFORMS
def socket_accept_thread():
    global client
    while True:
        try:
            print("Waiting WinForms...")
            client, addr = server.accept()
            print("WinForms Connected :", addr)
            threading.Thread(
                target=socket_receive_thread,
                daemon=True
            ).start()
        except Exception as e:
            print(e)

# 1. Tạo một hàm bọc listener lại
def start_firebase_listener():
    try:
        print("Đang khởi tạo Firebase Listener...")
        db.reference('/Nut_Bam').listen(firebase_listener)
    except Exception as e:
        print("Lỗi Firebase Listener:", e)


# 2. Khởi chạy luồng Bluetooth (nếu có)
if bluetooth is not None:
    threading.Thread(target=bluetooth_thread, daemon=True).start()
    print("Đã bật luồng Bluetooth.")

# 3. Khởi chạy luồng nhận dữ liệu từ Proteus
threading.Thread(target=proteus_receive_thread, daemon=True).start()
print("Đã bật luồng nhận dữ liệu Proteus.")

# 4. KHỞI CHẠY Firebase Listener bằng THREAD RIÊNG để không bị đơ
threading.Thread(target=start_firebase_listener, daemon=True).start()

# 5. KHỞI CHẠY Socket Thread
threading.Thread(
    target=socket_accept_thread,
    daemon=True
).start()

# 5. Vòng lặp giữ luồng chính (Main Thread) luôn sống
print("Hệ thống Bridge đang chạy...")
while True:
    time.sleep(1)

