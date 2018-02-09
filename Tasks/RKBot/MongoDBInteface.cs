using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Sysiphus.Tasks.RKBot
{

    class Waiter
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Company Company { get; set; }
        public List<string> Languages { get; set; }
    }
    class Company
    {
        public string Name { get; set; }
    }

    class MongoDBInteface
    {
        private MongoClient mongoClient;
        private IMongoDatabase database;
        private string connectionString;
        private const string databaseName = "RK7DB";

        public MongoDBInteface(string connectionString)
        {
            this.connectionString = connectionString;
            Connect();
        }

        private bool Connect()
        {
            mongoClient = new MongoClient(connectionString);
            database = mongoClient.GetDatabase(databaseName);
            return true;
        }
    }
}
