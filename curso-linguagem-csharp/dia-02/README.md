# 📘 Dia 02 - Programação Orientada a Objetos (POO)

## 🎯 Objetivos do Dia

Ao final deste dia, você será capaz de:

✅ Entender os fundamentos da Programação Orientada a Objetos  
✅ Criar e usar classes e objetos em C#  
✅ Implementar properties, fields e métodos  
✅ Aplicar encapsulamento e access modifiers  
✅ Trabalhar com construtores e sobrecarga  
✅ Entender a diferença entre reference e value types  
✅ Usar structs, records e tuples  
✅ Dominar ref, out e in parameters  

---

## 📚 Conteúdo do Dia

### 1️⃣ Classes e Objetos (2-3 horas)
**Pasta**: `01-classes-e-objetos/`

**Tópicos**:
- O que são classes e objetos
- Declaração de classes
- Fields vs Properties
- Auto-properties
- Métodos e parâmetros
- Encapsulamento
- Access modifiers (public, private, protected, internal)
- This keyword
- Static members
- Const vs Readonly

**Exercícios**: 10 exercícios práticos

---

### 2️⃣ Construtores e Sobrecarga (2-3 horas)
**Pasta**: `02-construtores-sobrecarga/`

**Tópicos**:
- Construtores padrão
- Construtores parametrizados
- Constructor overloading
- Constructor chaining (this)
- Inicializadores de objetos
- Optional parameters
- Named arguments
- Method overloading
- Expression-bodied members
- Primary constructors (C# 12)

**Exercícios**: 10 exercícios práticos

---

### 3️⃣ Referências vs Valores (2-3 horas)
**Pasta**: `03-referencias-vs-valores/`

**Tópicos**:
- Stack vs Heap
- Value types vs Reference types
- Boxing e Unboxing
- Ref parameters
- Out parameters
- In parameters (C# 7.2)
- Structs vs Classes
- Records (C# 9+)
- Record structs (C# 10)
- Tuples e ValueTuples

**Exercícios**: 10 exercícios práticos

---

## ⏰ Roteiro de Estudo Sugerido (8 horas)

### 🌅 Manhã (4 horas)

#### 09:00 - 10:30 | Tópico 1: Classes e Objetos
- [ ] 📖 Ler conteúdo teórico (45 min)
- [ ] 💻 Fazer exercícios 1-5 (45 min)

#### 10:30 - 10:45 | ☕ Intervalo

#### 10:45 - 12:30 | Tópico 1: Prática
- [ ] 💻 Fazer exercícios 6-10 (1h)
- [ ] 🔍 Revisar correções (30 min)
- [ ] 🎯 Projeto: Sistema de Biblioteca (15 min)

#### 12:30 - 14:00 | 🍽️ Almoço

---

### 🌆 Tarde (4 horas)

#### 14:00 - 15:30 | Tópico 2: Construtores
- [ ] 📖 Ler conteúdo teórico (45 min)
- [ ] 💻 Fazer exercícios 1-5 (45 min)

#### 15:30 - 15:45 | ☕ Intervalo

#### 15:45 - 17:00 | Tópico 3: Referências vs Valores
- [ ] 📖 Ler conteúdo teórico (45 min)
- [ ] 💻 Fazer exercícios 1-5 (30 min)

#### 17:00 - 18:00 | Projeto Final
- [ ] 💻 Exercícios finais (30 min)
- [ ] 🎯 Projeto integrador (30 min)

---

## 🎯 Projetos Práticos do Dia

### 1. Sistema de Biblioteca 📚
**Complexidade**: Média  
**Conceitos**: Classes, properties, métodos, encapsulamento

Crie um sistema para gerenciar livros, autores e empréstimos.

**Classes necessárias**:
- `Livro` (título, autor, ISBN, disponível)
- `Autor` (nome, nacionalidade, livros)
- `Usuario` (nome, CPF, livros emprestados)
- `Biblioteca` (catálogo, empréstimos)

---

### 2. Sistema de Contas Bancárias 💰
**Complexidade**: Média-Alta  
**Conceitos**: Construtores, sobrecarga, encapsulamento

Implemente diferentes tipos de contas bancárias.

**Classes necessárias**:
- `Conta` (número, titular, saldo)
- `ContaCorrente` (limite)
- `ContaPoupanca` (rendimento)
- `Transacao` (tipo, valor, data)

---

### 3. Sistema de Jogadores (RPG) ⚔️
**Complexidade**: Alta  
**Conceitos**: Structs, records, ref/out parameters

Crie um sistema de RPG com personagens e batalhas.

**Estruturas necessárias**:
- `Posicao` (struct: x, y)
- `Atributos` (record: força, defesa, HP)
- `Jogador` (classe com métodos de ataque/defesa)
- `Inventario` (itens, equipamentos)

---

## 📋 Checklist de Progresso

### Tópico 1: Classes e Objetos
- [ ] Li todo o conteúdo teórico
- [ ] Entendi a diferença entre classe e objeto
- [ ] Sei declarar classes e criar objetos
- [ ] Entendo fields vs properties
- [ ] Sei usar access modifiers
- [ ] Completei todos os 10 exercícios
- [ ] Revisei as correções
- [ ] Fiz o projeto da biblioteca

### Tópico 2: Construtores e Sobrecarga
- [ ] Li todo o conteúdo teórico
- [ ] Entendo construtores padrão e parametrizados
- [ ] Sei fazer constructor chaining
- [ ] Entendo overloading
- [ ] Sei usar optional parameters
- [ ] Completei todos os 10 exercícios
- [ ] Revisei as correções
- [ ] Fiz o projeto de contas bancárias

### Tópico 3: Referências vs Valores
- [ ] Li todo o conteúdo teórico
- [ ] Entendo stack vs heap
- [ ] Sei a diferença entre value e reference types
- [ ] Entendo ref, out e in parameters
- [ ] Sei quando usar struct vs class
- [ ] Conheço records (C# 9+)
- [ ] Completei todos os 10 exercícios
- [ ] Revisei as correções
- [ ] Fiz o projeto do RPG

---

## 💡 Dicas de Estudo

### 🎯 Foco Principal
A POO é um dos pilares do C#. Dedique tempo para:
1. **Entender conceitos** antes de codificar
2. **Praticar muito** - crie suas próprias classes
3. **Experimentar** - teste o comportamento dos objetos
4. **Comparar** - veja diferenças entre abordagens

### 📝 Tome Notas
- Anote diferenças entre fields e properties
- Liste quando usar cada access modifier
- Desenhe diagramas de classes
- Documente seus aprendizados

### 🔄 Pratique Refatoração
- Pegue código do Dia 01 e transforme em classes
- Melhore a organização
- Aplique encapsulamento
- Use properties ao invés de fields públicos

### 🤝 Conceitos Importantes
```csharp
// ❌ EVITE: Fields públicos
public class Pessoa
{
    public string nome;  // Não recomendado
}

// ✅ PREFIRA: Properties
public class Pessoa
{
    public string Nome { get; set; }  // Recomendado
}

// 🏆 MELHOR: Encapsulamento
public class Pessoa
{
    private string _nome;
    
    public string Nome
    {
        get => _nome;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Nome não pode ser vazio");
            _nome = value;
        }
    }
}
```

---

## 🎓 Conceitos-Chave do Dia

### Os 4 Pilares da POO
1. **Encapsulamento** 🔒
   - Esconder detalhes de implementação
   - Proteger dados com access modifiers
   - Expor interface pública

2. **Abstração** 🎭
   - Modelar objetos do mundo real
   - Focar no essencial
   - Esconder complexidade

3. **Herança** 👨‍👩‍👧 (veremos no Dia 03)
   - Reutilizar código
   - Criar hierarquias
   - Especializar comportamentos

4. **Polimorfismo** 🦎 (veremos no Dia 03)
   - Múltiplas formas
   - Sobrescrita de métodos
   - Interfaces

*Hoje focamos nos dois primeiros!*

---

## 🔍 Termos Importantes

| Termo | Significado |
|-------|-------------|
| **Classe** | Molde/template para criar objetos |
| **Objeto** | Instância de uma classe |
| **Field** | Variável de classe (dados) |
| **Property** | Acesso controlado a dados |
| **Método** | Comportamento/ação de uma classe |
| **Construtor** | Método especial para inicializar objetos |
| **Encapsulamento** | Ocultar dados e expor apenas o necessário |
| **Access Modifier** | Controla visibilidade (public, private, etc) |
| **Static** | Pertence à classe, não à instância |
| **This** | Referência à instância atual |
| **Stack** | Memória para value types |
| **Heap** | Memória para reference types |
| **Struct** | Value type customizado |
| **Record** | Reference type imutável (C# 9+) |

---

## 🚀 Desafios Extras

### 🥉 Iniciante
1. **Classe Simples**: Crie uma classe `Produto` com nome, preço e quantidade
2. **Métodos**: Adicione métodos para calcular valor total em estoque
3. **Validação**: Valide preço (não negativo) e quantidade (não negativo)

### 🥈 Intermediário
1. **Sistema de Pedidos**: Crie classes `Pedido`, `ItemPedido`, `Cliente`
2. **Cálculos**: Implemente cálculo de subtotal, desconto, total
3. **Construtores**: Use múltiplos construtores e chaining

### 🥇 Avançado
1. **E-commerce Completo**: Sistema com produtos, carrinho, pedidos, pagamento
2. **Structs**: Use structs para `Dimensoes`, `Peso`, `Preco`
3. **Records**: Use records para dados imutáveis (`Endereco`, `CPF`)
4. **Performance**: Compare performance de struct vs class

---

## ❓ FAQ - Perguntas Frequentes

### 1. Quando usar class vs struct?
**Use class quando:**
- Objeto pode ser nulo
- Precisa de herança
- Objeto é grande (> 16 bytes)
- Comportamento de referência é desejado

**Use struct quando:**
- Representa um único valor
- É pequeno (≤ 16 bytes)
- É imutável
- Performance é crítica

### 2. Field vs Property: qual usar?
**Sempre prefira properties!**
- Properties permitem validação
- Properties podem ser read-only
- Properties funcionam com data binding
- Fields só para casos muito específicos

### 3. Quando usar static?
Use static quando o membro:
- Não depende de instância específica
- É compartilhado por todos os objetos
- É uma utility/helper function
- Exemplo: `Math.PI`, `Console.WriteLine()`

### 4. O que é encapsulamento?
É esconder a implementação interna e expor apenas o necessário:
```csharp
public class ContaBancaria
{
    private decimal _saldo;  // Escondido
    
    public decimal Saldo => _saldo;  // Apenas leitura
    
    public void Depositar(decimal valor)  // Controle
    {
        if (valor > 0)
            _saldo += valor;
    }
}
```

### 5. Constructor chaining: para que serve?
Evita duplicação de código:
```csharp
public class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    
    public Pessoa() : this("Sem nome", 0) { }
    
    public Pessoa(string nome) : this(nome, 0) { }
    
    public Pessoa(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
    }
}
```

---

## 📚 Recursos Adicionais

### Documentação Oficial
- [Classes (C# Guide)](https://docs.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/classes)
- [Properties (C# Guide)](https://docs.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/properties)
- [Constructors (C# Guide)](https://docs.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/constructors)
- [Value Types vs Reference Types](https://docs.microsoft.com/dotnet/csharp/language-reference/builtin-types/value-types)

### Artigos Recomendados
- [OOP Principles in C#](https://www.c-sharpcorner.com/UploadFile/mkagrahari/object-oriented-programming-principles-in-C-Sharp/)
- [When to use struct vs class](https://stackoverflow.com/questions/521298/when-to-use-struct)
- [Records in C# 9](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-9#record-types)

### Vídeos
- C# Fundamentals for Absolute Beginners (Microsoft)
- OOP in C# (Mosh Hamedani)
- Advanced C# Features (Pluralsight)

---

## 🎯 Auto-Avaliação

Ao final do dia, você deve ser capaz de responder:

### Conceitual
- [ ] O que é uma classe? E um objeto?
- [ ] Qual a diferença entre field e property?
- [ ] O que é encapsulamento?
- [ ] Para que servem os access modifiers?
- [ ] O que é constructor overloading?
- [ ] Qual a diferença entre stack e heap?
- [ ] Quando usar struct ao invés de class?
- [ ] O que são records e quando usá-los?

### Prático
- [ ] Sei criar classes com properties e métodos
- [ ] Sei usar diferentes access modifiers
- [ ] Sei criar múltiplos construtores
- [ ] Sei usar ref, out e in parameters
- [ ] Sei quando usar struct vs class
- [ ] Consigo criar um sistema OO completo

### Projeto
- [ ] Criei pelo menos 2 dos 3 projetos propostos
- [ ] Apliquei encapsulamento corretamente
- [ ] Usei construtores adequadamente
- [ ] Escolhi os tipos corretos (class/struct/record)

---

## 🎊 Próximos Passos

Após completar o Dia 02, você estará pronto para:

**Dia 03 - Herança, Polimorfismo e Interfaces**
- Herança de classes
- Métodos virtuais e override
- Classes abstratas
- Interfaces
- Polimorfismo
- SOLID principles

---

## 📞 Suporte

**Dúvidas?**
- Revise o conteúdo teórico
- Analise as correções dos exercícios
- Experimente no código
- Consulte a documentação oficial
- Faça perguntas ao instrutor

---

**Bons estudos! 🚀**

*"A melhor maneira de aprender POO é praticando. Crie muitas classes, objetos e experimente!"*
