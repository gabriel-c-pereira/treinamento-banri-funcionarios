# ⚡ Lambda Expressions

> **Tempo estimado**: 2 horas  
> **Nível**: Intermediário

## 🎯 O que são Lambdas?

**Lambda expressions** são funções anônimas concisas, o que simplifica o código ao evitar a necessidade de definir métodos nomeados para tarefas simples. Elas são frequentemente usadas com recursos como LINQ (veremos em seguida) para realizar operações como filtros e ordenações em coleções, além de permitirem que funções sejam tratadas como variáveis e passadas como parâmetros para outros métodos. 

```csharp
// Sintaxe básica
(parâmetros) => expressão
(parâmetros) => { statements }
```

---

## 📝 Sintaxe

### Expression Lambda

```csharp
// Sem parâmetros
() => Console.WriteLine("Hello");

// Um parâmetro (parênteses opcionais)
x => x * 2
(x) => x * 2  // Equivalente

// Múltiplos parâmetros
(x, y) => x + y

// Com tipo explícito
(int x, int y) => x + y
```

### Statement Lambda

```csharp
// Múltiplas instruções
x => 
{
    int resultado = x * 2;
    Console.WriteLine(resultado);
    return resultado;
};

// Com múltiplos parâmetros
(x, y) =>
{
    Console.WriteLine($"Somando {x} + {y}");
    return x + y;
};
```

---

## 🎭 Delegates: Func e Action

### Func\<T, TResult> - Retorna Valor

```csharp
// Func<entrada, saída>
Func<int, int> dobro = x => x * 2;
int resultado = dobro(5);  // 10

// Múltiplas entradas
Func<int, int, int> soma = (a, b) => a + b;
int total = soma(3, 4);  // 7

// Até 16 parâmetros!
Func<int, int, int, int> somaMultipla = (a, b, c) => a + b + c;
```

### Action\<T> - Não Retorna Valor

```csharp
// Action<entrada(s)>
Action<string> imprimir = msg => Console.WriteLine(msg);
imprimir("Hello");

// Múltiplos parâmetros
Action<string, int> imprimirComId = (nome, id) => 
    Console.WriteLine($"{id}: {nome}");

imprimirComId("João", 123);

// Sem parâmetros
Action cumprimentar = () => Console.WriteLine("Olá!");
cumprimentar();
```

### Predicate\<T> - Retorna Bool

```csharp
// Predicate<T> é Func<T, bool>
Predicate<int> ehPar = x => x % 2 == 0;
bool resultado = ehPar(4);  // true

// Usado em List.Find, FindAll, etc
List<int> numeros = new() { 1, 2, 3, 4, 5 };
int primeiroPar = numeros.Find(x => x % 2 == 0);  // 2
List<int> pares = numeros.FindAll(x => x % 2 == 0);  // { 2, 4 }
```

---

## 🔄 Lambdas como Parâmetros

```csharp
public class Calculadora
{
    // Método que aceita lambda
    public int Operar(int a, int b, Func<int, int, int> operacao)
    {
        return operacao(a, b);
    }
}

var calc = new Calculadora();

// Passar diferentes lambdas
int soma = calc.Operar(5, 3, (a, b) => a + b);        // 8
int mult = calc.Operar(5, 3, (a, b) => a * b);        // 15
int max = calc.Operar(5, 3, (a, b) => Math.Max(a, b)); // 5
```

---

## 🎯 Closures - Captura de Variáveis

```csharp
int fator = 10;

Func<int, int> multiplicador = x => x * fator;

Console.WriteLine(multiplicador(5));  // 50

fator = 20;  // Mudou!
Console.WriteLine(multiplicador(5));  // 100 ← Capturou a variável!
```

### ⚠️ Cuidado com Loops

```csharp
// ❌ ERRO COMUM
var acoes = new List<Action>();
for (int i = 0; i < 3; i++)
{
    acoes.Add(() => Console.WriteLine(i));  // Captura a VARIÁVEL i
}

foreach (var acao in acoes)
    acao();  // Imprime: 3, 3, 3 ← Todos capturam o mesmo i!

// ✅ CORRETO
var acoes2 = new List<Action>();
for (int i = 0; i < 3; i++)
{
    int captura = i;  // Cópia local
    acoes2.Add(() => Console.WriteLine(captura));
}

foreach (var acao in acoes2)
    acao();  // Imprime: 0, 1, 2 ✓
```

---

## 🛠️ Lambdas com Coleções

```csharp
List<int> numeros = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Where - filtrar
var pares = numeros.Where(x => x % 2 == 0).ToList();

// Select - transformar
var dobros = numeros.Select(x => x * 2).ToList();

// OrderBy - ordenar
var ordenados = numeros.OrderBy(x => x).ToList();
var descendente = numeros.OrderByDescending(x => x).ToList();

// First - primeiro elemento
int primeiro = numeros.First(x => x > 5);  // 6

// Any - verifica se existe
bool temMaiorQue5 = numeros.Any(x => x > 5);  // true

// All - verifica se todos
bool todosPositivos = numeros.All(x => x > 0);  // true

// Count - conta quantos
int quantosPares = numeros.Count(x => x % 2 == 0);  // 5

// Sum - soma
int total = numeros.Sum(x => x);  // 55

// Average - média
double media = numeros.Average(x => x);  // 5.5
```

---

## 🎨 Exemplos Práticos

### 1. Validação de Dados

```csharp
public class Validator
{
    public bool Validar(string texto, Func<string, bool> regra)
    {
        return regra(texto);
    }
}

var validator = new Validator();

// Diferentes validações
bool emailValido = validator.Validar("teste@email.com", 
    email => email.Contains("@"));

bool senhaForte = validator.Validar("Abc123!", 
    senha => senha.Length >= 6 && 
             senha.Any(char.IsUpper) && 
             senha.Any(char.IsDigit));
```

### 2. Event Handlers

```csharp
button.Click += (sender, e) => 
{
    Console.WriteLine("Botão clicado!");
    ProcessarClick();
};
```

### 3. Builder Pattern

```csharp
public class QueryBuilder
{
    public QueryBuilder Where(Func<Item, bool> predicate)
    {
        // ...
        return this;
    }
    
    public QueryBuilder OrderBy(Func<Item, object> selector)
    {
        // ...
        return this;
    }
}

// Uso fluente com lambdas
var query = new QueryBuilder()
    .Where(x => x.Preco > 100)
    .OrderBy(x => x.Nome);
```

---

## 🎓 Resumo

✅ **Expression lambda**: `x => x * 2`  
✅ **Statement lambda**: `x => { return x * 2; }`  
✅ **Func\<T, TResult>**: retorna valor  
✅ **Action\<T>**: não retorna valor  
✅ **Closures**: captura de variáveis externas  
✅ **Uso em coleções**: Where, Select, OrderBy, etc  

➡️ **Próximo**: LINQ
