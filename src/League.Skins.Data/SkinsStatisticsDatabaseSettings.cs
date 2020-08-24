namespace League.Skins.Data
{
    public interface ISkinsStatisticsDatabaseSettings
    {
        string ChestDropsCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class SkinsStatisticsDatabaseSettings : ISkinsStatisticsDatabaseSettings
    {
        public string ChestDropsCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
