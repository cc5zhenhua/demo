using Newtonsoft.Json;
using System;

namespace ZEISSData.Models
{

   
    public class MessageModel
    {
        public string topic { get; set; }
        [JsonProperty("ref")]
        public string _ref { get; set; }
        public Payload payload { get; set; }
        //public object join_ref { get; set; }
        [JsonProperty("event")]
        public string _event { get; set; }
    }

    public class Payload
    {
        public DateTime timestamp { get; set; }
        public string status { get; set; }
        public string machine_id { get; set; }
        public string id { get; set; }
    }

}
