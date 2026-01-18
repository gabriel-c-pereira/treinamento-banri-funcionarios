// Exercício 3 Corrigido: Classificador de Idade
// Arquivo: Program.cs

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("    CLASSIFICADOR DE IDADE         ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

Console.Write("Digite a idade: ");
string entrada = Console.ReadLine();

// Validação de entrada
if (!int.TryParse(entrada, out int idade))
{
    Console.WriteLine("❌ ERRO: Digite um número válido!");
    return;
}

// Validação de range
if (idade < 0 || idade > 150)
{
    Console.WriteLine("❌ ERRO: Idade deve estar entre 0 e 150!");
    return;
}

Console.WriteLine();

// ═══════════════════════════════════════════════════════════
// SOLUÇÃO 1: if-else if-else (Clássico)
// ═══════════════════════════════════════════════════════════

string classificacao;
string emoji;

if (idade <= 12)
{
    classificacao = "Criança";
    emoji = "👶";
}
else if (idade <= 17)
{
    classificacao = "Adolescente";
    emoji = "🧒";
}
else if (idade <= 59)
{
    classificacao = "Adulto";
    emoji = "👨";
}
else
{
    classificacao = "Idoso";
    emoji = "👴";
}

Console.WriteLine($"{emoji} Classificação: {classificacao}");

// ═══════════════════════════════════════════════════════════
// SOLUÇÃO 2: Operador Ternário Aninhado
// ═══════════════════════════════════════════════════════════

string classificacao2 = idade <= 12 ? "Criança" :
                        idade <= 17 ? "Adolescente" :
                        idade <= 59 ? "Adulto" : "Idoso";

Console.WriteLine($"Classificação (ternário): {classificacao2}");

// ═══════════════════════════════════════════════════════════
// SOLUÇÃO 3: Switch Expression com Pattern Matching (C# 8+)
// ═══════════════════════════════════════════════════════════

string classificacao3 = idade switch
{
    <= 12 => "Criança",
    <= 17 => "Adolescente",
    <= 59 => "Adulto",
    _ => "Idoso"
};

Console.WriteLine($"Classificação (switch): {classificacao3}");

// ═══════════════════════════════════════════════════════════
// DESAFIO EXTRA: Subcategorias Detalhadas
// ═══════════════════════════════════════════════════════════

Console.WriteLine();
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("  CLASSIFICAÇÃO DETALHADA          ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

string classificacaoDetalhada = idade switch
{
    0 => "Recém-nascido",
    >= 1 and <= 2 => "Bebê",
    >= 3 and <= 5 => "Criança (primeira infância)",
    >= 6 and <= 12 => "Criança (segunda infância)",
    >= 13 and <= 15 => "Adolescente inicial",
    >= 16 and <= 17 => "Adolescente final",
    >= 18 and <= 25 => "Adulto jovem",
    >= 26 and <= 35 => "Adulto",
    >= 36 and <= 45 => "Meia-idade inicial",
    >= 46 and <= 59 => "Meia-idade",
    >= 60 and <= 75 => "Idoso ativo",
    >= 76 and <= 90 => "Idoso",
    > 90 => "Idoso longevo",
    _ => "Idade inválida"
};

Console.WriteLine($"📊 Classificação detalhada: {classificacaoDetalhada}");

// Informações adicionais baseadas na idade
Console.WriteLine();
Console.WriteLine("ℹ️  Informações relevantes:");

switch (idade)
{
    case <= 15:
        Console.WriteLine("   • Ensino fundamental");
        Console.WriteLine("   • Não pode votar");
        Console.WriteLine("   • Não pode dirigir");
        break;
    
    case 16 or 17:
        Console.WriteLine("   • Ensino médio");
        Console.WriteLine("   • Voto facultativo");
        Console.WriteLine("   • Não pode dirigir");
        break;
    
    case >= 18 and < 70:
        Console.WriteLine("   • Pode votar (obrigatório)");
        Console.WriteLine("   • Pode dirigir");
        Console.WriteLine("   • Responsabilidade penal completa");
        break;
    
    case >= 70:
        Console.WriteLine("   • Voto facultativo");
        Console.WriteLine("   • Direitos de transporte gratuito");
        Console.WriteLine("   • Prioridade em atendimentos");
        break;
}

// Expectativa de vida
Console.WriteLine();
if (idade < 80)
{
    int anosRestantes = 80 - idade;
    Console.WriteLine($"⏰ Expectativa: aproximadamente {anosRestantes} anos pela frente");
}

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. if-else if-else:
 *    - Avalia condições em ordem
 *    - Para na primeira condição verdadeira
 *    - Use quando as condições são mutuamente exclusivas
 * 
 *    IMPORTANTE: Ordem importa!
 *    if (idade <= 12)      ← Testa primeiro
 *    else if (idade <= 17) ← Só testa se idade > 12
 *    else if (idade <= 59) ← Só testa se idade > 17
 * 
 * 2. Operador Ternário:
 *    condicao ? valorSeTrue : valorSeFalse
 *    
 *    Pode ser aninhado, mas cuidado com legibilidade:
 *    x ? a : y ? b : z ? c : d  ← Difícil de ler!
 * 
 * 3. Switch Expression com Relational Patterns:
 *    idade switch
 *    {
 *        <= 12 => ...,     ← Menor ou igual
 *        > 12 and <= 17 => ← Range com 'and'
 *        _ => ...          ← Default
 *    }
 * 
 * 4. Pattern Matching Avançado:
 *    - Relational: <, <=, >, >=
 *    - Logical: and, or, not
 *    - Type: is string, is int
 *    - Property: { Idade: > 18 }
 * 
 * ═══════════════════════════════════════════════════════════
 * COMPARAÇÃO DAS ABORDAGENS
 * ═══════════════════════════════════════════════════════════
 * 
 * | Abordagem      | Legibilidade | Modernidade | Flexibilidade |
 * |----------------|--------------|-------------|---------------|
 * | if-else if     | ⭐⭐⭐⭐⭐  | ⭐⭐        | ⭐⭐⭐⭐⭐    |
 * | Ternário       | ⭐⭐⭐      | ⭐⭐⭐      | ⭐⭐⭐        |
 * | Switch Expr    | ⭐⭐⭐⭐⭐  | ⭐⭐⭐⭐⭐  | ⭐⭐⭐⭐      |
 * 
 * QUANDO USAR CADA UM:
 * 
 * if-else if:
 * ✅ Condições complexas
 * ✅ Múltiplas variáveis
 * ✅ Lógica dentro dos blocos
 * 
 * Ternário:
 * ✅ Atribuições simples
 * ✅ Código inline
 * ❌ Evite aninhamentos profundos
 * 
 * Switch Expression:
 * ✅ C# 8+ (moderno)
 * ✅ Múltiplas comparações
 * ✅ Pattern matching
 * ✅ Retorno direto de valor
 * 
 * ═══════════════════════════════════════════════════════════
 * VALIDAÇÃO DE ENTRADA
 * ═══════════════════════════════════════════════════════════
 * 
 * int.TryParse() é MELHOR que int.Parse() porque:
 * 
 * // ❌ int.Parse() lança exceção se falhar
 * int idade = int.Parse(entrada);  // Crash se não for número
 * 
 * // ✅ TryParse retorna bool + out parameter
 * if (int.TryParse(entrada, out int idade))
 * {
 *     // Sucesso: idade tem o valor
 * }
 * else
 * {
 *     // Falhou: entrada não é número válido
 * }
 * 
 * ═══════════════════════════════════════════════════════════
 * EARLY RETURN PATTERN
 * ═══════════════════════════════════════════════════════════
 * 
 * Em vez de:
 * if (valido)
 * {
 *     // 50 linhas de código
 * }
 * else
 * {
 *     erro();
 * }
 * 
 * Prefira:
 * if (!valido)
 * {
 *     erro();
 *     return;  ← Sai cedo
 * }
 * // 50 linhas sem indentação
 * 
 * Vantagens:
 * ✅ Menos indentação
 * ✅ Mais legível
 * ✅ Lógica de erro no topo
 * 
 * ═══════════════════════════════════════════════════════════
 * EXTENSÕES POSSÍVEIS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. Adicionar mais categorias:
 *    - Escola (fundamental, médio, superior)
 *    - Direitos legais
 *    - Benefícios sociais
 * 
 * 2. Calcular idade em anos/meses/dias:
 *    DateTime nascimento = new DateTime(2000, 5, 15);
 *    TimeSpan diferenca = DateTime.Now - nascimento;
 * 
 * 3. Verificar maioridade em outros países:
 *    USA: 21 para bebidas alcoólicas
 *    Japão: 20 para maioridade
 * 
 * ═══════════════════════════════════════════════════════════
 */