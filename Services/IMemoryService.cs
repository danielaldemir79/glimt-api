using Glimt.Api.Models;

namespace Glimt.Api.Services;

public interface IMemoryService
{
    Task<List<MemoryEntry>> GetAllAsync();
}