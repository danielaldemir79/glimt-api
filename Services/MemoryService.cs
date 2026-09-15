using Glimt.Api.Data;
using Glimt.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Glimt.Api.Services;

public class MemoryService(AppDbContext context) : IMemoryService
{
    public async Task<List<MemoryEntry>> GetAllAsync()
    {
        // Tidslinjen ska visa de senaste minnena först
        return await context.MemoryEntries
            .OrderByDescending(memory => memory.Date)
            .ToListAsync();
    }

    public async Task<MemoryEntry> CreateAsync(MemoryEntry memory)
    {
        context.MemoryEntries.Add(memory);
        await context.SaveChangesAsync();
        return memory;
    }
    
}