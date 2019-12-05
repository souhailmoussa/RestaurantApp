namespace RestaurantApplication.Api.Configuration
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using RestaurantApplication.Api.Common;
    using RestaurantApplication.Api.Mongo;
    using System;

    public class ConfigService : IConfigService
    {
        private readonly string configConnectionString;
        private readonly MongoService mongoService;
        private const string AppSettings = "appsettings";
        private const string ActiveProfile = "activeprofile";
        private const string ActiveProfileId = "activeprofile";

        public ConfigService()
        {
            mongoService = MongoService.GetService(configConnectionString);
        }

        public T Get<T>(string sectionName) where T : class
        {
            var activeProfileCollection = mongoService.GetCollection<BsonDocument>(ActiveProfile);

            var activeProfile = activeProfileCollection.FirstOrDefault(m => m["_id"] == ActiveProfileId);

            if (activeProfile == null)
            {
                return null;
            }

            var appSettingsCollection = mongoService.GetCollection<BsonDocument>(AppSettings);

            var activeAppSettings = appSettingsCollection.FirstOrDefault(m => m["_id"] == activeProfile["name"]);

            if (activeAppSettings == null || activeAppSettings[sectionName] == null)
            {
                return null;
            }

            return BsonSerializer.Deserialize<T>(activeAppSettings[sectionName].AsBsonDocument);
        }

        public string GetAll()
        {
            var activeProfileCollection = mongoService.GetCollection<BsonDocument>(ActiveProfile);

            var activeProfile = activeProfileCollection.FirstOrDefault(m => m["_id"] == ActiveProfileId);

            if (activeProfile == null)
            {
                return null;
            }

            var appSettingsCollection = mongoService.GetCollection<BsonDocument>(AppSettings);

            var activeAppSettings = appSettingsCollection.FirstOrDefault(m => m["_id"] == activeProfile["name"]);

            return Newtonsoft.Json.JsonConvert.SerializeObject(activeAppSettings);
        }
    }
}
