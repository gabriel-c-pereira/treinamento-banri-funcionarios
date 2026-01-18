public record CsvData(
    string[] Headers,
    List<Dictionary<string, string>> Rows
);

public class CsvProcessor
{
    public CsvData LoadCsv(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Arquivo CSV não encontrado", path);

        var lines = File.ReadAllLines(path);
        if (lines.Length == 0)
            throw new InvalidDataException("Arquivo CSV vazio");

        var headers = lines[0].Split(',').Select(h => h.Trim('"')).ToArray();
        var rows = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Length; i++)
        {
            var values = ParseCsvLine(lines[i]);
            if (values.Length == headers.Length)
            {
                var row = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length; j++)
                {
                    row[headers[j]] = values[j];
                }
                rows.Add(row);
            }
        }

        return new CsvData(headers, rows);
    }

    public CsvData Filter(CsvData data, Func<Dictionary<string, string>, bool> predicate)
    {
        var filteredRows = data.Rows.Where(predicate).ToList();
        return new CsvData(data.Headers, filteredRows);
    }

    public CsvData Sort(CsvData data, string columnName, bool ascending = true)
    {
        if (!data.Headers.Contains(columnName))
            throw new ArgumentException($"Coluna '{columnName}' não encontrada");

        var sortedRows = ascending
            ? data.Rows.OrderBy(row => row[columnName]).ToList()
            : data.Rows.OrderByDescending(row => row[columnName]).ToList();

        return new CsvData(data.Headers, sortedRows);
    }

    public void SaveCsv(CsvData data, string outputPath)
    {
        using var writer = new StreamWriter(outputPath);

        // Cabeçalho
        writer.WriteLine(string.Join(",", data.Headers.Select(h => $"\"{h}\"")));

        // Dados
        foreach (var row in data.Rows)
        {
            var values = data.Headers.Select(h => $"\"{row[h]}\"");
            writer.WriteLine(string.Join(",", values));
        }
    }

    private string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var current = "";
        var inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current += '"';
                    i++; // Pula próxima aspa
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current.Trim('"'));
                current = "";
            }
            else
            {
                current += c;
            }
        }

        result.Add(current.Trim('"'));
        return result.ToArray();
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        var processor = new CsvProcessor();

        try
        {
            // Carrega CSV
            var data = processor.LoadCsv("vendas.csv");
            Console.WriteLine($"CSV carregado: {data.Rows.Count} linhas");

            // Filtra vendas > 1000
            var filtrado = processor.Filter(data,
                row => decimal.TryParse(row["Valor"], out var valor) && valor > 1000);

            Console.WriteLine($"Após filtro: {filtrado.Rows.Count} linhas");

            // Ordena por data
            var ordenado = processor.Sort(filtrado, "Data", ascending: false);

            // Salva resultado
            processor.SaveCsv(ordenado, "vendas_filtradas.csv");
            Console.WriteLine("Resultado salvo em 'vendas_filtradas.csv'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}