using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Sysiphus.Tasks.RKBot.MongoClasses;

namespace Sysiphus.Tasks.RKBot
{

    class MongoDBInteface
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        private string _connectionString;
        private const string DatabaseName = "RK7DB";

        public MongoDBInteface(string connectionString)
        {
            this._connectionString = connectionString;
            Connect();
        }

        private void Connect()
        {
            _mongoClient = new MongoClient(_connectionString);
            _database = _mongoClient.GetDatabase(DatabaseName);
        }

        public void SaveShift(Shift shift)
        {
            var collection = _database.GetCollection<Shift>("shifts");
            collection.InsertOne(shift);
        }
    }
}
