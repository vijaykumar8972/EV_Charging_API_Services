using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EVCharging.Models
{
    public class FeedBackMdel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int UserId{get;set;}
        public string UserName { get; set; }
        public string StationName { get; set; }
        public string Comment { get; set; }
        public string Image { get; set; }
        public DateTime CraetedBy { get; set; }


    }
}
