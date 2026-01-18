# 📘 Sintaxe Básica do C# - Variáveis, Tipos e Operadores

## 🎯 Objetivos de Aprendizado

Ao final desta seção, você será capaz de:
- Declarar e usar variáveis de diferentes tipos
- Compreender tipos de valor vs tipos de referência
- Utilizar operadores aritméticos, lógicos e relacionais
- Trabalhar com conversões de tipos
- Aplicar boas práticas de nomenclatura

---

## 📊 Sistema de Tipos do C#

O C# é uma linguagem **fortemente tipada**, o que significa que cada variável deve ter um tipo definido. O sistema de tipos é dividido em dois grandes grupos:

```
┌────────────────────────────────────────┐
│         TIPOS EM C#                    │
├────────────────┬───────────────────────┤
│  Tipos de Valor│  Tipos de Complexos   │
├────────────────┼───────────────────────┤
│ • int          │ • string              │
│ • double       │ • object              │
│ • bool         │ • arrays              │
│ • char         │ • classes             │
│ • struct       │ • interfaces          │
│ • enum         │ • delegates           │
└────────────────┴───────────────────────┘
     Stack              Heap
  (Mais rápido)     (Mais flexível)
```

---

## 🔢 Tipos de Dados Numéricos

### Números Inteiros

| Tipo | Tamanho | Intervalo | Uso Típico |
|------|---------|-----------|------------|
| `byte` | 8 bits | 0 a 255 | Dados binários, imagens |
| `sbyte` | 8 bits | -128 a 127 | Valores pequenos com sinal |
| `short` | 16 bits | -32.768 a 32.767 | Contadores pequenos |
| `ushort` | 16 bits | 0 a 65.535 | IDs, portas de rede |
| `int` | 32 bits | -2,1 bilhões a 2,1 bilhões | **Uso geral** ⭐ |
| `uint` | 32 bits | 0 a 4,2 bilhões | IDs grandes |
| `long` | 64 bits | ±9,2 quintilhões | Timestamps, IDs DB |
| `ulong` | 64 bits | 0 a 18,4 quintilhões | Grandes contadores |

```csharp
// Exemplos práticos
int idade = 25;                    // Uso mais comum
long populacaoMundial = 8_000_000_000L; // L indica long
byte nivelBateria = 85;            // 0-100%
short ano = 2025;                  // Anos
```

> **💡 Dica**: Use `int` como padrão. Use `long` apenas quando os valores forem realmente grandes.

### Números de Ponto Flutuante

| Tipo | Tamanho | Precisão | Intervalo | Uso |
|------|---------|----------|-----------|-----|
| `float` | 32 bits | ~6-7 dígitos | ±1.5E-45 a ±3.4E38 | Gráficos, jogos |
| `double` | 64 bits | ~15-16 dígitos | ±5.0E-324 a ±1.7E308 | **Científico** ⭐ |
| `decimal` | 128 bits | ~28-29 dígitos | ±1.0E-28 a ±7.9E28 | **Financeiro** ⭐ |

```csharp
// Exemplos práticos
float temperatura = 36.5f;         // f indica float
double pi = 3.14159265359;         // Padrão para decimais
decimal preco = 99.99m;            // m indica decimal (finanças!)

// Quando usar cada um?
double areaCirculo = Math.PI * raio * raio;  // Científico
decimal totalCompra = 150.75m;               // Dinheiro
float velocidade = 60.5f;                    // Jogos/gráficos
```

> **⚠️ IMPORTANTE**: Para valores monetários, **SEMPRE use `decimal`** para evitar erros de arredondamento!

```csharp
// ❌ ERRADO - Perde precisão
double saldo = 0.1 + 0.2;  // Resultado: 0.30000000000000004

// ✅ CORRETO - Precisão exata
decimal saldo = 0.1m + 0.2m;  // Resultado: 0.3
```

---

## 📝 Tipos de Texto e Caracteres

### char - Caractere Único

```csharp
char letra = 'A';                  // Aspas simples
char digito = '7';
char simbolo = '@';
char unicode = '\u0041';           // 'A' em Unicode
char emoji = '😊';                 // Emojis funcionam!

// Operações com caracteres
bool ehLetra = char.IsLetter(letra);      // true
bool ehDigito = char.IsDigit(digito);     // true
char maiuscula = char.ToUpper('a');       // 'A'
```

### string - Cadeia de Caracteres

```csharp
// Declaração básica
string nome = "João Silva";
string vazio = "";                 // String vazia
string? nulo = null;              // Pode ser nulo (C# 8+)

// Strings são imutáveis
string original = "Hello";
string nova = original.ToUpper(); // "HELLO"
// 'original' continua "Hello" - não foi modificada!

// Métodos úteis
string texto = "  C# é incrível!  ";
int tamanho = texto.Length;                    // 17
string maiuscula = texto.ToUpper();            // "  C# É INCRÍVEL!  "
string minuscula = texto.ToLower();            // "  c# é incrível!  "
string semEspacos = texto.Trim();              // "C# é incrível!"
bool contem = texto.Contains("C#");            // true
bool comecaCom = texto.StartsWith("C#");       // false (espaços)
string substituido = texto.Replace("C#", "CSharp");

// Concatenação
string primeiroNome = "João";
string sobrenome = "Silva";
string nomeCompleto = primeiroNome + " " + sobrenome;

// Interpolação (RECOMENDADO)
string mensagem = $"{primeiroNome} {sobrenome} tem {idade} anos";
```

---

## ✅ bool - Valores Lógicos

```csharp
bool ativo = true;
bool bloqueado = false;

// Operações lógicas
bool maiorDeIdade = idade >= 18;
bool precisaAutorizacao = idade < 18 && !temPermissao;

// Comparações retornam bool
bool igual = (5 == 5);           // true
bool diferente = (5 != 3);       // true
bool maior = (10 > 5);           // true
```

---

## 🎯 Declaração de Variáveis

### Sintaxe Básica

```csharp
// Forma explícita (tipo definido)
int numero = 42;
string nome = "Maria";
double preco = 19.99;

// Declaração sem inicialização
int quantidade;
quantidade = 10;  // Deve ser inicializada antes de usar

// Múltiplas variáveis do mesmo tipo
int x = 1, y = 2, z = 3;
```

### Inferência de Tipo com `var`

```csharp
// O compilador deduz o tipo automaticamente
var idade = 25;           // int
var nome = "João";        // string
var preco = 99.99m;       // decimal
var ativo = true;         // bool

// ✅ Use var quando o tipo é óbvio
var lista = new List<string>();
var cliente = new Cliente();

// ❌ Evite var quando não é claro
var valor = ObterValor();  // Que tipo é?
int valor = ObterValor();  // Melhor!
```

### Constantes

```csharp
// Valores que nunca mudam
const double PI = 3.14159;
const int DIAS_SEMANA = 7;
const string MENSAGEM_ERRO = "Operação inválida";

// ❌ ERRO: Não pode modificar
// PI = 3.14;  // Erro de compilação

// Convenção: MAIÚSCULAS com underscore
const int MAX_TENTATIVAS = 3;
```

---

## ➕ Operadores Aritméticos

```csharp
int a = 10, b = 3;

// Operadores básicos
int soma = a + b;         // 13
int sub = a - b;          // 7
int mult = a * b;         // 30
int div = a / b;          // 3 (divisão inteira!)
int resto = a % b;        // 1 (módulo)

// ⚠️ Cuidado com divisão inteira
int resultado = 5 / 2;          // 2 (não 2.5!)
double correto = 5.0 / 2;       // 2.5

// Incremento/Decremento
contador++;    // +1
++contador;    // +1
contador--;    // -1

// Compostos
numero += 5;   // numero = numero + 5
numero *= 2;   // numero = numero * 2
```

---

## 🔗 Operadores Relacionais

```csharp
int a = 10, b = 20;

bool igual = (a == b);            // false
bool diferente = (a != b);        // true
bool maior = (a > b);             // false
bool menor = (a < b);             // true
bool maiorIgual = (a >= 10);      // true
bool menorIgual = (b <= 20);      // true

// Comparação de strings
string nome1 = "João";
string nome2 = "joão";

bool iguais = (nome1 == nome2);                          // false
bool iguaisIgnoreCase = nome1.Equals(nome2, 
    StringComparison.OrdinalIgnoreCase);                 // true
```

---

## 🧠 Operadores Lógicos

```csharp
bool temCNH = true;
bool maiorDe18 = true;
bool estaCansado = false;

// E lógico (AND) - todas devem ser true
bool podeDirigir = temCNH && maiorDe18 && !estaCansado;  // true

// OU lógico (OR) - pelo menos uma deve ser true
bool podeEntrar = temCNH || temPassaporte;  // true se tiver qualquer um

// NÃO lógico (NOT) - inverte o valor
bool naoTemCNH = !temCNH;  // false

// Exemplos práticos
bool aprovado = (nota >= 7) && (frequencia >= 75);
bool precisaRecuperar = (nota < 7) || (frequencia < 75);
```

### Tabela Verdade

```
A      B      A && B   A || B   !A
──────────────────────────────────
true   true   true     true     false
true   false  false    true     false
false  true   false    true     true
false  false  false    false    true
```

---

## 🔄 Conversões de Tipo

### Conversão Implícita (Automática)

```csharp
// De menor para maior - sem perda de dados
int inteiro = 100;
long longo = inteiro;         // OK - int cabe em long
double decimal = inteiro;     // OK - int cabe em double

byte b = 50;
int i = b;                    // OK
```

### Conversão Explícita (Cast)

```csharp
// De maior para menor - pode perder dados
double d = 123.45;
int i = (int)d;              // i = 123 (perde decimais!)
```

### Conversão com Métodos

```csharp
// Converter strings para números
string texto = "123";
int numero = int.Parse(texto);           // 123
int numero2 = Convert.ToInt32(texto);    // 123

// TryParse - mais seguro (não lança exceção)
string entrada = "abc";
bool sucesso = int.TryParse(entrada, out int resultado);
if (sucesso)
{
    Console.WriteLine($"Convertido: {resultado}");
}
else
{
    Console.WriteLine("Conversão falhou!");
}

// Converter números para strings
int idade = 25;
string idadeTexto = idade.ToString();          // "25"
string formatado = idade.ToString("D3");       // "025"

decimal preco = 19.99m;
string precoTexto = preco.ToString("C");       // "R$ 19,99"
string precoFixo = preco.ToString("F2");       // "19.99"
```

### Tabela de Conversão

| De | Para | Método | Exemplo |
|----|------|--------|---------|
| string | int | `int.Parse()` / `TryParse()` | `int.Parse("123")` |
| string | double | `double.Parse()` / `TryParse()` | `double.Parse("12.3")` |
| string | bool | `bool.Parse()` / `TryParse()` | `bool.Parse("true")` |
| int | string | `.ToString()` | `123.ToString()` |
| double | int | `(int)` ou `Convert.ToInt32()` | `(int)12.5` |
| int | double | Implícito | `double d = 123;` |

---

## 🎨 Boas Práticas de Nomenclatura

### Convenções do C#

```csharp
// Classes e Structs: PascalCase
public class ContaBancaria { }
public struct Coordenada { }

// Métodos e Propriedades: PascalCase
public void CalcularSaldo() { }
public string Nome { get; set; }

// Variáveis locais e parâmetros: camelCase
int idadeUsuario = 25;
void ProcessarPedido(string numeroPedido) { }

// Constantes: PascalCase ou UPPER_CASE
public const int MaximoTentativas = 3;
public const string MENSAGEM_ERRO = "Erro!";

// Campos privados: _camelCase (com underscore)
private int _contador;
private string _nomeCompleto;

// Interfaces: IName (começa com I)
public interface IRepositorio { }
public interface IServico { }

// Enums: PascalCase (singular)
public enum StatusPedido
{
    Pendente,
    Processando,
    Concluido,
    Cancelado
}
```

### Nomes Descritivos

```csharp
// ❌ RUIM - Nomes vagos
int x = 10;
string s = "João";
bool f = true;

// ✅ BOM - Nomes descritivos
int idadeUsuario = 10;
string nomeCliente = "João";
bool contaAtiva = true;

// ❌ RUIM - Abreviações obscuras
int qnt = 5;
string nm = "Test";

// ✅ BOM - Palavras completas
int quantidade = 5;
string nome = "Test";

// ✅ ÓTIMO - Contexto claro
int quantidadeProdutosCarrinho = 5;
string nomeArquivoRelatorio = "vendas_2025.pdf";
```

---

## 💡 Tipos Nullable

```csharp
// Tipos de valor não podem ser null por padrão
int numero = null;  // ❌ ERRO

// Nullable types (C# 2.0+)
int? numeroNulavel = null;  // ✅ OK
double? temperatura = null;
bool? confirmado = null;

// Verificando null
if (numeroNulavel.HasValue)
{
    Console.WriteLine($"Valor: {numeroNulavel.Value}");
}
else
{
    Console.WriteLine("Valor é nulo");
}

// Operador null-coalescing (??)
int valor = numeroNulavel ?? 0;  // Se for null, usa 0

// Nullable reference types (C# 8+)
string? nome = null;  // Pode ser null
string sobrenome = "Silva";  // Não pode ser null
```

---

## 🎓 Resumo dos Pontos-Chave

✅ **int** é o tipo inteiro padrão  
✅ **double** para científico, **decimal** para dinheiro  
✅ **string** é imutável (operações criam novas strings)  
✅ **var** usa inferência de tipo (use quando óbvio)  
✅ **const** para constantes em tempo de compilação  
✅ Use **TryParse()** para conversões seguras  
✅ Operadores lógicos usam **curto-circuito**  
✅ **Nullable types** permitem null em tipos de valor  
✅ Siga as **convenções de nomenclatura** do C#  

---

## 📚 Referências

- [C# Types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/built-in-types)
- [Operators](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/)
- [Type Conversion](https://learn.microsoft.com/dotnet/csharp/programming-guide/types/casting-and-type-conversions)
- [Coding Conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

## ⏭️ Próximos Passos

Agora você está pronto para aprender:
- Estruturas de controle (if, else, switch)
- Loops (for, while, foreach)
- Arrays e coleções

**Continue para**: `03-estruturas-controle/01-conteudo.md`
