namespace Salhia.KidsLibrary.Infrastructure.Services.Pdf.Models;

public class PdfStoryData
{
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<PdfStoryItem> Items { get; set; } = [];
    public bool IsAIStory { get; set; } = false;
    public string? HeroName { get; set; }
}
