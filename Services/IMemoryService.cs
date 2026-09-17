using Glimt.Api.Models;

namespace Glimt.Api.Services;

public interface IMemoryService
{
    Task<List<MemoryEntry>> GetAllAsync();
    Task<MemoryEntry> CreateAsync(MemoryEntry memory);
    Task<MemoryEntry?> UpdateAsync(int id, MemoryEntry updatedMemory);
    Task<bool> DeleteAsync(int id);
}