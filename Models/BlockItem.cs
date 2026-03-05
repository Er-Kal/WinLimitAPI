using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WinLimitAPI;

public class BlockItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string FriendlyName {get;set;} = string.Empty;
    public string ExecutableName {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public DateTime TimeAdded {get;set;} = DateTime.UtcNow;
}