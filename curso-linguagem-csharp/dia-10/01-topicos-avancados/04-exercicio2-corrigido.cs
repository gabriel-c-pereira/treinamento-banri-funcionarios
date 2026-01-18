public interface IMapper
{
    TDest Map<TSource, TDest>(TSource source) where TDest : new();
    void Map<TSource, TDest>(TSource source, TDest destination);
    object Map(object source, Type destinationType);
}

public class ReflectionMapper : IMapper
{
    private readonly Dictionary<string, PropertyMap> _propertyMaps = new();

    public TDest Map<TSource, TDest>(TSource source) where TDest : new()
    {
        var destination = new TDest();
        Map(source, destination);
        return destination;
    }

    public void Map<TSource, TDest>(TSource source, TDest destination)
    {
        if (source == null || destination == null)
            return;

        var sourceType = typeof(TSource);
        var destinationType = typeof(TDest);

        var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destinationProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destProperty in destinationProperties)
        {
            if (!destProperty.CanWrite)
                continue;

            // Primeiro tenta mapear por nome exato
            var sourceProperty = sourceProperties.FirstOrDefault(p =>
                p.Name.Equals(destProperty.Name, StringComparison.OrdinalIgnoreCase));

            if (sourceProperty == null)
                continue;

            if (!sourceProperty.CanRead)
                continue;

            try
            {
                var value = sourceProperty.GetValue(source);
                var convertedValue = ConvertValue(value, destProperty.PropertyType);
                destProperty.SetValue(destination, convertedValue);
            }
            catch (Exception ex)
            {
                // Log error and continue
                Console.WriteLine($"Error mapping property {destProperty.Name}: {ex.Message}");
            }
        }
    }

    public object Map(object source, Type destinationType)
    {
        if (source == null)
            return null;

        var destination = Activator.CreateInstance(destinationType);
        MapInternal(source, destination, source.GetType(), destinationType);
        return destination;
    }

    private void MapInternal(object source, object destination, Type sourceType, Type destinationType)
    {
        var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destinationProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destProperty in destinationProperties)
        {
            if (!destProperty.CanWrite)
                continue;

            var sourceProperty = sourceProperties.FirstOrDefault(p =>
                p.Name.Equals(destProperty.Name, StringComparison.OrdinalIgnoreCase));

            if (sourceProperty == null)
                continue;

            if (!sourceProperty.CanRead)
                continue;

            try
            {
                var value = sourceProperty.GetValue(source);
                var convertedValue = ConvertValue(value, destProperty.PropertyType);
                destProperty.SetValue(destination, convertedValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error mapping property {destProperty.Name}: {ex.Message}");
            }
        }
    }

    private object ConvertValue(object value, Type targetType)
    {
        if (value == null)
            return null;

        var sourceType = value.GetType();

        if (targetType.IsAssignableFrom(sourceType))
            return value;

        // Conversões básicas
        if (targetType == typeof(string))
            return value.ToString();

        if (targetType == typeof(int) && sourceType == typeof(string))
        {
            if (int.TryParse((string)value, out var result))
                return result;
        }

        if (targetType == typeof(decimal) && sourceType == typeof(string))
        {
            if (decimal.TryParse((string)value, out var result))
                return result;
        }

        if (targetType == typeof(bool) && sourceType == typeof(string))
        {
            if (bool.TryParse((string)value, out var result))
                return result;
        }

        // Conversão genérica usando Convert
        try
        {
            return Convert.ChangeType(value, targetType);
        }
        catch
        {
            throw new InvalidCastException($"Cannot convert {sourceType.Name} to {targetType.Name}");
        }
    }
}

public class PropertyMap
{
    public string SourceProperty { get; set; }
    public string DestinationProperty { get; set; }
    public Func<object, object> Converter { get; set; }
}

// Exemplo de uso
public class PessoaDto
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public string DataNascimento { get; set; }
}

public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascimento { get; set; }
}

public class MappingExample
{
    private readonly IMapper _mapper;

    public MappingExample(IMapper mapper)
    {
        _mapper = mapper;
    }

    public PessoaDto MapPessoaToDto(Pessoa pessoa)
    {
        var dto = _mapper.Map<Pessoa, PessoaDto>(pessoa);
        // Ajuste manual para propriedade composta
        dto.NomeCompleto = $"{pessoa.Nome} {pessoa.Sobrenome}";
        dto.DataNascimento = pessoa.DataNascimento.ToString("dd/MM/yyyy");
        return dto;
    }

    public Pessoa MapDtoToPessoa(PessoaDto dto)
    {
        var pessoa = _mapper.Map<PessoaDto, Pessoa>(dto);
        // Separar nome completo
        var partesNome = dto.NomeCompleto?.Split(' ') ?? new string[0];
        pessoa.Nome = partesNome.Length > 0 ? partesNome[0] : string.Empty;
        pessoa.Sobrenome = partesNome.Length > 1 ? string.Join(" ", partesNome.Skip(1)) : string.Empty;

        // Converter data
        if (DateTime.TryParse(dto.DataNascimento, out var dataNasc))
        {
            pessoa.DataNascimento = dataNasc;
        }

        return pessoa;
    }
}