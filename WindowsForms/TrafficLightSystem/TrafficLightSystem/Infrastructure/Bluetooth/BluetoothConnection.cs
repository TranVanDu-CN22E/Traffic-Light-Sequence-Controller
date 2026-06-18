using System.IO.Ports;
using TrafficLightSystem.Infrastructure.Interfaces;

namespace TrafficLightSystem.Infrastructure.Bluetooth
{
    public class BluetoothConnection : IConnection
    {
        private SerialPort _serial;
        private readonly string _port;

        public BluetoothConnection(string port)
        {
            _port = port;
        }

        public bool IsConnected => _serial != null && _serial.IsOpen;

        public event Action<string> DataReceived;

        public bool Connect()
        {
            if (IsConnected) return true;

            try
            {
                _serial = new SerialPort(_port, 19200)
                {
                    NewLine = "\n",
                    ReadTimeout = 500
                };

                _serial.DataReceived += Serial_DataReceived;
                _serial.Open();

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("COM đang bị chiếm hoặc không có quyền truy cập!");
                return false;
            }
            catch (IOException)
            {
                MessageBox.Show("COM không tồn tại hoặc thiết bị chưa kết nối!");
                return false;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Tên COM không hợp lệ!");
                return false;
            }
            catch (Exception ex)
            {
                DataReceived?.Invoke("BLUETOOTH ERRO: " + ex.Message);
                MessageBox.Show("Lỗi kết nối Bluetooth: " + ex.Message);
                return false;
            }
        }

        public bool Disconnect()
        {
            if (_serial == null) return true;
            try
            {
                _serial.DataReceived -= Serial_DataReceived;

                if (_serial.IsOpen)
                    _serial.Close();

                _serial.Dispose();
                _serial = null;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Send(string data)
        {
            if (!IsConnected) return;

            _serial.WriteLine(data);
        }

        // =========================
        // RECEIVE EVENT
        // =========================
        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = _serial.ReadLine();
                DataReceived?.Invoke(data);
            }
            catch
            {
                // ignore
            }
        }
    }
}