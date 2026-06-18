using System.Net.Sockets;
using System.Text;
using TrafficLightSystem.Infrastructure.Interfaces;

namespace TrafficLightSystem.Infrastructure.COM
{
    public class TcpConnection : IConnection
    {
        private TcpClient _client;
        private NetworkStream _stream;

        private readonly string _ip = "127.0.0.1";
        private readonly int _port = 5000;

        public event Action<string> DataReceived;

        public bool Connect()
        {
            try
            {
                if (_client != null && _client.Connected)
                    return true;

                _client = new TcpClient();
                _client.Connect(_ip, _port);

                _stream = _client.GetStream();

                StartReceiveThread();

                return true;
            }
            catch (SocketException ex)
            {
                DataReceived?.Invoke("TCP ERROR: " + ex.Message);
                MessageBox.Show("Lỗi kết nối TCP: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                DataReceived?.Invoke("TCP ERROR: " + ex.Message);
                MessageBox.Show("Lỗi kết nối TCP: " + ex.Message);
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                _stream?.Close();
                _client?.Close();

                _stream = null;
                _client = null;

                return true;
            }
            catch 
            { 
                return false;
            }
        }

        public void Send(string data)
        {
            try
            {
                if (_stream == null) return;

                byte[] buffer = Encoding.UTF8.GetBytes(data + "\n");
                _stream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                DataReceived?.Invoke("SEND ERROR");
            }
        }

        // =========================
        // RECEIVE THREAD
        // =========================
        private void StartReceiveThread()
        {
            Thread thread = new Thread(() =>
            {
                byte[] buffer = new byte[1024];

                while (_client != null && _client.Connected)
                {
                    try
                    {
                        int len = _stream.Read(buffer, 0, buffer.Length);

                        if (len > 0)
                        {
                            string msg = Encoding.UTF8.GetString(buffer, 0, len);
                            DataReceived?.Invoke(msg);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }
    }
}