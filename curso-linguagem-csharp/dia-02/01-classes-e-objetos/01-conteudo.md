# 📘 Classes e Objetos

## 🎯 Objetivos

Ao final deste tópico, você será capaz de:
- Entender classes e objetos
- Declarar e usar classes
- Trabalhar com propriedades e métodos
- Aplicar encapsulamento
- Usar modificadores de acesso

---

## 📚 O que é POO?

**POO** organiza código em **objetos** que representam entidades reais.

### 🌍 Analogia: Carro

```csharp
public class Carro
{
    // Propriedades (dados)
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Velocidade { get; private set; }

    // Métodos (ações)
    public void Acelerar(int incremento)
    {
        Velocidade += incremento;
        Console.WriteLine($"Velocidade: {Velocidade} km/h");
    }
}

// Criando objeto
Carro meuCarro = new Carro { Marca = "Toyota", Modelo = "Corolla" };
meuCarro.Acelerar(50); // Velocidade: 50 km/h
```

---

## 🏗️ Estrutura de uma Classe

```csharp
public class Pessoa
{
    // Fields (privados)
    private string _nome;
    private int _idade;

    // Properties (acesso controlado)
    public string Nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public int Idade
    {
        get { return _idade; }
        set
        {
            if (value >= 0 && value <= 150)
                _idade = value;
        }
    }

    // Auto-property (simplificada)
    public string Email { get; set; }

    // Métodos
    public void Apresentar()
    {
        Console.WriteLine($"Olá, sou {Nome}, {Idade} anos");
    }
}
```

### Usando a Classe

```csharp
Pessoa pessoa = new Pessoa();
pessoa.Nome = "João";
pessoa.Idade = 25;
pessoa.Email = "joao@email.com";
pessoa.Apresentar(); // Olá, sou João, 25 anos
```

---

## 🔑 Fields vs Properties

### Fields (Campos)
- Variáveis privadas da classe
- Convenção: `_nomeCampo`

### Properties (Propriedades)
- Acesso controlado aos dados
- Podem ter validação
- Sintaxe: `get` e `set`

```csharp
public class Produto
{
    private decimal _preco;

    public decimal Preco
    {
        get { return _preco; }
        set
        {
            if (value < 0) throw new ArgumentException("Preço inválido");
            _preco = value;
        }
    }
}
```

---

## � Modificadores de Acesso

| Modificador | Classe | Assembly | Herança | Externo |
|-------------|--------|----------|---------|---------|
| `public` | ✅ | ✅ | ✅ | ✅ |
| `private` | ✅ | ❌ | ❌ | ❌ |
| `protected` | ✅ | ❌ | ✅ | ❌ |
| `internal` | ✅ | ✅ | ❌ | ❌ |

**Regra geral**: Use `private` por padrão!

---

## 📝 Métodos

### Sintaxe Básica
```csharp
public void MetodoSimples()
{
    Console.WriteLine("Olá!");
}

public int Somar(int a, int b)
{
    return a + b;
}

public void MetodoComParametros(string nome, int idade = 18)
{
    Console.WriteLine($"{nome} tem {idade} anos");
}
```

### Tipos Especiais

```csharp
// Método estático (pertence à classe)
public static void MetodoEstatico()
{
    Console.WriteLine("Chamado sem instância");
}

// Método de extensão
public static class Extensoes
{
    public static bool EhPar(this int numero)
    {
        return numero % 2 == 0;
    }
}
```

---

## 🏭 Construtores

### Construtor Padrão
```csharp
public class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }

    // Construtor padrão (gerado automaticamente se não definido)
    public Pessoa() { }

    // Construtor parametrizado
    public Pessoa(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
    }
}

// Uso
Pessoa p1 = new Pessoa(); // Construtor padrão
Pessoa p2 = new Pessoa("João", 25); // Construtor parametrizado
```

---

## 📊 Membros Estáticos

### Quando Usar
- Compartilhados por todas as instâncias
- Não precisam de objeto para serem acessados
- Ex: `Math.PI`, `Console.WriteLine()`

```csharp
public class Calculadora
{
    public static int Somar(int a, int b)
    {
        return a + b;
    }

    public static readonly double PI = 3.14159;
}

// Uso
int resultado = Calculadora.Somar(5, 3); // 8
double pi = Calculadora.PI; // 3.14159
```

---

## 🎯 Boas Práticas

### ✅ Faça
- Use propriedades em vez de fields públicos
- Valide dados em setters
- Use nomes descritivos
- Mantenha classes coesas

### ❌ Evite
- Fields públicos
- Lógica complexa em propriedades
- Classes muito grandes
- Dependências desnecessárias

---

## 📋 Resumo

- **Classe**: Molde para criar objetos
- **Objeto**: Instância de uma classe
- **Properties**: Acesso controlado aos dados
- **Métodos**: Comportamentos do objeto
- **Encapsulamento**: Proteger dados internos
- **Modificadores**: Controlam visibilidade

**Próximo tópico**: Construtores e sobrecarga!

### ✅ O que Aprendemos

1. **Classes e Objetos**
   - Classe = molde/template
   - Objeto = instância da classe
   - Sintaxe: `public class NomeClasse { }`

2. **Fields vs Properties**
   - Fields: variáveis internas (privadas)
   - Properties: acesso controlado (públicas)
   - Auto-properties: sintaxe simplificada

3. **Encapsulamento**
   - Esconder detalhes de implementação
   - Expor apenas o necessário
   - Proteger dados com validação

4. **Access Modifiers**
   - public, private, protected, internal
   - Controlam visibilidade
   - Regra: private por padrão

5. **This Keyword**
   - Referencia a instância atual
   - Diferencia parâmetros de fields
   - Passa instância como parâmetro

6. **Static Members**
   - Pertencem à classe, não à instância
   - Compartilhados por todos os objetos
   - Acesso sem criar instância

7. **Const vs Readonly**
   - Const: compilação, static implícito
   - Readonly: runtime, pode ser instance

8. **Expression-Bodied Members**
   - Sintaxe com `=>`
   - Mais concisa e legível
   - Para membros simples

---

## 🎯 Próximos Passos

No próximo tópico veremos:
- **Construtores** (padrão, parametrizados, chaining)
- **Sobrecarga** (overloading)
- **Inicializadores de objetos**
- **Optional parameters**
- **Named arguments**

---

**Continue praticando! A POO fica mais clara com a prática.** 🚀
