using System.Text;
using System.Text.Json;
using TrafficLightSystem.Infrastructure.Interfaces;

namespace TrafficLightSystem.Infrastructure.Internet
{
    public class FirebaseConnection : IConnection
    {
        private readonly HttpClient _httpClient;
        private bool _isConnected;

        private readonly CancellationTokenSource _cts = new();

        public event Action<string> DataReceived;
        private string _lastValue = "";

        public FirebaseConnection()
        {
            _httpClient = new HttpClient();
        }

        public bool Connect()
        {
            _isConnected = true;

            StartListening();
            return _isConnected;
        }

        public bool Disconnect()
        {
            _isConnected = false;
            _cts.Cancel();
            return _isConnected;
        }

        public async void Send(string data)
        {
            if (!_isConnected) return;

            try
            {
                string json = JsonSerializer.Serialize(data);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                await _httpClient.PutAsync(
                    FirebaseConfig.DatabaseUrl + "Nut_Bam.json",
                    content
                );
            }
            catch (Exception ex)
            {
                DataReceived?.Invoke("Firebase SEND ERROR: " + ex.Message);
            }
        }

        // =========================
        // LISTEN FIREBASE (POLLING)
        // =========================
        private async void StartListening()
        {
            while (_isConnected && !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var result = await _httpClient.GetStringAsync(
                        FirebaseConfig.DatabaseUrl + "Arduino_Data.json"
                    );

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        // remove quote JSON string
                        var clean = JsonSerializer.Deserialize<string>(result);

                        if (clean != _lastValue)
                        {
                            _lastValue = clean;
                            DataReceived?.Invoke(clean);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DataReceived?.Invoke("Firebase LISTEN ERROR: " + ex.Message);
                }

                await Task.Delay(500);
            }
        }
    }
}