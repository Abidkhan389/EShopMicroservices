
using FluentValidation;

namespace Catalog.API.Products.CreateProduct;
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
	:ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name cannot contain whitespace or be empty.");
        RuleFor(x => x.Category)
            .NotNull().WithMessage("Category cannot be null.")
            .Must(categories => categories != null && categories.Any())
            .WithMessage("Category list must contain at least one item.")
            .Must(categories => categories.All(cat => !string.IsNullOrWhiteSpace(cat)))
            .WithMessage("Category list cannot contain null or whitespace items.");
        RuleFor(x => x.Description)
           .Must(desp => !string.IsNullOrWhiteSpace(desp))
           .WithMessage("Description cannot contain whitespace or be empty.");
        RuleFor(x => x.ImageFile)
           .Must(imageFile => !string.IsNullOrWhiteSpace(imageFile))
           .WithMessage("ImageFile cannot contain whitespace or be empty.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
internal class CreateProductCommandHandler
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IDocumentSession _session;

    public CreateProductCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name= command.Name,
            Category= command.Category,
            Description=command.Description,
            ImageFile=command.ImageFile,
            Price=command.Price
        };
        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken);
        return new CreateProductResult(product.Id);
    }
}


