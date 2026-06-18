namespace TrafficLightSystem.Infrastructure.Interfaces
{
    public interface IConnection
    {
        bool Connect();
        bool Disconnect();
        void Send(string data);

        /// <summary>
        /// EVENT: Nhận dữ liệu phản hồi từ thiết bị gửi ngược về
        /// 
        /// Vai trò:
        /// - Arduino / server / Firebase / Bluetooth gửi data lên
        /// - Infrastructure bắt được dữ liệu
        /// - Đẩy lên Application (Controller)
        /// - UI subscribe để hiển thị realtime
        /// 
        /// Ví dụ:
        /// "ACK"
        /// "POWER_ON_OK"
        /// "LOG,Device started"
        /// </summary>
        event Action<string> DataReceived;
    }
}
