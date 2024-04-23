namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger _logger;

    public GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
    {
        _session = session;
        _logger = logger;
    }
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with {@Query}", query);

        var products = await _session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}

