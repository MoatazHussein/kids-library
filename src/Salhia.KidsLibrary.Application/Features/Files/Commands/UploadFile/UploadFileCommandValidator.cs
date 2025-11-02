using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.Files.Commands.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    private readonly string[] _allowedExtensions = { ".txt", ".pdf" };
    private const long _maxFileSize = 10 * 1024 * 1024; // 10 MB

    public UploadFileCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(_maxFileSize)
            .WithMessage($"File size must not exceed {_maxFileSize / 1024 / 1024} MB.")
            .When(x => x.File != null);

        RuleFor(x => x.File.FileName)
            .Must(HaveAllowedExtension)
            .WithMessage($"Only {string.Join(", ", _allowedExtensions)} files are allowed.")
            .When(x => x.File != null);
    }

    private bool HaveAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }
}
