using System.ComponentModel.DataAnnotations;

namespace Glimt.Api.Models;

public class MemoryEntry
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Titel är obligatorisk.")]
    public string Title { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "Beskrivning är obligatorisk.")]
    public string Description { get; set; } = string.Empty;

    // Databasen sparar bildens sökväg, inte själva bildfilen
    public string? ImagePath { get; set; }
}