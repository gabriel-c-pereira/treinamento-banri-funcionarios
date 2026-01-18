# 🔍 LINQ - Language Integrated Query

> **Tempo estimado**: 2 horas  
> **Nível**: Intermediário/Avançado

## 🎯 O que é LINQ?

**LINQ** (Language Integrated Query) permite fazer consultas em coleções C# usando sintaxe similar a SQL.

```csharp
// Sem LINQ ❌
List<Produto> resultado = new();
foreach (var produto in produtos)
{
    if (produto.Preco > 100)
        resultado.Add(produto);
}

// Com LINQ ✅
var resultado = produtos.Where(p => p.Preco > 100).ToList();
```

---

## 📝 Method Syntax vs Query Syntax

### Method Syntax (Mais Comum)

```csharp
var result = produtos
    .Where(p => p.Preco > 100)
    .OrderBy(p => p.Nome)
    .Select(p => p.Nome)
    .ToList();
```

### Query Syntax (SQL-like)

```csharp
var result = from p in produtos
             where p.Preco > 100
             orderby p.Nome
             select p.Nome;
```

**💡 Dica**: Use method syntax. É mais flexível e comum.

---

## 🔎 Where - Filtrar

```csharp
List<int> numeros = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Números pares
var pares = numeros.Where(x => x % 2 == 0);

// Produtos caros
var produtosCaros = produtos.Where(p => p.Preco > 100);

// Múltiplas condições
var resultado = produtos
    .Where(p => p.Preco > 100 && p.Estoque > 0);
```

---

## 🔄 Select - Projetar/Transformar

```csharp
List<Produto> produtos = ObterProdutos();

// Selecionar apenas nomes
var nomes = produtos.Select(p => p.Nome);

// Transformar em outro tipo
var precos = produtos.Select(p => p.Preco);

// Criar objeto anônimo
var resumo = produtos.Select(p => new 
{ 
    p.Nome, 
    p.Preco,
    PrecoComDesconto = p.Preco * 0.9m
});

// Transformação complexa
var produtosDTO = produtos.Select(p => new ProdutoDTO
{
    Id = p.Id,
    NomeCompleto = $"{p.Nome} - {p.Categoria}",
    PrecoFormatado = p.Preco.ToString("C")
});
```

---

## 📊 OrderBy - Ordenar

```csharp
// Crescente
var ordenados = produtos.OrderBy(p => p.Preco);

// Decrescente
var ordenadosDesc = produtos.OrderByDescending(p => p.Preco);

// Ordenar por múltiplos campos
var multiOrdem = produtos
    .OrderBy(p => p.Categoria)  // Primeiro por categoria
    .ThenBy(p => p.Preco)       // Depois por preço
    .ThenByDescending(p => p.Nome);  // Depois por nome (desc)
```

---

## 🎯 First, FirstOrDefault, Single

```csharp
List<Produto> produtos = ObterProdutos();

// First - primeiro elemento (lança exceção se vazio)
var primeiro = produtos.First();
var primeiroFiltrado = produtos.First(p => p.Preco > 100);

// FirstOrDefault - retorna default se vazio (null para classes)
var primeiroOuNull = produtos.FirstOrDefault();
var primeiroOuNull2 = produtos.FirstOrDefault(p => p.Preco > 1000);

// Single - único elemento (exceção se 0 ou >1)
var unico = produtos.Single(p => p.Id == 123);

// SingleOrDefault - permite 0 ou 1
var unicoOuNull = produtos.SingleOrDefault(p => p.Id == 123);

// Last, LastOrDefault
var ultimo = produtos.Last();
var ultimoOuNull = produtos.LastOrDefault();
```

---

## ✅ Any, All, Contains

```csharp
// Any - existe pelo menos um?
bool temCaros = produtos.Any(p => p.Preco > 100);
bool temProdutos = produtos.Any();  // Lista não está vazia?

// All - todos atendem?
bool todosEmEstoque = produtos.All(p => p.Estoque > 0);
bool todosBaratos = produtos.All(p => p.Preco < 1000);

// Contains - contém elemento específico?
bool contemProduto = produtos.Contains(produto1);
bool contemId = produtos.Select(p => p.Id).Contains(123);
```

---

## 📈 Agregações

```csharp
List<int> numeros = new() { 1, 2, 3, 4, 5 };

// Count - quantidade
int total = numeros.Count();
int maioresQue2 = numeros.Count(x => x > 2);  // 3

// Sum - soma
int soma = numeros.Sum();  // 15
decimal totalPrecos = produtos.Sum(p => p.Preco);

// Average - média
double media = numeros.Average();  // 3
decimal precoMedio = produtos.Average(p => p.Preco);

// Min - mínimo
int minimo = numeros.Min();  // 1
decimal menorPreco = produtos.Min(p => p.Preco);

// Max - máximo
int maximo = numeros.Max();  // 5
decimal maiorPreco = produtos.Max(p => p.Preco);
```

---

## 🗂️ GroupBy - Agrupar

```csharp
// Agrupar por categoria
var grupos = produtos.GroupBy(p => p.Categoria);

foreach (var grupo in grupos)
{
    Console.WriteLine($"Categoria: {grupo.Key}");
    Console.WriteLine($"Quantidade: {grupo.Count()}");
    Console.WriteLine($"Preço médio: {grupo.Average(p => p.Preco):C}");
    
    foreach (var produto in grupo)
        Console.WriteLine($"  - {produto.Nome}");
}

// Com Select para formatar
var resumoPorCategoria = produtos
    .GroupBy(p => p.Categoria)
    .Select(g => new
    {
        Categoria = g.Key,
        Quantidade = g.Count(),
        PrecoTotal = g.Sum(p => p.Preco),
        PrecoMedio = g.Average(p => p.Preco)
    });
```

---

## 🔗 Join - Juntar Coleções

```csharp
List<Cliente> clientes = ObterClientes();
List<Pedido> pedidos = ObterPedidos();

// Inner Join
var pedidosComClientes = from p in pedidos
                         join c in clientes on p.ClienteId equals c.Id
                         select new
                         {
                             NumeroPedido = p.Numero,
                             NomeCliente = c.Nome,
                             Total = p.Total
                         };

// Method syntax
var resultado = pedidos
    .Join(clientes,
          p => p.ClienteId,    // chave do pedido
          c => c.Id,           // chave do cliente
          (p, c) => new        // resultado
          {
              p.Numero,
              c.Nome,
              p.Total
          });

// Left Join (GroupJoin)
var clientesComPedidos = clientes
    .GroupJoin(pedidos,
              c => c.Id,
              p => p.ClienteId,
              (cliente, pedidosDoCliente) => new
              {
                  cliente.Nome,
                  QuantidadePedidos = pedidosDoCliente.Count(),
                  TotalGasto = pedidosDoCliente.Sum(p => p.Total)
              });
```

---

## 🎛️ Skip e Take - Paginação

```csharp
int paginaAtual = 2;
int itensPorPagina = 10;

var paginaDeResultados = produtos
    .Skip((paginaAtual - 1) * itensPorPagina)
    .Take(itensPorPagina)
    .ToList();

// Exemplo: página 2, 10 itens por página
// Skip(10) - pula os primeiros 10
// Take(10) - pega os próximos 10
```

---

## 🔀 Distinct, Union, Intersect, Except

```csharp
List<int> lista1 = new() { 1, 2, 3, 3, 4 };
List<int> lista2 = new() { 3, 4, 5, 6 };

// Distinct - remove duplicatas
var unicos = lista1.Distinct();  // { 1, 2, 3, 4 }

// Union - união (sem duplicatas)
var uniao = lista1.Union(lista2);  // { 1, 2, 3, 4, 5, 6 }

// Intersect - interseção (em comum)
var intersecao = lista1.Intersect(lista2);  // { 3, 4 }

// Except - diferença
var diferenca = lista1.Except(lista2);  // { 1, 2 }
```

---

## ⚡ Deferred Execution

```csharp
// Query NÃO é executada aqui!
var query = produtos.Where(p => p.Preco > 100);

// É executada quando enumera
foreach (var p in query)  // ← Aqui executa!
    Console.WriteLine(p.Nome);

// Ou quando força materialização
var lista = query.ToList();  // ← Aqui executa!
var array = query.ToArray();
int count = query.Count();
```

### Immediate Execution

```csharp
// Métodos que executam imediatamente:
var lista = produtos.ToList();        // ← Executa agora
var array = produtos.ToArray();       // ← Executa agora
var dict = produtos.ToDictionary(p => p.Id);  // ← Executa agora

int count = produtos.Count();         // ← Executa agora
decimal total = produtos.Sum(p => p.Preco);   // ← Executa agora
var primeiro = produtos.First();      // ← Executa agora
```

---

## 💡 Exemplo Completo

```csharp
// Consulta complexa
var relatorio = produtos
    .Where(p => p.Estoque > 0)                    // Filtrar
    .OrderBy(p => p.Categoria)                     // Ordenar
    .ThenByDescending(p => p.Preco)                // Depois por preço
    .GroupBy(p => p.Categoria)                     // Agrupar
    .Select(g => new                               // Projetar
    {
        Categoria = g.Key,
        Quantidade = g.Count(),
        TotalEstoque = g.Sum(p => p.Estoque),
        PrecoMedio = g.Average(p => p.Preco),
        MaisCaro = g.OrderByDescending(p => p.Preco).First()
    })
    .ToList();                                     // Materializar
```

---

## 🎓 Resumo

✅ **Where**: filtrar  
✅ **Select**: transformar/projetar  
✅ **OrderBy**: ordenar  
✅ **GroupBy**: agrupar  
✅ **First/FirstOrDefault**: primeiro elemento  
✅ **Any/All**: verificações  
✅ **Sum/Average/Min/Max**: agregações  
✅ **Join**: juntar coleções  
✅ **Skip/Take**: paginação  
✅ **Deferred execution**: query executa quando enumera  

➡️ **Próximo**: Projeto Final
