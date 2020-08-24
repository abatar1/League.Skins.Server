using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace League.Skins.Data.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastSessionTime { get; set; }
        public List<User> Editors { get; set; }
        public List<User> Editable { get; set; }
    }
}
