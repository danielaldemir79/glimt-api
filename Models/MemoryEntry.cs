namespace Glimt.Api.Models;

public class MemoryEntry
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public string Description { get; set; } = string.Empty;

    // Databasen sparar bildens sökväg, inte själva bildfilen
    public string? ImagePath { get; set; }
}