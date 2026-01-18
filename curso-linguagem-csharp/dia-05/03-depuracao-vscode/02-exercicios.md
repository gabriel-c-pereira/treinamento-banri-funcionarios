# Exercícios — Depuração no VS Code

## Instruções Gerais

- Todos os exercícios devem ser desenvolvidos no VS Code com C# Dev Kit
- Tire screenshots/capturas dos momentos-chave da depuração
- Documente os passos seguidos e as descobertas
- Use Git para versionar seu progresso

**Tempo estimado:** 6-8 horas

---

## Exercício 1: Configuração Básica ⭐

### Objetivo
Configurar o ambiente de depuração e usar breakpoints básicos para identificar bugs simples.

### Requisitos

Crie um projeto console chamado `CalculadoraDebug`:

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Calculadora ===\n");
        
        Console.Write("Digite o primeiro número: ");
        int num1 = int.Parse(Console.ReadLine());
        
        Console.Write("Digite o segundo número: ");
        int num2 = int.Parse(Console.ReadLine());
        
        Console.Write("Digite a operação (+, -, *, /): ");
        string operacao = Console.ReadLine();
        
        double resultado = Calcular(num1, num2, operacao);
        Console.WriteLine($"\nResultado: {resultado}");
    }
    
    static double Calcular(int a, int b, string op)
    {
        switch (op)
        {
            case "+":
                return a + b;
            case "-":
                return a - b;
            case "*":
                return a * b;
            case "/":
                return a / b;  // BUG: divisão por zero não tratada
            default:
                return 0;
        }
    }
}
```

### Tarefas

1. **Configure launch.json automaticamente**
   - Pressione F5 e deixe o VS Code gerar as configurações
   - Verifique os arquivos criados em `.vscode/`

2. **Adicione breakpoints**
   - Linha onde `num1` é lido
   - Linha onde `num2` é lido
   - Primeira linha do método `Calcular()`
   - Linha do `return a / b`

3. **Execute e inspecione**
   - Inicie a depuração (F5)
   - A cada breakpoint, anote os valores no painel **Locals**
   - Use **Step Over (F10)** e **Step Into (F11)**

4. **Identifique o bug**
   - Teste com divisão por zero
   - Capture a exceção quando ocorrer
   - Analise o Call Stack

### Entrega

- Código com breakpoints documentados
- Screenshot do momento da exceção
- Texto explicando como identificou o bug via depurador

### Critérios de Avaliação

- ✅ launch.json configurado corretamente
- ✅ 4 breakpoints posicionados estrategicamente
- ✅ Bug identificado e documentado
- ✅ Screenshots claros

---

## Exercício 2: Breakpoints Condicionais ⭐⭐

### Objetivo
Usar breakpoints avançados para debugar apenas situações específicas em loops.

### Requisitos

Crie um programa que processa lista de produtos:

```csharp
using System;
using System.Collections.Generic;

record Produto(int Id, string Nome, decimal Preco, int Estoque);

class Program
{
    static void Main()
    {
        var produtos = GerarProdutos();
        
        decimal valorTotal = 0;
        int itensForaEstoque = 0;
        int itensCaros = 0;
        
        foreach (var produto in produtos)
        {
            // BUG: lógica está somando produtos sem estoque
            valorTotal += produto.Preco * produto.Estoque;
            
            if (produto.Estoque == 0)
                itensForaEstoque++;
            
            if (produto.Preco > 1000)
                itensCaros++;
        }
        
        Console.WriteLine($"Valor total em estoque: R$ {valorTotal:N2}");
        Console.WriteLine($"Itens fora de estoque: {itensForaEstoque}");
        Console.WriteLine($"Itens caros (>R$1000): {itensCaros}");
    }
    
    static List<Produto> GerarProdutos()
    {
        return new List<Produto>
        {
            new(1, "Mouse", 50.00m, 100),
            new(2, "Teclado", 150.00m, 50),
            new(3, "Monitor", 1200.00m, 0),    // Sem estoque
            new(4, "Webcam", 300.00m, 25),
            new(5, "Headset", 250.00m, 30),
            new(6, "SSD 1TB", 800.00m, 15),
            new(7, "RTX 4090", 15000.00m, 0),  // Caro e sem estoque
            new(8, "Gabinete", 400.00m, 20)
        };
    }
}
```

### Tarefas

1. **Breakpoint Condicional #1**
   - Na linha do `foreach`
   - Condição: `produto.Estoque == 0`
   - Deve parar apenas nos produtos sem estoque

2. **Breakpoint Condicional #2**
   - Na linha `valorTotal +=`
   - Condição: `produto.Preco > 1000`
   - Deve parar apenas em produtos caros

3. **Logpoint**
   - Na linha `valorTotal +=`
   - Mensagem: `Processando {produto.Nome}: {produto.Preco:C} x {produto.Estoque} = {produto.Preco * produto.Estoque:C}`
   - Não deve parar execução

4. **Hit Count Breakpoint**
   - No início do `foreach`
   - Configurar para parar na 5ª iteração
   - Verificar qual produto está sendo processado

### Entrega

- Screenshots de cada tipo de breakpoint configurado
- Log completo do Debug Console com as mensagens do Logpoint
- Análise: "O breakpoint condicional economizou X cliques de F5"

### Critérios de Avaliação

- ✅ 2 breakpoints condicionais funcionando
- ✅ Logpoint exibindo mensagens corretas
- ✅ Hit count parando na iteração correta
- ✅ Análise de eficiência

---

## Exercício 3: Watch Expressions e Inspeção Profunda ⭐⭐

### Objetivo
Dominar Watch, Locals e modificação de variáveis em tempo de execução.

### Requisitos

Crie um sistema de pedidos com bug sutil:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

record ItemPedido(string Produto, int Quantidade, decimal PrecoUnitario)
{
    public decimal Subtotal => Quantidade * PrecoUnitario;
}

class Pedido
{
    public int Id { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
    public decimal Desconto { get; set; }
    public decimal TaxaEntrega { get; set; }
    
    public decimal CalcularTotal()
    {
        decimal subtotal = Itens.Sum(i => i.Subtotal);
        decimal comDesconto = subtotal - Desconto;  // BUG: desconto pode ser maior que subtotal
        decimal total = comDesconto + TaxaEntrega;
        return total;
    }
}

class Program
{
    static void Main()
    {
        var pedido = new Pedido
        {
            Id = 1001,
            Desconto = 100.00m,
            TaxaEntrega = 15.00m
        };
        
        pedido.Itens.Add(new ItemPedido("Mouse", 2, 50.00m));
        pedido.Itens.Add(new ItemPedido("Cabo USB", 3, 10.00m));
        
        var total = pedido.CalcularTotal();
        Console.WriteLine($"Total do pedido {pedido.Id}: R$ {total:N2}");
    }
}
```

### Tarefas

1. **Adicione ao Watch:**
   ```
   pedido.Itens.Count
   pedido.Itens.Sum(i => i.Subtotal)
   pedido.Desconto
   pedido.TaxaEntrega
   pedido.CalcularTotal()
   ```

2. **Inspecione passo a passo:**
   - Breakpoint no início de `CalcularTotal()`
   - Use Step Over (F10) em cada linha
   - Anote o valor de cada variável local

3. **Identifique o problema:**
   - Quando `comDesconto` fica negativo?
   - Use o Watch para calcular: `subtotal - Desconto`

4. **Modifique em runtime:**
   - Pare no breakpoint
   - No painel Variables, altere `Desconto` para `50.00`
   - Continue execução (F5)
   - Verifique se o bug desaparece

5. **Use Debug Console:**
   ```
   > pedido.Itens[0].Produto
   > pedido.Itens.Where(i => i.Quantidade > 2).ToList()
   > Math.Max(0, subtotal - Desconto)
   ```

### Entrega

- Tabela com valores de cada variável em cada passo
- Screenshot do Watch com todas as expressões
- Transcrição do Debug Console
- Solução proposta para o bug

### Critérios de Avaliação

- ✅ 5 expressões no Watch funcionando
- ✅ Bug identificado via inspeção
- ✅ Modificação de variável demonstrada
- ✅ Debug Console usado corretamente

---

## Exercício 4: Call Stack e Exceções ⭐⭐⭐

### Objetivo
Usar Call Stack para rastrear origem de exceções em código com múltiplas camadas.

### Requisitos

Crie uma aplicação em camadas com bug:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

// Camada de Dados
class ProdutoRepository
{
    private List<Produto> _produtos = new();
    
    public void Adicionar(Produto produto)
    {
        if (produto == null)
            throw new ArgumentNullException(nameof(produto));
        
        _produtos.Add(produto);
    }
    
    public Produto BuscarPorId(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        return produto;  // BUG: retorna null se não encontrar
    }
}

// Camada de Negócio
class ProdutoService
{
    private readonly ProdutoRepository _repository = new();
    
    public void CadastrarProduto(string nome, decimal preco)
    {
        var produto = new Produto(GetProximoId(), nome, preco);
        _repository.Adicionar(produto);
    }
    
    public decimal ObterPreco(int produtoId)
    {
        var produto = _repository.BuscarPorId(produtoId);
        return produto.Preco;  // BUG: NullReferenceException se produto == null
    }
    
    private int GetProximoId() => 1;  // Simplificado
}

// Camada de Apresentação
class ProdutoController
{
    private readonly ProdutoService _service = new();
    
    public void ExibirPreco(int produtoId)
    {
        try
        {
            var preco = _service.ObterPreco(produtoId);
            Console.WriteLine($"Preço: R$ {preco:N2}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}

record Produto(int Id, string Nome, decimal Preco);

class Program
{
    static void Main()
    {
        var controller = new ProdutoController();
        controller.ExibirPreco(999);  // Produto inexistente
    }
}
```

### Tarefas

1. **Configure Exception Breakpoint:**
   - Run > New Breakpoint > Exception Breakpoint
   - Tipo: `System.NullReferenceException`
   - Deve parar ANTES do catch

2. **Analise Call Stack:**
   - Execute até a exceção
   - Abra painel Call Stack
   - Anote TODA a hierarquia:
     ```
     ObterPreco() linha X
     ExibirPreco() linha Y
     Main() linha Z
     ```

3. **Navegue pelos frames:**
   - Clique em cada nível do Call Stack
   - Anote as variáveis locais de cada método
   - Capture screenshot de cada nível

4. **Trace a origem:**
   - Em qual método o bug foi introduzido?
   - Qual camada deveria ter tratado isso?
   - Onde o null foi criado?

5. **Implemente Exception Wrapping:**
   ```csharp
   public Produto BuscarPorId(int id)
   {
       var produto = _produtos.FirstOrDefault(p => p.Id == id);
       
       if (produto == null)
           throw new KeyNotFoundException($"Produto {id} não encontrado");
       
       return produto;
   }
   ```
   - Execute novamente com Exception Breakpoint
   - Compare os dois Call Stacks

### Entrega

- Diagrama do Call Stack completo (pode ser textual)
- Screenshots de cada frame do stack
- Análise: "A exceção foi lançada em ____, propagou por ____ e foi capturada em ____"
- Código corrigido com exception wrapping

### Critérios de Avaliação

- ✅ Exception breakpoint configurado
- ✅ Call Stack completamente documentado
- ✅ Navegação entre frames demonstrada
- ✅ Origem do bug identificada
- ✅ Solução implementada

---

## Exercício 5: Async/Await e Debugging ⭐⭐⭐

### Objetivo
Depurar código assíncrono e entender async call stack.

### Requisitos

```csharp
using System;
using System.Threading.Tasks;
using System.Net.Http;

class ApiClient
{
    private static readonly HttpClient _http = new();
    
    public async Task<string> BuscarDadosAsync(string url)
    {
        await Task.Delay(1000);  // Simula latência
        
        try
        {
            var response = await _http.GetStringAsync(url);
            return response;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Erro ao buscar dados", ex);
        }
    }
}

class DataProcessor
{
    private readonly ApiClient _client = new();
    
    public async Task<int> ProcessarDadosAsync(string url)
    {
        var dados = await _client.BuscarDadosAsync(url);
        
        // BUG: não valida se dados é null/vazio
        var linhas = dados.Split('\n');
        return linhas.Length;
    }
}

class Program
{
    static async Task Main()
    {
        var processor = new DataProcessor();
        
        try
        {
            var resultado = await processor.ProcessarDadosAsync("https://invalid-url-xyz.com/api/data");
            Console.WriteLine($"Linhas processadas: {resultado}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
```

### Tarefas

1. **Breakpoints em código async:**
   - Na linha `await Task.Delay(1000)`
   - Na linha `await _http.GetStringAsync(url)`
   - Na linha `await _client.BuscarDadosAsync(url)`
   - Na linha `await processor.ProcessarDadosAsync(...)`

2. **Observe Async Call Stack:**
   - Execute e pare em cada breakpoint
   - Capture o Call Stack em cada ponto
   - Note como os frames mudam com `await`

3. **Inspecione Tasks:**
   - No painel Variables, expanda os objetos Task
   - Veja Status, Result, Exception
   - Capture screenshot

4. **Debug Console com async:**
   ```
   > dados
   > dados?.Length
   > string.IsNullOrEmpty(dados)
   ```

5. **Tratamento de exceções assíncronas:**
   - Configure Exception Breakpoint para `HttpRequestException`
   - Veja onde a exceção é wrapeada
   - Analise InnerException no Variables

### Entrega

- Screenshots do Call Stack em cada await
- Comparação: "Call stack síncrono vs assíncrono"
- Debug Console log completo
- Explicação: "Como exceções são propagadas em código async?"

### Critérios de Avaliação

- ✅ Breakpoints em código assíncrono
- ✅ Async call stack documentado
- ✅ Tasks inspecionadas
- ✅ Exception handling assíncrono entendido

---

## Exercício 6: Depuração de Performance ⭐⭐⭐⭐

### Objetivo
Usar depurador para identificar gargalos de performance.

### Requisitos

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class PerformanceAnalyzer
{
    public void AnalisarAlgoritmos()
    {
        var numeros = GerarNumeros(10000);
        
        // Algoritmo 1: LINQ
        var sw1 = Stopwatch.StartNew();
        var resultado1 = numeros.Where(n => n % 2 == 0).Sum();
        sw1.Stop();
        
        // Algoritmo 2: For tradicional
        var sw2 = Stopwatch.StartNew();
        int resultado2 = 0;
        for (int i = 0; i < numeros.Count; i++)
        {
            if (numeros[i] % 2 == 0)
                resultado2 += numeros[i];
        }
        sw2.Stop();
        
        // Algoritmo 3: Foreach
        var sw3 = Stopwatch.StartNew();
        int resultado3 = 0;
        foreach (var numero in numeros)
        {
            if (numero % 2 == 0)
                resultado3 += numero;
        }
        sw3.Stop();
        
        Console.WriteLine($"LINQ: {sw1.ElapsedMilliseconds}ms");
        Console.WriteLine($"For: {sw2.ElapsedMilliseconds}ms");
        Console.WriteLine($"Foreach: {sw3.ElapsedMilliseconds}ms");
    }
    
    private List<int> GerarNumeros(int quantidade)
    {
        var random = new Random();
        var lista = new List<int>(quantidade);
        
        for (int i = 0; i < quantidade; i++)
        {
            lista.Add(random.Next(1, 1000));
        }
        
        return lista;
    }
}
```

### Tarefas

1. **Medição com Watch:**
   ```
   sw1.ElapsedMilliseconds
   sw2.ElapsedMilliseconds
   sw3.ElapsedMilliseconds
   ```

2. **Breakpoints após cada medição:**
   - Após `sw1.Stop()`
   - Após `sw2.Stop()`
   - Após `sw3.Stop()`
   - Compare tempos no Watch

3. **Hit Count em loops:**
   - Breakpoint com hit count no for
   - Configure: `% 1000` (a cada mil iterações)
   - Verifique progresso

4. **Profiling via Debug Console:**
   ```
   > numeros.Count
   > GC.GetTotalMemory(false)
   > Process.GetCurrentProcess().WorkingSet64
   ```

### Entrega

- Tabela comparativa de performance
- Screenshots do Watch em cada algoritmo
- Análise: qual é mais rápido e por quê?

### Critérios de Avaliação

- ✅ Medições precisas capturadas
- ✅ Watch usado para comparação
- ✅ Análise de performance documentada

---

## Exercício 7: Depuração Remota (Avançado) ⭐⭐⭐⭐⭐

### Objetivo
Configurar e usar depuração remota em container Docker.

### Requisitos

1. **Dockerfile:**

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet build -c Debug

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/bin/Debug/net8.0/ .

# Instalar vsdbg (depurador remoto)
RUN apt-get update && apt-get install -y curl
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

EXPOSE 5000
ENTRYPOINT ["dotnet", "MeuApp.dll"]
```

2. **launch.json:**

```json
{
    "name": "Docker .NET Attach",
    "type": "coreclr",
    "request": "attach",
    "processId": "${command:pickRemoteProcess}",
    "pipeTransport": {
        "pipeCwd": "${workspaceFolder}",
        "pipeProgram": "docker",
        "pipeArgs": ["exec", "-i", "meu-container"],
        "debuggerPath": "/vsdbg/vsdbg",
        "quoteArgs": false
    },
    "sourceFileMap": {
        "/app": "${workspaceFolder}"
    }
}
```

### Tarefas

1. Build e execute o container
2. Anexe o depurador
3. Adicione breakpoints
4. Faça requisições HTTP e veja breakpoints ativarem
5. Documente todo o processo

### Entrega

- Dockerfile completo
- launch.json configurado
- Screenshots da depuração funcionando
- Tutorial passo-a-passo

### Critérios de Avaliação

- ✅ Container construído corretamente
- ✅ Depurador anexado com sucesso
- ✅ Breakpoints funcionando remotamente
- ✅ Processo documentado

---

## 🎓 Resumo de Habilidades

Ao completar estes exercícios, você dominará:

- ✅ Configuração de launch.json para diferentes cenários
- ✅ Breakpoints básicos, condicionais, logpoints e hit count
- ✅ Navegação por código (Step Into/Over/Out)
- ✅ Inspeção e modificação de variáveis
- ✅ Análise de Call Stack (síncrono e assíncrono)
- ✅ Debug Console para expressões complexas
- ✅ Exception breakpoints
- ✅ Depuração de performance
- ✅ Depuração remota em containers

**Tempo total estimado:** 8-10 horas
