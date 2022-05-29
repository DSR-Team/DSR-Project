using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_NFT : MonoBehaviour
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Root
    {
        public string contract { get; set; }
        public int tokenId { get; set; }
        public List<string> creators { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string mimeType { get; set; }
        public List<string> tags { get; set; }
        public string artifactUri { get; set; }
        public string displayUri { get; set; }
        public string thumbnailUri { get; set; }
        public double latestSoldPrice { get; set; }
    }

}
