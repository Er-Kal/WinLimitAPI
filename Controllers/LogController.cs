using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace WinLimitAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IMongoCollection<AppBlockedLog> _logCollection;
    public LogController(UserManager<User> userManager, IMongoClient mongoClient)
    {
        _userManager = userManager;

        var database = mongoClient.GetDatabase("WinLimitDb");
        _logCollection = database.GetCollection<AppBlockedLog>("AppBlockedLogs");
    }

    [HttpPost]
    public async Task<IActionResult> PostLog([FromBody] AppBlockedLog log)
    {
        var user = await _userManager.GetUserAsync(User);
        log.Email = user?.Email?? string.Empty;
        log.CreatedAt = DateTime.UtcNow;
        await _logCollection.InsertOneAsync(log);

        return Ok();
    }
}
