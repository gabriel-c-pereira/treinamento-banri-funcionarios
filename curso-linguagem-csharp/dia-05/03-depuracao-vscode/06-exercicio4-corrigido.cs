// Camada de Dados
class ProdutoRepository
{
    public Produto BuscarPorId(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        
        if (produto == null)
            throw new KeyNotFoundException($"Produto {id} não encontrado no repositório");
        
        return produto;
    }
}

// Camada de Negócio
class ProdutoService
{
    public decimal ObterPreco(int produtoId)
    {
        try
        {
            var produto = _repository.BuscarPorId(produtoId);
            return produto.Preco;
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidOperationException(
                $"Não foi possível obter o preço do produto {produtoId}", 
                ex);
        }
    }
}