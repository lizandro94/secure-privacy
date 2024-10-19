namespace webapi.Database
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public required string CollectionName { get; set; }
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}