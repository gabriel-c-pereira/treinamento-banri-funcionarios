# 📘 DIA 03 - Herança, Polimorfismo e Interfaces

> **Duração**: 8 horas  
> **Nível**: Intermediário  
> **Pré-requisitos**: Dia 01 e Dia 02 completos

---

## 🎯 Objetivos do Dia

Ao final deste dia, você será capaz de:

- ✅ Implementar **herança** para reutilizar código
- ✅ Aplicar **polimorfismo** para criar código flexível
- ✅ Usar **classes abstratas** para definir contratos
- ✅ Implementar **interfaces** para múltipla herança
- ✅ Aplicar princípios **SOLID** (Interface Segregation e Dependency Inversion)
- ✅ Trabalhar com **tipos genéricos** (generics)

---

## 📚 Conteúdo do Dia

### 1️⃣ **Herança** (2h)
📖 Arquivo: `01-heranca/01-conteudo.md`

**O que você vai aprender:**
- Conceito de herança (is-a relationship)
- Sintaxe básica: `class Derivada : Base`
- Keyword `base` para chamar construtor/método da classe pai
- Override de métodos
- `virtual`, `override`, `sealed`
- Modificadores de acesso em herança
- `protected` vs `private` vs `public`
- Quando usar herança

**Exercícios Práticos:**
- ✏️ Sistema de Funcionários (Funcionario → Gerente, Desenvolvedor)
- ✏️ Hierarquia de Animais
- ✏️ Sistema Bancário com herança
- ✏️ **PROJETO**: Sistema de E-commerce com Produtos

---

### 2️⃣ **Polimorfismo** (2h)
📖 Arquivo: `02-polimorfismo/01-conteudo.md`

**O que você vai aprender:**
- Polimorfismo em tempo de execução
- Upcasting e Downcasting
- `is` e `as` operators
- Pattern matching para tipos
- Métodos abstratos vs virtuais
- Polimorfismo com interfaces
- Covariance e Contravariance (preview)

**Exercícios Práticos:**
- ✏️ Calculadora de formas geométricas
- ✏️ Sistema de pagamentos
- ✏️ Processador de mensagens
- ✏️ **PROJETO**: Sistema de Transporte com múltiplos veículos

---

### 3️⃣ **Classes Abstratas e Interfaces** (2h)
📖 Arquivo: `03-classes-abstratas-interfaces/01-conteudo.md`

**O que você vai aprender:**
- Classes abstratas: quando e como usar
- Métodos abstratos vs concretos
- Interfaces: conceito e sintaxe
- Interface vs Classe Abstrata (comparação)
- Implementação explícita de interfaces
- Default interface methods (C# 8+)
- Múltiplas interfaces
- Interface segregation

**Exercícios Práticos:**
- ✏️ Sistema de persistência (IRepository)
- ✏️ Sistema de notificações
- ✏️ Logger com múltiplas implementações
- ✏️ **PROJETO**: Sistema de Pagamentos com interfaces

---

### 4️⃣ **SOLID: ISP e DIP** (1h)
📖 Arquivo: `04-solid-isp-dip/01-conteudo.md`

**O que você vai aprender:**
- **ISP** (Interface Segregation Principle)
  - Interfaces pequenas e específicas
  - Evitar "fat interfaces"
- **DIP** (Dependency Inversion Principle)
  - Depender de abstrações, não de implementações
  - Injeção de dependência
  - IoC (Inversion of Control)

**Exercícios Práticos:**
- ✏️ Refatorar código violando ISP
- ✏️ Implementar DIP com interfaces
- ✏️ Sistema com injeção de dependência
- ✏️ **PROJETO**: Refatoração completa aplicando SOLID

---

### 5️⃣ **Tipos Genéricos** (1h)
📖 Arquivo: `05-generics/01-conteudo.md`

**O que você vai aprender:**
- Generics: conceito e benefícios
- Classes genéricas (`List<T>`, `Dictionary<TKey, TValue>`)
- Métodos genéricos
- Constraints: `where T : class`, `where T : struct`, `where T : new()`
- Covariância e Contravariância (`out`, `in`)
- Generics vs Object vs dynamic

**Exercícios Práticos:**
- ✏️ Criar lista genérica customizada
- ✏️ Repository genérico
- ✏️ Cache genérico
- ✏️ **PROJETO**: Sistema de Collections genéricas

---

## ⏱️ Cronograma Sugerido (8 horas)

```
09:00 - 11:00  │ 1️⃣ Herança + Exercícios
               │ ☕ Break (15 min)
               │
11:15 - 13:00  │ 2️⃣ Polimorfismo + Exercícios
               │ 🍽️ Almoço (1h)
               │
14:00 - 16:00  │ 3️⃣ Classes Abstratas e Interfaces + Exercícios
               │ ☕ Break (15 min)
               │
16:15 - 17:15  │ 4️⃣ SOLID (ISP e DIP) + Exercícios
               │
17:15 - 18:00  │ 5️⃣ Tipos Genéricos + Exercícios
               │
18:00+         │ 🏆 Projetos finais e revisão
```

---

## 🎯 Projetos do Dia

### Projeto 1: **Sistema de E-commerce**
- Herança: `Produto` → `ProdutoFisico`, `ProdutoDigital`
- Polimorfismo: Cálculo de frete
- **Tempo estimado**: 45 minutos

### Projeto 2: **Sistema de Transporte**
- Herança: `Veiculo` → `Carro`, `Moto`, `Caminhao`
- Polimorfismo: Cálculo de combustível
- **Tempo estimado**: 45 minutos

### Projeto 3: **Sistema de Pagamentos**
- Interfaces: `IPagamento`, `IValidavel`, `IReembolsavel`
- Classes: `CartaoCredito`, `Pix`, `Boleto`
- **Tempo estimado**: 60 minutos

### Projeto 4: **Sistema Completo com SOLID**
- Aplicação de todos os conceitos
- Refatoração com SOLID
- **Tempo estimado**: 90 minutos

---

## 📋 Checklist de Progresso

### Tópico 1: Herança
- [ ] Ler conteúdo teórico
- [ ] Fazer exercícios 1-5
- [ ] Comparar com correções
- [ ] Fazer exercícios 6-10
- [ ] Completar projeto final

### Tópico 2: Polimorfismo
- [ ] Ler conteúdo teórico
- [ ] Fazer exercícios 1-5
- [ ] Comparar com correções
- [ ] Fazer exercícios 6-10
- [ ] Completar projeto final

### Tópico 3: Classes Abstratas e Interfaces
- [ ] Ler conteúdo teórico
- [ ] Fazer exercícios 1-5
- [ ] Comparar com correções
- [ ] Fazer exercícios 6-10
- [ ] Completar projeto final

### Tópico 4: SOLID (ISP e DIP)
- [ ] Ler conteúdo teórico
- [ ] Fazer exercícios 1-5
- [ ] Comparar com correções
- [ ] Completar projeto de refatoração

### Tópico 5: Tipos Genéricos
- [ ] Ler conteúdo teórico
- [ ] Fazer exercícios 1-5
- [ ] Comparar com correções
- [ ] Fazer exercícios 6-10
- [ ] Completar projeto final

---

## 🎓 Conceitos-Chave

### Herança
```csharp
// Herança básica
public class Funcionario
{
    public string Nome { get; set; }
    public decimal Salario { get; set; }
    
    public virtual decimal CalcularBonus() => Salario * 0.10m;
}

public class Gerente : Funcionario
{
    public int NumeroSubordinados { get; set; }
    
    public override decimal CalcularBonus() => Salario * 0.20m;
}
```

### Polimorfismo
```csharp
// Usar a classe base para diferentes tipos
Funcionario func1 = new Funcionario { Salario = 5000 };
Funcionario func2 = new Gerente { Salario = 10000 };

Console.WriteLine(func1.CalcularBonus()); // 500
Console.WriteLine(func2.CalcularBonus()); // 2000 (método do Gerente!)
```

### Interfaces
```csharp
public interface IPagamento
{
    bool ProcessarPagamento(decimal valor);
    bool Cancelar();
}

public class CartaoCredito : IPagamento
{
    public bool ProcessarPagamento(decimal valor)
    {
        // Implementação específica
        return true;
    }
    
    public bool Cancelar()
    {
        // Implementação específica
        return true;
    }
}
```

### Generics
```csharp
public class Repository<T> where T : class
{
    private List<T> _items = new();
    
    public void Add(T item) => _items.Add(item);
    public T GetById(int id) => _items[id];
    public List<T> GetAll() => _items;
}

// Uso
var repoClientes = new Repository<Cliente>();
var repoProdutos = new Repository<Produto>();
```

---

## ❓ FAQ

**Q: Quando devo usar herança vs interfaces?**  
A: Use **herança** quando há uma relação "é um" clara e você quer reutilizar implementação. Use **interfaces** quando você quer definir um contrato sem implementação ou quando precisa de "múltipla herança".

**Q: O que é polimorfismo na prática?**  
A: É a capacidade de tratar objetos de diferentes tipos através de uma interface comum (classe base ou interface), permitindo que cada tipo execute sua própria versão do método.

**Q: Classes abstratas vs interfaces - qual usar?**  
A:
- **Classe abstrata**: Quando você quer compartilhar código entre classes relacionadas
- **Interface**: Quando você quer definir um contrato que classes não relacionadas podem implementar

**Q: O que são generics e por que usar?**  
A: Generics permitem criar código type-safe e reutilizável sem perder performance. Evitam boxing/unboxing e permitem detecção de erros em tempo de compilação.

**Q: SOLID é obrigatório?**  
A: Não é obrigatório, mas seguir SOLID resulta em código mais manutenível, testável e flexível. É uma das melhores práticas da indústria.

---

## 🔗 Recursos Adicionais

### Documentação Oficial
- [Herança (Microsoft Docs)](https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/object-oriented/inheritance)
- [Polimorfismo (Microsoft Docs)](https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [Interfaces (Microsoft Docs)](https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/types/interfaces)
- [Generics (Microsoft Docs)](https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/types/generics)

### Artigos Recomendados
- [SOLID Principles in C#](https://www.c-sharpcorner.com/article/solid-principles-in-c-sharp/)
- [When to use Abstract Class vs Interface](https://stackoverflow.com/questions/761194/interface-vs-abstract-class-general-oo)

### Vídeos
- [C# Inheritance Tutorial](https://www.youtube.com/watch?v=9oq7Cva4fSs)
- [SOLID Principles Explained](https://www.youtube.com/watch?v=TMuno5RZNeE)

---

## 🎯 Objetivos de Aprendizagem

Ao completar este dia, você terá:

✅ Compreensão profunda de **herança** e quando usá-la  
✅ Domínio de **polimorfismo** e suas aplicações  
✅ Habilidade para escolher entre **classes abstratas** e **interfaces**  
✅ Conhecimento de princípios **SOLID** (ISP e DIP)  
✅ Capacidade de criar código **genérico** e reutilizável  
✅ 5 projetos completos demonstrando todos os conceitos  

---

## 🚀 Próximos Passos

Após completar o Dia 03:
1. ✅ Revisar todos os exercícios e correções
2. ✅ Completar os 5 projetos finais
3. ✅ Fazer auto-avaliação
4. ➡️ Prosseguir para o **Dia 04** - Coleções, Listas e LINQ

---

## 📊 Auto-Avaliação

Após concluir o dia, avalie seu entendimento:

| Conceito | 😕 Precisa revisar | 😊 Entendi | 🚀 Dominei |
|----------|-------------------|-----------|-----------|
| Herança básica | ⬜ | ⬜ | ⬜ |
| Polimorfismo | ⬜ | ⬜ | ⬜ |
| Classes abstratas | ⬜ | ⬜ | ⬜ |
| Interfaces | ⬜ | ⬜ | ⬜ |
| ISP (SOLID) | ⬜ | ⬜ | ⬜ |
| DIP (SOLID) | ⬜ | ⬜ | ⬜ |
| Generics | ⬜ | ⬜ | ⬜ |

---

**Boa sorte no Dia 03! Este é um dos dias mais importantes do curso! 💪**

*Lembre-se: Herança, Polimorfismo e Interfaces são os pilares da POO. Domine-os e você dominará C#!*
