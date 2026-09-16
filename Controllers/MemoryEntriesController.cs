using Glimt.Api.Models;
using Glimt.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Glimt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemoryEntriesController(IMemoryService memoryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MemoryEntry>>> GetAll()
    {
        List<MemoryEntry> memories = await memoryService.GetAllAsync();

        return Ok(memories);
    }

    [HttpPost]
    public async Task<ActionResult<MemoryEntry>> Create(MemoryEntry memory)
    {
        MemoryEntry createdMemory = await memoryService.CreateAsync(memory);

        return StatusCode(201, createdMemory);
    }

    [HttpPut("{id}")]
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
}