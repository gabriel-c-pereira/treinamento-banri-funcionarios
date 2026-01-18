# 📝 Exercícios - Razor Pages

## 🎯 Objetivo

Praticar o desenvolvimento de aplicações web usando Razor Pages no ASP.NET Core, incluindo model binding, validação, layouts e partial views.

---

## ✏️ Exercício 1: Página Básica com Lista

**Dificuldade**: ⭐ Iniciante

Crie uma Razor Page que exiba uma lista de produtos:

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
    public DateTime DataCadastro { get; set; }
}
```

2. **Página Index.cshtml.cs**:
```csharp
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private static List<Produto> _produtos = new();

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;

        // Dados de exemplo
        if (!_produtos.Any())
        {
            _produtos.AddRange(new[]
            {
                new Produto { Id = 1, Nome = "Notebook", Descricao = "Notebook Dell", Preco = 3500, Estoque = 10, DataCadastro = DateTime.Now },
                new Produto { Id = 2, Nome = "Mouse", Descricao = "Mouse óptico", Preco = 50, Estoque = 50, DataCadastro = DateTime.Now },
                new Produto { Id = 3, Nome = "Teclado", Descricao = "Teclado mecânico", Preco = 200, Estoque = 25, DataCadastro = DateTime.Now }
            });
        }
    }

    public List<Produto> Produtos { get; set; }

    public async Task OnGetAsync()
    {
        _logger.LogInformation("Carregando lista de produtos");
        Produtos = _produtos;
        await Task.CompletedTask;
    }
}
```

3. **Página Index.cshtml**:
```html
@page
@model IndexModel
@{
    ViewData["Title"] = "Produtos";
}

<h1>Lista de Produtos</h1>

<table class="table table-striped">
    <thead>
        <tr>
            <th>ID</th>
            <th>Nome</th>
            <th>Descrição</th>
            <th>Preço</th>
            <th>Estoque</th>
            <th>Data Cadastro</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var produto in Model.Produtos)
        {
            <tr>
                <td>@produto.Id</td>
                <td>@produto.Nome</td>
                <td>@produto.Descricao</td>
                <td>R$ @produto.Preco.ToString("F2")</td>
                <td>@produto.Estoque</td>
                <td>@produto.DataCadastro.ToString("dd/MM/yyyy")</td>
            </tr>
        }
    </tbody>
</table>

<div class="mt-3">
    <a asp-page="Create" class="btn btn-primary">Novo Produto</a>
</div>
```

---

## ✏️ Exercício 2: Formulário com Validação

**Dificuldade**: ⭐⭐ Intermediário

Implemente páginas de criação e edição de produtos com validação:

### Requisitos:
1. **Página Create.cshtml.cs**:
```csharp
public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ILogger<CreateModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public Produto Produto { get; set; }

    public IActionResult OnGet()
    {
        Produto = new Produto();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Simula salvamento
        Produto.Id = new Random().Next(1000, 9999);
        Produto.DataCadastro = DateTime.Now;

        _logger.LogInformation($"Produto criado: {Produto.Nome}");

        TempData["Mensagem"] = "Produto criado com sucesso!";
        return RedirectToPage("Index");
    }
}
```

2. **Página Create.cshtml**:
```html
@page
@model CreateModel
@{
    ViewData["Title"] = "Novo Produto";
}

<h1>Criar Novo Produto</h1>

<div class="row">
    <div class="col-md-6">
        <form method="post">
            <div class="mb-3">
                <label asp-for="Produto.Nome" class="form-label"></label>
                <input asp-for="Produto.Nome" class="form-control" />
                <span asp-validation-for="Produto.Nome" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <label asp-for="Produto.Descricao" class="form-label"></label>
                <textarea asp-for="Produto.Descricao" class="form-control" rows="3"></textarea>
                <span asp-validation-for="Produto.Descricao" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <label asp-for="Produto.Preco" class="form-label"></label>
                <input asp-for="Produto.Preco" type="number" step="0.01" class="form-control" />
                <span asp-validation-for="Produto.Preco" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <label asp-for="Produto.Estoque" class="form-label"></label>
                <input asp-for="Produto.Estoque" type="number" class="form-control" />
                <span asp-validation-for="Produto.Estoque" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <button type="submit" class="btn btn-primary">Salvar</button>
                <a asp-page="Index" class="btn btn-secondary">Cancelar</a>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

3. **Modelo com validação**:
```csharp
public class Produto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Nome { get; set; }

    [StringLength(500, ErrorMessage = "Descrição não pode exceder 500 caracteres")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0.01, 999999.99, ErrorMessage = "Preço deve ser entre 0.01 e 999999.99")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "Estoque é obrigatório")]
    [Range(0, 9999, ErrorMessage = "Estoque deve ser entre 0 e 9999")]
    public int Estoque { get; set; }

    public DateTime DataCadastro { get; set; }
}
```

---

## ✏️ Exercício 3: Layout e Partial Views

**Dificuldade**: ⭐⭐⭐ Avançado

Crie um layout compartilhado e partial views para componentes reutilizáveis:

### Requisitos:
1. **Layout _Layout.cshtml** (Pages/Shared/):
```html
<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Minha Loja</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" asp-page="/Index">Minha Loja</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-page="/Index">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-page="/Produtos/Index">Produtos</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-page="/Clientes/Index">Clientes</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>

    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2025 - Minha Loja - <a asp-page="/Privacy">Privacy</a>
        </div>
    </footer>

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js"></script>
    @RenderSection("Scripts", required: false)
</body>
</html>
```

2. **Partial View _Mensagem.cshtml** (Pages/Shared/):
```html
@if (TempData["Mensagem"] != null)
{
    <div class="alert alert-success alert-dismissible fade show" role="alert">
        @TempData["Mensagem"]
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
}

@if (TempData["Erro"] != null)
{
    <div class="alert alert-danger alert-dismissible fade show" role="alert">
        @TempData["Erro"]
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
}
```

3. **Partial View _ProdutoCard.cshtml** (Pages/Shared/):
```html
@model Produto

<div class="card h-100">
    <div class="card-body">
        <h5 class="card-title">@Model.Nome</h5>
        <p class="card-text">@Model.Descricao</p>
        <p class="card-text">
            <strong>R$ @Model.Preco.ToString("F2")</strong>
        </p>
        <p class="card-text">
            <small class="text-muted">Estoque: @Model.Estoque unidades</small>
        </p>
    </div>
    <div class="card-footer">
        <a asp-page="Details" asp-route-id="@Model.Id" class="btn btn-primary">Ver Detalhes</a>
        <a asp-page="Edit" asp-route-id="@Model.Id" class="btn btn-secondary">Editar</a>
    </div>
</div>
```

4. **Uso do layout e partials**:
```html
@page
@{
    Layout = "_Layout";
}

<partial name="_Mensagem" />

<div class="row">
    @foreach (var produto in Model.Produtos)
    {
        <div class="col-md-4 mb-4">
            <partial name="_ProdutoCard" model="produto" />
        </div>
    }
</div>
```
