using System;
using System.Collections.Generic;
using System.Linq;

record ItemPedido(string Produto, int Quantidade, decimal PrecoUnitario)
{
    public decimal Subtotal => Quantidade * PrecoUnitario;
}

class Pedido
{
    public int Id { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
    public decimal Desconto { get; set; }
    public decimal TaxaEntrega { get; set; }
    
    public decimal CalcularTotal()
    {
        // ⬤ Breakpoint aqui
        decimal subtotal = Itens.Sum(i => i.Subtotal);           // F10 → subtotal = 130.00
        decimal comDesconto = subtotal - Desconto;                // F10 → comDesconto = 30.00 ✅
        decimal total = Math.Max(0, comDesconto) + TaxaEntrega;  // F10 → total = 45.00 ✅
        return total;
    }
}

class Program
{
    static void Main()
    {
        var pedido = new Pedido
        {
            Id = 1001,
            Desconto = 100.00m,
            TaxaEntrega = 15.00m
        };
        
        pedido.Itens.Add(new ItemPedido("Mouse", 2, 50.00m));      // Subtotal: 100.00
        pedido.Itens.Add(new ItemPedido("Cabo USB", 3, 10.00m));   // Subtotal: 30.00
        
        var total = pedido.CalcularTotal();
        Console.WriteLine($"Total do pedido {pedido.Id}: R$ {total:N2}");
    }
}

// ═══════════════════════════════════════════════════

decimal comDesconto = subtotal - Desconto;  // Pode ficar negativo!

// ═══════════════════════════════════════════════════

decimal comDesconto = Math.Max(0, subtotal - Desconto);
// ou
decimal total = Math.Max(subtotal - Desconto, 0) + TaxaEntrega;