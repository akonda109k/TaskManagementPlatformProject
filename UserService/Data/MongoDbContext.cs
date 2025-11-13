namespace UserService.Data
{
    using MongoDB.Driver;
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; }

        public MongoDbContext(IConfiguration configuration)
        {
            var settings = configuration.GetSection("MongoDbSettings");
            var client = new MongoClient(settings["ConnectionString"]);
            Database = client.GetDatabase(settings["DatabaseName"]);
        }
    }
}
