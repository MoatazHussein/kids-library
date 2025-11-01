using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Salhia.KidsLibrary.Application.Features.Images.Commands.UploadImage;

public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private const byte maxSizeInMB = 2;

    public UploadImageCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(BeAnImage).WithMessage("Only image files are allowed.")
            .Must(HaveValidSize).WithMessage($"File must be {maxSizeInMB} MB or less.");
    }

    private bool BeAnImage(IFormFile file)
    {
        if (file == null) return false;

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedExtensions.Contains(ext);
    }

    private bool HaveValidSize(IFormFile file)
    {
        const long maxSizeInBytes = maxSizeInMB * 1024 * 1024; 
        return file?.Length <= maxSizeInBytes;
    }
}
