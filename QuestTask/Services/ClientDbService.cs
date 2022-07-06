using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using QuestTask.DbObjects;

namespace QuestTask.Services
{
    public class ClientDbService : IClientDbService
    {
        MongoClient dbClient;

        public ClientDbService()
        {
            dbClient = new MongoClient("mongodb://127.0.0.1:27017");
        }

        public async Task<Root> FindClient(int clientId)
        {
            IMongoDatabase db = dbClient.GetDatabase("quest");
            var clients = db.GetCollection<BsonDocument>("clients");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", clientId);
            var docs = await clients.Find(filter).ToListAsync();
            if (docs.Count > 0)
                return BsonSerializer.Deserialize<Root>(docs.First());
            throw new ArgumentException($"Nonexistant clientId {clientId}");
        }

        public async Task<List<Root>> FindAllClients()
        {
            IMongoDatabase db = dbClient.GetDatabase("quest");
            var clients = db.GetCollection<BsonDocument>("clients");
            var docs = await clients.Find(new BsonDocument()).ToListAsync();
            var result = new List<Root>();
            foreach (var doc in docs)
                result.Add(BsonSerializer.Deserialize<Root>(doc));
            return result;
        }

        public async Task<long> DeleteClient(int clientId)
        {
            IMongoDatabase db = dbClient.GetDatabase("quest");
            var clients = db.GetCollection<BsonDocument>("clients");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", clientId);
            var result = await clients.DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        public async Task<long> UpdateClient(Root client)
        {
            IMongoDatabase db = dbClient.GetDatabase("quest");
            var clients = db.GetCollection<BsonDocument>("clients");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", client.id);
            var newDoc = client.ToBsonDocument();
            var result = await clients.ReplaceOneAsync(filter, newDoc);
            return result.ModifiedCount;
        }
    }
}
