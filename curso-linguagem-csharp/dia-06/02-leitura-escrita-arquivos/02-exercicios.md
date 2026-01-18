# Exercícios — Leitura e Escrita de Arquivos

## Exercício 1: Contador de Palavras ⭐

**Objetivo:** Processar arquivo de texto e gerar estatísticas.

**Requisitos:**
- Criar classe `TextAnalyzer` com métodos:
  - `AnalyzeFile(string path)` - lê arquivo e retorna estatísticas
  - `GetTopWords(int count)` - retorna palavras mais frequentes
  - `SaveReport(string outputPath)` - salva relatório em arquivo

**Estrutura:**
```csharp
record TextStats(
    int TotalLines,
    int TotalWords,
    int TotalChars,
    Dictionary<string, int> WordFrequency
);
```

**Teste:** Use arquivo de texto grande (ex: livro) e verifique estatísticas.

---

## Exercício 2: Processador de CSV ⭐⭐

**Objetivo:** Manipular dados CSV com filtros e ordenação.

**Requisitos:**
- Criar classe `CsvProcessor` com métodos:
  - `LoadCsv(string path)` - lê CSV com cabeçalho
  - `Filter(Func<Dictionary<string, string>, bool> predicate)` - filtra linhas
  - `Sort(string columnName, bool ascending = true)` - ordena por coluna
  - `SaveCsv(string outputPath)` - salva resultado

**Estrutura:**
```csharp
record CsvData(
    string[] Headers,
    List<Dictionary<string, string>> Rows
);
```

**Teste:** Use CSV de vendas, filtre por valor > 1000, ordene por data.

---

## Exercício 3: Merge de Logs ⭐⭐⭐

**Objetivo:** Consolidar múltiplos arquivos de log.

**Requisitos:**
- Criar classe `LogMerger` com métodos:
  - `AddLogFile(string path)` - adiciona arquivo de log
  - `MergeByTimestamp()` - ordena todas entradas por timestamp
  - `RemoveDuplicates()` - remove entradas duplicadas
  - `SaveMerged(string outputPath)` - salva log consolidado

**Estrutura:**
```csharp
record LogEntry(
    DateTime Timestamp,
    string Level,
    string Message,
    string Source
);
```

**Teste:** Use 3+ arquivos de log diferentes, verifique ordenação e unicidade.
