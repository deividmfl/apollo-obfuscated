using Newtonsoft.Json;

namespace WebsocketTransport.Models
{
    public class EfficientWorker1B87
    {
        public bool client { get; set; }
        public string data { get; set; }
        public string tag { get; set; }
    }

    public class TacticalEngineAC13
    {
        public static string SpectreWorker8E4F(object obj)
        {
            return SerializationEngine.EncodeToJson(obj);
        }

        public static T Deserialize<T>(string json)
        {
            return SerializationEngine.DecodeFromJson<T>(json);
        }
    }
}
