# 🎯 Exercícios - Classes Abstratas e Interfaces

> **Tempo estimado**: 2 horas  
> **Dificuldade**: Intermediário/Avançado

---

## 📝 Instruções Gerais

- Use classes abstratas quando houver código compartilhado
- Use interfaces para definir contratos
- Implemente múltiplas interfaces quando necessário
- Teste todos os cenários

---

## 🟢 Exercício 1: Sistema de Pagamentos com Interface

Crie um sistema de pagamentos usando apenas interfaces.

**Requisitos:**
- Interface `IPagamento` com métodos: `bool Processar(decimal valor)`, `bool Cancelar()`, `string ObterRecibo()`
- Implemente: `CartaoCredito`, `Pix`, `Boleto`
- Cada classe tem sua própria lógica de processamento
- Teste com lista de `IPagamento`

---

## 🟢 Exercício 2: Formas Geométricas com Classe Abstrata

Sistema de formas usando classe abstrata.

**Requisitos:**
- Classe abstrata `Forma2D` com:
  - Métodos abstratos: `CalcularArea()`, `CalcularPerimetro()`
  - Método concreto: `ExibirInformacoes()`
- Classes: `Circulo`, `Retangulo`, `Triangulo`, `Trapezio`
- Propriedades relevantes em cada classe
- Lista de formas e exibir informações

---

## 🟡 Exercício 3: Repository Pattern

Implemente o padrão Repository usando interfaces.

**Requisitos:**
```csharp
public interface IRepository<T>
{
    void Add(T item);
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Update(T item);
    void Delete(int id);
}
```

**Implementações:**
- `MemoryRepository<T>`: armazena em memória (List)
- `FileRepository<T>`: salva em arquivo JSON
- Crie classes `Cliente` e `Produto`
- Teste ambas implementações

---

## 🟡 Exercício 4: Sistema de Notificações

Sistema que suporta múltiplos canais de notificação.

**Requisitos:**
- Interfaces: `INotificavel`, `IEnviavel`, `IAgendavel`
- Classes que implementam múltiplas interfaces:
  - `Email`: `INotificavel`, `IEnviavel`, `IAgendavel`
  - `SMS`: `INotificavel`, `IEnviavel`
  - `PushNotification`: `INotificavel`, `IAgendavel`
  - `Telegram`: todas as interfaces
- Método que aceita `IEnviavel` e envia para todos

---

## 🟡 Exercício 5: Sistema de Logging

Crie sistema de logging com múltiplas implementações.

**Requisitos:**
```csharp
public interface ILogger
{
    void Log(string message);
    void LogError(string message);
    void LogWarning(string message);
    void LogInfo(string message);
}
```

**Implementações:**
- `ConsoleLogger`: escreve no console (cores diferentes por tipo)
- `FileLogger`: escreve em arquivo de texto
- `DatabaseLogger`: simula gravação em BD (use Dictionary)
- `CompositeLogger`: combina múltiplos loggers
- Teste logging em diferentes níveis

---

## 🟡 Exercício 6: Animais com Múltiplas Interfaces

Sistema de zoológico com características específicas.

**Requisitos:**
- Classe abstrata `Animal` com propriedades básicas
- Interfaces: `IVoador`, `INadador`, `ITerrestre`, `ICarnivoro`, `IHerbivoro`
- Classes que implementam múltiplas interfaces:
  - `Pato`: `IVoador`, `INadador`, `ITerrestre`
  - `Peixe`: `INadador`, `ICarnivoro`
  - `Aguia`: `IVoador`, `ICarnivoro`
  - `Cavalo`: `ITerrestre`, `IHerbivoro`
  - `Pinguim`: `INadador`, `ITerrestre` (não voa!)
- Método `AlimentarAnimais(List<Animal> animais)`

---

## 🔴 Exercício 7: Sistema de Arquivos Abstrato

Sistema de arquivos usando abstração.

**Requisitos:**
- Classe abstrata `ItemArquivo`:
  - Propriedades: Nome, Tamanho, DataCriacao
  - Métodos abstratos: `Abrir()`, `Deletar()`, `Copiar()`
  - Método concreto: `ExibirDetalhes()`
- Classes derivadas: `Arquivo`, `Pasta`, `Atalho`
- Interface `IComprimivel` com método `Comprimir()`
- Interface `ICriptografavel` com método `Criptografar()`
- `Arquivo` implementa ambas interfaces
- `Pasta` pode conter lista de `ItemArquivo`

---

## 🔴 Exercício 8: Sistema de Veículos Elétricos

Hierarquia complexa com abstratas e interfaces.

**Requisitos:**
- Classe abstrata `Veiculo`:
  - Propriedades comuns
  - Método abstrato `Acelerar()`
- Interface `IEletrico`:
  - `int CapacidadeBateria { get; }`
  - `void Carregar(int minutos)`
  - `double CalcularAutonomia()`
- Interface `IAutonomo`:
  - `void AtivarPilotoAutomatico()`
  - `void Desativar()`
- Classes:
  - `CarroEletrico`: `Veiculo`, `IEletrico`
  - `CarroHibrido`: `Veiculo`, `IEletrico` (bateria menor)
  - `CarroAutonomo`: `Veiculo`, `IEletrico`, `IAutonomo`
  - `Tesla`: `Veiculo`, `IEletrico`, `IAutonomo` (top de linha)

---

## 🔴 Exercício 9: Sistema de Serialização

Implemente sistema de serialização com múltiplos formatos.

**Requisitos:**
```csharp
public interface ISerializador<T>
{
    string Serializar(T obj);
    T Deserializar(string data);
}
```

**Implementações:**
- `JsonSerializador<T>`: simule JSON
- `XmlSerializador<T>`: simule XML
- `BinarySerializador<T>`: simule binário
- Classe abstrata `BaseSerializador<T>`:
  - Método concreto `ValidarDados()`
  - Métodos abstratos `Serializar()` e `Deserializar()`
- Classes finais herdam de `BaseSerializador` e implementam `ISerializador`
- Teste com classes `Cliente`, `Produto`, `Pedido`

---

## 🔴 Exercício 10: Sistema de E-commerce com SOLID (PROJETO FINAL)

Sistema completo aplicando classes abstratas, interfaces e SOLID.

### Requisitos:

#### 1. Interfaces Base
```csharp
public interface IEntity
{
    int Id { get; set; }
    DateTime DataCriacao { get; set; }
}

public interface IValidavel
{
    bool Validar();
    List<string> ObterErros();
}

public interface ICalculavel
{
    decimal CalcularValorTotal();
}
```

#### 2. Classe Abstrata Base
```csharp
public abstract class EntidadeBase : IEntity, IValidavel
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    
    public abstract bool Validar();
    public abstract List<string> ObterErros();
    
    protected bool ValidarString(string valor, string nomeCampo)
    {
        // implementação comum
    }
}
```

#### 3. Produtos
- Classe abstrata `Produto : EntidadeBase, ICalculavel`
- Interfaces específicas: `IEstocavel`, `IPromocional`, `IAvaliavel`
- Classes concretas:
  - `ProdutoFisico`: `IEstocavel`, `IPromocional`, `IAvaliavel`
  - `ProdutoDigital`: `IPromocional`, `IAvaliavel`
  - `Servico`: `IAvaliavel`

#### 4. Carrinho e Pedido
```csharp
public interface ICarrinho : ICalculavel
{
    void AdicionarItem(Produto produto, int quantidade);
    void RemoverItem(int produtoId);
    void AplicarCupom(ICupomDesconto cupom);
    Pedido FinalizarCompra();
}

public abstract class Pedido : EntidadeBase, ICalculavel
{
    public abstract void ProcessarPagamento(IPagamento pagamento);
    public abstract void CalcularFrete(ICalculadoraFrete calculadora);
}
```

#### 5. Sistema de Pagamento
- Interface `IPagamento`
- Classe abstrata `PagamentoBase : IPagamento`
- Interfaces adicionais: `IParcelavel`, `IReembolsavel`
- Implementações concretas

#### 6. Sistema de Frete
```csharp
public interface ICalculadoraFrete
{
    decimal Calcular(Pedido pedido, string cepDestino);
}

public interface ITransportadora
{
    string Nome { get; }
    int PrazoEntrega { get; }
    decimal CalcularValor(double peso, string cepDestino);
}
```

#### 7. Sistema de Desconto
- Interface `ICupomDesconto`
- Classe abstrata `DescontoBase : ICupomDesconto`
- Tipos: percentual, fixo, progressivo, primeira compra

#### 8. Relatórios e Análises
```csharp
public interface IRelatorio
{
    string GerarRelatorio();
    void Exportar(IExportador exportador);
}

public interface IExportador
{
    void Exportar(string dados, string nomeArquivo);
}
```

**Tipos de relatório:**
- `RelatorioVendas`
- `RelatorioProdutos`
- `RelatorioClientes`

**Exportadores:**
- `ExportadorPDF`
- `ExportadorExcel`
- `ExportadorCSV`

#### 9. Notificações
```csharp
public interface INotificador
{
    Task Notificar(string destinatario, string mensagem);
}

public abstract class NotificadorBase : INotificador
{
    protected abstract bool ValidarDestinatario(string destinatario);
    public abstract Task Notificar(string destinatario, string mensagem);
}
```

#### 10. Features Obrigatórias

**a) Repository Pattern:**
```csharp
public interface IRepository<T> where T : IEntity
{
    void Add(T entity);
    T GetById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Func<T, bool> predicate);
    void Update(T entity);
    void Delete(int id);
}
```

**b) Unit of Work:**
```csharp
public interface IUnitOfWork : IDisposable
{
    IRepository<Produto> Produtos { get; }
    IRepository<Cliente> Clientes { get; }
    IRepository<Pedido> Pedidos { get; }
    void Commit();
    void Rollback();
}
```

**c) Service Layer:**
```csharp
public interface IServicoBase<T> where T : IEntity
{
    T Criar(T entity);
    T Atualizar(T entity);
    void Deletar(int id);
    T ObterPorId(int id);
    IEnumerable<T> ObterTodos();
}
```

**d) Implementar serviços:**
- `ServicoProduto`
- `ServicoCliente`
- `ServicoPedido`
- `ServicoFrete`
- `ServicoPagamento`

### Testes Obrigatórios:

```csharp
// 1. Criar produtos de diferentes tipos
var produtoFisico = new ProdutoFisico 
{ 
    Nome = "Notebook",
    Preco = 3000,
    Peso = 2.5,
    EstoqueAtual = 10
};

var produtoDigital = new ProdutoDigital
{
    Nome = "E-book",
    Preco = 50,
    TamanhoMB = 15,
    LinkDownload = "https://..."
};

// 2. Usar repository
IRepository<Produto> repo = new ProdutoRepository();
repo.Add(produtoFisico);
repo.Add(produtoDigital);

// 3. Criar carrinho e adicionar produtos
ICarrinho carrinho = new Carrinho();
carrinho.AdicionarItem(produtoFisico, 2);
carrinho.AdicionarItem(produtoDigital, 1);

// 4. Aplicar desconto
ICupomDesconto cupom = new CupomPercentual("DESCONTO10", 10);
carrinho.AplicarCupom(cupom);

// 5. Processar pagamento
IPagamento pagamento = new PagamentoCartao("1234-5678", 3);
pedido.ProcessarPagamento(pagamento);

// 6. Calcular frete
ICalculadoraFrete calculadora = new CalculadoraFreteCorreios();
pedido.CalcularFrete(calculadora);

// 7. Notificar cliente
INotificador notificador = new NotificadorEmail();
await notificador.Notificar(cliente.Email, "Pedido confirmado!");

// 8. Gerar relatório
IRelatorio relatorio = new RelatorioVendas(pedidos);
IExportador exportador = new ExportadorPDF();
relatorio.Exportar(exportador);
```

### Estrutura Esperada:
- 20+ interfaces
- 10+ classes abstratas
- 30+ classes concretas
- 800-1000 linhas de código
- Aplicação de TODOS os princípios SOLID
- Comentários explicativos em todas as interfaces

---

## 📚 Dicas

- Classe abstrata quando houver comportamento compartilhado
- Interface quando for apenas contrato
- Múltiplas interfaces para separar responsabilidades
- Segregue interfaces (ISP)
- Dependa de abstrações (DIP)

---

## 🎯 Critérios de Avaliação

- ✅ Uso correto de abstratas vs interfaces
- ✅ Implementação de múltiplas interfaces
- ✅ Código reutilizável
- ✅ Baixo acoplamento
- ✅ Alta coesão
- ✅ Princípios SOLID aplicados

Boa sorte! 🚀
