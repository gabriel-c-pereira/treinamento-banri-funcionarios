# 🎯 Exercícios - Sintaxe Básica do C#

## 📋 Instruções Gerais

- Crie um novo projeto para cada exercício ou use o mesmo projeto com comentários separando cada exercício
- Teste seu código após cada implementação
- Preste atenção aos tipos de dados escolhidos
- Use nomes de variáveis descritivos

---

## Exercício 1: Calculadora Básica 🧮

**Objetivo**: Praticar declaração de variáveis e operadores aritméticos.

### Tarefas:

Crie um programa que:

1. Declare duas variáveis numéricas com valores à sua escolha
2. Execute e exiba todas as operações básicas:
   - Soma
   - Subtração
   - Multiplicação
   - Divisão
   - Resto da divisão (módulo)

### Exemplo de código base:

```csharp
int numero1 = 20;
int numero2 = 3;

// TODO: Calcule e exiba cada operação
Console.WriteLine($"Soma: {numero1} + {numero2} = ???");
```

### Saída esperada:

```
Soma: 20 + 3 = 23
Subtração: 20 - 3 = 17
Multiplicação: 20 * 3 = 60
Divisão: 20 / 3 = 6
Resto: 20 % 3 = 2
```

### Desafio extra:

- Faça a divisão retornar um valor decimal
- Adicione formatação para exibir apenas 2 casas decimais

---

## Exercício 2: Conversor de Temperaturas 🌡️

**Objetivo**: Trabalhar com tipos numéricos e conversões.

### Tarefas:

Crie um programa que converta temperaturas de Celsius para:
- Fahrenheit: `F = C * 9/5 + 32`
- Kelvin: `K = C + 273.15`

### Requisitos:

```csharp
double celsius = 25.0;

// TODO: Implemente as conversões
double fahrenheit = ???;
double kelvin = ???;

// TODO: Exiba os resultados formatados
```

### Saída esperada:

```
Temperatura em Celsius: 25.00°C
Temperatura em Fahrenheit: 77.00°F
Temperatura em Kelvin: 298.15K
```

### Dicas:

- Use `double` para precisão decimal
- Use formatação `:F2` para 2 casas decimais
- Atenção à ordem das operações (parênteses!)

---

## Exercício 3: Operações com Strings 📝

**Objetivo**: Manipular strings e usar seus métodos.

### Tarefas:

```csharp
string frase = "  Aprendendo C# é Muito Legal!  ";

// TODO: Execute e exiba:
// 1. Tamanho da string original
// 2. String em maiúsculas
// 3. String em minúsculas
// 4. String sem espaços nas extremidades
// 5. Substitua "Legal" por "Divertido"
// 6. Verifique se contém a palavra "C#"
// 7. Extraia apenas "Aprendendo C#"
```

### Saída esperada:

```
String original: "  Aprendendo C# é Muito Legal!  "
Tamanho: 33 caracteres

Maiúsculas: "  APRENDENDO C# É MUITO LEGAL!  "
Minúsculas: "  aprendendo c# é muito legal!  "
Sem espaços extras: "Aprendendo C# é Muito Legal!"
Substituída: "Aprendendo C# é Muito Divertido!"

Contém "C#"? Sim
Substring extraída: "Aprendendo C#"
```

---

## Exercício 4: Loja de Produtos 🛒

**Objetivo**: Trabalhar com tipos decimais e cálculos financeiros.

### Tarefas:

Simule uma compra com 3 produtos:

```csharp
// Produto 1
string nomeProduto1 = "Mouse Gamer";
decimal precoProduto1 = 89.90m;
int quantidadeProduto1 = 2;

// TODO: Crie variáveis para mais 2 produtos

// TODO: Calcule:
// - Subtotal de cada produto
// - Total da compra
// - Desconto de 10% se total > 200
// - Total final

// TODO: Exiba um recibo formatado
```

### Saída esperada:

```
═══════════════════════════════════════════════
                   RECIBO
═══════════════════════════════════════════════
Item                Qtd    Preço    Subtotal
───────────────────────────────────────────────
Mouse Gamer           2    R$ 89.90  R$ 179.80
Teclado Mecânico      1    R$ 250.00 R$ 250.00
Mousepad              1    R$ 30.50  R$ 30.50
───────────────────────────────────────────────
                         Subtotal:  R$ 460.30
                         Desconto:  R$ 46.03
───────────────────────────────────────────────
                    TOTAL A PAGAR:  R$ 414.27
═══════════════════════════════════════════════
```

---

## Exercício 5: Teste de Operadores Lógicos 🧩

**Objetivo**: Praticar operadores relacionais e lógicos.

### Tarefas:

```csharp
int idade = 20;
bool temCNH = true;
bool temCarro = false;
double saldo = 150.00;

// TODO: Crie expressões booleanas para verificar:
// 1. É maior de idade (>= 18)
// 2. Pode dirigir (maior de idade E tem CNH)
// 3. Pode viajar de carro (pode dirigir E tem carro)
// 4. Pode comprar produto de R$ 100 (saldo >= 100)
// 5. Situação crítica (menor de idade OU sem CNH)

// TODO: Exiba cada verificação com resultado
```

### Saída esperada:

```
VERIFICAÇÕES:
═════════════════════════════════════
✓ É maior de idade: Sim
✓ Pode dirigir: Sim
✗ Pode viajar de carro próprio: Não
✓ Pode comprar produto (R$ 100): Sim
✗ Situação crítica: Não
```

---

## Exercício 6: Conversões de Tipo 🔄

**Objetivo**: Praticar conversões implícitas e explícitas.

### Tarefas:

```csharp
// 1. Conversão Implícita
byte numeroPequeno = 50;
int numeroMedio = /* Converta de byte */;
long numeroGrande = /* Converta de int */;
double numeroDecimal = /* Converta de int */;

// 2. Conversão Explícita (Cast)
double pi = 3.14159;
int piInteiro = /* Cast para int */;
int piArredondado = /* Arredonde e converta */;

// 3. Parse e TryParse
string texto1 = "42";
string texto2 = "abc";

// Parse (pode gerar erro)
int numero1 = /* Parse texto1 */;

// TryParse (seguro)
bool sucesso = /* TryParse texto2 */;

// 4. ToString
int idade = 25;
string idadeTexto = /* Converta para string */;
string idadeFormatada = /* Converta com 3 dígitos (025) */;

// TODO: Exiba todos os resultados
```

### Saída esperada:

```
CONVERSÕES DE TIPO
════════════════════════════════════════

Conversões Implícitas:
  byte (50) → int: 50
  int (50) → long: 50
  int (50) → double: 50.0

Conversões Explícitas:
  double (3.14159) → int: 3 (truncado)
  double (3.14159) → int: 3 (arredondado)

Parse e TryParse:
  "42" → int: 42 (sucesso)
  "abc" → int: Falhou na conversão

ToString:
  int (25) → string: "25"
  int (25) → string formatada: "025"
```

---

## Exercício 7: Calculadora de Horas 🕐

**Objetivo**: Trabalhar com operações matemáticas e formatação.

### Tarefas:

Um funcionário trabalhou:
- Segunda: 8 horas
- Terça: 7.5 horas
- Quarta: 8 horas
- Quinta: 9 horas
- Sexta: 6 horas

```csharp
double horasSegunda = 8.0;
// TODO: Declare as outras variáveis

// TODO: Calcule:
// - Total de horas trabalhadas na semana
// - Média de horas por dia
// - Se trabalhou mais de 40h, calcule horas extras (acima de 40h)
// - Valor a receber (R$ 50/hora normal, R$ 75/hora extra)

// TODO: Exiba relatório formatado
```

### Saída esperada:

```
═══════════════════════════════════
    RELATÓRIO SEMANAL DE HORAS
═══════════════════════════════════
Segunda-feira:      8.00 horas
Terça-feira:        7.50 horas
Quarta-feira:       8.00 horas
Quinta-feira:       9.00 horas
Sexta-feira:        6.00 horas
───────────────────────────────────
Total:             38.50 horas
Média diária:       7.70 horas
───────────────────────────────────
Horas normais:     38.50 @ R$ 50/h
Horas extras:       0.00 @ R$ 75/h
───────────────────────────────────
TOTAL A RECEBER:   R$ 1,925.00
═══════════════════════════════════
```

---

## Exercício 8: Validador de Dados 🔍

**Objetivo**: Combinar tipos, conversões e operadores lógicos.

### Tarefas:

Crie um validador que receba dados em formato string e valide:

```csharp
string idadeStr = "25";
string alturaStr = "1.75";
string pesoStr = "70.5";
string nomeStr = "   ";
string emailStr = "joao@email";

// TODO: Valide cada campo:
// 1. Idade: deve ser número entre 0 e 150
// 2. Altura: deve ser número entre 0.5 e 3.0
// 3. Peso: deve ser número entre 2 e 500
// 4. Nome: não pode estar vazio ou só espaços
// 5. Email: deve conter "@" e "."

// Use TryParse para conversões seguras
// Exiba mensagem de sucesso ou erro para cada campo
```

### Saída esperada:

```
VALIDAÇÃO DE DADOS
═════════════════════════════════════════════
✓ Idade: 25 anos - Válido
✓ Altura: 1.75 m - Válido
✓ Peso: 70.5 kg - Válido
✗ Nome: Campo obrigatório não preenchido
✗ Email: Formato inválido
═════════════════════════════════════════════
Resultado: 3 campos válidos, 2 campos inválidos
```

---

## Exercício 9: Projeto Final - Sistema de Cadastro 🎯

**Objetivo**: Consolidar todo o aprendizado.

### Descrição:

Crie um sistema que cadastre informações de um aluno:

### Requisitos:

1. **Dados do Aluno**:
   - Nome completo
   - Idade
   - Matrícula (numérica)
   - Curso
   - Período (1 a 10)

2. **Notas** (usar decimal):
   - Nota 1
   - Nota 2
   - Nota 3

3. **Cálculos**:
   - Média das notas
   - Status: "Aprovado" (>= 7), "Recuperação" (5-6.9), "Reprovado" (< 5)
   - Pontos necessários para aprovação (se em recuperação)

4. **Formatação**:
   - Use linha e colunas organizadas
   - Números formatados (2 casas decimais)
   - Visual agradável

### Template:

```csharp
// Dados do aluno
string nomeCompleto = "Ana Paula Santos";
int idade = 20;
long matricula = 202501234;
string curso = "Ciência da Computação";
int periodo = 3;

// Notas
decimal nota1 = 8.5m;
decimal nota2 = 7.0m;
decimal nota3 = 6.5m;

// TODO: Implemente os cálculos

// TODO: Crie um relatório bonito e completo
```

### Saída esperada:

```
╔═══════════════════════════════════════════════════╗
║          SISTEMA ACADÊMICO - CADASTRO             ║
╠═══════════════════════════════════════════════════╣
║ DADOS PESSOAIS                                    ║
╠═══════════════════════════════════════════════════╣
║ Nome: Ana Paula Santos                            ║
║ Idade: 20 anos                                    ║
║ Matrícula: 202501234                              ║
║ Curso: Ciência da Computação                      ║
║ Período: 3º                                       ║
╠═══════════════════════════════════════════════════╣
║ DESEMPENHO ACADÊMICO                              ║
╠═══════════════════════════════════════════════════╣
║ Nota 1: 8.50                                      ║
║ Nota 2: 7.00                                      ║
║ Nota 3: 6.50                                      ║
║                                                   ║
║ Média Final: 7.33                                 ║
║ Status: ✓ APROVADO                                ║
╚═══════════════════════════════════════════════════╝
```

---

## 🎓 Critérios de Avaliação

- ✅ **Tipos corretos**: Uso adequado de int, double, decimal, string, bool
- ✅ **Nomenclatura**: Variáveis com nomes descritivos (camelCase)
- ✅ **Formatação**: Saída bem organizada e legível
- ✅ **Cálculos**: Operações matemáticas corretas
- ✅ **Conversões**: Uso correto de Parse/TryParse/Cast
- ✅ **Boas práticas**: Código limpo e comentado

---

## 💡 Dicas para Sucesso

1. **Teste incrementalmente**: Execute após cada pequena mudança
2. **Use var com critério**: Apenas quando o tipo é óbvio
3. **Decimal para dinheiro**: Sempre use `decimal` para valores monetários
4. **TryParse é mais seguro**: Use em produção ao invés de Parse
5. **Interpolação de strings**: Use `$""` para melhor legibilidade
6. **Formatação**: `:F2` para decimais, `:C` para moeda, `:D3` para padding

---

## 📚 Referências

- [Built-in Types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/built-in-types)
- [Operators and Expressions](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/)
- [String Interpolation](https://learn.microsoft.com/dotnet/csharp/language-reference/tokens/interpolated)

---

## ⏭️ Próximo Tópico

Após completar estes exercícios, você estará preparado para:
- Estruturas de decisão (if, else, switch)
- Loops e iterações
- Arrays e coleções básicas

**Continue para**: `03-estruturas-controle/`
