using Google.Cloud.Firestore;
using TrafficLightSystem.Domain.Models;
using TrafficLightSystem.Infrastructure.Interfaces;

namespace TrafficLightSystem.Infrastructure.Internet
{
    public class FirestoreService : ILogService
    {
        private readonly FirestoreDb _db;

        public FirestoreService()
        {
            try
            {
                var path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Keys",
                    "serviceAccountKey.json"
                );

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Không tìm thấy file key", path);
                }

                Environment.SetEnvironmentVariable(
                    "GOOGLE_APPLICATION_CREDENTIALS",
                    path
                );

                _db = FirestoreDb.Create("trafficlightcontrol-86ea4");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // hoặc log ra file
                throw;
            }
        }

        public async Task<List<TrafficLog>> GetLogsAsync()
        {
            var logs = new List<TrafficLog>();

            var snapshot = await _db
                .Collection("logs")
                .OrderByDescending("timestamp")
                .GetSnapshotAsync();

            foreach (var doc in snapshot.Documents)
            {
                var log = doc.ConvertTo<TrafficLog>();
                logs.Add(log);
            }

            return logs;
        }
    }
}