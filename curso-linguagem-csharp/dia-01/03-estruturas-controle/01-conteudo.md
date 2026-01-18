# 📘 Estruturas de Controle - Decisões e Loops

## 🎯 Objetivos de Aprendizado

Ao final desta seção, você será capaz de:
- Tomar decisões no código com `if`/`else` e `switch`
- Criar loops eficientes com `for`, `while` e `foreach`
- Usar pattern matching moderno do C# 8+
- Controlar o fluxo com `break`, `continue` e `return`
- Aplicar estruturas de controle em problemas reais

---

## 🔀 Estruturas Condicionais

### 1. `if` / `else` / `else if`

A estrutura mais básica para tomar decisões:

```csharp
int idade = 18;

if (idade >= 18)
{
    Console.WriteLine("Maior de idade");
}
```

#### `if else`

```csharp
int idade = 16;

if (idade >= 18)
{
    Console.WriteLine("Pode dirigir");
}
else
{
    Console.WriteLine("Não pode dirigir");
}
```

#### `if` `else if` `else` (Cadeia)

```csharp
decimal nota = 7.5m;

if (nota >= 9)
{
    Console.WriteLine("Excelente!");
}
else if (nota >= 7)
{
    Console.WriteLine("Aprovado");
}
else if (nota >= 5)
{
    Console.WriteLine("Recuperação");
}
else
{
    Console.WriteLine("Reprovado");
}
```

#### `if` sem chaves (não recomendado)

```csharp
// ❌ Evite - difícil manutenção
if (condicao)
    Console.WriteLine("Uma linha");

// ✅ Sempre use chaves
if (condicao)
{
    Console.WriteLine("Melhor prática");
}
```

#### Condições Compostas

```csharp
int idade = 25;
bool temCNH = true;
bool temCarro = false;

// AND (&&) - todas devem ser verdadeiras
if (idade >= 18 && temCNH)
{
    Console.WriteLine("Pode dirigir");
}

// OR (||) - pelo menos uma verdadeira
if (temCNH || temCarro)
{
    Console.WriteLine("Tem algum requisito");
}

// NOT (!) - inverte o valor
if (!temCarro)
{
    Console.WriteLine("Não tem carro");
}

// Combinação complexa
if ((idade >= 18 && temCNH) || temAutorizacaoEspecial)
{
    Console.WriteLine("Autorizado");
}
```

#### `if` Aninhados

```csharp
if (usuario != null)
{
    if (usuario.IsAtivo)
    {
        if (usuario.TemPermissao("admin"))
        {
            Console.WriteLine("Acesso total");
        }
    }
}

// ✅ Melhor: Early return
if (usuario == null) return;
if (!usuario.IsAtivo) return;
if (!usuario.TemPermissao("admin")) return;

Console.WriteLine("Acesso total");
```

---

### 2. Operador Ternário

Forma compacta de `if else` para expressões simples:

```csharp
// Sintaxe: condicao ? valorSeVerdadeiro : valorSeFalso

int idade = 20;
string status = idade >= 18 ? "Maior" : "Menor";
Console.WriteLine(status);  // "Maior"

// Equivalente a:
string status;
if (idade >= 18)
    status = "Maior";
else
    status = "Menor";

// Exemplos práticos
int numero = -5;
int absoluto = numero >= 0 ? numero : -numero;

bool temDesconto = totalCompra > 100;
decimal desconto = temDesconto ? 0.10m : 0.0m;

// ⚠️ Evite aninhamento excessivo
string resultado = idade >= 18 ? 
                   (temCNH ? "Pode dirigir" : "Sem CNH") : 
                   "Menor de idade";  // Difícil de ler!
```

---

### 3. `switch` - Versão Clássica

Quando você tem múltiplas comparações com o mesmo valor:

```csharp
int diaSemana = 3;

switch (diaSemana)
{
    case 1:
        Console.WriteLine("Segunda-feira");
        break;
    case 2:
        Console.WriteLine("Terça-feira");
        break;
    case 3:
        Console.WriteLine("Quarta-feira");
        break;
    case 4:
        Console.WriteLine("Quinta-feira");
        break;
    case 5:
        Console.WriteLine("Sexta-feira");
        break;
    case 6:
    case 7:
        Console.WriteLine("Fim de semana");
        break;
    default:
        Console.WriteLine("Dia inválido");
        break;
}
```

#### Fall-through (múltiplos cases)

```csharp
char letra = 'A';

switch (letra)
{
    case 'a':
    case 'A':
        Console.WriteLine("Vogal A");
        break;
    case 'e':
    case 'E':
        Console.WriteLine("Vogal E");
        break;
    case 'i':
    case 'I':
        Console.WriteLine("Vogal I");
        break;
    default:
        Console.WriteLine("Outra letra");
        break;
}
```

#### `switch` com strings

```csharp
string comando = "abrir";

switch (comando.ToUpper())
{
    case "ABRIR":
        Console.WriteLine("Abrindo arquivo...");
        break;
    case "SALVAR":
        Console.WriteLine("Salvando arquivo...");
        break;
    case "FECHAR":
        Console.WriteLine("Fechando arquivo...");
        break;
    default:
        Console.WriteLine("Comando não reconhecido");
        break;
}
```

---

### 4. `switch` Expression (C# 8+) 🔥

Forma moderna e concisa do switch:

```csharp
// Sintaxe antiga
string GetDiaSemana(int dia)
{
    switch (dia)
    {
        case 1: 
            return "Segunda";
        case 2: 
            return "Terça";
        case 3: 
            return "Quarta";
        case 4: 
            return "Quinta";
        case 5: 
            return "Sexta";
        case 6: 
            return "Sábado";
        case 7: 
            return "Domingo";
        default: 
            return "Inválido";
    }
}

// ✅ Sintaxe moderna (C# 8+)
string GetDiaSemana(int dia) => dia switch
{
    1 => "Segunda",
    2 => "Terça",
    3 => "Quarta",
    4 => "Quinta",
    5 => "Sexta",
    6 => "Sábado",
    7 => "Domingo",
    _ => "Inválido"  // _ é o default
};

// Exemplo com cálculo
decimal CalcularDesconto(int quantidade) => quantidade switch
{
    < 10 => 0,
    >= 10 and < 50 => 0.05m,
    >= 50 and < 100 => 0.10m,
    >= 100 => 0.15m
};

// Múltiplos valores
string GetEstacaoAno(int mes) => mes switch
{
    12 or 1 or 2 => "Verão",
    3 or 4 or 5 => "Outono",
    6 or 7 or 8 => "Inverno",
    9 or 10 or 11 => "Primavera",
    _ => "Mês inválido"
};
```

---

### 5. Pattern Matching (C# 7+) 🚀

Recurso poderoso para comparações complexas:

#### Type Pattern

```csharp
object obj = "Hello";

if (obj is string texto)
{
    Console.WriteLine($"É uma string: {texto}");
    Console.WriteLine($"Tamanho: {texto.Length}");
}

// Com switch
string DescreverObjeto(object obj) => obj switch
{
    int numero => $"Número inteiro: {numero}",
    string texto => $"Texto: {texto}",
    double d => $"Decimal: {d:F2}",
    bool b => $"Booleano: {b}",
    null => "Nulo",
    _ => "Tipo desconhecido"
};
```

#### Property Pattern

```csharp
public class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
}

string ClassificarPessoa(Pessoa p) => p switch
{
    { Idade: < 13 } => "Criança",
    { Idade: >= 13 and < 18 } => "Adolescente",
    { Idade: >= 18 and < 60 } => "Adulto",
    { Idade: >= 60 } => "Idoso",
    null => "Pessoa inválida"
};

// Pattern mais complexo
string DescreverPessoa(Pessoa p) => p switch
{
    { Nome: "Admin", Idade: > 18 } => "Administrador adulto",
    { Idade: < 18 } => "Menor de idade",
    { Nome: null } => "Nome não informado",
    _ => $"{p.Nome}, {p.Idade} anos"
};
```

#### Relational Pattern (C# 9+)

```csharp
string ClassificarTemperatura(double temp) => temp switch
{
    < 0 => "Congelante",
    >= 0 and < 10 => "Muito frio",
    >= 10 and < 20 => "Frio",
    >= 20 and < 30 => "Agradável",
    >= 30 and < 40 => "Quente",
    >= 40 => "Muito quente"
};
```

#### Logical Pattern (C# 9+)

```csharp
bool EhLetraVogal(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u' 
                                   or 'A' or 'E' or 'I' or 'O' or 'U';

bool EhNumeroValido(int n) => n is >= 0 and <= 100;

bool EhPrimo(int n) => n is 2 or 3 or 5 or 7 or 11 or 13 or 17 or 19 or 23;
```

---

## 🔁 Estruturas de Repetição (Loops)

### 1. `for` - Loop Contado

Usado quando você sabe quantas vezes quer repetir:

```csharp
// Sintaxe: for (inicialização; condição; incremento)

// Exemplo básico: 0 a 9
for (int i = 0; i < 10; i++)
{
    Console.WriteLine($"Número: {i}");
}

// Exemplo: 1 a 10
for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"Número: {i}");
}

// Decrementando: 10 a 1
for (int i = 10; i >= 1; i--)
{
    Console.WriteLine($"Contagem regressiva: {i}");
}

// Incremento diferente: 0, 2, 4, 6, 8
for (int i = 0; i < 10; i += 2)
{
    Console.WriteLine($"Par: {i}");
}

// Múltiplas variáveis
for (int i = 0, j = 10; i < j; i++, j--)
{
    Console.WriteLine($"i={i}, j={j}");
}
```

#### Tabuada

```csharp
int numero = 7;

Console.WriteLine($"Tabuada do {numero}:");
for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"{numero} x {i} = {numero * i}");
}
```

#### Iterando Arrays

```csharp
int[] numeros = { 10, 20, 30, 40, 50 };

for (int i = 0; i < numeros.Length; i++)
{
    Console.WriteLine($"[{i}] = {numeros[i]}");
}

// Modificando valores
for (int i = 0; i < numeros.Length; i++)
{
    numeros[i] *= 2;  // Dobra cada valor
}
```

---

### 2. `while` - Loop Condicional

Executa enquanto a condição for verdadeira:

```csharp
// Sintaxe: while (condição)

int contador = 0;
while (contador < 5)
{
    Console.WriteLine($"Contador: {contador}");
    contador++;
}

// Exemplo: leitura até usuário digitar "sair"
string entrada = "";
while (entrada != "sair")
{
    Console.Write("Digite algo (ou 'sair'): ");
    entrada = Console.ReadLine().ToLower();
    Console.WriteLine($"Você digitou: {entrada}");
}

// Exemplo: soma até o número ser maior que 100
int soma = 0;
int numero = 1;
while (soma <= 100)
{
    soma += numero;
    numero++;
}
Console.WriteLine($"Soma: {soma}, Último número: {numero}");
```

#### `while` com validação

```csharp
int idade;
bool entradaValida = false;

while (!entradaValida)
{
    Console.Write("Digite sua idade (0-120): ");
    string entrada = Console.ReadLine();
    
    if (int.TryParse(entrada, out idade))
    {
        if (idade >= 0 && idade <= 120)
        {
            entradaValida = true;
            Console.WriteLine($"Idade válida: {idade}");
        }
        else
        {
            Console.WriteLine("Idade deve estar entre 0 e 120!");
        }
    }
    else
    {
        Console.WriteLine("Por favor, digite um número válido!");
    }
}
```

---

### 3. `do while` - Loop com Teste no Final

Executa pelo menos uma vez, depois testa a condição:

```csharp
// Sintaxe: do { } while (condição);

int contador = 0;
do
{
    Console.WriteLine($"Contador: {contador}");
    contador++;
} while (contador < 5);

// Diferença crucial: executa pelo menos uma vez
int numero = 10;
do
{
    Console.WriteLine("Executou!");  // Imprime mesmo com numero >= 10
} while (numero < 10);
```

#### Menu Interativo

```csharp
int opcao;
do
{
    Console.WriteLine();
    Console.WriteLine("===== MENU =====");
    Console.WriteLine("1. Cadastrar");
    Console.WriteLine("2. Listar");
    Console.WriteLine("3. Atualizar");
    Console.WriteLine("4. Deletar");
    Console.WriteLine("0. Sair");
    Console.Write("Escolha: ");
    
    opcao = int.Parse(Console.ReadLine());
    
    switch (opcao)
    {
        case 1:
            Console.WriteLine("Cadastrando...");
            break;
        case 2:
            Console.WriteLine("Listando...");
            break;
        case 3:
            Console.WriteLine("Atualizando...");
            break;
        case 4:
            Console.WriteLine("Deletando...");
            break;
        case 0:
            Console.WriteLine("Saindo...");
            break;
        default:
            Console.WriteLine("Opção inválida!");
            break;
    }
} while (opcao != 0);
```

---

### 4. `foreach` - Iteração em Coleções

Percorre todos os elementos de uma coleção:

```csharp
// Sintaxe: foreach (tipo variavel in colecao)

// Array
int[] numeros = { 1, 2, 3, 4, 5 };
foreach (int numero in numeros)
{
    Console.WriteLine(numero);
}

// String (é uma coleção de chars)
string nome = "C# Rocks";
foreach (char letra in nome)
{
    Console.WriteLine(letra);
}

// Lista
List<string> frutas = new List<string> { "Maçã", "Banana", "Laranja" };
foreach (string fruta in frutas)
{
    Console.WriteLine(fruta);
}

// Dictionary
Dictionary<string, int> idades = new Dictionary<string, int>
{
    { "João", 25 },
    { "Maria", 30 },
    { "Pedro", 28 }
};

foreach (KeyValuePair<string, int> item in idades)
{
    Console.WriteLine($"{item.Key} tem {item.Value} anos");
}

// Ou com var (mais limpo)
foreach (var item in idades)
{
    Console.WriteLine($"{item.Key} tem {item.Value} anos");
}
```

#### ⚠️ `foreach` é Read-Only

```csharp
int[] numeros = { 1, 2, 3, 4, 5 };

// ❌ ERRO: Não pode modificar o elemento
foreach (int numero in numeros)
{
    numero = numero * 2;  // Erro de compilação!
}

// ✅ Use for para modificar
for (int i = 0; i < numeros.Length; i++)
{
    numeros[i] *= 2;  // OK
}
```

---

## 🎮 Controle de Fluxo

### 1. `break` - Sair do Loop

```csharp
// Procurar um número
int[] numeros = { 1, 5, 8, 12, 15, 20 };
int procurado = 12;
bool encontrado = false;

for (int i = 0; i < numeros.Length; i++)
{
    if (numeros[i] == procurado)
    {
        Console.WriteLine($"Encontrado na posição {i}");
        encontrado = true;
        break;  // Sai do loop
    }
}

if (!encontrado)
{
    Console.WriteLine("Não encontrado");
}

// Em switch (obrigatório)
switch (opcao)
{
    case 1:
        Console.WriteLine("Opção 1");
        break;  // Obrigatório!
    case 2:
        Console.WriteLine("Opção 2");
        break;
}
```

---

### 2. `continue` - Pular Iteração

```csharp
// Imprimir apenas números pares
for (int i = 1; i <= 10; i++)
{
    if (i % 2 != 0)  // Se for ímpar
    {
        continue;  // Pula para próxima iteração
    }
    Console.WriteLine(i);  // Só executa para pares
}

// Processar apenas válidos
string[] entradas = { "10", "abc", "20", "xyz", "30" };
foreach (string entrada in entradas)
{
    if (!int.TryParse(entrada, out int numero))
    {
        Console.WriteLine($"Ignorando: {entrada}");
        continue;  // Pula para próximo
    }
    Console.WriteLine($"Processando: {numero}");
}
```

---

### 3. `return` - Sair do Método

```csharp
bool EhPrimo(int numero)
{
    if (numero <= 1)
        return false;  // Sai imediatamente
    
    if (numero == 2)
        return true;
    
    for (int i = 2; i <= Math.Sqrt(numero); i++)
    {
        if (numero % i == 0)
            return false;  // Não é primo, sai
    }
    
    return true;  // É primo
}

// Early return (boas práticas)
string ProcessarUsuario(Usuario usuario)
{
    if (usuario == null)
        return "Usuário nulo";
    
    if (!usuario.IsAtivo)
        return "Usuário inativo";
    
    if (usuario.Idade < 18)
        return "Menor de idade";
    
    // Código principal aqui
    return "Processado com sucesso";
}
```

---

## 📊 Comparação de Loops

| Loop | Quando Usar | Vantagens |
|------|-------------|-----------|
| **for** | Sabe quantas iterações | Índice disponível, flexível |
| **while** | Condição no início | Controle total da condição |
| **do-while** | Executar pelo menos 1x | Menu, validação |
| **foreach** | Iterar coleções | Simples, sem índice |

---

## 💡 Boas Práticas

### ✅ Faça:

```csharp
// Use nomes descritivos
for (int indiceAluno = 0; indiceAluno < alunos.Length; indiceAluno++)

// Use foreach quando não precisa do índice
foreach (var aluno in alunos)

// Prefira early return
if (!condicao) return;

```

### ❌ Evite:

```csharp
// Nomes ruins
for (int x = 0; x < 10; x++)

// Loop infinito acidental
while (true)  // Sem break dentro

// Aninhamento profundo
if (a) {
    if (b) {
        if (c) {
            if (d) {
                // Difícil de ler!
            }
        }
    }
}

```

---

## 🎓 Resumo dos Pontos-Chave

✅ **if/else** para decisões simples  
✅ **switch** para múltiplas comparações  
✅ **switch expression** é mais moderno e limpo  
✅ **Pattern matching** é poderoso (use!)  
✅ **for** quando sabe quantas iterações  
✅ **while** quando condição é desconhecida  
✅ **do-while** para executar pelo menos uma vez  
✅ **foreach** para iterar coleções (mais simples)  
✅ **break** sai do loop, **continue** pula iteração  
✅ **return** sai do método  

---

## 📚 Referências

- [Selection statements](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/selection-statements)
- [Iteration statements](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/iteration-statements)
- [Pattern matching](https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/pattern-matching)
- [Switch expression](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/switch-expression)

---

## ⏭️ Próximos Passos

Agora você domina as estruturas de controle! Próximo:
- **Dia 02**: Programação Orientada a Objetos
- Classes e objetos
- Construtores e métodos
- Encapsulamento

**Continue para**: `02-exercicios.md`
