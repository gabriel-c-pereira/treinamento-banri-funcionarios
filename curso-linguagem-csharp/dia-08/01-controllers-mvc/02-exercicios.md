# 📝 Exercícios - Controllers e MVC

## 🎯 Objetivo

Praticar a criação de controllers MVC no ASP.NET Core, implementando ações CRUD, model binding, validação e diferentes tipos de resposta.

---

## ✏️ Exercício 1: Controller Básico

**Dificuldade**: ⭐ Iniciante

Crie um controller `ClientesController` com as seguintes ações:

### Requisitos:
1. **Modelo Cliente**:
```csharp
public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataCadastro { get; set; }
}
```

2. **Ações do Controller**:
   - `Index()` - Lista todos os clientes (GET)
   - `Details(int id)` - Mostra detalhes de um cliente (GET)
   - `Create()` - Formulário de criação (GET)
   - `Create(Cliente cliente)` - Processa criação (POST)
   - `Edit(int id)` - Formulário de edição (GET)
   - `Edit(Cliente cliente)` - Processa edição (POST)
   - `Delete(int id)` - Confirmação de exclusão (GET)
   - `DeleteConfirmed(int id)` - Processa exclusão (POST)

3. **Funcionalidades**:
   - Use uma lista estática para armazenar os dados
   - Implemente validação básica (Nome e Email obrigatórios)
   - Use `ModelState.IsValid` para validação
   - Redirecione após operações bem-sucedidas

---

## ✏️ Exercício 2: Model Binding e Validação

**Dificuldade**: ⭐⭐ Intermediário

Implemente um controller `PedidosController` que demonstre diferentes tipos de model binding:

### Requisitos:
1. **Modelos**:
```csharp
public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemPedido> Itens { get; set; }
    public StatusPedido Status { get; set; }
}

public class ItemPedido
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}

public enum StatusPedido
{
    Pendente,
    Processando,
    Enviado,
    Entregue,
    Cancelado
}
```

2. **Ações com diferentes tipos de binding**:
   - `BuscarPorStatus(StatusPedido status)` - Query string
   - `BuscarPorCliente(int clienteId)` - Route parameter
   - `CriarPedido(Pedido pedido)` - Body (JSON)
   - `AtualizarItens(int pedidoId, List<ItemPedido> itens)` - Form data

3. **Validações**:
   - ValorTotal deve ser maior que 0
   - ClienteId deve existir
   - DataPedido não pode ser futura
   - Itens deve ter pelo menos 1 item

---

## ✏️ Exercício 3: Routing e Action Results

**Dificuldade**: ⭐⭐⭐ Avançado

Crie um controller `ProdutosController` com rotas customizadas e diferentes tipos de retorno:

### Requisitos:
1. **Modelo Produto**:
```csharp
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public string Categoria { get; set; }
}
```

2. **Rotas Customizadas**:
   - `GET /produtos` → `ListarTodos()`
   - `GET /produtos/categoria/{categoria}` → `PorCategoria(string categoria)`
   - `GET /produtos/{id}/preco` → `ObterPreco(int id)`
   - `POST /produtos` → `Criar(Produto produto)`
   - `PUT /produtos/{id}` → `Atualizar(int id, Produto produto)`
   - `DELETE /produtos/{id}` → `Remover(int id)`

3. **Diferentes Action Results**:
   - `View()` para páginas HTML
   - `Json()` para APIs
   - `File()` para downloads
   - `RedirectToAction()` para redirecionamentos
   - `NotFound()` para recursos inexistentes
   - `BadRequest()` para dados inválidos

4. **Funcionalidades Avançadas**:
   - Filtros de ação para logging
   - Cache de saída
   - Compressão de resposta
   - Tratamento de erros personalizado
- Filtros de autorização
- Tratamento de erros consistente
