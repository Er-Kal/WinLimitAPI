using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WinLimitAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlockItemController : ControllerBase
{
    private readonly AppDbContext _context;

    public BlockItemController(AppDbContext context)
    {
        _context=context;
    }

    // GET: /api/block
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlockItem>>> GetBlocks()
    {
        return await _context.BlockItems.ToListAsync();
    }

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
    }
}