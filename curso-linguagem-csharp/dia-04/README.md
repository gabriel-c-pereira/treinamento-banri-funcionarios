# 📘 DIA 04 - Coleções, Listas e LINQ

> **Duração**: 8 horas  
> **Pré-requisitos**: Dias 01-03 completos  
> **Nível**: Intermediário

---

## 🎯 Objetivos do Dia

Ao final deste dia, você será capaz de:

✅ Trabalhar com **arrays** e coleções genéricas  
✅ Usar `List<T>`, `Dictionary<TKey, TValue>`, `HashSet<T>`, `Queue<T>` e `Stack<T>`  
✅ Escrever **Lambda Expressions** eficientes  
✅ Dominar **LINQ** para consultas de dados  
✅ Aplicar **métodos de extensão** personalizados  
✅ Manipular dados de forma funcional  

---

## 📚 Conteúdo do Dia

### 1️⃣ **Arrays e Coleções Genéricas** (2h)

**O que você aprenderá:**
- Arrays unidimensionais e multidimensionais
- `List<T>`: lista dinâmica mais usada
- `Dictionary<TKey, TValue>`: pares chave-valor
- `HashSet<T>`: coleção sem duplicatas
- `Queue<T>`: fila (FIFO)
- `Stack<T>`: pilha (LIFO)
- Quando usar cada coleção

**Conceitos:**
```csharp
// List - dinâmica e indexável
List<int> numeros = new() { 1, 2, 3 };

// Dictionary - busca rápida por chave
Dictionary<string, Cliente> clientes = new();

// HashSet - sem duplicatas
HashSet<string> emails = new();

// Queue - fila (primeiro a entrar, primeiro a sair)
Queue<Pedido> filaProcessamento = new();

// Stack - pilha (último a entrar, primeiro a sair)
Stack<string> historicoNavegacao = new();
```

**Exercícios:**
- Sistema de agenda de contatos (Dictionary)
- Gerenciador de tarefas (List)
- Fila de atendimento (Queue)
- Sistema de cache LRU (Stack + Dictionary)

---

### 2️⃣ **Lambda Expressions** (2h)

**O que você aprenderá:**
- Sintaxe de lambdas
- Expression lambdas vs Statement lambdas
- Delegates e Func/Action
- Closures e captura de variáveis
- Lambdas como parâmetros

**Conceitos:**
```csharp
// Expression lambda
Func<int, int> dobro = x => x * 2;

// Statement lambda
Action<string> imprimir = mensagem => 
{
    Console.WriteLine($"Mensagem: {mensagem}");
};

// Lambda com múltiplos parâmetros
Func<int, int, int> soma = (a, b) => a + b;

// Lambda inline
List<int> pares = numeros.Where(x => x % 2 == 0).ToList();
```

**Exercícios:**
- Calculadora com lambdas
- Sistema de filtros dinâmicos
- Event handlers com lambdas
- Builder pattern com fluent API

---

### 3️⃣ **LINQ - Introdução e Métodos Básicos** (2h)

**O que você aprenderá:**
- O que é LINQ (Language Integrated Query)
- Query syntax vs Method syntax
- Métodos fundamentais:
  - `Where`: filtrar
  - `Select`: projetar
  - `OrderBy/OrderByDescending`: ordenar
  - `GroupBy`: agrupar
  - `First/FirstOrDefault`: obter primeiro
  - `Any/All`: verificar condições
  - `Count/Sum/Average/Min/Max`: agregações

**Conceitos:**
```csharp
List<Produto> produtos = ObterProdutos();

// Method syntax (mais comum)
var produtosCaros = produtos
    .Where(p => p.Preco > 100)
    .OrderBy(p => p.Nome)
    .Select(p => new { p.Nome, p.Preco })
    .ToList();

// Query syntax (SQL-like)
var produtosCaros2 = from p in produtos
                     where p.Preco > 100
                     orderby p.Nome
                     select new { p.Nome, p.Preco };
```

**Exercícios:**
- Consultas em catálogo de produtos
- Análise de vendas
- Relatórios com agregações
- Dashboard analítico

---

### 4️⃣ **LINQ Avançado e Performance** (1h 30min)

**O que você aprenderá:**
- Join de coleções
- SelectMany para flatten
- Distinct, Union, Intersect, Except
- Skip, Take para paginação
- Defer red execution vs Immediate execution
- Lazy evaluation
- Performance e otimizações

**Conceitos:**
```csharp
// Join de coleções
var pedidosComClientes = from p in pedidos
                         join c in clientes on p.ClienteId equals c.Id
                         select new { p.Numero, c.Nome, p.Total };

// SelectMany - flatten
var todosProdutos = pedidos.SelectMany(p => p.Itens);

// Paginação
var pagina = produtos.Skip(20).Take(10);

// Distinct
var categoriasUnicas = produtos.Select(p => p.Categoria).Distinct();
```

**Exercícios:**
- Sistema de relatórios complexos
- Paginação de resultados
- Junções de múltiplas tabelas
- Análise de performance

---

### 5️⃣ **Projeto Final: Sistema de E-commerce** (30min)

Sistema completo integrando todos os conceitos.

**Features:**
- Catálogo de produtos (List + LINQ)
- Carrinho de compras (Dictionary)
- Filtros dinâmicos (Lambda + LINQ)
- Relatórios de vendas (LINQ aggregations)
- Sistema de recomendação (LINQ + algoritmo)
- Paginação de resultados
- Cache de consultas (Dictionary)

---

## ⏱️ Cronograma Sugerido (8 horas)

### 🌅 Manhã (4 horas)

#### 09:00 - 10:30 | Tópico 1: Arrays e Coleções
- Teoria: tipos de coleções
- Exemplos práticos de cada tipo
- Exercícios 1-3

#### 10:30 - 10:45 | ☕ Intervalo

#### 10:45 - 12:00 | Tópico 1: Prática
- Exercícios 4-7
- Projeto: Sistema de Agenda

#### 12:00 - 14:00 | 🍽️ Almoço

---

### 🌆 Tarde (4 horas)

#### 14:00 - 15:30 | Tópico 2: Lambda Expressions
- Sintaxe e delegates
- Exercícios 1-5
- Closures e captures

#### 15:30 - 15:45 | ☕ Intervalo

#### 15:45 - 17:00 | Tópico 3: LINQ Básico
- Where, Select, OrderBy
- Exercícios 1-5
- Query vs Method syntax

#### 17:00 - 18:00 | Tópicos 4-5: LINQ Avançado + Projeto
- Join, GroupBy, agregações
- Projeto final integrado

---

## 🎯 Projetos do Dia

### Projeto 1: **Sistema de Biblioteca** 📚
- Catálogo de livros (List)
- Índice por autor (Dictionary)
- Sistema de empréstimos (Queue)
- Histórico de leituras (Stack)
- Busca e filtros (LINQ)

### Projeto 2: **Sistema de Vendas** 💰
- Catálogo de produtos
- Carrinho de compras
- Histórico de pedidos
- Relatórios de vendas
- Dashboard analítico

### Projeto 3: **Rede Social Simplificada** 👥
- Lista de usuários
- Sistema de amizades (HashSet)
- Feed de postagens (List + LINQ)
- Sistema de recomendação
- Estatísticas e métricas

---

## 📋 Checklist de Progresso

### Tópico 1: Arrays e Coleções
- [ ] Entendo a diferença entre array e List
- [ ] Sei quando usar Dictionary vs List
- [ ] Conheço HashSet e suas vantagens
- [ ] Entendo Queue (FIFO) vs Stack (LIFO)
- [ ] Completei exercícios 1-10

### Tópico 2: Lambda Expressions
- [ ] Sei escrever expression lambdas
- [ ] Entendo statement lambdas
- [ ] Conheço Func e Action
- [ ] Sei usar lambdas como parâmetros
- [ ] Completei exercícios 1-10

### Tópico 3: LINQ Básico
- [ ] Domino Where, Select, OrderBy
- [ ] Sei usar First, Any, All
- [ ] Conheço agregações (Sum, Average, etc)
- [ ] Entendo query vs method syntax
- [ ] Completei exercícios 1-10

### Tópico 4: LINQ Avançado
- [ ] Sei fazer Join de coleções
- [ ] Uso SelectMany corretamente
- [ ] Entendo paginação (Skip/Take)
- [ ] Conheço deferred execution
- [ ] Completei exercícios 1-10

### Projeto Final
- [ ] Implementei catálogo de produtos
- [ ] Criei sistema de carrinho
- [ ] Adicionei filtros e buscas
- [ ] Gerei relatórios com LINQ
- [ ] Sistema está funcional

---

## 🎓 Conceitos-Chave

**Coleções:**
- `List<T>`: dinâmica, indexável, permite duplicatas
- `Dictionary<TKey, TValue>`: chave-valor, busca O(1)
- `HashSet<T>`: sem duplicatas, operações de conjunto
- `Queue<T>`: FIFO (First In, First Out)
- `Stack<T>`: LIFO (Last In, First Out)

**Lambda:**
- Funções anônimas concisas
- `=>` (vai para) operador lambda
- `Func<T, TResult>`: retorna valor
- `Action<T>`: não retorna valor

**LINQ:**
- **Deferred execution**: query só executa quando enumera
- **Immediate execution**: métodos como `ToList()`, `Count()`
- **Method chaining**: encadear operações
- **Query comprehension**: sintaxe SQL-like

---

## ❓ FAQ

**P: Qual a diferença entre array e List?**  
R: Array tem tamanho fixo, List é dinâmica. Use List na maioria dos casos.

**P: Quando usar Dictionary?**  
R: Quando precisa buscar rapidamente por chave. Ex: cache, índices.

**P: Query syntax ou Method syntax?**  
R: Method syntax é mais comum e poderoso. Query syntax é bom para queries complexas com joins.

**P: LINQ é lento?**  
R: Não! É otimizado e usa deferred execution. Evite `ToList()` desnecessários.

**P: Posso usar LINQ com banco de dados?**  
R: Sim! Entity Framework usa LINQ to Entities.

**P: O que são closures?**  
R: Lambda que captura variáveis externas. Cuidado com loops!

---

## 💡 Dicas Importantes

### ✅ Faça:
- Use `List<T>` para coleções dinâmicas
- Use `Dictionary<TKey, TValue>` para lookups rápidos
- Prefira LINQ method syntax
- Evite modificar coleção durante iteração
- Use `var` com LINQ para tipos anônimos
- Comente queries LINQ complexas

### ❌ Evite:
- Arrays quando precisa tamanho dinâmico
- `foreach` quando LINQ é mais claro
- `ToList()` desnecessários (quebra deferred execution)
- Capturar variáveis de loop em lambdas
- Queries LINQ muito longas (quebre em steps)

---

## 📚 Recursos Complementares

### 📖 Documentação Oficial
- [Collections (C#)](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/collections)
- [Lambda Expressions](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions)
- [LINQ Overview](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/)
- [Standard Query Operators](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/standard-query-operators-overview)

### 🎥 Vídeos Recomendados
- C# Collections Explained (YouTube)
- LINQ Tutorial - Complete Guide
- Lambda Expressions Deep Dive

### 📱 Ferramentas Úteis
- LINQPad: ferramenta para testar queries LINQ
- Benchmark.NET: medir performance de coleções
- ReSharper: sugestões de LINQ

---

## 🏆 Desafios Extras

1. **🥇 Implementar sua própria coleção genérica**
2. **🥈 Criar métodos de extensão LINQ personalizados**
3. **🥉 Otimizar query LINQ lenta**
4. **🎯 Sistema de cache com expiração**
5. **🚀 Benchmark de diferentes coleções**

---

## 🎬 Auto-avaliação

Antes de prosseguir para o Dia 05, certifique-se:

- [ ] Sei escolher a coleção certa para cada situação
- [ ] Escrevo lambdas com confiança
- [ ] Domino os métodos LINQ básicos
- [ ] Entendo deferred vs immediate execution
- [ ] Consigo fazer joins e agregações
- [ ] Completei o projeto final
- [ ] Código está no GitHub

---

## 📈 Próximo Dia

**Dia 05**: Tratamento de Exceções e Depuração
- Try/Catch/Finally
- Exceções personalizadas
- Logging
- Debugging no Visual Studio

---

<div align="center">

**Boa sorte e divirta-se com coleções e LINQ!** 🚀

📖 [Voltar ao README principal](../README.md)

</div>
