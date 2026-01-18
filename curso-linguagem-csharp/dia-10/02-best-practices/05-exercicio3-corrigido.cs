// Commands
public interface ICommand { }

public class CreateProductCommand : ICommand
{
    public string Name { get; }
    public string Description { get; }
    public decimal Price { get; }
    public int Stock { get; }

    public CreateProductCommand(string name, string description, decimal price, int stock)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price > 0 ? price : throw new ArgumentException("Price must be positive", nameof(price));
        Stock = stock >= 0 ? stock : throw new ArgumentException("Stock cannot be negative", nameof(stock));
    }
}

public class UpdateProductCommand : ICommand
{
    public Guid ProductId { get; }
    public string Name { get; }
    public string Description { get; }
    public decimal Price { get; }
    public int Stock { get; }

    public UpdateProductCommand(Guid productId, string name, string description, decimal price, int stock)
    {
        ProductId = productId != Guid.Empty ? productId : throw new ArgumentException("Invalid product ID", nameof(productId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price > 0 ? price : throw new ArgumentException("Price must be positive", nameof(price));
        Stock = stock >= 0 ? stock : throw new ArgumentException("Stock cannot be negative", nameof(stock));
    }
}

public class DeleteProductCommand : ICommand
{
    public Guid ProductId { get; }

    public DeleteProductCommand(Guid productId)
    {
        ProductId = productId != Guid.Empty ? productId : throw new ArgumentException("Invalid product ID", nameof(productId));
    }
}

// Queries
public interface IQuery<TResult> { }

public class GetProductByIdQuery : IQuery<ProductDto>
{
    public Guid ProductId { get; }

    public GetProductByIdQuery(Guid productId)
    {
        ProductId = productId != Guid.Empty ? productId : throw new ArgumentException("Invalid product ID", nameof(productId));
    }
}

public class GetAllProductsQuery : IQuery<IEnumerable<ProductDto>>
{
    public string SearchTerm { get; }
    public decimal? MinPrice { get; }
    public decimal? MaxPrice { get; }
    public bool? InStock { get; }

    public GetAllProductsQuery(string searchTerm = null, decimal? minPrice = null, decimal? maxPrice = null, bool? inStock = null)
    {
        SearchTerm = searchTerm;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        InStock = inStock;
    }
}

public class GetProductSummaryQuery : IQuery<ProductSummaryDto>
{
    public Guid ProductId { get; }

    public GetProductSummaryQuery(Guid productId)
    {
        ProductId = productId != Guid.Empty ? productId : throw new ArgumentException("Invalid product ID", nameof(productId));
    }
}

// DTOs
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ProductSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string StockStatus { get; set; }
}

// Handlers
public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}

// Command Handlers
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(CreateProductCommand command)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Stock = command.Stock,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);
    }
}

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(UpdateProductCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId);
        if (product == null)
            throw new ProductNotFoundException(command.ProductId);

        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.Stock = command.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);
    }
}

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(DeleteProductCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId);
        if (product == null)
            throw new ProductNotFoundException(command.ProductId);

        await _productRepository.DeleteAsync(command.ProductId);
    }
}

// Query Handlers
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> HandleAsync(GetProductByIdQuery query)
    {
        var product = await _productRepository.GetByIdAsync(query.ProductId);
        if (product == null)
            throw new ProductNotFoundException(query.ProductId);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> HandleAsync(GetAllProductsQuery query)
    {
        var products = await _productRepository.GetAllAsync();

        // Aplicar filtros
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            products = products.Where(p =>
                p.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (query.MinPrice.HasValue)
        {
            products = products.Where(p => p.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= query.MaxPrice.Value);
        }

        if (query.InStock.HasValue)
        {
            products = products.Where(p => query.InStock.Value ? p.Stock > 0 : p.Stock == 0);
        }

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });
    }
}

public class GetProductSummaryQueryHandler : IQueryHandler<GetProductSummaryQuery, ProductSummaryDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductSummaryQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductSummaryDto> HandleAsync(GetProductSummaryQuery query)
    {
        var product = await _productRepository.GetByIdAsync(query.ProductId);
        if (product == null)
            throw new ProductNotFoundException(query.ProductId);

        var stockStatus = product.Stock switch
        {
            0 => "Fora de estoque",
            <= 5 => "Estoque baixo",
            <= 20 => "Estoque médio",
            _ => "Em estoque"
        };

        return new ProductSummaryDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            StockStatus = stockStatus
        };
    }
}

// Repository
public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
}

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Exceptions
public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(Guid productId)
        : base($"Product with ID {productId} not found") { }
}

// Mediator pattern para despachar commands e queries
public interface IMediator
{
    Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
    Task<TResult> SendAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
}

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(typeof(TCommand));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException($"No handler found for command {typeof(TCommand).Name}");

        var method = handlerType.GetMethod("HandleAsync");
        await (Task)method.Invoke(handler, new object[] { command });
    }

    public async Task<TResult> SendAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TQuery), typeof(TResult));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException($"No handler found for query {typeof(TQuery).Name}");

        var method = handlerType.GetMethod("HandleAsync");
        return await (Task<TResult>)method.Invoke(handler, new object[] { query });
    }
}