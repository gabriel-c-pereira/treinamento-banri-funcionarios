# 🎯 Exercícios - Estruturas de Controle

## 📋 Instruções Gerais

- Complete os exercícios em ordem crescente de dificuldade
- Teste cada solução antes de prosseguir
- Experimente diferentes abordagens
- Use nomes de variáveis descritivos

---

## Exercício 1: Calculadora com Menu 🧮

**Objetivo**: Praticar switch e operadores aritméticos.

### Tarefas:

Crie uma calculadora que:
1. Exibe um menu com operações (+, -, *, /)
2. Lê dois números do usuário
3. Executa a operação escolhida
4. Exibe o resultado


### Saída esperada:

```
===== CALCULADORA =====
1. Somar
2. Subtrair
3. Multiplicar
4. Dividir
Escolha a operação: 1
Digite o primeiro número: 10
Digite o segundo número: 5
Resultado: 10 + 5 = 15
```

### Desafio extra:
- Adicione validação para divisão por zero
- Implemente operação de potência (^)

---

## Exercício 2: Par ou Ímpar 🎲

**Objetivo**: Praticar operador módulo (%) e if/else.

### Tarefas:

```csharp
// TODO: Leia um número do usuário
// TODO: Verifique se é par ou ímpar usando %
// TODO: Exiba o resultado

```

### Saída esperada:

```
Digite um número: 7
O número 7 é ÍMPAR

Digite um número: 10
O número 10 é PAR
```

### Desafio extra:
- Leia 10 números e conte quantos são pares e ímpares
- Identifique se é positivo/negativo também

---

## Exercício 3: Classificador de Idade 👶👴

**Objetivo**: Praticar if-else if-else encadeado.

### Tarefas:

Crie um programa que classifica a idade em categorias:
- 0-12: Criança
- 13-17: Adolescente
- 18-59: Adulto
- 60+: Idoso


### Saída esperada:

```
Digite a idade: 25
Classificação: Adulto
```

---

## Exercício 4: Dias da Semana 📅

**Objetivo**: Praticar switch.

### Tarefas:

```csharp
Console.Write("Digite um número (1-7): ");
int dia = int.Parse(Console.ReadLine());

// TODO: Use switch para exibir o dia da semana
// 1 = Domingo, 2 = Segunda, ..., 7 = Sábado

// TODO: Informe também se é dia útil ou final de semana
```

### Saída esperada:

```
Digite um número (1-7): 3
Dia: Terça-feira
Tipo: Dia útil
```

### Desafio extra:
- Implemente com switch clássico
- Reimplemente com switch expression
- Adicione nome do dia em inglês também

---

## Exercício 5: Tabuada Completa ✖️

**Objetivo**: Praticar loops for aninhados.

### Tarefas:

```csharp
Console.Write("Digite um número (1-10): ");
int numero = int.Parse(Console.ReadLine());

// TODO: Exiba a tabuada de multiplicação de 1 a 10
```

### Saída esperada:

```
Digite um número (1-10): 7

Tabuada do 7:
7 x 1 = 7
7 x 2 = 14
7 x 3 = 21
...
7 x 10 = 70
```

### Desafio extra:
- Exiba tabuadas de 1 a 10
- Formate em tabela bonita
- Adicione tabuada de divisão também

---

## Exercício 6: Contador de Vogais 🔤

**Objetivo**: Praticar foreach e switch com caracteres.

### Tarefas:

```csharp
Console.Write("Digite uma frase: ");
string frase = Console.ReadLine();

// TODO: Percorra cada caractere da frase
// TODO: Conte vogais e consoantes
// TODO: Ignore espaços e números
```

### Saída esperada:

```
Digite uma frase: Programar em C# é legal!
Vogais: 7
Consoantes: 11
```

### Desafio extra:
- Conte cada vogal individualmente (a, e, i, o, u)
- Identifique letras maiúsculas e minúsculas
- Conte números e símbolos também

---

## Exercício 7: Número Primo 🔢

**Objetivo**: Praticar loops e lógica matemática.

### Tarefas:

Crie um programa que verifica se um número é primo:

```csharp
Console.Write("Digite um número: ");
int numero = int.Parse(Console.ReadLine());

// TODO: Implemente a lógica de verificação
// Dica: número primo só é divisível por 1 e ele mesmo
// Teste divisões de 2 até √numero
```

### Saída esperada:

```
Digite um número: 17
17 é PRIMO

Digite um número: 15
15 NÃO é primo (divisível por 3 e 5)
```

### Desafio extra:
- Liste todos os primos até 100
- Mostre os divisores quando não for primo
- Otimize para parar ao encontrar primeiro divisor

---

## Exercício 8: Jogo de Adivinhação 🎮

**Objetivo**: Praticar while, random e if/else.

### Tarefas:

```csharp
Random random = new Random();
int numeroSecreto = random.Next(1, 101);  // 1 a 100
int tentativas = 0;
int palpite;

Console.WriteLine("=== JOGO DE ADIVINHAÇÃO ===");
Console.WriteLine("Adivinhe o número entre 1 e 100");

// TODO: Use do-while para ler palpites
// TODO: Dê dicas: "Muito alto" ou "Muito baixo"
// TODO: Conte as tentativas
// TODO: Pare quando acertar

Console.WriteLine($"Parabéns! Você acertou em {tentativas} tentativas!");
```

### Saída esperada:

```
=== JOGO DE ADIVINHAÇÃO ===
Adivinhe o número entre 1 e 100
Digite seu palpite: 50
Muito baixo!
Digite seu palpite: 75
Muito alto!
Digite seu palpite: 63
Muito baixo!
Digite seu palpite: 69
Parabéns! Você acertou em 4 tentativas!
```

### Desafio extra:
- Limite de tentativas (máximo 7)
- Sistema de pontuação (menos tentativas = mais pontos)
- Opção de jogar novamente

---

## Exercício 9: Validador de Dados 🔍

**Objetivo**: Praticar validação com loops e if/else.

### Tarefas:

Crie um programa que lê e valida dados do usuário:

```csharp
// 1. Nome (não vazio, mínimo 3 caracteres)
string nome;
while (true)
{
    Console.Write("Digite seu nome: ");
    nome = Console.ReadLine();
    
    // TODO: Valide o nome
    // Se válido: break
    // Se inválido: mostre erro e tente novamente
}

// 2. Idade (entre 0 e 120)
// TODO: Implemente validação similar

// 3. Email (deve conter @ e .)
// TODO: Implemente validação

Console.WriteLine("\nDados válidos:");
Console.WriteLine($"Nome: {nome}");
Console.WriteLine($"Idade: {idade}");
Console.WriteLine($"Email: {email}");
```

### Saída esperada:

```
Digite seu nome: Jo
❌ Nome deve ter pelo menos 3 caracteres!
Digite seu nome: João Silva
✓ Nome válido!

Digite sua idade: 200
❌ Idade deve estar entre 0 e 120!
Digite sua idade: 25
✓ Idade válida!

Digite seu email: joao@exemplo
❌ Email inválido!
Digite seu email: joao@exemplo.com
✓ Email válido!

Dados válidos:
Nome: João Silva
Idade: 25
Email: joao@exemplo.com
```

### Desafio extra:
- Adicione mais validações (CPF, telefone)
- Crie uma função para cada validação
- Permita 3 tentativas antes de desistir

---

## Exercício 10: Sistema de Notas 📊

**Objetivo**: Projeto integrado com múltiplas estruturas de controle.

### Descrição:

Crie um sistema completo para gerenciar notas de alunos:

### Funcionalidades:

1. **Menu Principal** (do-while):
   - 1. Adicionar aluno e notas
   - 2. Calcular média
   - 3. Verificar aprovação
   - 4. Estatísticas da turma
   - 5. Sair

2. **Adicionar Aluno**:
   - Leia nome e 3 notas (0-10)
   - Valide cada nota

3. **Calcular Média**:
   - Média = (N1 + N2 + N3) / 3

4. **Verificar Aprovação**:
   - Média >= 7: Aprovado
   - Média >= 5: Recuperação
   - Média < 5: Reprovado

5. **Estatísticas**:
   - Total de alunos
   - Média geral da turma
   - Maior e menor nota
   - Taxa de aprovação

### Template:

```csharp
// Arrays para armazenar dados (máximo 50 alunos)
string[] nomes = new string[50];
double[] nota1 = new double[50];
double[] nota2 = new double[50];
double[] nota3 = new double[50];
int totalAlunos = 0;

int opcao;
do
{
    Console.WriteLine("\n════════════════════════════════");
    Console.WriteLine("   SISTEMA DE GERENCIAMENTO");
    Console.WriteLine("         DE NOTAS");
    Console.WriteLine("════════════════════════════════");
    Console.WriteLine("1. Adicionar aluno");
    Console.WriteLine("2. Calcular médias");
    Console.WriteLine("3. Verificar aprovações");
    Console.WriteLine("4. Estatísticas da turma");
    Console.WriteLine("5. Sair");
    Console.WriteLine("════════════════════════════════");
    Console.Write("Escolha uma opção: ");
    
    opcao = int.Parse(Console.ReadLine());
    
    switch (opcao)
    {
        case 1:
            // TODO: Adicionar aluno
            break;
        case 2:
            // TODO: Calcular médias
            break;
        case 3:
            // TODO: Verificar aprovações
            break;
        case 4:
            // TODO: Estatísticas
            break;
        case 5:
            Console.WriteLine("Encerrando sistema...");
            break;
        default:
            Console.WriteLine("Opção inválida!");
            break;
    }
    
} while (opcao != 5);
```

### Saída esperada:

```
════════════════════════════════
   SISTEMA DE GERENCIAMENTO
         DE NOTAS
════════════════════════════════
1. Adicionar aluno
2. Calcular médias
3. Verificar aprovações
4. Estatísticas da turma
5. Sair
════════════════════════════════
Escolha uma opção: 1

Digite o nome do aluno: João Silva
Digite a nota 1 (0-10): 8.5
Digite a nota 2 (0-10): 7.0
Digite a nota 3 (0-10): 9.0
✓ Aluno adicionado com sucesso!

...

Escolha uma opção: 2

MÉDIAS DOS ALUNOS:
─────────────────────────────────
João Silva: 8.17
Maria Santos: 7.50
Pedro Costa: 5.33
─────────────────────────────────

...

Escolha uma opção: 4

ESTATÍSTICAS DA TURMA:
═══════════════════════════════════
Total de alunos: 3
Média geral: 7.00
Maior nota: 9.0 (João Silva)
Menor nota: 4.5 (Pedro Costa)
Taxa de aprovação: 66.7%
═══════════════════════════════════
```

### Desafio extra:
- Salve dados em arquivo
- Permita editar/remover alunos
- Adicione sistema de pesos nas notas
- Gere gráfico de distribuição com caracteres ASCII
- Ordene alunos por média

---

## 🎓 Critérios de Avaliação

- ✅ **Funcionalidade**: Código funciona corretamente
- ✅ **Validação**: Trata entradas inválidas
- ✅ **Estruturas**: Usa as estruturas de controle adequadas
- ✅ **Legibilidade**: Código limpo e organizado
- ✅ **Nomenclatura**: Variáveis com nomes descritivos
- ✅ **Comentários**: Código documentado quando necessário

---

## 💡 Dicas de Resolução

### Para if/else:
```csharp
// ✅ BOM
if (idade >= 18)
{
    // código
}
else if (idade >= 13)
{
    // código
}
else
{
    // código
}

// ❌ RUIM
if (idade >= 18) { /* código */ }
if (idade >= 13 && idade < 18) { /* redundante */ }
if (idade < 13) { /* redundante */ }
```

### Para switch:
```csharp
// ✅ MODERNO (C# 8+)
string resultado = opcao switch
{
    1 => "Opção 1",
    2 => "Opção 2",
    _ => "Inválido"
};

// ✅ CLÁSSICO
switch (opcao)
{
    case 1:
        resultado = "Opção 1";
        break;
    default:
        resultado = "Inválido";
        break;
}
```

### Para loops:
```csharp
// Use for quando sabe quantas iterações
for (int i = 0; i < 10; i++) { }

// Use while quando não sabe
while (condicao) { }

// Use do-while para executar pelo menos 1x
do { } while (condicao);

// Use foreach para coleções
foreach (var item in lista) { }
```

---

## 📚 Recursos de Apoio

- [if statement](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/selection-statements#the-if-statement)
- [switch statement](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement)
- [for loop](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/iteration-statements#the-for-statement)
- [while loop](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/iteration-statements#the-while-statement)
- [Pattern matching](https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/pattern-matching)

---

## ⏭️ Próximo Passo

Após completar estes exercícios, você estará pronto para:
- **Dia 02**: Programação Orientada a Objetos
- Classes e objetos
- Métodos e construtores
- Encapsulamento

**Veja as correções em**: `03-exercicio1-corrigido.cs` e seguintes
