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

    // GET: /api/block
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlockItem>>> GetBlocks()
    {
        var blocks = await _blockItemsCollection.Find(n => true).ToListAsync();
        return Ok(blocks);
    }
    /*
    [HttpPost]
    public async Task<ActionResult<BlockItem>> PostBlock(BlockItem blockItem)
    {
        if (blockItem.TimeAdded == default)
        {
            blockItem.TimeAdded = DateTime.UtcNow;
        }
        _context.BlockItems.Add(blockItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBlocks), new {id=blockItem.Id}, blockItem);
    }*/
}