# 📘 Construtores e Sobrecarga

## 🎯 Objetivos

✅ Entender construtores e sua importância  
✅ Criar construtores parametrizados  
✅ Implementar constructor overloading  
✅ Usar constructor chaining  
✅ Trabalhar com inicializadores de objetos  
✅ Usar optional parameters e named arguments  
✅ Implementar method overloading  
✅ Conhecer primary constructors (C# 12)  

---

## 🏗️ O que são Construtores?

**Construtor** é um método especial que é chamado automaticamente quando um objeto é criado. Ele **inicializa** o estado do objeto.

### Características:
- Mesmo nome da classe
- Não tem tipo de retorno (nem void)
- Pode ter parâmetros
- Pode ser sobrecarregado (overloading)

```csharp
public class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    
    // Construtor padrão (sem parâmetros)
    public Pessoa()
    {
        Nome = "Sem nome";
        Idade = 0;
        Console.WriteLine("Construtor padrão chamado!");
    }
    
    // Construtor parametrizado
    public Pessoa(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
        Console.WriteLine($"Construtor parametrizado chamado: {nome}");
    }
}

// Uso:
var p1 = new Pessoa();                    // Chama construtor padrão
var p2 = new Pessoa("João", 25);          // Chama construtor parametrizado
```

---

## 🔧 Tipos de Construtores

### 1. Construtor Padrão (Default Constructor)

Se você **não declarar nenhum construtor**, o C# cria um automaticamente:

```csharp
public class Produto
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    
    // C# cria automaticamente:
    // public Produto() { }
}

var produto = new Produto();  // ✅ Funciona
```

⚠️ **IMPORTANTE**: Se você criar qualquer construtor, o padrão NÃO é criado automaticamente!

```csharp
public class Produto
{
    public string Nome { get; set; }
    
    // Criou um construtor com parâmetro
    public Produto(string nome)
    {
        Nome = nome;
    }
}

// var p = new Produto();  // ❌ ERRO! Não existe construtor sem parâmetros
var p = new Produto("TV");  // ✅ OK
```

### 2. Construtor Parametrizado

Aceita parâmetros para inicializar o objeto:

```csharp
public class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int Paginas { get; set; }
    
    public Livro(string titulo, string autor, int paginas)
    {
        Titulo = titulo;
        Autor = autor;
        Paginas = paginas;
    }
}

var livro = new Livro("Clean Code", "Robert Martin", 464);
```

### 3. Constructor Overloading (Sobrecarga)

Múltiplos construtores com **diferentes parâmetros**:

```csharp
public class ContaBancaria
{
    public string Titular { get; set; }
    public decimal Saldo { get; private set; }
    
    // Construtor 1: Sem parâmetros
    public ContaBancaria()
    {
        Titular = "Não definido";
        Saldo = 0;
    }
    
    // Construtor 2: Só titular
    public ContaBancaria(string titular)
    {
        Titular = titular;
        Saldo = 0;
    }
    
    // Construtor 3: Titular e saldo inicial
    public ContaBancaria(string titular, decimal saldoInicial)
    {
        Titular = titular;
        Saldo = saldoInicial;
    }
}

// Uso - escolha o construtor adequado:
var conta1 = new ContaBancaria();
var conta2 = new ContaBancaria("João");
var conta3 = new ContaBancaria("Maria", 1000);
```

### 4. Constructor Chaining (Encadeamento)

Um construtor chama outro usando `: this(...)`:

```csharp
public class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    
    // Construtor principal (mais completo)
    public Pessoa(string nome, int idade, string email)
    {
        Nome = nome;
        Idade = idade;
        Email = email;
    }
    
    // Chama o principal com email padrão
    public Pessoa(string nome, int idade)
        : this(nome, idade, "nao@informado.com")
    {
    }
    
    // Chama o anterior com idade padrão
    public Pessoa(string nome)
        : this(nome, 0)
    {
    }
    
    // Chama o anterior com nome padrão
    public Pessoa()
        : this("Sem nome")
    {
    }
}

// Todos funcionam:
var p1 = new Pessoa();
var p2 = new Pessoa("João");
var p3 = new Pessoa("Maria", 25);
var p4 = new Pessoa("Pedro", 30, "pedro@email.com");
```

**Benefícios do Chaining:**
- ✅ Evita duplicação de código
- ✅ Centraliza lógica de inicialização
- ✅ Facilita manutenção

---

## 🎨 Inicializadores de Objetos (Object Initializers)

C# permite inicializar properties diretamente na criação:

```csharp
public class Carro
{
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Ano { get; set; }
    public string Cor { get; set; }
}

// Sem inicializador (tradicional):
var carro1 = new Carro();
carro1.Marca = "Toyota";
carro1.Modelo = "Corolla";
carro1.Ano = 2024;
carro1.Cor = "Prata";

// Com inicializador (moderno):
var carro2 = new Carro
{
    Marca = "Honda",
    Modelo = "Civic",
    Ano = 2024,
    Cor = "Preto"
};

// Pode combinar com construtor:
public class Carro
{
    public Carro(string marca, string modelo)
    {
        Marca = marca;
        Modelo = modelo;
    }
    
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Ano { get; set; }
}

var carro3 = new Carro("Ford", "Fusion")
{
    Ano = 2024  // Inicializa property adicional
};
```

---

## ⚙️ Optional Parameters (Parâmetros Opcionais)

C# permite parâmetros com valores padrão:

```csharp
public class Produto
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public string Categoria { get; set; }
    public bool Ativo { get; set; }
    
    // Parâmetros opcionais (têm valores padrão)
    public Produto(string nome, decimal preco, 
                   string categoria = "Geral", bool ativo = true)
    {
        Nome = nome;
        Preco = preco;
        Categoria = categoria;
        Ativo = ativo;
    }
}

// Uso:
var p1 = new Produto("Notebook", 3000);                    // usa padrões
var p2 = new Produto("Mouse", 50, "Informática");          // sobrescreve categoria
var p3 = new Produto("Teclado", 150, "Informática", false); // sobrescreve tudo
```

**Regras:**
- Parâmetros opcionais devem vir **depois** dos obrigatórios
- Devem ter valor padrão (literal ou `default`)

```csharp
// ❌ ERRADO:
public Classe(int opcional = 0, string obrigatorio)  // Erro!

// ✅ CORRETO:
public Classe(string obrigatorio, int opcional = 0)  // OK!
```

---

## 🏷️ Named Arguments (Argumentos Nomeados)

Permite especificar argumentos pelo nome, não pela posição:

```csharp
public class Configuracao
{
    public Configuracao(string host, int port, string user, string password,
                       bool ssl = false, int timeout = 30)
    {
        // ...
    }
}

// Sem named arguments (confuso):
var config1 = new Configuracao("localhost", 5432, "admin", "pass", true, 60);

// Com named arguments (claro):
var config2 = new Configuracao(
    host: "localhost",
    port: 5432,
    user: "admin",
    password: "pass",
    ssl: true,
    timeout: 60
);

// Pode pular opcionais:
var config3 = new Configuracao(
    host: "localhost",
    port: 5432,
    user: "admin",
    password: "pass",
    ssl: true  // timeout usa o padrão (30)
);

// Pode mudar a ordem (com named):
var config4 = new Configuracao(
    password: "pass",
    user: "admin",
    port: 5432,
    host: "localhost"
);
```

---

## 🔄 Method Overloading (Sobrecarga de Métodos)

Múltiplos métodos com o **mesmo nome** mas **parâmetros diferentes**:

```csharp
public class Calculadora
{
    // Overload 1: Dois inteiros
    public int Somar(int a, int b)
    {
        return a + b;
    }
    
    // Overload 2: Três inteiros
    public int Somar(int a, int b, int c)
    {
        return a + b + c;
    }
    
    // Overload 3: Dois doubles
    public double Somar(double a, double b)
    {
        return a + b;
    }
    
    // Overload 4: Array de inteiros
    public int Somar(params int[] numeros)
    {
        return numeros.Sum();
    }
}

var calc = new Calculadora();
calc.Somar(5, 10);              // Chama overload 1
calc.Somar(5, 10, 15);          // Chama overload 2
calc.Somar(5.5, 10.5);          // Chama overload 3
calc.Somar(1, 2, 3, 4, 5, 6);   // Chama overload 4
```

**O que diferencia overloads:**
- ✅ Número de parâmetros
- ✅ Tipo dos parâmetros
- ✅ Ordem dos parâmetros
- ❌ **NÃO** tipo de retorno

```csharp
// ❌ ERRO - só difere no retorno:
public int Calcular(int x) { return x * 2; }
public double Calcular(int x) { return x * 2.0; }  // ERRO!

// ✅ OK - difere nos parâmetros:
public int Calcular(int x) { return x * 2; }
public double Calcular(double x) { return x * 2.0; }  // OK!
```

---

## 🆕 Primary Constructors (C# 12)

Nova sintaxe concisa para construtores simples:

```csharp
// ANTES (C# 11):
public class Pessoa
{
    public string Nome { get; }
    public int Idade { get; }
    
    public Pessoa(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
    }
}

// AGORA (C# 12):
public class Pessoa(string nome, int idade)
{
    public string Nome { get; } = nome;
    public int Idade { get; } = idade;
}

// Ou ainda mais simples:
public class Pessoa(string Nome, int Idade);  // Record-like

// Com lógica adicional:
public class ContaBancaria(string titular, decimal saldoInicial)
{
    public string Titular { get; } = titular;
    public decimal Saldo { get; private set; } = saldoInicial;
    
    public void Depositar(decimal valor)
    {
        if (valor > 0)
            Saldo += valor;
    }
}
```

---

## 💡 Exemplo Completo

```csharp
public class Funcionario
{
    // Properties
    public string Nome { get; set; }
    public string CPF { get; }
    public decimal Salario { get; private set; }
    public DateTime DataAdmissao { get; }
    public string Departamento { get; set; }
    
    // Construtor principal
    public Funcionario(string nome, string cpf, decimal salario, 
                      DateTime dataAdmissao, string departamento = "Geral")
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório");
        
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF é obrigatório");
        
        if (salario < 0)
            throw new ArgumentException("Salário não pode ser negativo");
        
        Nome = nome;
        CPF = cpf;
        Salario = salario;
        DataAdmissao = dataAdmissao;
        Departamento = departamento;
    }
    
    // Constructor chaining - admissão hoje
    public Funcionario(string nome, string cpf, decimal salario, string departamento = "Geral")
        : this(nome, cpf, salario, DateTime.Today, departamento)
    {
    }
    
    // Constructor chaining - departamento e data padrão
    public Funcionario(string nome, string cpf, decimal salario)
        : this(nome, cpf, salario, DateTime.Today, "Geral")
    {
    }
    
    // Método com overload
    public void AumentarSalario(decimal valor)
    {
        if (valor > 0)
            Salario += valor;
    }
    
    public void AumentarSalario(double percentual)
    {
        if (percentual > 0 && percentual <= 100)
            Salario *= (1 + (decimal)percentual / 100);
    }
    
    public void ExibirDados()
    {
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"CPF: {CPF}");
        Console.WriteLine($"Salário: R$ {Salario:F2}");
        Console.WriteLine($"Admissão: {DataAdmissao:dd/MM/yyyy}");
        Console.WriteLine($"Departamento: {Departamento}");
    }
}

// Uso:
var f1 = new Funcionario("João Silva", "123.456.789-00", 5000);
var f2 = new Funcionario("Maria Santos", "987.654.321-00", 6000, "TI");
var f3 = new Funcionario("Pedro Costa", "111.222.333-44", 4500, 
                        new DateTime(2020, 1, 15), "RH");

f1.AumentarSalario(500);    // Aumenta R$ 500
f2.AumentarSalario(10);     // Aumenta 10%
```

---

## 📚 Resumo

✅ **Construtores** inicializam objetos  
✅ **Overloading** permite múltiplos construtores  
✅ **Chaining** evita duplicação com `: this()`  
✅ **Object Initializers** simplificam inicialização  
✅ **Optional Parameters** dão flexibilidade  
✅ **Named Arguments** melhoram legibilidade  
✅ **Method Overloading** adapta comportamento  
✅ **Primary Constructors** (C# 12) reduzem boilerplate  

**Próximo tópico**: Referências vs Valores! 🚀
