using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AkaSwap
{
    // using https://json2csharp.com/
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Owners
    {
        public int property1 { get; set; }
        public int property2 { get; set; }
    }

    public class OwnerAliases
    {
        public string property1 { get; set; }
        public string property2 { get; set; }
    }

    public class Token
    {
        //public string contract { get; set; }
        public int tokenId { get; set; }
        public List<string> creators { get; set; }
        //public List<string> aliases { get; set; }
        //public List<int> royalties { get; set; }
        //public Owners owners { get; set; }
        //public OwnerAliases ownerAliases { get; set; }
        public int amount { get; set; }
        //public int highestSoldPrice { get; set; }
        //public DateTime highestSoldTime { get; set; }
        public int recentlySoldPrice { get; set; }
        public DateTime recentlySoldTime { get; set; }
        //public object sale { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string mimeType { get; set; }
        //image, gif, video, 3dmodel, interactive, audio, svg, pdf.

        public List<string> tags { get; set; }
        public string artifactUri { get; set; }
        public string displayUri { get; set; }
        public string thumbnailUri { get; set; }
        //public object additionalInfo { get; set; }
    }
    public class Root
    {
        public List<Token> tokens { get; set; }
        public int count { get; set; }
    }

}
