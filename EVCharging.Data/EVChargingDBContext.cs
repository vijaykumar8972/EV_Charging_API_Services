using EVCharging.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVCharging.Data
{
    public class EVChargingDBContext
    {
        private readonly IMongoDatabase _database;

        public EVChargingDBContext(IEVChargingDatabaseSettings iEVChargingDatabase)
        {
            try
            {
                var client = new MongoClient(iEVChargingDatabase.ConnectionString);
                if (client != null)
                    _database = client.GetDatabase(iEVChargingDatabase.DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to MongoDb server.", ex);
            }
        }
        public IMongoCollection<Users> users => _database.GetCollection<Users>("Users");
        public IMongoCollection<MSTManufacture> Manufacture => _database.GetCollection<MSTManufacture>("MSTManufacture");
        public IMongoCollection<MSTModel> Model => _database.GetCollection<MSTModel>("MSTModel");
        public IMongoCollection<MstChargingAbout> ChargingAbout => _database.GetCollection<MstChargingAbout>("MstAboutus");
        public IMongoCollection<FeedBackMdel> feedback => _database.GetCollection<FeedBackMdel>("FeedBack");
        public IMongoCollection<ChargingHistoryModel> charginghistory => _database.GetCollection<ChargingHistoryModel>("ChargingHistoryModel");
    }
}
