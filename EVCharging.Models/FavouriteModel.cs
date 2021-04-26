using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EVCharging.Models
{
   public  class FavModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string Userid { get; set; }
        public string stationname { get; set; }
        public string stationid { get; set; }
        public string port { get; set; }
        public string kw { get; set; }
        public string city { get; set; }
        public string country { get; set; }

    }
}
