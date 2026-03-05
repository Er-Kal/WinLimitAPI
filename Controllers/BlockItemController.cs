using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace WinLimitAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlockItemController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IMongoCollection<BlockItem> _blockItemsCollection;

    public BlockItemController(UserManager<User> userManager, IMongoClient mongoClient)
    {
        _userManager = userManager;
        var database = mongoClient.GetDatabase("WinLimitDb");
        _blockItemsCollection = database.GetCollection<BlockItem>("BlockItems");
    }

    // GET: /api/blockitem
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlockItem>>> GetBlocks()
    {
        var blocks = await _blockItemsCollection.Find(n => true).ToListAsync();
        return Ok(blocks);
    }

    // POST: /api/blockitem
    [HttpPost]
    public async Task<ActionResult<BlockItem>> CreateBlock([FromBody] BlockItem blockItem)
    {
        if (blockItem == null)
            return BadRequest("Block item cannot be null.");

        await _blockItemsCollection.InsertOneAsync(blockItem);

        return CreatedAtAction(nameof(GetBlocks), new { id = blockItem.Id }, blockItem);
    }

    [HttpGet("latest")]
    public async Task<ActionResult<IEnumerable<BlockItem>>> GetLatestBlockItems([FromQuery] int count = 20)
    {
        var blocks = await _blockItemsCollection.Find(_ => true).SortByDescending(b => b.id).Limit(count).ToListAsync();

        return Ok(blocks);
    }
}