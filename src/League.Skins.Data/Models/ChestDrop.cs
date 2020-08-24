using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace League.Skins.Data.Models
{
    public enum SkinRarity
    {
        Ultimate,
        Mythic,
        Legendary,
        Epic,
        Standard,
        Timeworn
    }

    public enum DropType
    {
        ChampionSkinShard,
        ChampionSkinPermanent,
        ChampionShard,
        EmotePermanent,
        SummonerWardShard,
        SummonerIconPermanent,
    }

    public enum AdditionalDropType
    {
        HextechChest,
        Gemstone,
        MysticSkinPermanent
    }

    public enum ChestType
    {
        Hextech,
        Masterwork
    }

    public class ChestDrop
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public User Creator { get; set; }
        public User Owner { get; set; }
        public string Comment { get; set; }
        public ChestType ChestType { get; set; }
        public DropType DropType { get; set; }
        public AdditionalDropType? AdditionalDropType { get; set; }
        public SkinRarity? SkinRarity { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
