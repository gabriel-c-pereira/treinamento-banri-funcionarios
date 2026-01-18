# 03 - Depuração no VS Code

## 🎯 Objetivos

Ao final deste módulo, você será capaz de:
- Configurar o debugger C# no VS Code para diferentes tipos de projetos
- Usar breakpoints avançados (condicionais, logpoints, hit count)
- Navegar pelo código durante depuração (Step Into/Over/Out)
- Inspecionar e modificar variáveis em tempo de execução
- Analisar o Call Stack e entender o fluxo de execução
- Depurar aplicações remotas e anexar a processos em execução

---

## 📑 Índice

1. [Configuração Inicial](#1-configuração-inicial)
2. [Breakpoints Básicos](#2-breakpoints-básicos)
3. [Breakpoints Avançados](#3-breakpoints-avançados)
4. [Navegação no Código](#4-navegação-no-código)
5. [Inspeção de Variáveis](#5-inspeção-de-variáveis)
6. [Call Stack](#6-call-stack)
7. [Debug Console](#7-debug-console)
8. [Depuração Remota](#8-depuração-remota)
9. [Troubleshooting](#9-troubleshooting)
10. [Melhores Práticas](#10-melhores-práticas)

---

## 1. Configuração Inicial

### 1.1 Extensões Necessárias

Certifique-se de ter instalado:

```bash
# C# Dev Kit (inclui o depurador)
code --install-extension ms-dotnettools.csdevkit

# Ou apenas o depurador C#
code --install-extension ms-dotnettools.csharp
```

### 1.2 Estrutura do Projeto

```
MeuProjeto/
├── .vscode/
│   ├── launch.json        # Configurações de depuração
│   └── tasks.json         # Tarefas de build
├── Program.cs
└── MeuProjeto.csproj
```

### 1.3 Configuração Automática

Quando você abre um projeto .NET, o VS Code geralmente oferece gerar as configurações automaticamente:

1. Abra a pasta do projeto no VS Code
2. Pressione **F5** ou vá em **Run > Start Debugging**
3. Selecione "C#" como ambiente
4. O VS Code criará `.vscode/launch.json` e `.vscode/tasks.json`

### 1.4 launch.json Manual

Se precisar criar manualmente:

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            // Configuração para Console App
            "name": ".NET Core Launch (console)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            
            // Caminho do executável compilado
            "program": "${workspaceFolder}/bin/Debug/net8.0/MeuProjeto.dll",
            
            // Argumentos da linha de comando
            "args": [],
            
            // Diretório de trabalho
            "cwd": "${workspaceFolder}",
            
            // Para no primeiro breakpoint
            "stopAtEntry": false,
            
            // Console interno do VS Code
            "console": "internalConsole",
            
            // Habilita logging detalhado
            "logging": {
                "moduleLoad": false
            },
            
            // Variáveis de ambiente
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        },
        {
            // Configuração para Web App
            "name": ".NET Core Launch (web)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/bin/Debug/net8.0/MeuWebApp.dll",
            "args": [],
            "cwd": "${workspaceFolder}",
            "stopAtEntry": false,
            
            // Abre o navegador automaticamente
            "serverReadyAction": {
                "action": "openExternally",
                "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
            },
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            },
            
            // Console externo (para ver logs do Kestrel)
            "console": "externalTerminal"
        },
        {
            // Anexar a processo em execução
            "name": ".NET Core Attach",
            "type": "coreclr",
            "request": "attach",
            "processId": "${command:pickProcess}"
        }
    ]
}
```

### 1.5 tasks.json

Tarefas de build automático antes da depuração:

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/MeuProjeto.csproj",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile"
        },
        {
            "label": "publish",
            "command": "dotnet",
            "type": "process",
            "args": [
                "publish",
                "${workspaceFolder}/MeuProjeto.csproj",
                "/property:GenerateFullPaths=true",
                "/consoleloggerparameters:NoSummary"
            ],
            "problemMatcher": "$msCompile"
        },
        {
            "label": "watch",
            "command": "dotnet",
            "type": "process",
            "args": [
                "watch",
                "run",
                "--project",
                "${workspaceFolder}/MeuProjeto.csproj"
            ],
            "problemMatcher": "$msCompile"
        }
    ]
}
```

---

## 2. Breakpoints Básicos

### 2.1 Adicionar Breakpoint

**3 maneiras:**

1. **Clique na margem esquerda** (ao lado do número da linha)
2. **F9** com cursor na linha desejada
3. **Menu:** Run > Toggle Breakpoint

```csharp
public class Calculadora
{
    public int Somar(int a, int b)
    {
        // ⬤ Breakpoint aqui (linha 5)
        int resultado = a + b;
        
        // ⬤ Outro breakpoint (linha 8)
        return resultado;
    }
}
```

### 2.2 Tipos de Breakpoints

| Cor | Tipo | Descrição |
|-----|------|-----------|
| 🔴 | Ativo | Depuração vai parar nesta linha |
| ⚪ | Desativado | Temporariamente ignorado |
| ⚫ | Inválido | Código não foi compilado ou não é executável |

### 2.3 Gerenciar Breakpoints

**Painel Breakpoints:**
- View > Debug > Breakpoints (`Ctrl+Shift+D`)

Ações disponíveis:
- ✅ **Enable/Disable:** Ativar/desativar sem remover
- ✅ **Edit:** Adicionar condições
- ❌ **Remove:** Excluir breakpoint
- 🗑️ **Remove All:** Limpar todos

### 2.4 Exemplo Prático

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Iniciando aplicação...");  // ⬤ Breakpoint 1
        
        var numeros = new[] { 1, 2, 3, 4, 5 };
        int soma = 0;
        
        foreach (var numero in numeros)              // ⬤ Breakpoint 2 (em loop)
        {
            soma += numero;
        }
        
        Console.WriteLine($"Soma: {soma}");          // ⬤ Breakpoint 3
    }
}
```

**Quando executar (F5):**
1. Para no Breakpoint 1
2. F5 continua até Breakpoint 2
3. F5 novamente vai para próxima iteração do loop
4. E assim por diante...

---

## 3. Breakpoints Avançados

### 3.1 Conditional Breakpoint

Para apenas quando uma condição é verdadeira:

```csharp
for (int i = 0; i < 100; i++)
{
    ProcessarItem(i);  // ⬤ Condicional: i == 50
}
```

**Como configurar:**
1. Botão direito no breakpoint > Edit Breakpoint
2. Selecione "Expression"
3. Digite: `i == 50`

**Operadores suportados:**
```csharp
// Comparação
x == 10
nome == "João"
saldo > 1000
idade >= 18 && ativo == true

// Chamada de método
lista.Count > 0
usuario.IsAdmin()
string.IsNullOrEmpty(texto)

// Expressões complexas
numeros.Any(n => n > 100)
```

### 3.2 Hit Count Breakpoint

Para apenas na N-ésima execução:

```csharp
while (true)
{
    var dados = ObterProximoItem();  // ⬤ Hit Count: 100
    Processar(dados);
}
```

**Configuração:**
1. Botão direito > Edit Breakpoint
2. Selecione "Hit Count"
3. Digite o número (ex: `100`)

**Operadores:**
- `= 100`: Exatamente na 100ª vez
- `> 100`: Após 100 execuções
- `>= 100`: Na 100ª e seguintes
- `% 10`: A cada 10 execuções (múltiplos)

### 3.3 Logpoint

Imprime mensagem SEM parar a execução:

```csharp
public void ProcessarPedido(Pedido pedido)
{
    // 💬 Logpoint: Processando pedido {pedido.Id} - Total: {pedido.Total}
    ValidarPedido(pedido);
    SalvarNoBanco(pedido);
}
```

**Como criar:**
1. Botão direito > Add Logpoint
2. Digite a mensagem: `Pedido {pedido.Id} - Total: {pedido.Total}`

**Variáveis suportadas:**
```csharp
// Logpoint: Processando cliente {cliente.Nome}, idade {cliente.Idade}, ativo: {cliente.Ativo}

// Saída no Debug Console:
// Processando cliente João Silva, idade 35, ativo: True
```

### 3.4 Data Breakpoint

Para quando o valor de uma variável muda (apenas .NET 5+):

```csharp
public class ContaBancaria
{
    private decimal _saldo = 1000m;  // ⬤ Data Breakpoint
    
    public void Sacar(decimal valor)
    {
        _saldo -= valor;  // Para quando _saldo mudar
    }
}
```

**Como configurar:**
1. Durante depuração, no painel Variables
2. Botão direito na variável > Break When Value Changes

### 3.5 Exception Breakpoint

Para automaticamente quando exceção é lançada:

**Configuração:**
1. Run > New Breakpoint > Exception Breakpoint
2. Digite o tipo: `System.InvalidOperationException`

Ou configure tudo no menu:

```
Run > Exception Breakpoints
```

Opções:
- ✅ **All Exceptions:** Para em qualquer exceção
- ✅ **User-Unhandled:** Apenas exceções não tratadas
- ✅ **Specific Types:** Tipos específicos

### 3.6 Exemplo Completo

```csharp
using System;
using System.Collections.Generic;

class ProcessadorPedidos
{
    static void Main()
    {
        var pedidos = GerarPedidos(100);
        
        int contador = 0;
        foreach (var pedido in pedidos)
        {
            contador++;
            
            // ⬤ Condicional: pedido.Valor > 1000
            // ⬤ Hit Count: % 10 (a cada 10)
            // 💬 Logpoint: Processando pedido #{contador}: R$ {pedido.Valor}
            
            try
            {
                ProcessarPedido(pedido);
            }
            catch (Exception ex)  // ⬤ Exception Breakpoint: InvalidOperationException
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
    
    static void ProcessarPedido(Pedido pedido)
    {
        if (pedido.Valor <= 0)
            throw new InvalidOperationException("Valor inválido");
        
        // Processamento...
    }
    
    static List<Pedido> GerarPedidos(int quantidade)
    {
        var lista = new List<Pedido>();
        var random = new Random();
        
        for (int i = 0; i < quantidade; i++)
        {
            lista.Add(new Pedido 
            { 
                Id = i + 1, 
                Valor = random.Next(100, 2000) 
            });
        }
        
        return lista;
    }
}

record Pedido
{
    public int Id { get; init; }
    public decimal Valor { get; init; }
}
```

---

## 4. Navegação no Código

### 4.1 Controles de Depuração

![Barra de Depuração](../../../assets/step%20into.PNG)

| Botão | Atalho | Nome | Função |
|-------|--------|------|--------|
| ▶️ | **F5** | Continue | Continua até próximo breakpoint |
| ⏭️ | **F10** | Step Over | Executa linha atual (não entra em métodos) |
| ⬇️ | **F11** | Step Into | Entra dentro de métodos |
| ⬆️ | **Shift+F11** | Step Out | Sai do método atual |
| 🔄 | **Ctrl+Shift+F5** | Restart | Reinicia a depuração |
| ⏹️ | **Shift+F5** | Stop | Encerra a depuração |

### 4.2 Step Over (F10)

Executa a linha atual sem entrar em métodos:

```csharp
static void Main()
{
    int a = 10;              // ⬤ Parado aqui
    int b = 20;              // F10 → vai para aqui
    int soma = Somar(a, b);  // F10 → executa Somar() e vai para próxima linha
    Console.WriteLine(soma); // F10 → chega aqui
}

static int Somar(int x, int y)
{
    // NÃO entra aqui com F10
    return x + y;
}
```

**Quando usar:**
- ✅ Método conhecido e confiável
- ✅ Não precisa ver detalhes internos
- ✅ Foco na lógica do método atual

### 4.3 Step Into (F11)

Entra dentro dos métodos:

```csharp
static void Main()
{
    int a = 10;
    int b = 20;
    int soma = Somar(a, b);  // ⬤ F11 aqui
}

static int Somar(int x, int y)
{
    // F11 → ENTRA aqui
    int resultado = x + y;   // ⬤ Para nesta linha
    return resultado;
}
```

**Quando usar:**
- ✅ Investigar bug dentro do método
- ✅ Entender fluxo de execução
- ✅ Verificar valores de parâmetros

### 4.4 Step Out (Shift+F11)

Sai do método atual e volta para quem chamou:

```csharp
static void Main()
{
    Console.WriteLine("Início");
    ProcessarDados();        // ⬤ Volta para aqui após Step Out
    Console.WriteLine("Fim");
}

static void ProcessarDados()
{
    for (int i = 0; i < 100; i++)
    {
        // ⬤ Parado aqui no meio do loop
        // Shift+F11 → sai do método inteiro
        ProcessarItem(i);
    }
}
```

**Quando usar:**
- ✅ Entrou em método por engano
- ✅ Já viu o que precisava
- ✅ Loop muito longo

### 4.5 Continue (F5)

Continua execução até próximo breakpoint:

```csharp
static void Main()
{
    Console.WriteLine("1");  // ⬤ Breakpoint 1
    Console.WriteLine("2");
    Console.WriteLine("3");
    Console.WriteLine("4");  // ⬤ Breakpoint 2
}

// F5 no Breakpoint 1 → vai direto para Breakpoint 2
```

### 4.6 Run to Cursor

Executa até a linha onde está o cursor (sem criar breakpoint):

```csharp
static void Main()
{
    Console.WriteLine("1");  // ⬤ Parado aqui
    Console.WriteLine("2");
    Console.WriteLine("3");
    Console.WriteLine("4");  // ← Cursor aqui
    Console.WriteLine("5");
}

// Botão direito → Run to Cursor
// Ou: Ctrl+F10
```

### 4.7 Set Next Statement

Move o ponto de execução sem executar código intermediário:

```csharp
static void Main()
{
    int x = 10;              // ⬤ Parado aqui
    Console.WriteLine("A");  // ← Pulado
    Console.WriteLine("B");  // ← Pulado
    Console.WriteLine("C");  // ← Mover execução para aqui
}

// Botão direito na linha C → Set Next Statement
// Ctrl+Shift+F10
```

⚠️ **Cuidado:** Pode causar comportamento inesperado!

---

## 5. Inspeção de Variáveis

### 5.1 Painel Variables

![Variables](../../../assets/locals.PNG)

**Locais automáticos:**
- **Locals:** Variáveis do escopo atual
- **Arguments:** Parâmetros do método
- **this:** Objeto atual (em classes)

```csharp
public void ProcessarPedido(Pedido pedido, decimal desconto)
{
    // ⬤ Breakpoint aqui
    
    // No painel Variables:
    // Locals
    //   └─ total = 1500.00
    // Arguments
    //   ├─ pedido = { Id: 123, Valor: 1500.00 }
    //   └─ desconto = 0.10
    // this
    //   └─ _repositorio = { ... }
    
    var total = pedido.Valor * (1 - desconto);
}
```

### 5.2 Expandir Objetos

```csharp
public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public Endereco Endereco { get; set; }
    public List<Pedido> Pedidos { get; set; }
}

// No debugger:
// cliente
//   ├─ Id = 1
//   ├─ Nome = "João Silva"
//   ├─ Endereco
//   │   ├─ Rua = "Av. Paulista"
//   │   ├─ Numero = 1000
//   │   └─ Cidade = "São Paulo"
//   └─ Pedidos (Count = 3)
//       ├─ [0] = { Id: 101, Valor: 250.00 }
//       ├─ [1] = { Id: 102, Valor: 180.50 }
//       └─ [2] = { Id: 103, Valor: 420.00 }
```

### 5.3 Painel Watch

Monitora expressões específicas:

![Watch](../../../assets/watch.PNG)

```csharp
public void CalcularDesconto(decimal valor, decimal taxaDesconto)
{
    // ⬤ Breakpoint
    
    // Adicione ao Watch:
    // valor
    // taxaDesconto
    // valor * taxaDesconto
    // valor - (valor * taxaDesconto)
    // Math.Round(valor * taxaDesconto, 2)
    
    var desconto = valor * taxaDesconto;
    var valorFinal = valor - desconto;
}
```

**Como adicionar:**
1. Painel Watch > ➕
2. Digite a expressão
3. Ou: Selecione variável no código > Botão direito > Add to Watch

### 5.4 Hover sobre Variáveis

Passe o mouse sobre qualquer variável durante depuração:

```csharp
int soma = numeros.Sum();  // Hover sobre 'soma' mostra valor
```

### 5.5 Modificar Valores

Durante depuração, você pode alterar variáveis:

```csharp
public void ValidarIdade(int idade)
{
    // ⬤ Breakpoint aqui com idade = 15
    
    if (idade < 18)
    {
        // Problema: lógica não funciona para menores
    }
}

// No painel Variables:
// 1. Clique em 'idade'
// 2. Altere para 25
// 3. Continue depuração com novo valor
```

⚠️ **Uso:** Testar cenários sem recompilar

### 5.6 Copy Value / Copy as Expression

```csharp
var cliente = new Cliente 
{ 
    Id = 1, 
    Nome = "João", 
    Pedidos = new List<int> { 101, 102, 103 }
};

// Botão direito no Variables:
// • Copy Value → "{ Id = 1, Nome = João, ... }"
// • Copy as Expression → "cliente"
```

---

## 6. Call Stack

![Call Stack](../../../assets/CallStack.PNG)

### 6.1 O que é Call Stack?

Mostra a **cadeia de chamadas** que levou ao ponto atual:

```csharp
static void Main()
{
    ProcessarDados();
}

static void ProcessarDados()
{
    ValidarEntrada();
}

static void ValidarEntrada()
{
    VerificarPermissoes();  // ⬤ Parado aqui
}

static void VerificarPermissoes()
{
    // Execução atual
}

// Call Stack:
// VerificarPermissoes()        ← Topo (linha atual)
// ValidarEntrada()             ← Quem chamou
// ProcessarDados()             ← Quem chamou ValidarEntrada
// Main()                       ← Raiz
```

### 6.2 Navegar no Stack

Clique em qualquer frame para ver:
- Código daquele nível
- Variáveis daquele escopo

```csharp
// Call Stack:
// VerificarPermissoes()  ← Clique aqui
//   ↓ Variáveis: usuario, permissoes
// ValidarEntrada()       ← Ou clique aqui
//   ↓ Variáveis: dados, valido
// ProcessarDados()
//   ↓ Variáveis: arquivo, linhas
```

### 6.3 Async Call Stack

Com código assíncrono, o stack pode ser mais complexo:

```csharp
static async Task Main()
{
    await ProcessarAsync();
}

static async Task ProcessarAsync()
{
    await Task.Delay(1000);
    await BuscarDadosAsync();  // ⬤ Breakpoint
}

static async Task<string> BuscarDadosAsync()
{
    // Call Stack Assíncrono:
    // BuscarDadosAsync()              ← Atual
    // ProcessarAsync()                ← Aguardando
    // Main()                          ← Início
    // [Async] ...                     ← Internos do runtime
}
```

### 6.4 Filtrar Frames

Ocultar frames do sistema/bibliotecas:

```
Call Stack (clique no ⚙️ Settings)
☑️ Show External Code
☐ Show File Names
☐ Show Function Names
```

---

## 7. Debug Console

![Debug Console](../../../assets/Imediate.PNG)

### 7.1 Avaliar Expressões

Durante depuração, execute código no contexto atual:

```csharp
public void ProcessarLista(List<int> numeros)
{
    // ⬤ Breakpoint aqui
    
    var soma = numeros.Sum();
}

// No Debug Console (Ctrl+Shift+Y):
> numeros.Count
5

> numeros.Max()
100

> numeros.Where(n => n > 50).ToList()
[75, 80, 100]

> Math.Sqrt(numeros.Sum())
14.142135623730951
```

### 7.2 Chamar Métodos

```csharp
public class CalculadoraService
{
    public decimal CalcularDesconto(decimal valor, decimal taxa)
    {
        // ⬤ Breakpoint
        return valor * taxa;
    }
    
    private decimal ObterTaxaPremium()
    {
        return 0.15m;
    }
}

// Debug Console:
> CalcularDesconto(1000, 0.10)
100.0

> ObterTaxaPremium()
0.15

> this
{ CalculadoraService }
```

### 7.3 Modificar Estado

```csharp
public class ContaBancaria
{
    private decimal _saldo = 1000m;
    
    public void Sacar(decimal valor)
    {
        // ⬤ Breakpoint antes da validação
        
        if (valor > _saldo)
            throw new InvalidOperationException("Saldo insuficiente");
    }
}

// Debug Console:
> _saldo
1000.0

> _saldo = 5000m  // Modifica para testar cenário
5000.0

// Agora F5 não vai lançar exceção
```

### 7.4 Logs e Mensagens

```csharp
// Debug Console mostra:
// • Console.WriteLine()
// • Debug.WriteLine()
// • Trace.WriteLine()
// • Logpoints
// • Exceções não tratadas

foreach (var item in lista)
{
    Console.WriteLine($"Processando {item}");  // Aparece no Console
}
```

---

## 8. Depuração Remota

### 8.1 Attach to Process

Anexar a processo já em execução:

```bash
# Terminal 1: Iniciar aplicação
dotnet run

# Terminal 2: Obter PID
dotnet tool install -g dotnet-dump
dotnet dump ps

# VS Code:
# 1. Run > Attach to Process (Ctrl+Shift+P)
# 2. Selecione o processo
# 3. Breakpoints agora funcionam
```

**launch.json para attach:**

```json
{
    "name": ".NET Core Attach",
    "type": "coreclr",
    "request": "attach",
    "processId": "${command:pickProcess}"
}
```

### 8.2 Depuração de Container Docker

```dockerfile
# Dockerfile.debug
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS debug
WORKDIR /app
COPY . .
RUN dotnet build

# Expõe porta do depurador
EXPOSE 5000
EXPOSE 5001

# Inicia com símbolos de depuração
ENTRYPOINT ["dotnet", "run", "--no-build"]
```

**launch.json para Docker:**

```json
{
    "name": "Docker .NET Core Launch",
    "type": "coreclr",
    "request": "launch",
    "preLaunchTask": "docker-build",
    "program": "/app/bin/Debug/net8.0/MeuApp.dll",
    "cwd": "/app",
    "stopAtEntry": false,
    "console": "internalConsole",
    
    // Configuração Docker
    "pipeTransport": {
        "pipeCwd": "${workspaceFolder}",
        "pipeProgram": "docker",
        "pipeArgs": [
            "exec",
            "-i",
            "meu-container"
        ],
        "debuggerPath": "/vsdbg/vsdbg",
        "quoteArgs": false
    }
}
```

### 8.3 Remote Debugging (SSH)

Para depurar em servidor remoto via SSH:

```json
{
    "name": "Remote SSH Debug",
    "type": "coreclr",
    "request": "launch",
    "program": "/home/user/app/MeuApp.dll",
    "cwd": "/home/user/app",
    
    "pipeTransport": {
        "pipeCwd": "${workspaceFolder}",
        "pipeProgram": "ssh",
        "pipeArgs": [
            "-T",
            "user@servidor.com"
        ],
        "debuggerPath": "~/vsdbg/vsdbg"
    }
}
```

**Instalar vsdbg no servidor:**

```bash
# No servidor remoto
curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg
```

---

## 9. Troubleshooting

### 9.1 Breakpoints Não Param

**Problema:** Círculos vazios ⚪ em vez de cheios 🔴

**Soluções:**

```json
// launch.json - Habilitar símbolos de depuração
{
    "justMyCode": false,  // Permite depurar código externo
    "suppressJITOptimizations": true,  // Desabilita otimizações JIT
    "enableStepFiltering": false  // Permite step em código filtrado
}
```

```xml
<!-- .csproj - Gerar símbolos em Release -->
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
    <DebugType>full</DebugType>
    <DebugSymbols>true</DebugSymbols>
</PropertyGroup>
```

### 9.2 "Cannot find program" Error

```json
// Verifique o caminho no launch.json
"program": "${workspaceFolder}/bin/Debug/net8.0/MeuProjeto.dll",

// Se nome do projeto mudou, atualize aqui
```

### 9.3 Variables Mostram "Cannot evaluate"

```csharp
// Pode ocorrer com:
// • Código otimizado
// • Variáveis descartadas pelo compilador
// • Async/await complexo

// Solução:
#pragma warning disable CS0219 // Variable is assigned but never used
int debug = 0;  // Força compilador manter variável
#pragma warning restore CS0219
```

### 9.4 Símbolos Não Carregados

```
Módulo 'MeuProjeto.dll' carregado sem símbolos
```

**Soluções:**

```bash
# 1. Limpar e recompilar
dotnet clean
dotnet build

# 2. Verificar .pdb gerado
ls bin/Debug/net8.0/*.pdb

# 3. Verificar .csproj
```

```xml
<PropertyGroup>
    <DebugType>portable</DebugType>  <!-- ou 'full' -->
</PropertyGroup>
```

---

## 10. Melhores Práticas

### ✅ DO: Use Breakpoints Condicionais

```csharp
// ❌ NÃO faça assim
foreach (var item in lista)
{
    if (item.Id == 12345)  // Código só para debug
    {
        // Breakpoint aqui
    }
}

// ✅ FAÇA assim
foreach (var item in lista)
{
    // ⬤ Breakpoint Condicional: item.Id == 12345
    ProcessarItem(item);
}
```

### ✅ DO: Use Logpoints em Loops

```csharp
// ❌ NÃO: Breakpoint normal para cada iteração
for (int i = 0; i < 1000; i++)
{
    // ⬤ Para 1000 vezes!
    ProcessarItem(i);
}

// ✅ FAÇA: Logpoint para registrar sem parar
for (int i = 0; i < 1000; i++)
{
    // 💬 Logpoint: "Item {i} processado"
    ProcessarItem(i);
}
```

### ✅ DO: Nomeie Configurações Claramente

```json
{
    "configurations": [
        {
            "name": "Debug - API Local",  // ✅ Claro
            // ...
        },
        {
            "name": "Debug - API Docker",  // ✅ Específico
            // ...
        },
        {
            "name": ".NET Core Launch (web)",  // ❌ Genérico
            // ...
        }
    ]
}
```

### ✅ DO: Use Watch para Expressões Complexas

```csharp
public void ProcessarPedidos(List<Pedido> pedidos)
{
    // Watch:
    // pedidos.Count
    // pedidos.Sum(p => p.Valor)
    // pedidos.Where(p => p.Status == "Pendente").Count()
    // pedidos.Average(p => p.Valor)
}
```

### ⚠️ DON'T: Modifique Estado Sem Necessidade

```csharp
// ❌ Perigoso: modificar sem entender impacto
// Debug Console:
> _usuario.Permissoes = "Admin"  // Pode quebrar lógica

// ✅ Melhor: Apenas inspecione
> _usuario.Permissoes
"User"
```

### ✅ DO: Documente Configurações Especiais

```json
{
    "name": "Debug - Com Seed de Dados",
    "env": {
        // Cria dados de teste automaticamente
        "SEED_DATA": "true",
        "DB_CONNECTION": "Server=localhost;Database=TestDB"
    }
}
```

---

## 📚 Recursos Adicionais

### Documentação Oficial

- [VS Code Debugging](https://code.visualstudio.com/docs/editor/debugging)
- [C# Debugging](https://code.visualstudio.com/docs/languages/csharp)
- [Launch.json Reference](https://code.visualstudio.com/docs/cpp/launch-json-reference)

### Atalhos Essenciais

| Ação | Windows/Linux | macOS |
|------|---------------|-------|
| Start Debugging | F5 | F5 |
| Start Without Debugging | Ctrl+F5 | Cmd+F5 |
| Stop | Shift+F5 | Shift+F5 |
| Restart | Ctrl+Shift+F5 | Cmd+Shift+F5 |
| Step Over | F10 | F10 |
| Step Into | F11 | F11 |
| Step Out | Shift+F11 | Shift+F11 |
| Continue | F5 | F5 |
| Toggle Breakpoint | F9 | F9 |
| Debug Console | Ctrl+Shift+Y | Cmd+Shift+Y |

---

## 🎓 Resumo

Você aprendeu:

1. **Configurar** depurador para diferentes projetos (.NET, Web, Docker)
2. **Usar breakpoints** (básicos, condicionais, logpoints, hit count)
3. **Navegar** pelo código (Step Into/Over/Out, Run to Cursor)
4. **Inspecionar** variáveis (Locals, Watch, hover, modificar valores)
5. **Analisar** Call Stack para entender fluxo de execução
6. **Usar** Debug Console para avaliar expressões
7. **Depurar remotamente** (attach, Docker, SSH)
8. **Resolver** problemas comuns (símbolos, breakpoints, paths)
9. **Aplicar** melhores práticas de depuração

**Próximo tópico:** Logging e Rastreamento com Serilog e Application Insights
