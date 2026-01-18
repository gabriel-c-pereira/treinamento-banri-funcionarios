using System.Globalization;

public record LogEntry(
    DateTime Timestamp,
    string Level,
    string Message,
    string Source
);

public class LogMerger
{
    private readonly List<LogEntry> _entries = new();

    public void AddLogFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Arquivo de log não encontrado", path);

        var lines = File.ReadAllLines(path);
        var source = Path.GetFileNameWithoutExtension(path);

        foreach (var line in lines)
        {
            var entry = ParseLogLine(line, source);
            if (entry != null)
            {
                _entries.Add(entry);
            }
        }
    }

    public void MergeByTimestamp()
    {
        _entries.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
    }

    public void RemoveDuplicates()
    {
        // Remove duplicatas baseadas em timestamp + message
        var unique = new HashSet<string>();
        _entries.RemoveAll(entry =>
        {
            var key = $"{entry.Timestamp:yyyy-MM-dd HH:mm:ss}|{entry.Message}";
            return !unique.Add(key);
        });
    }

    public void SaveMerged(string outputPath)
    {
        using var writer = new StreamWriter(outputPath);

        writer.WriteLine("# LOG CONSOLIDADO");
        writer.WriteLine($"# Gerado em: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        writer.WriteLine($"# Total de entradas: {_entries.Count}");
        writer.WriteLine();

        foreach (var entry in _entries)
        {
            writer.WriteLine(FormatLogEntry(entry));
        }
    }

    private LogEntry? ParseLogLine(string line, string source)
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            return null;

        // Formato esperado: [2025-10-27 14:30:45] INFO Mensagem do log
        var match = Regex.Match(line, @"\[(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\]\s+(\w+)\s+(.+)");

        if (!match.Success)
            return null;

        if (!DateTime.TryParseExact(match.Groups[1].Value,
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var timestamp))
        {
            return null;
        }

        return new LogEntry(
            Timestamp: timestamp,
            Level: match.Groups[2].Value,
            Message: match.Groups[3].Value.Trim(),
            Source: source
        );
    }

    private string FormatLogEntry(LogEntry entry)
    {
        return $"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}] [{entry.Level}] [{entry.Source}] {entry.Message}";
    }

    public int GetEntryCount() => _entries.Count;
}

// Programa principal
class Program
{
    static void Main()
    {
        var merger = new LogMerger();

        try
        {
            // Adiciona arquivos de log
            merger.AddLogFile("app1.log");
            merger.AddLogFile("app2.log");
            merger.AddLogFile("system.log");

            Console.WriteLine($"Total de entradas carregadas: {merger.GetEntryCount()}");

            // Processa
            merger.MergeByTimestamp();
            merger.RemoveDuplicates();

            Console.WriteLine($"Após processamento: {merger.GetEntryCount()} entradas únicas");

            // Salva
            merger.SaveMerged("logs_consolidados.log");
            Console.WriteLine("Logs consolidados salvos em 'logs_consolidados.log'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}