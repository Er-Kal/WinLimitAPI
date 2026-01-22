namespace WinLimitAPI;

public class BlockItem
{
    public int Id {get; set;}
    public string FriendlyName {get;set;} = string.Empty;
    public string ExecutableName {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public DateTime TimeAdded {get;set;} = DateTime.UtcNow;
}