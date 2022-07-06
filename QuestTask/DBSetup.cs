using MongoDB.Bson;
using MongoDB.Driver;
using QuestTask.DbObjects;
using System.Text.Json;

namespace QuestTask
{
    public static class DBSetup
    {
        public static Root[]? GetSampleData()
        {
            var text = File.ReadAllText("DbData.json");
            var data = JsonSerializer.Deserialize<DbObjects.Root[]>(text);
            return data;
        }
        public static void InitializeDbWithSampleData()
        {
            var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = dbClient.GetDatabase("quest");
            db.DropCollection("clients");
            var clientsCollection = db.GetCollection<BsonDocument>("clients");
            var data = GetSampleData();
            if (data != null)
            {
                var clients = new Dictionary<string, DbObjects.Root>();
                foreach (var client in data)
                {
                    var doc2 = client.ToBsonDocument();
                    clientsCollection.InsertOne(doc2);
                }
            }
        }

        public static IApplicationBuilder UseMongoDb(this IApplicationBuilder app)
        {
            try
            {
                InitializeDbWithSampleData();
                return app;
            }
            catch (Exception ex)
            {
                var logger = app.ApplicationServices.GetService<ILogger>();
                logger?.LogError(ex.Message);
            }
            return app;
        }
    }
}
