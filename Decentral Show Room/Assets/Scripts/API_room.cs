using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class API_room
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Root
    {
        public string name { get; set; }
        public string image { get; set; }
        public string owner { get; set; }
        public string id { get; set; }
        public List<Dictionary<string, string>> metadata { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

}
