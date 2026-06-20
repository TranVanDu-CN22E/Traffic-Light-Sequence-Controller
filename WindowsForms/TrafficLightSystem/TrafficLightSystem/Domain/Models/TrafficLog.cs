using Google.Cloud.Firestore;

namespace TrafficLightSystem.Domain.Models
{
    [FirestoreData]
    public class TrafficLog
    {
        [FirestoreProperty]
        public string message { get; set; }

        [FirestoreProperty]
        public Timestamp? timestamp { get; set; }
    }
}