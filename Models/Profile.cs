using MongoDB.Bson.Serialization.Attributes;

namespace WinLimitAPI;

public class Profile
{
    [BsonId]
    public Guid UserId {get; set;}
    public string BlockedAppsSettings {get;set;} = string.Empty;
    public string ScheduleSettings {get;set;} = string.Empty;
}