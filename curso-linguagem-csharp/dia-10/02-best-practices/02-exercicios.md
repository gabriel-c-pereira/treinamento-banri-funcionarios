# Exercícios — Melhores Práticas

## Exercício 1: SOLID Principles ⭐

Refatore o código abaixo aplicando os princípios SOLID:
- Identifique violações dos princípios
- Crie interfaces apropriadas
- Separe responsabilidades
- Use injeção de dependência

```csharp
public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        // Validar pedido
        if (order.Items.Count == 0)
            throw new Exception("Pedido sem itens");

        // Calcular total
        decimal total = 0;
        foreach (var item in order.Items)
        {
            total += item.Price * item.Quantity;
        }

        // Salvar no banco
        using (var connection = new SqlConnection("connectionString"))
        {
            connection.Open();
            // SQL para salvar pedido
        }

        // Enviar email
        var smtp = new SmtpClient("smtp.server.com");
        smtp.Send("admin@store.com", order.CustomerEmail, "Pedido processado", $"Total: {total}");
    }
}
```

## Exercício 2: Domain-Driven Design ⭐⭐

Implemente uma entidade de domínio seguindo DDD:
- Crie Value Objects para dados complexos
- Implemente regras de negócio na entidade
- Use Domain Services para lógica complexa
- Crie Repository interface

## Exercício 3: CQRS Pattern ⭐⭐⭐

Implemente CQRS básico para operações de produto:
- Separe Commands de Queries
- Crie Command e Query Handlers
- Implemente Command para criar produto
- Implemente Query para buscar produtos
