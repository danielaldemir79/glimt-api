using Glimt.Api.Models;
using Glimt.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Glimt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemoryEntriesController(IMemoryService memoryService)
    : ControllerBase
{
    /// <summary>
    /// Hämtar alla minnen
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<MemoryEntry>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MemoryEntry>>> GetAll()
    {
        List<MemoryEntry> memories = await memoryService.GetAllAsync();

        return Ok(memories);
    }

    /// <summary>
    /// Skapar ett nytt minne
    /// </summary>
    [HttpPost]
    [ProducesResponseType<MemoryEntry>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MemoryEntry>> Create(MemoryEntry memory)
    {
        MemoryEntry createdMemory = await memoryService.CreateAsync(memory);

        return StatusCode(201, createdMemory);
    }

    /// <summary>
    /// Uppdaterar ett befintligt minne
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType<MemoryEntry>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MemoryEntry>> Update(int id, MemoryEntry updatedMemory)
    {
        MemoryEntry? memory =
            await memoryService.UpdateAsync(id, updatedMemory);

        if (memory is null)
        {
            return NotFound();
        }

        return Ok(memory);
    }

    /// <summary>
    /// Tar bort ett befintligt minne
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await memoryService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}