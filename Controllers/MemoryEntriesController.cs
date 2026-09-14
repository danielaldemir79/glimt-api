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
}