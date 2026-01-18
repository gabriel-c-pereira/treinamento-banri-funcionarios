# 🎯 Exercícios - Introdução ao C# e .NET

## 📋 Instruções Gerais

- Complete todos os exercícios em ordem
- Teste seu código após cada exercício
- Consulte a documentação quando necessário
- Salve seus arquivos com nomes descritivos

---

## Exercício 1: Configuração do Ambiente ⚙️

**Objetivo**: Verificar se seu ambiente está configurado corretamente.

### Tarefas:

1. Abra o terminal/prompt de comando
2. Execute os seguintes comandos e anote as saídas:

```bash
dotnet --version
dotnet --info
dotnet --list-sdks
```

3. Crie um arquivo de texto chamado `ambiente-verificacao.txt` e cole os resultados

### Critérios de Sucesso:

- ✅ O comando `dotnet --version` deve mostrar versão 6.0 ou superior
- ✅ Você consegue ver pelo menos um SDK listado

---

## Exercício 2: Primeiro Projeto Console 🖥️

**Objetivo**: Criar e executar seu primeiro projeto console em C#.

### Tarefas:

1. Crie um novo projeto console chamado `MeuPrimeiroProjeto`
2. Execute o projeto e observe a saída padrão
3. Modifique o código para exibir:
   ```
   Olá! Bem-vindo ao mundo C#!
   Meu nome é: [Seu Nome]
   Este é meu primeiro projeto em .NET 8
   ```

### Comandos necessários:

```bash
dotnet new console -n MeuPrimeiroProjeto
cd MeuPrimeiroProjeto
dotnet run
```

### Dica:

Use múltiplos `Console.WriteLine()` para cada linha de saída.

### Exemplo de saída esperada:

```
Olá! Bem-vindo ao mundo C#!
Meu nome é: João Silva
Este é meu primeiro projeto em .NET 8
```

---

## Exercício 3: Explorando Comandos dotnet 🔧

**Objetivo**: Familiarizar-se com comandos essenciais do dotnet CLI.

### Tarefas:

1. Na pasta `MeuPrimeiroProjeto`, execute:
   - `dotnet build` (compila o projeto)
   - `dotnet clean` (limpa arquivos compilados)
   - `dotnet build` novamente
   - Observe a diferença de tempo

2. Explore a pasta `bin/` e `obj/` criadas
3. Liste o conteúdo de cada uma em um arquivo `estrutura-projeto.txt`

### Perguntas para reflexão:

1. Qual a diferença entre `dotnet run` e `dotnet build`?
2. Para que serve a pasta `obj/`?
3. Onde fica o executável final do projeto?

---

## Exercício 4: Personalizando o Projeto 🎨

**Objetivo**: Modificar configurações do projeto.

### Tarefas:

1. Abra o arquivo `.csproj` do seu projeto
2. Observe as propriedades atuais
3. Adicione estas novas linhas dentro de `<PropertyGroup>`:

```xml
<AssemblyName>MeuApp</AssemblyName>
<Version>1.0.0</Version>
<Authors>Seu Nome</Authors>
<Description>Meu primeiro projeto .NET</Description>
```

4. Compile e execute novamente com `dotnet run`

### Resultado esperado:

O projeto deve compilar sem erros. O nome do executável mudará para `MeuApp.dll`.

---

## Exercício 5: Trabalhando com Argumentos 📝

**Objetivo**: Receber e processar argumentos da linha de comando.

### Tarefas:

Modifique seu `Program.cs` para:

1. Verificar se há argumentos passados
2. Se houver argumentos, exibir: "Você passou [N] argumentos"
3. Listar cada argumento com seu índice

### Código base:

```csharp
// Se precisar usar args, adicione no topo:
// args vem automaticamente disponível
static void Main(string[] args)
{
    if (args.Length == 0)
    {
        Console.WriteLine("Nenhum argumento foi passado.");
    }
    else
    {
        Console.WriteLine($"Você passou {args.Length} argumento(s):");
        
        // TODO: Implemente um loop para exibir cada argumento
    }
}
```

### Teste seu programa:

```bash
dotnet run -- teste argumento1 argumento2
```

### Saída esperada:

```
Você passou 3 argumento(s):
[0] teste
[1] argumento1
[2] argumento2
```

---

## Exercício 6: Criando Múltiplos Projetos 📦

**Objetivo**: Entender diferentes tipos de projetos .NET.

### Tarefas:

Crie os seguintes projetos em pastas separadas:

1. **Console App** chamado `AppConsole`
2. **Class Library** chamado `MinhaLib`
3. **Web API** chamado `MinhaApi`

### Comandos:

```bash
# Volte para a pasta principal primeiro
cd ..

# Crie os projetos
dotnet new console -n AppConsole
dotnet new classlib -n MinhaLib
dotnet new webapi -n MinhaApi
```

### Perguntas:

1. Qual a diferença no `.csproj` entre console e classlib?
2. O que há de diferente no projeto webapi?
3. Qual projeto NÃO tem `Program.cs`?

---

## Exercício 7: Gerenciando Pacotes NuGet 📚

**Objetivo**: Adicionar e usar pacotes externos.

### Tarefas:

No projeto `AppConsole`:

1. Adicione o pacote `Newtonsoft.Json`:
   ```bash
   cd AppConsole
   dotnet add package Newtonsoft.Json
   ```

2. Modifique o `Program.cs`:

```csharp
using Newtonsoft.Json;

var pessoa = new 
{
    Nome = "Maria",
    Idade = 25,
    Cidade = "São Paulo"
};

string json = JsonConvert.SerializeObject(pessoa, Formatting.Indented);
Console.WriteLine("Objeto convertido para JSON:");
Console.WriteLine(json);
```

3. Execute e observe o resultado

### Saída esperada:

```json
Objeto convertido para JSON:
{
  "Nome": "Maria",
  "Idade": 25,
  "Cidade": "São Paulo"
}
```

---

## Exercício 8: Explorando Namespaces 🗂️

**Objetivo**: Entender organização de código com namespaces.

### Tarefas:

1. No projeto `MinhaLib`, crie uma nova classe `Calculadora.cs`:

```csharp
namespace MinhaLib.Matematica
{
    public class Calculadora
    {
        public int Somar(int a, int b)
        {
            return a + b;
        }
        
        public int Subtrair(int a, int b)
        {
            return a - b;
        }
    }
}
```

2. Compile o projeto:
   ```bash
   dotnet build
   ```

### Desafio Extra:

Adicione métodos para:
- Multiplicar
- Dividir (com tratamento para divisão por zero)

---

## Exercício 9: Formatação de Saída 🎭

**Objetivo**: Praticar diferentes formas de formatação de strings.

### Tarefas:

Crie um novo programa que exiba:

```csharp
string nome = "João";
int idade = 28;
double altura = 1.75;
DateTime hoje = DateTime.Now;

// TODO: Complete as linhas abaixo usando diferentes métodos de formatação

// 1. Concatenação
Console.WriteLine("Olá, " + nome + "!");

// 2. String.Format
Console.WriteLine(/* Seu código aqui */);

// 3. Interpolação (recomendado)
Console.WriteLine($"Nome: {nome}, Idade: {idade}");

// 4. Formatação de números
Console.WriteLine($"Altura: {altura:F2} metros");

// 5. Formatação de data
Console.WriteLine(/* Formate a data no padrão dd/MM/yyyy */);
```

### Saída esperada:

```
Olá, João!
Nome: João, Idade: 28
Altura: 1.75 metros
Data: 14/10/2025
```

---

## Exercício 10: Criando um Projeto Completo 🚀

**Objetivo**: Consolidar todo o aprendizado criando um mini-projeto.

### Descrição do Projeto:

Crie uma aplicação console que:

1. Exibe um menu de opções
2. Solicita o nome do usuário
3. Mostra informações sobre o .NET instalado
4. Exibe a data e hora atual formatada (use o `System.DateTime.Now` para obter a data e hora atual)
5. Permite sair da aplicação

### Estrutura sugerida:

```csharp
Console.WriteLine("=================================");
Console.WriteLine("   BEM-VINDO AO SISTEMA .NET     ");
Console.WriteLine("=================================");
Console.WriteLine();

Console.Write("Digite seu nome: ");
string nome = Console.ReadLine() ?? "Visitante";

// TODO: Implemente o restante do programa
```

### Funcionalidades esperadas:

- ✅ Menu com pelo menos 3 opções
- ✅ Uso de interpolação de strings
- ✅ Formatação de data/hora
- ✅ Leitura de entrada do usuário
- ✅ Código organizado e comentado

### Exemplo de interação:

```
=================================
   BEM-VINDO AO SISTEMA .NET     
=================================

Digite seu nome: João

Olá, João!

MENU:
[1] Ver informações do sistema
[2] Ver data e hora
[3] Sair

Escolha uma opção: 1

Sistema: Windows
Versão .NET: 8.0.0
...
```

---

## 🎓 Critérios de Avaliação

Seu trabalho será avaliado com base em:

- ✅ **Funcionamento**: O código executa sem erros
- ✅ **Completude**: Todos os exercícios foram feitos
- ✅ **Organização**: Código limpo e bem estruturado
- ✅ **Boas práticas**: Uso correto de convenções C#
- ✅ **Comentários**: Código documentado quando necessário

---

## 💡 Dicas Finais

1. **Teste frequentemente**: Execute `dotnet run` após cada mudança
2. **Leia os erros**: O compilador C# dá mensagens claras
3. **Use o IntelliSense**: Ctrl+Space mostra sugestões
4. **Consulte a documentação**: https://learn.microsoft.com/dotnet
5. **Não copie e cole**: Digite o código para memorizar

---

## 📚 Recursos de Ajuda

- **Documentação do Console**: https://learn.microsoft.com/dotnet/api/system.console
- **String Formatting**: https://learn.microsoft.com/dotnet/standard/base-types/formatting-types
- **dotnet CLI**: https://learn.microsoft.com/dotnet/core/tools/

---

## ⏭️ Próximos Passos

Após completar estes exercícios, você estará pronto para:
- Trabalhar com tipos de dados e variáveis
- Usar operadores e expressões
- Implementar estruturas de controle

**Continue para**: `03-exercicio1-corrigido.cs`
