# 📘 Introdução ao C# e .NET

## 🎯 Objetivos de Aprendizado

Ao final desta seção, você será capaz de:
- Compreender a história e evolução do .NET
- Entender a arquitetura do .NET moderno (.NET 8/9)
- Conhecer o ecossistema de ferramentas .NET
- Configurar seu ambiente de desenvolvimento
- Criar e executar seu primeiro programa em C#

---

## 📖 História do .NET: Da Evolução à Modernidade

### 🌟 A Origem (2002)

O **.NET Framework** foi lançado pela Microsoft em 2002 como uma plataforma robusta para desenvolvimento de aplicações Windows. Foi uma revolução que trouxe:

- **Gerenciamento automático de memória** (Garbage Collection)
- **Sistema de tipos comum** (Common Type System - CTS)
- **Múltiplas linguagens** rodando na mesma plataforma
- **Bibliotecas ricas e extensas** (BCL - Base Class Library)

### 🔄 A Grande Mudança: .NET Core (2016)

Em 2016, a Microsoft lançou o **.NET Core**, uma reescrita completa e moderna do .NET Framework, com foco em:

- ✅ **Cross-platform**: Windows, Linux e macOS
- ✅ **Open Source**: Código aberto no GitHub
- ✅ **Alto desempenho**: Otimizado para aplicações modernas
- ✅ **Modular**: Apenas o que você precisa via NuGet
- ✅ **Cloud-ready**: Perfeito para containers e microserviços

### 🚀 .NET Unificado (.NET 5+)

Em 2020, a Microsoft unificou tudo em uma única plataforma: **.NET 5** (pulando a versão 4 para evitar confusão com .NET Framework 4.x).

**Linha do tempo:**
```
2002 ─── .NET Framework 1.0
2016 ─── .NET Core 1.0 (Nova era)
2020 ─── .NET 5 (Unificação)
2021 ─── .NET 6 
2022 ─── .NET 7
2023 ─── .NET 8 
2024 ─── .NET 9 
...
```

> **💡 Nota**: Versões LTS (Long Term Support) têm suporte por 3 anos. Use .NET 8 para projetos de produção.

---

## 🏗️ Arquitetura do .NET Moderno

### Componentes Principais

```
┌─────────────────────────────────────────────────────┐
│           Suas Aplicações (C#, F#, VB)              │
├─────────────────────────────────────────────────────┤
│  ASP.NET Core │ Windows Forms │ WPF │ MAUI │ Unity │
├─────────────────────────────────────────────────────┤
│        Bibliotecas Base do .NET (BCL)               │
├─────────────────────────────────────────────────────┤
│      Runtime (.NET CLR - Common Language Runtime)   │
├─────────────────────────────────────────────────────┤
│    Sistema Operacional (Windows│Linux│macOS)       │
└─────────────────────────────────────────────────────┘
```

### 🔧 CLR - Common Language Runtime

O **CLR** é o coração do .NET. Ele é responsável por:

1. **Compilação JIT (Just-In-Time)**
   ```
   Código C# → Compilador → IL (Intermediate Language) → JIT → Código Nativo
   ```

2. **Gerenciamento de Memória**
   - Garbage Collection automático
   - Gerenciamento de heap e stack

3. **Segurança**
   - Code Access Security
   - Type Safety (segurança de tipos)

4. **Tratamento de Exceções**
   - Mecanismo unificado de exceções

### 📦 Ecossistema .NET

| Componente | Descrição | Uso |
|------------|-----------|-----|
| **SDK** | Ferramentas de desenvolvimento | Compilar, executar, publicar |
| **Runtime** | Executa aplicações .NET | Necessário em produção |
| **NuGet** | Gerenciador de pacotes | Bibliotecas de terceiros |
| **dotnet CLI** | Interface de linha de comando | Criar projetos, restaurar pacotes |
| **Visual Studio** | IDE completa | Desenvolvimento profissional |
| **VS Code** | Editor leve com C# DevKit | Alternativa multiplataforma |
| **Rider** | IDE da JetBrains | Alternativa premium |

---

## 🛠️ Configurando o Ambiente de Desenvolvimento

### Passo 1: Instalar o .NET SDK

#### Windows
```powershell
# Baixe e instale de:
# https://dotnet.microsoft.com/download

# Verifique a instalação
dotnet --version
```

#### macOS
```bash
# Via Homebrew
brew install dotnet

# Ou baixe de:
# https://dotnet.microsoft.com/download

# Verifique
dotnet --version
```

#### Linux (Ubuntu/Debian)
```bash
# Adicione o repositório Microsoft
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Instale o SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0

# Verifique
dotnet --version
```

### Passo 2: Escolher e Configurar a IDE

#### Opção 1: Visual Studio Code (Recomendado para iniciantes)

1. **Instale o VS Code**: https://code.visualstudio.com/
2. **Instale a extensão C# DevKit**:
   - Abra o VS Code
   - Vá em Extensions (Ctrl+Shift+X)
   - Procure por "C# Dev Kit"
   - Clique em Install

#### Opção 2: Visual Studio (Windows)

1. Baixe o **Visual Studio Community** (gratuito): https://visualstudio.microsoft.com/
2. Durante a instalação, selecione:
   - "Desenvolvimento para desktop com .NET"
   - "Desenvolvimento Web e ASP.NET"

### Passo 3: Verificar a Instalação

```bash
# Ver versão do .NET
dotnet --version

# Ver informações detalhadas
dotnet --info

# Listar SDKs instalados
dotnet --list-sdks

# Listar runtimes instalados
dotnet --list-runtimes
```

---

## 👨‍💻 Seu Primeiro Programa em C#

### Método 1: Usando o Terminal (dotnet CLI)

```bash
# 1. Criar um novo projeto
dotnet new console -n MeuPrimeiroProjeto

# 2. Navegar até a pasta
cd MeuPrimeiroProjeto

# 3. Ver o conteúdo criado
ls  # ou dir no Windows
```

Você verá estes arquivos:
```
MeuPrimeiroProjeto/
├── Program.cs           ← Seu código C#
├── MeuPrimeiroProjeto.csproj  ← Configuração do projeto
└── obj/                 ← Arquivos temporários
```

### 4. Entendendo o Program.cs

```csharp
// Program.cs (C# 10+)
// Top-level statements - código mais limpo e direto!

Console.WriteLine("Hello, World!");
```

> **💡 Novidade**: A partir do C# 10, não é mais necessário escrever `class Program` e `static void Main`. O compilador faz isso automaticamente!

### 5. Executar o Programa

```bash
dotnet run
```

**Saída:**
```
Hello, World!
```

---

## 🎨 Anatomia de um Programa C#

### Versão Tradicional (C# 9 e anterior)

```csharp
using System;

namespace MeuPrimeiroProjeto
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
```

### Versão Moderna (C# 10+)

```csharp
// Top-level statements
Console.WriteLine("Hello, World!");
```

### Comparação dos Elementos

| Elemento | Tradicional | Moderno (C# 10+) | Descrição |
|----------|-------------|------------------|-----------|
| **using** | Necessário | Implícito | Abrevia namespaces |
| **namespace** | Obrigatório | Opcional | Organiza código |
| **class** | Obrigatório | Gerado automaticamente | Define uma classe |
| **Main** | Obrigatório | Gerado automaticamente | Ponto de entrada |

---

## 🔍 Entendendo o Projeto .csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

**Explicação:**

| Propriedade | Significado |
|-------------|-------------|
| `OutputType` | Tipo de saída (Exe = executável, Library = DLL) |
| `TargetFramework` | Versão do .NET (net8.0, net9.0, etc.) |
| `ImplicitUsings` | Importações automáticas de namespaces comuns |
| `Nullable` | Habilita verificação de referências nulas (C# 8+) |

---

## 🚀 Comandos Essenciais do dotnet CLI

### Criação de Projetos

```bash
# Console application
dotnet new console -n NomeDoProjeto

# Web API
dotnet new webapi -n MinhaAPI

# Class Library
dotnet new classlib -n MinhaLib

# Blazor Web App
dotnet new blazor -n MeuBlazor

# Ver todos os templates disponíveis
dotnet new list
```

### Gerenciamento do Projeto

```bash
# Restaurar pacotes NuGet
dotnet restore

# Compilar o projeto
dotnet build

# Executar o projeto
dotnet run

# Executar com argumentos
dotnet run -- arg1 arg2

# Limpar arquivos de build
dotnet clean

# Publicar aplicação para produção
dotnet publish -c Release
```

### Gerenciamento de Pacotes

```bash
# Adicionar pacote NuGet
dotnet add package NomeDoPacote

# Adicionar versão específica
dotnet add package Newtonsoft.Json --version 13.0.3

# Remover pacote
dotnet remove package NomeDoPacote

# Listar pacotes do projeto
dotnet list package
```

---

## 💡 Conceitos Importantes

### 1. Compilação e Execução

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│  Código C#   │ ──> │      IL      │ ──> │ Código Nativo│
│ (Program.cs) │     │(Intermediário│     │   (CPU)      │
└──────────────┘     └──────────────┘     └──────────────┘
   Compilação           Executável          JIT Compilation
   (dotnet build)    (.dll ou .exe)        (Runtime)
```

### 2. Garbage Collection

O .NET gerencia a memória automaticamente:

```csharp
// Você cria objetos livremente
var pessoa = new Pessoa();
var lista = new List<int>();

// O Garbage Collector libera memória automaticamente
// quando os objetos não são mais usados
```

### 3. Namespaces

Organizam o código em hierarquias lógicas:

```csharp
// Definindo namespace
namespace MeuProjeto.Modelos
{
    public class Usuario
    {
        public string Nome { get; set; }
    }
}

// Usando namespace
using MeuProjeto.Modelos;

var usuario = new Usuario { Nome = "João" };
```

---

## 📚 Estrutura de um Projeto .NET

```
MeuProjeto/
├── MeuProjeto.sln              ← Solution file (agrupa projetos)
├── src/                        ← Código fonte
│   ├── MeuProjeto.Api/        ← Projeto da API
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Program.cs
│   │   └── MeuProjeto.Api.csproj
│   └── MeuProjeto.Core/       ← Projeto de biblioteca
│       ├── Entities/
│       ├── Interfaces/
│       └── MeuProjeto.Core.csproj
├── tests/                      ← Testes
│   └── MeuProjeto.Tests/
│       └── MeuProjeto.Tests.csproj
├── .gitignore                  ← Arquivos ignorados pelo Git
└── README.md                   ← Documentação
```

---

## 🎓 Resumo dos Pontos-Chave

✅ **.NET é multiplataforma** (Windows, Linux, macOS)  
✅ **.NET 8 é LTS** (use em produção)  
✅ **CLR gerencia memória** automaticamente  
✅ **dotnet CLI** é sua ferramenta principal  
✅ **C# 10+ usa top-level statements** (código mais limpo)  
✅ **NuGet gerencia pacotes** de terceiros  
✅ **Visual Studio Code** é ótimo para começar  

---

## 🔗 Recursos Adicionais

- **Documentação Oficial**: https://learn.microsoft.com/dotnet
- **C# Documentation**: https://learn.microsoft.com/dotnet/csharp
- **GitHub do .NET**: https://github.com/dotnet
- **NuGet Gallery**: https://www.nuget.org
- **Roadmap .NET**: https://github.com/dotnet/core/tree/main/roadmap

---

## ⏭️ Próximos Passos

Agora que você entende o ecossistema .NET, vamos aprender sobre:
- Sintaxe básica do C#
- Tipos de dados e variáveis
- Operadores e expressões
- Estruturas de controle

**Continue para**: `02-sintaxe-basica/01-conteudo.md`
