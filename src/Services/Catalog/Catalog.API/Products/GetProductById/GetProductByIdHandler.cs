using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductById;
public record GetProductByIdQuery(Guid Id): IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);
internal class GetProductByIdQueryHandler
     //(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger _logger;

    public GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    {
        _session = session;
        _logger = logger;
    }
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", query);

        var product = await _session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException();
        }

        return new GetProductByIdResult(product);
    }
}

