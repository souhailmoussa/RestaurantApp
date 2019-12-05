namespace RestaurantApplication.Api.Mongo
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;

    public class MongoService
    {
        public Lazy<IMongoDatabase> Database { get; private set; }

        private MongoService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            Database = new Lazy<IMongoDatabase>(() =>
            {
                var mongoUrl = new MongoUrl(connectionString);
                var client = new MongoClient(mongoUrl);

                //Dev mode
                //var client = new MongoClient(new MongoClientSettings()
                //{
                //    Server = mongoUrl.Server,
                //    ClusterConfigurator = cb =>
                //    {
                //        cb.Subscribe<MongoDB.Driver.Core.Events.CommandStartedEvent>(e =>
                //        {
                //            Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                //        });
                //    }
                //});

                //http://mongodb.github.io/mongo-csharp-driver/2.0/reference/driver/connecting/
                //The implementation of IMongoDatabase provided by a MongoClient is thread-safe and is safe to be 
                //stored globally or in an IoC container.
                return client.GetDatabase(mongoUrl.DatabaseName);
            });
        }

        public object GetCollection<T>(object userFeedback)
        {
            throw new NotImplementedException();
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName) => Database.Value.GetCollection<T>(collectionName);

        public static MongoService GetService(string connectionString)
        {
            return new MongoService(connectionString);
        }
    }
}
