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

    public async Task<MemoryEntry?> UpdateAsync(int id, MemoryEntry updatedMemory)
    {
        MemoryEntry? existingMemory = await context.MemoryEntries.FindAsync(id);

        if (existingMemory is null)
        {
            return null;
        }

        existingMemory.Title = updatedMemory.Title;
        existingMemory.Date = updatedMemory.Date;
        existingMemory.Description = updatedMemory.Description;
        existingMemory.ImagePath = updatedMemory.ImagePath;

        await context.SaveChangesAsync();

        return existingMemory;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        MemoryEntry? memory =
            await context.MemoryEntries.FindAsync(id);

        if (memory is null)
        {
            return false;
        }

        context.MemoryEntries.Remove(memory);
        await context.SaveChangesAsync();

        return true;
    }
    
}