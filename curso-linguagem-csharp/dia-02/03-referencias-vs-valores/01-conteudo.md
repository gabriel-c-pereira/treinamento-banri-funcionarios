# 📘 Referências vs Valores

## 🎯 Objetivos

✅ Entender Stack vs Heap  
✅ Diferenciar Value Types e Reference Types  
✅ Compreender Boxing e Unboxing  
✅ Usar ref, out e in parameters  
✅ Saber quando usar struct vs class  
✅ Conhecer Records (C# 9+)  
✅ Trabalhar com Tuples e ValueTuples  

---

## 🧠 Memória: Stack vs Heap

C# usa duas áreas principais de memória:

### Stack (Pilha)
- **Rápida** e **organizada**
- Armazena **value types** e **referências**
- LIFO (Last In, First Out)
- **Automática** - limpa quando método termina
- **Tamanho limitado** (~1MB)

### Heap (Monte)
- **Mais lenta** que stack
- Armazena **reference types** (objetos)
- **Gerenciada pelo Garbage Collector**
- **Tamanho maior** (limitado pela RAM)

```
┌─────────────────────────────────────────────┐
│  STACK              │  HEAP                 │
├─────────────────────┼───────────────────────┤
│  int x = 5          │                       │
│  [5]                │                       │
│                     │                       │
│  Pessoa p = new...  │  Objeto Pessoa        │
│  [ref: 0x1234] ────┼─→ {Nome="João",...}  │
│                     │                       │
└─────────────────────┴───────────────────────┘
```

---

## 🎯 Value Types (Tipos de Valor)

Armazenados **diretamente na stack**. Contêm o **valor real**.

### Value Types Incluem:
- Primitivos: `int`, `double`, `bool`, `char`, `decimal`
- `struct`: tipos personalizados
- `enum`: enumerações
- `ValueTuple`: tuplas de valor

```csharp
// Value types
int a = 10;
int b = a;  // COPIA o valor
b = 20;

Console.WriteLine(a);  // 10 (não mudou!)
Console.WriteLine(b);  // 20
```

**Comportamento:**
- ✅ Cópia independente
- ✅ Rápido
- ✅ Sem Garbage Collection
- ❌ Pode ser grande e ineficiente

---

## 🔗 Reference Types (Tipos de Referência)

Armazenados no **heap**. Variável contém **referência** (endereço).

### Reference Types Incluem:
- `class`: classes
- `string`: strings
- `array`: arrays
- `delegate`: delegados
- `object`: tipo base

```csharp
// Reference types
class Pessoa
{
    public string Nome { get; set; }
}

Pessoa p1 = new Pessoa { Nome = "João" };
Pessoa p2 = p1;  // COPIA a referência (não o objeto!)
p2.Nome = "Maria";

Console.WriteLine(p1.Nome);  // Maria (mudou!)
Console.WriteLine(p2.Nome);  // Maria (mesmo objeto!)
```

**Comportamento:**
- ✅ Compartilhamento eficiente
- ✅ Tamanho ilimitado
- ❌ Mais lento que value types
- ❌ Precisa de Garbage Collection

---

## 📦 Boxing e Unboxing

**Boxing**: Converter value type → reference type (object)  
**Unboxing**: Converter reference type (object) → value type

```csharp
// BOXING - value type → object
int valor = 123;
object obj = valor;  // Boxing automático (copia para heap)

// UNBOXING - object → value type
int valorNovamente = (int)obj;  // Unboxing explícito

// Exemplo prático:
ArrayList lista = new ArrayList();
lista.Add(42);  // Boxing (int → object)
int numero = (int)lista[0];  // Unboxing (object → int)
```

**Performance:**
- ❌ Boxing/Unboxing são **lentos**
- ❌ Criam cópias e alocações no heap
- ✅ Use `List<int>` ao invés de `ArrayList` para evitar

---

## 🔀 ref, out e in Parameters

### ref - Passagem por Referência

```csharp
void Dobrar(ref int numero)
{
    numero *= 2;
}

int x = 10;
Dobrar(ref x);
Console.WriteLine(x);  // 20 (modificado!)
```

**Regras:**
- Variável **deve ser inicializada** antes
- Modificações afetam a variável original
- Deve usar `ref` na chamada

### out - Saída de Múltiplos Valores

```csharp
bool TentarParsear(string texto, out int resultado)
{
    if (int.TryParse(texto, out resultado))
        return true;
    
    resultado = 0;
    return false;
}

// Uso:
if (TentarParsear("123", out int numero))
    Console.WriteLine($"Sucesso: {numero}");

// C# 7+: Declaração inline
if (TentarParsear("456", out var num))
    Console.WriteLine($"Sucesso: {num}");
```

**Regras:**
- Variável **não precisa** ser inicializada antes
- **Deve ser atribuída** dentro do método
- Deve usar `out` na chamada

### in - Passagem Somente Leitura (C# 7.2+)

```csharp
void ProcessarGrande(in GrandeStruct dados)
{
    // dados é readonly aqui
    // dados.campo = 10;  // ERRO!
    
    Console.WriteLine(dados.Valor);  // OK
}

struct GrandeStruct
{
    public int Valor { get; init; }
    // ... muitos campos
}

var dados = new GrandeStruct { Valor = 100 };
ProcessarGrande(in dados);  // Passa por referência mas readonly
```

**Benefícios:**
- ✅ Evita cópia de structs grandes
- ✅ Garante que não será modificado
- ✅ Performance melhor

### 📊 Comparação

| Modificador | Inicialização | Modificação | Atribuição Obrigatória |
|-------------|---------------|-------------|------------------------|
| **ref** | ✅ Obrigatória | ✅ Permitida | ❌ Não |
| **out** | ❌ Opcional | ✅ Permitida | ✅ Sim |
| **in** | ✅ Obrigatória | ❌ Readonly | ❌ Não |

---

## 🏗️ Struct vs Class

### Struct (Value Type)

```csharp
public struct Ponto
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Ponto(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public double DistanciaOrigem()
    {
        return Math.Sqrt(X * X + Y * Y);
    }
}

// Uso:
Ponto p1 = new Ponto(3, 4);
Ponto p2 = p1;  // COPIA
p2.X = 10;

Console.WriteLine(p1.X);  // 3 (não mudou!)
Console.WriteLine(p2.X);  // 10
```

### Quando Usar Struct?

✅ **Use struct quando:**
- Representa um **único valor** simples
- É **pequeno** (≤ 16 bytes recomendado)
- É **imutável** (readonly)
- Raramente é "boxed"
- Performance é crítica

**Exemplos:** `Point`, `Color`, `DateTime`, `TimeSpan`

❌ **NÃO use struct quando:**
- Objeto é grande (> 16 bytes)
- Precisa de herança
- Precisa ser null
- Será modificado frequentemente

### 📊 Struct vs Class

| Aspecto | Struct | Class |
|---------|--------|-------|
| **Tipo** | Value | Reference |
| **Memória** | Stack | Heap |
| **Cópia** | Valor | Referência |
| **Null** | ❌ Não (C# < 8) | ✅ Sim |
| **Herança** | ❌ Não | ✅ Sim |
| **Default ctor** | Sempre existe | Opcional |
| **Performance** | Mais rápido | Mais lento |
| **Tamanho** | Pequeno | Qualquer |

---

## 📝 Records (C# 9+)

**Record** é um reference type **imutável** otimizado para dados.

### Record Class (padrão)

```csharp
// Sintaxe concisa
public record Pessoa(string Nome, int Idade);

// Uso:
var p1 = new Pessoa("João", 25);
Console.WriteLine(p1.Nome);  // João
Console.WriteLine(p1);  // Pessoa { Nome = João, Idade = 25 }

// Imutabilidade - não pode modificar:
// p1.Nome = "Maria";  // ERRO! Init-only

// "with" para criar cópia modificada:
var p2 = p1 with { Nome = "Maria" };
Console.WriteLine(p1.Nome);  // João (original não muda)
Console.WriteLine(p2.Nome);  // Maria (nova cópia)

// Comparação por valor:
var p3 = new Pessoa("João", 25);
Console.WriteLine(p1 == p3);  // True (mesmo valor!)
```

### Record Struct (C# 10+)

```csharp
public record struct Ponto(int X, int Y);

var p1 = new Ponto(10, 20);
var p2 = p1 with { X = 30 };

Console.WriteLine(p1);  // Ponto { X = 10, Y = 20 }
Console.WriteLine(p2);  // Ponto { X = 30, Y = 20 }
```

### Records Completos

```csharp
public record Produto
{
    public string Nome { get; init; }
    public decimal Preco { get; init; }
    public string Categoria { get; init; }
    
    public Produto(string nome, decimal preco)
    {
        Nome = nome;
        Preco = preco;
        Categoria = "Geral";
    }
    
    // Método personalizado
    public decimal PrecoComDesconto(decimal percentual)
    {
        return Preco * (1 - percentual / 100);
    }
}
```

### Benefícios dos Records

✅ **Sintaxe concisa**  
✅ **Imutabilidade por padrão**  
✅ **Comparação por valor**  
✅ **ToString() automático**  
✅ **Deconstrução automática**  
✅ **with-expressions**  

---

## 🎭 Tuples e ValueTuples

### Tuple (class - antigo)

```csharp
Tuple<string, int> pessoa = new Tuple<string, int>("João", 25);
Console.WriteLine(pessoa.Item1);  // João
Console.WriteLine(pessoa.Item2);  // 25
```

### ValueTuple (struct - moderno C# 7+)

```csharp
// Sintaxe concisa
(string nome, int idade) pessoa = ("João", 25);
Console.WriteLine(pessoa.nome);   // João
Console.WriteLine(pessoa.idade);  // 25

// Sem nomes (Item1, Item2...):
var ponto = (10, 20);
Console.WriteLine(ponto.Item1);  // 10

// Retorno de método:
(string Nome, int Idade) ObterPessoa()
{
    return ("Maria", 30);
}

var p = ObterPessoa();
Console.WriteLine(p.Nome);   // Maria
Console.WriteLine(p.Idade);  // 30

// Deconstrução:
var (nome, idade) = ObterPessoa();
Console.WriteLine(nome);   // Maria
Console.WriteLine(idade);  // 30

// Descartar valores:
var (n, _) = ObterPessoa();  // Ignora idade
```

### Quando Usar Tuples?

✅ **Use quando:**
- Retornar múltiplos valores temporariamente
- Agrupamento rápido de dados
- Dados que não precisam de nome de classe

❌ **NÃO use quando:**
- Dados serão usados extensivamente
- Precisa de validação ou lógica
- API pública (prefira classes/records)

---

## 💡 Exemplo Completo

```csharp
// VALUE TYPE - Struct para coordenadas
public struct Coordenada
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    
    public Coordenada(double lat, double lon)
    {
        Latitude = lat;
        Longitude = lon;
    }
}

// REFERENCE TYPE - Class para localização
public class Localizacao
{
    public string Nome { get; set; }
    public Coordenada Posicao { get; set; }
    
    public Localizacao(string nome, Coordenada posicao)
    {
        Nome = nome;
        Posicao = posicao;
    }
}

// RECORD - Dados imutáveis
public record Endereco(string Rua, string Cidade, string CEP);

// USO:
var coord = new Coordenada(-23.550520, -46.633308);
var local = new Localizacao("São Paulo", coord);
var endereco = new Endereco("Av. Paulista", "São Paulo", "01310-100");

// Demonstração de comportamento:
var coord2 = coord;  // Copia valor
var local2 = local;  // Copia referência

coord2 = new Coordenada(0, 0);
local2.Nome = "Rio de Janeiro";

Console.WriteLine(coord.Latitude);  // -23.550520 (não mudou)
Console.WriteLine(local.Nome);      // Rio de Janeiro (mudou!)
```

---

## 📚 Resumo

| Conceito | Tipo | Memória | Cópia | Null | Quando Usar |
|----------|------|---------|-------|------|-------------|
| **class** | Reference | Heap | Referência | ✅ | Objetos complexos |
| **struct** | Value | Stack | Valor | ❌ (C# < 8) | Dados pequenos |
| **record** | Reference* | Heap | Referência | ✅ | Dados imutáveis |
| **record struct** | Value | Stack | Valor | ❌ | Dados pequenos imutáveis |
| **tuple** | Value | Stack | Valor | ❌ | Retornos temporários |

*Record class por padrão, pode ser record struct

---

**Parabéns! Você completou o Dia 02!** 🎉

Agora você domina:
✅ Classes e Objetos  
✅ Construtores e Sobrecarga  
✅ Referências vs Valores  
✅ Structs, Records e Tuples  

**Próximo**: Dia 03 - Herança e Polimorfismo! 🚀
