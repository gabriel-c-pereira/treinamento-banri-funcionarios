# 01 - Leitura e Escrita de Arquivos

## 📚 Introdução

O namespace `System.IO` fornece classes para trabalhar com arquivos e diretórios no .NET. Neste tópico, você aprenderá as diferentes formas de ler e escrever arquivos, desde operações simples até streaming eficiente de grandes volumes de dados.

## 🎯 Objetivos

- Usar `File` e `FileInfo` para operações de arquivo
- Trabalhar com `Stream`, `StreamReader` e `StreamWriter`
- Entender diferenças entre leitura/escrita síncrona e assíncrona
- Implementar tratamento de erros de I/O
- Aplicar best practices de performance

## 📂 Classes Principais

### File vs FileInfo

```csharp
// File: métodos estáticos, operações únicas
File.WriteAllText("dados.txt", "Conteúdo");
string conteudo = File.ReadAllText("dados.txt");

// FileInfo: instância, múltiplas operações
var arquivo = new FileInfo("dados.txt");
long tamanho = arquivo.Length;
DateTime criacao = arquivo.CreationTime;
bool existe = arquivo.Exists;
```

**Quando usar cada um:**
- `File`: operações únicas e simples
- `FileInfo`: múltiplas operações no mesmo arquivo (mais eficiente)

### Hierarquia de Streams

```
Stream (abstrata)
├── FileStream      // Arquivos
├── MemoryStream    // Memória
├── NetworkStream   // Rede

StreamReader/StreamWriter
├── Trabalham sobre Stream
└── Facilitam leitura/escrita de texto
```

## 🔧 Operações Básicas

### Leitura Completa

```csharp
// Método 1: File.ReadAllText (mais simples)
string conteudo = File.ReadAllText("arquivo.txt");

// Método 2: File.ReadAllLines (retorna array)
string[] linhas = File.ReadAllLines("arquivo.txt");

// Método 3: File.ReadAllBytes (dados binários)
byte[] bytes = File.ReadAllBytes("imagem.png");
```

### Escrita Completa

```csharp
// Sobrescreve arquivo
File.WriteAllText("saida.txt", "Novo conteúdo");

// Anexa ao final
File.AppendAllText("log.txt", $"{DateTime.Now}: Evento\n");

// Múltiplas linhas
string[] linhas = { "Linha 1", "Linha 2", "Linha 3" };
File.WriteAllLines("saida.txt", linhas);
```

### StreamReader - Leitura Linha a Linha

```csharp
using (var reader = new StreamReader("grande-arquivo.txt"))
{
    string? linha;
    int numeroLinha = 0;
    
    while ((linha = reader.ReadLine()) != null)
    {
        numeroLinha++;
        Console.WriteLine($"{numeroLinha}: {linha}");
    }
}

// C# 8+ using declaration
using var reader = new StreamReader("arquivo.txt");
while (!reader.EndOfStream)
{
    string linha = reader.ReadLine();
    // Processa linha
}
```

### StreamWriter - Escrita Eficiente

```csharp
using (var writer = new StreamWriter("saida.txt"))
{
    for (int i = 0; i < 1000; i++)
    {
        writer.WriteLine($"Linha {i}");
    }
    // Flush automático ao dispor
}

using var writer = new StreamWriter("log.txt", append: true);
writer.WriteLine($"{DateTime.Now}: Evento registrado");
```

### FileStream - Controle Total

```csharp
// Leitura binária
using (var fs = new FileStream("dados.bin", FileMode.Open, FileAccess.Read))
{
    byte[] buffer = new byte[1024];
    int bytesLidos;
    
    while ((bytesLidos = fs.Read(buffer, 0, buffer.Length)) > 0)
    {
        // Processa buffer[0..bytesLidos]
    }
}

// Escrita binária
using (var fs = new FileStream("saida.bin", FileMode.Create, FileAccess.Write))
{
    byte[] dados = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"
    fs.Write(dados, 0, dados.Length);
}
```

## 🚀 Operações Assíncronas

```csharp
// Leitura assíncrona
string conteudo = await File.ReadAllTextAsync("arquivo.txt");

// Escrita assíncrona
await File.WriteAllTextAsync("saida.txt", "Conteúdo");

// StreamReader assíncrono
using var reader = new StreamReader("arquivo.txt");
while (!reader.EndOfStream)
{
    string linha = await reader.ReadLineAsync();
    await ProcessarLinhaAsync(linha);
}
```

## ⚠️ Tratamento de Exceções

```csharp
try
{
    string conteudo = File.ReadAllText("config.json");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Arquivo não encontrado: {ex.FileName}");
    // Criar arquivo padrão
}
catch (DirectoryNotFoundException ex)
{
    Console.WriteLine($"Diretório não existe: {ex.Message}");
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"Sem permissão: {ex.Message}");
}
catch (IOException ex)
{
    Console.WriteLine($"Erro de I/O: {ex.Message}");
}
```

## 💡 Best Practices

1. **Sempre use `using`** para garantir fechamento de streams
2. **Prefira operações assíncronas** para I/O
3. **Use buffering** para grandes volumes
4. **Trate exceções específicas** de I/O
5. **Valide caminhos** antes de usar

## 📊 Exemplo Completo

```csharp
public class ProcessadorLog
{
    public async Task ProcessarArquivoLogAsync(string caminhoEntrada, string caminhoSaida)
    {
        var linhasFiltradas = new List<string>();
        
        // Lê linha a linha (eficiente para arquivos grandes)
        using (var reader = new StreamReader(caminhoEntrada))
        {
            while (!reader.EndOfStream)
            {
                string linha = await reader.ReadLineAsync();
                
                if (linha.Contains("ERROR"))
                    linhasFiltradas.Add(linha);
            }
        }
        
        // Escreve resultado
        await File.WriteAllLinesAsync(caminhoSaida, linhasFiltradas);
        
        Console.WriteLine($"Processado: {linhasFiltradas.Count} erros encontrados");
    }
}
```
