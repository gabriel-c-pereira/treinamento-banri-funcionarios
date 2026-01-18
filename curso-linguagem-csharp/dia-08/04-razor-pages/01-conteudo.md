# Razor Pages

## O que são Razor Pages?

Razor Pages é um framework de páginas para ASP.NET Core que simplifica a criação de interfaces web.

## Estrutura Básica

```csharp
// Index.cshtml.cs
public class IndexModel : PageModel
{
    private readonly IProdutoService _service;

    public IndexModel(IProdutoService service)
    {
        _service = service;
    }

    public IList<Produto> Produtos { get; set; }

    public async Task OnGetAsync()
    {
        Produtos = await _service.GetAllAsync();
    }
}
```

```html
<!-- Index.cshtml -->
@page
@model IndexModel

<h1>Produtos</h1>

<table class="table">
    <thead>
        <tr>
            <th>Nome</th>
            <th>Preço</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var produto in Model.Produtos)
        {
            <tr>
                <td>@produto.Nome</td>
                <td>@produto.Preco.ToString("C")</td>
            </tr>
        }
    </tbody>
</table>
```

## Model Binding

```csharp
public class CreateModel : PageModel
{
    [BindProperty]
    public CreateProdutoViewModel Produto { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Salvar produto
        return RedirectToPage("Index");
    }
}
```

## Layouts e Partial Views

```html
<!-- _Layout.cshtml -->
<!DOCTYPE html>
<html>
<head>
    <title>@ViewData["Title"]</title>
</head>
<body>
    <nav>
        <a asp-page="Index">Home</a>
        <a asp-page="Create">Novo Produto</a>
    </nav>

    <main>
        @RenderBody()
    </main>
</body>
</html>
```

## Tag Helpers

```html
<!-- Form com Tag Helpers -->
<form asp-page-handler="Create" method="post">
    <div class="form-group">
        <label asp-for="Produto.Nome"></label>
        <input asp-for="Produto.Nome" class="form-control" />
        <span asp-validation-for="Produto.Nome"></span>
    </div>

    <button type="submit" class="btn btn-primary">Salvar</button>
</form>
```
