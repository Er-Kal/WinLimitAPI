using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace WinLimitAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IMongoCollection<Profile> _profileCollection;
    public ProfileController(UserManager<User> userManager, IMongoClient mongoClient)
    {
        _userManager = userManager;

        var database = mongoClient.GetDatabase("WinLimitDb");
        _profileCollection = database.GetCollection<Profile>("Profiles");
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        User? user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var profile = await _profileCollection.Find(n => n.UserId == user.Id).ToListAsync();

        return Ok(profile[0]);
    }

    [HttpPatch]
    public async Task<IActionResult> PatchProfile([FromBody] PatchNoteDto patchDoc)
    {
        User? user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();


        // Filter to apply to upsert
        var filter = Builders<Profile>.Filter.Eq(p => p.UserId, user.Id);

        var updateBuilder = Builders<Profile>.Update;
        var updates = new List<UpdateDefinition<Profile>>();

        if (patchDoc.Schedules != null)
        {
            updates.Add(updateBuilder.Set(p => p.ScheduleSettings, patchDoc.Schedules));
        }
        
        if (patchDoc.BlockedApps != null)
        {
            updates.Add(updateBuilder.Set(p => p.BlockedAppsSettings, patchDoc.BlockedApps));
        }

        if (updates.Count == 0)
        {
            return BadRequest("No fields were provided to update");
        }

        var combinedUpdate = updateBuilder.Combine(updates);

        var options = new UpdateOptions{IsUpsert=true};

        return Ok(new {Message = "Profile updated successfully"});
        
    }
}

public record PatchNoteDto(string? Schedules, string? BlockedApps);