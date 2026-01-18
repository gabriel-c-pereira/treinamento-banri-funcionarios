foreach (var produto in produtos)  // ⬤ Clique para adicionar breakpoint
{
    // Botão direito no breakpoint > Edit Breakpoint
    // Selecione: Expression
    // Digite: produto.Estoque == 0
    
    valorTotal += produto.Preco * produto.Estoque;
    // ...
}

// ═══════════════════════════════════════════════════

valorTotal += produto.Preco * produto.Estoque;  // ⬤ Breakpoint aqui

// ═══════════════════════════════════════════════════

valorTotal += produto.Preco * produto.Estoque;  // 💬 Logpoint aqui

// ═══════════════════════════════════════════════════

foreach (var produto in produtos)  // ⬤ Hit Count breakpoint