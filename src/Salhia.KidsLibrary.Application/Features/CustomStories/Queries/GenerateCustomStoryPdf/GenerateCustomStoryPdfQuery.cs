using MediatR;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GenerateCustomStoryPdf;

public record GenerateCustomStoryPdfQuery(string Id) : IRequest<byte[]>;
