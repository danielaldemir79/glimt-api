using Microsoft.AspNetCore.Mvc;

namespace Glimt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController(IWebHostEnvironment environment)
    : ControllerBase
{
    private static readonly string[] AllowedExtensions =
        [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSize = 5 * 1024 * 1024;

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult> Upload(IFormFile image)
    {
        if (image.Length == 0)
        {
            return BadRequest("Välj en bildfil.");
        }

        if (image.Length > MaxFileSize)
        {
            return BadRequest("Bilden får vara högst 5 MB.");
        }

        string extension =
            Path.GetExtension(image.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
        {
            return BadRequest(
                "Endast JPG, JPEG, PNG och WEBP är tillåtna.");
        }

        // Unikt filnamn hindrar att bilder med samma namn skriver över varandra
        string fileName = $"{Guid.NewGuid()}{extension}";

        // Bildfilen sparas i wwwroot, databasen sparar bara den publika sökvägen
        string uploadsPath = Path.Combine(
            environment.ContentRootPath,
            "wwwroot",
            "uploads");

        Directory.CreateDirectory(uploadsPath);

        string filePath = Path.Combine(uploadsPath, fileName);

        await using FileStream stream = System.IO.File.Create(filePath);
        await image.CopyToAsync(stream);

        string imagePath = $"/uploads/{fileName}";

        return Ok(new { imagePath });
    }

}