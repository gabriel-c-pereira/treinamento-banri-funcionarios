public interface IEntity<TKey> where TKey : IEquatable<TKey>
{
    TKey Id { get; set; }
}

public class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
}

public interface IRepository<T, TKey> where T : class, IEntity<TKey>, new()
                                      where TKey : IEquatable<TKey>
{
    Task<T> GetByIdAsync(TKey id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TKey id);
    Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
    Task<T> FindByPropertyAsync<TValue>(string propertyName, TValue value);
}

public class Repository<T, TKey> : IRepository<T, TKey> where T : class, IEntity<TKey>, new()
                                                        where TKey : IEquatable<TKey>
{
    private readonly List<T> _entities = new();

    public async Task<T> GetByIdAsync(TKey id)
    {
        return await Task.FromResult(_entities.FirstOrDefault(e => e.Id.Equals(id)));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Task.FromResult(_entities.AsEnumerable());
    }

    public async Task AddAsync(T entity)
    {
        if (entity.Id == null || entity.Id.Equals(default(TKey)))
        {
            // Simular geração de ID
            entity.Id = GenerateId();
        }

        _entities.Add(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(T entity)
    {
        var existing = _entities.FirstOrDefault(e => e.Id.Equals(entity.Id));
        if (existing != null)
        {
            var index = _entities.IndexOf(existing);
            _entities[index] = entity;
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(TKey id)
    {
        var entity = _entities.FirstOrDefault(e => e.Id.Equals(id));
        if (entity != null)
        {
            _entities.Remove(entity);
        }
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
    {
        return await Task.FromResult(_entities.Where(predicate));
    }

    public async Task<T> FindByPropertyAsync<TValue>(string propertyName, TValue value)
    {
        var property = typeof(T).GetProperty(propertyName);
        if (property == null)
            throw new ArgumentException($"Property {propertyName} not found on type {typeof(T).Name}");

        return await Task.FromResult(_entities.FirstOrDefault(e =>
        {
            var propertyValue = property.GetValue(e);
            return propertyValue != null && propertyValue.Equals(value);
        }));
    }

    private TKey GenerateId()
    {
        // Implementação simplificada para diferentes tipos de chave
        if (typeof(TKey) == typeof(int))
        {
            return (TKey)(object)(_entities.Count + 1);
        }
        else if (typeof(TKey) == typeof(Guid))
        {
            return (TKey)(object)Guid.NewGuid();
        }
        else if (typeof(TKey) == typeof(string))
        {
            return (TKey)(object)Guid.NewGuid().ToString();
        }

        throw new NotSupportedException($"Key type {typeof(TKey).Name} not supported");
    }
}

// Exemplo de uso
public class Produto : Entity<int>
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public bool Ativo { get; set; }
}

public class ProdutoService
{
    private readonly IRepository<Produto, int> _repository;

    public ProdutoService(IRepository<Produto, int> repository)
    {
        _repository = repository;
    }

    public async Task<Produto> GetProdutoByNomeAsync(string nome)
    {
        return await _repository.FindByPropertyAsync("Nome", nome);
    }

    public async Task<IEnumerable<Produto>> GetProdutosAtivosAsync()
    {
        return await _repository.FindAsync(p => p.Ativo);
    }
}