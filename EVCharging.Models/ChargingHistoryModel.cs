using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EVCharging.Models
{
  public  class ChargingHistoryModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string StationName { get; set; }
        public int StationId { get; set; }
        public string KW { get; set; }
        public string PortType { get; set; }
        public DateTime CraetedBy { get; set; }

    }
}
