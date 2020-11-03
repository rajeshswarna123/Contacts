using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AddressBook.Core.Entities.Base
{
    public class BaseEntity : FullAudit, IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string shardkey { get; set; }
    }

    public interface IEntity : IAuditFields
    {
        string Id { get; set; }

        string shardkey { get; set; }
    }
}
