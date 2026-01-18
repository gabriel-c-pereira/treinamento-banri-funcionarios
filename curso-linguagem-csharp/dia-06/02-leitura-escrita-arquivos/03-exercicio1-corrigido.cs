using System.Text.RegularExpressions;

public record TextStats(
    int TotalLines,
    int TotalWords,
    int TotalChars,
    Dictionary<string, int> WordFrequency
);

public class TextAnalyzer
{
    public TextStats AnalyzeFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Arquivo não encontrado", path);

        var content = File.ReadAllText(path);
        var lines = File.ReadAllLines(path);

        var wordFrequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // Conta palavras (remove pontuação)
        var words = Regex.Split(content.ToLower(), @"\W+")
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .ToArray();

        foreach (var word in words)
        {
            if (wordFrequency.ContainsKey(word))
                wordFrequency[word]++;
            else
                wordFrequency[word] = 1;
        }

        return new TextStats(
            TotalLines: lines.Length,
            TotalWords: words.Length,
            TotalChars: content.Length,
            WordFrequency: wordFrequency
        );
    }

    public IEnumerable<KeyValuePair<string, int>> GetTopWords(int count)
    {
        return _currentStats?.WordFrequency
            .OrderByDescending(kv => kv.Value)
            .Take(count) ?? Enumerable.Empty<KeyValuePair<string, int>>();
    }

    public void SaveReport(string outputPath)
    {
        if (_currentStats == null) return;

        using var writer = new StreamWriter(outputPath);
        writer.WriteLine("=== RELATÓRIO DE ANÁLISE DE TEXTO ===");
        writer.WriteLine($"Arquivo analisado: {Path.GetFileName(_inputPath)}");
        writer.WriteLine($"Data/Hora: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        writer.WriteLine();

        writer.WriteLine("ESTATÍSTICAS GERAIS:");
        writer.WriteLine($"• Total de linhas: {_currentStats.TotalLines}");
        writer.WriteLine($"• Total de palavras: {_currentStats.TotalWords}");
        writer.WriteLine($"• Total de caracteres: {_currentStats.TotalChars}");
        writer.WriteLine();

        writer.WriteLine("PALAVRAS MAIS FREQUENTES:");
        var topWords = GetTopWords(10);
        foreach (var (word, count) in topWords)
        {
            writer.WriteLine($"• {word}: {count} vezes");
        }
    }

    private TextStats? _currentStats;
    private string? _inputPath;

    public TextStats AnalyzeAndStore(string path)
    {
        _inputPath = path;
        _currentStats = AnalyzeFile(path);
        return _currentStats;
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        var analyzer = new TextAnalyzer();

        try
        {
            // Analisa arquivo
            var stats = analyzer.AnalyzeAndStore("texto.txt");

            Console.WriteLine("Análise concluída:");
            Console.WriteLine($"Linhas: {stats.TotalLines}");
            Console.WriteLine($"Palavras: {stats.TotalWords}");
            Console.WriteLine($"Caracteres: {stats.TotalChars}");

            Console.WriteLine("\nTop 5 palavras:");
            foreach (var (word, count) in analyzer.GetTopWords(5))
            {
                Console.WriteLine($"  {word}: {count}");
            }

            // Salva relatório
            analyzer.SaveReport("relatorio.txt");
            Console.WriteLine("\nRelatório salvo em 'relatorio.txt'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}