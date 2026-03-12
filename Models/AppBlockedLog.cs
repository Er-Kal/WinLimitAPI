using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WinLimitAPI;

public class AppBlockedLog
{
    public string Email { get; set; } = string.Empty;
    public string AppName {get;set;} = string.Empty;
    public string FriendlyAppName {get;set;} = string.Empty;
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
}