namespace CursoCSharp.Dia02.Referencias;

/// <summary>
/// EXERCÍCIO 2 - ref, out e Tuplas
/// 
/// Demonstra diferentes formas de retornar múltiplos valores:
/// - ref: Modificar variável existente
/// - out: Retornar múltiplos valores
/// - Tuplas: Alternativa moderna ao out
/// </summary>

// =============================================
// CALCULADORA COM ref e out
// =============================================
public class Calculadora
{
    // ═══════════════════════════════════════
    // MÉTODOS COM ref
    // ═══════════════════════════════════════

    /// <summary>
    /// Dobra o valor da variável passada
    /// </summary>
    public void Dobrar(ref int numero)
    {
        Console.WriteLine($"   Antes: {numero}");
        numero *= 2;
        Console.WriteLine($"   Depois: {numero}");
    }

    /// <summary>
    /// Troca os valores de duas variáveis
    /// </summary>
    public void Trocar(ref int a, ref int b)
    {
        Console.WriteLine($"   Antes: a={a}, b={b}");
        int temp = a;
        a = b;
        b = temp;
        Console.WriteLine($"   Depois: a={a}, b={b}");
    }

    /// <summary>
    /// Incrementa um contador (demonstração de ref)
    /// </summary>
    public void Incrementar(ref int contador)
    {
        contador++;
    }

    // ═══════════════════════════════════════
    // MÉTODOS COM out
    // ═══════════════════════════════════════

    /// <summary>
    /// Divide dois números retornando quociente e resto
    /// </summary>
    public void Dividir(int a, int b, out int quociente, out int resto)
    {
        if (b == 0)
        {
            quociente = 0;
            resto = 0;
            Console.WriteLine("   ⚠️  Divisão por zero!");
            return;
        }

        quociente = a / b;
        resto = a % b;
        Console.WriteLine($"   {a} ÷ {b} = {quociente} (resto {resto})");
    }

    /// <summary>
    /// Tenta converter string para int
    /// Retorna bool indicando sucesso, valor via out
    /// </summary>
    public bool ConverterParaInt(string texto, out int resultado)
    {
        bool sucesso = int.TryParse(texto, out resultado);

        if (sucesso)
            Console.WriteLine($"   ✅ '{texto}' convertido para {resultado}");
        else
            Console.WriteLine($"   ❌ '{texto}' não é um número válido");

        return sucesso;
    }

    /// <summary>
    /// Calcula estatísticas básicas de um array
    /// </summary>
    public void CalcularEstatisticas(int[] numeros, out double media, out int minimo, out int maximo)
    {
        if (numeros == null || numeros.Length == 0)
        {
            media = 0;
            minimo = 0;
            maximo = 0;
            return;
        }

        media = numeros.Average();
        minimo = numeros.Min();
        maximo = numeros.Max();

        Console.WriteLine($"   Média: {media:F2}, Min: {minimo}, Max: {maximo}");
    }

    // ═══════════════════════════════════════
    // MÉTODOS COM TUPLAS (Alternativa ao out)
    // ═══════════════════════════════════════

    /// <summary>
    /// Divide usando tupla (mais moderno que out)
    /// </summary>
    public (int Quociente, int Resto) DividirComTupla(int a, int b)
    {
        if (b == 0)
        {
            Console.WriteLine("   ⚠️  Divisão por zero!");
            return (0, 0);
        }

        Console.WriteLine($"   {a} ÷ {b} = {a / b} (resto {a % b})");
        return (a / b, a % b);
    }

    /// <summary>
    /// Converte para int usando tupla
    /// </summary>
    public (bool Sucesso, int Valor) ConverterComTupla(string texto)
    {
        bool sucesso = int.TryParse(texto, out int valor);

        if (sucesso)
            Console.WriteLine($"   ✅ '{texto}' convertido para {valor}");
        else
            Console.WriteLine($"   ❌ '{texto}' não é um número válido");

        return (sucesso, valor);
    }

    /// <summary>
    /// Estatísticas usando tupla nomeada
    /// </summary>
    public (double Media, int Minimo, int Maximo, int Soma) CalcularEstatisticasCompletas(int[] numeros)
    {
        if (numeros == null || numeros.Length == 0)
            return (0, 0, 0, 0);

        var resultado = (
            Media: numeros.Average(),
            Minimo: numeros.Min(),
            Maximo: numeros.Max(),
            Soma: numeros.Sum()
        );

        Console.WriteLine($"   Média: {resultado.Media:F2}, Min: {resultado.Minimo}, Max: {resultado.Maximo}, Soma: {resultado.Soma}");
        return resultado;
    }

    /// <summary>
    /// Resolver equação de segundo grau: ax² + bx + c = 0
    /// </summary>
    public (bool TemSolucao, double? X1, double? X2) ResolverEquacaoSegundoGrau(double a, double b, double c)
    {
        if (a == 0)
        {
            Console.WriteLine("   ❌ Não é equação de segundo grau (a = 0)");
            return (false, null, null);
        }

        double delta = b * b - 4 * a * c;

        if (delta < 0)
        {
            Console.WriteLine("   ❌ Sem solução real (delta < 0)");
            return (false, null, null);
        }

        if (delta == 0)
        {
            double x = -b / (2 * a);
            Console.WriteLine($"   ✅ Uma solução: x = {x:F2}");
            return (true, x, null);
        }

        double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
        double x2 = (-b - Math.Sqrt(delta)) / (2 * a);
        Console.WriteLine($"   ✅ Duas soluções: x1 = {x1:F2}, x2 = {x2:F2}");
        return (true, x1, x2);
    }
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaRefOut
{
    public static void Main()
    {
        var calc = new Calculadora();

        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("        ref, out e TUPLAS");
        Console.WriteLine("═══════════════════════════════════════\n");

        TestarRef(calc);
        Console.WriteLine();

        TestarOut(calc);
        Console.WriteLine();

        TestarTuplas(calc);
        Console.WriteLine();

        CompararAbordagens(calc);
        Console.WriteLine();

        ExemplosAvancados(calc);
    }

    static void TestarRef(Calculadora calc)
    {
        Console.WriteLine("=== TESTANDO ref ===\n");

        // Dobrar
        Console.WriteLine("1. Dobrar:");
        int numero = 10;
        Console.WriteLine($"Valor inicial: {numero}");
        calc.Dobrar(ref numero);
        Console.WriteLine($"Valor final: {numero} ← Variável original modificada!\n");

        // Trocar
        Console.WriteLine("2. Trocar:");
        int a = 5, b = 15;
        Console.WriteLine($"Valores iniciais: a={a}, b={b}");
        calc.Trocar(ref a, ref b);
        Console.WriteLine($"Valores finais: a={a}, b={b} ← Trocados!\n");

        // Incrementar em loop
        Console.WriteLine("3. Incrementar em loop:");
        int contador = 0;
        for (int i = 0; i < 5; i++)
        {
            calc.Incrementar(ref contador);
            Console.WriteLine($"   Iteração {i + 1}: contador = {contador}");
        }
    }

    static void TestarOut(Calculadora calc)
    {
        Console.WriteLine("=== TESTANDO out ===\n");

        // Dividir
        Console.WriteLine("1. Dividir:");
        calc.Dividir(17, 5, out int quociente, out int resto);
        Console.WriteLine($"Resultado: {quociente}, Resto: {resto}\n");

        // Declaração inline (C# 7+)
        Console.WriteLine("2. Dividir com declaração inline:");
        calc.Dividir(100, 7, out int q, out int r);
        Console.WriteLine($"Resultado: {q}, Resto: {r}\n");

        // Converter
        Console.WriteLine("3. Converter:");
        if (calc.ConverterParaInt("123", out int valor1))
        {
            Console.WriteLine($"Valor convertido: {valor1}\n");
        }

        if (calc.ConverterParaInt("abc", out int valor2))
        {
            Console.WriteLine($"Valor convertido: {valor2}");
        }
        else
        {
            Console.WriteLine($"Conversão falhou, valor default: {valor2}\n");
        }

        // Estatísticas
        Console.WriteLine("4. Estatísticas:");
        int[] numeros = { 10, 20, 30, 40, 50 };
        calc.CalcularEstatisticas(numeros, out double media, out int min, out int max);
        Console.WriteLine($"Resultados: Média={media:F2}, Min={min}, Max={max}");
    }

    static void TestarTuplas(Calculadora calc)
    {
        Console.WriteLine("=== TESTANDO TUPLAS ===\n");

        // Dividir com tupla
        Console.WriteLine("1. Dividir com tupla:");
        var resultado = calc.DividirComTupla(17, 5);
        Console.WriteLine($"Resultado: {resultado.Quociente}, Resto: {resultado.Resto}\n");

        // Deconstrução
        Console.WriteLine("2. Com deconstrução:");
        var (quociente, resto) = calc.DividirComTupla(100, 7);
        Console.WriteLine($"Resultado: {quociente}, Resto: {resto}\n");

        // Converter com tupla
        Console.WriteLine("3. Converter com tupla:");
        var (sucesso1, valor1) = calc.ConverterComTupla("456");
        Console.WriteLine($"Sucesso: {sucesso1}, Valor: {valor1}\n");

        var (sucesso2, valor2) = calc.ConverterComTupla("xyz");
        Console.WriteLine($"Sucesso: {sucesso2}, Valor: {valor2}\n");

        // Estatísticas completas
        Console.WriteLine("4. Estatísticas completas:");
        int[] numeros = { 10, 20, 30, 40, 50 };
        var stats = calc.CalcularEstatisticasCompletas(numeros);
        Console.WriteLine($"Stats: Media={stats.Media:F2}, Min={stats.Minimo}, Max={stats.Maximo}, Soma={stats.Soma}");
    }

    static void CompararAbordagens(Calculadora calc)
    {
        Console.WriteLine("=== COMPARANDO ABORDAGENS ===\n");

        Console.WriteLine("Dividir 25 por 4:\n");

        // Abordagem 1: out
        Console.WriteLine("1. Com out:");
        calc.Dividir(25, 4, out int q1, out int r1);
        Console.WriteLine($"   Uso: q1={q1}, r1={r1}\n");

        // Abordagem 2: tupla
        Console.WriteLine("2. Com tupla:");
        var (q2, r2) = calc.DividirComTupla(25, 4);
        Console.WriteLine($"   Uso: q2={q2}, r2={r2}\n");

        Console.WriteLine("💡 QUAL É MELHOR?\n");
        Console.WriteLine("✅ out:");
        Console.WriteLine("   • Padrão clássico de C#");
        Console.WriteLine("   • Muito usado em APIs .NET (TryParse, TryGetValue)");
        Console.WriteLine("   • Bom para 2-3 valores de retorno\n");

        Console.WriteLine("✅ Tuplas:");
        Console.WriteLine("   • Mais moderna (C# 7+)");
        Console.WriteLine("   • Sintaxe mais limpa");
        Console.WriteLine("   • Melhor para 3+ valores");
        Console.WriteLine("   • Nomes descritivos");
        Console.WriteLine("   • Deconstrução elegante");
    }

    static void ExemplosAvancados(Calculadora calc)
    {
        Console.WriteLine("\n=== EXEMPLOS AVANÇADOS ===\n");

        // Equação de segundo grau
        Console.WriteLine("1. Resolver equação: 2x² - 8x + 6 = 0");
        var (temSolucao, x1, x2) = calc.ResolverEquacaoSegundoGrau(2, -8, 6);
        if (temSolucao)
        {
            if (x2.HasValue)
                Console.WriteLine($"   Soluções: x1={x1:F2}, x2={x2:F2}");
            else
                Console.WriteLine($"   Solução única: x={x1:F2}");
        }
        Console.WriteLine();

        Console.WriteLine("2. Resolver equação: x² + 2x + 5 = 0 (sem solução real)");
        calc.ResolverEquacaoSegundoGrau(1, 2, 5);
        Console.WriteLine();

        Console.WriteLine("3. Descartar valores com _:");
        var (_, minimo, _) = calc.CalcularEstatisticasCompletas(new[] { 5, 10, 15, 20 });
        Console.WriteLine($"   Só interessa o mínimo: {minimo}");
    }
}

// =============================================
// GUIDELINES
// =============================================
public class GuidelinesRefOut
{
    public static void Exibir()
    {
        Console.WriteLine("\n═══════════════════════════════════════");
        Console.WriteLine("     QUANDO USAR CADA UM?");
        Console.WriteLine("═══════════════════════════════════════\n");

        Console.WriteLine("📋 ref:");
        Console.WriteLine("   ✅ Quando precisa MODIFICAR uma variável existente");
        Console.WriteLine("   ✅ Swap, increment, update operations");
        Console.WriteLine("   ✅ Performance (evitar cópia de structs grandes)");
        Console.WriteLine("   ⚠️  Variável DEVE ser inicializada antes\n");

        Console.WriteLine("📋 out:");
        Console.WriteLine("   ✅ Retornar múltiplos valores");
        Console.WriteLine("   ✅ Padrão Try* (TryParse, TryGetValue)");
        Console.WriteLine("   ✅ Quando o método GARANTE atribuir um valor");
        Console.WriteLine("   ⚠️  Variável NÃO precisa ser inicializada");
        Console.WriteLine("   ⚠️  DEVE ser atribuída dentro do método\n");

        Console.WriteLine("📋 Tuplas:");
        Console.WriteLine("   ✅ Retornar múltiplos valores (alternativa moderna ao out)");
        Console.WriteLine("   ✅ Valores nomeados (mais legível)");
        Console.WriteLine("   ✅ 3+ valores de retorno");
        Console.WriteLine("   ✅ Deconstrução elegante");
        Console.WriteLine("   ✅ Sem necessidade de declarar variáveis beforehand\n");

        Console.WriteLine("💡 RECOMENDAÇÃO GERAL:");
        Console.WriteLine("   • ref: Para modificar variáveis existentes");
        Console.WriteLine("   • out: APIs legadas ou padrão Try*");
        Console.WriteLine("   • Tuplas: Novos códigos, múltiplos retornos");
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ ref Parameter
 *    - Modificar variável original
 *    - Variável DEVE ser inicializada
 *    - Passa referência (não cópia)
 *    - Use cases: Swap, Increment, Update
 * 
 * ✅ out Parameter
 *    - Retornar múltiplos valores
 *    - Variável NÃO precisa ser inicializada
 *    - DEVE ser atribuída dentro do método
 *    - Padrão Try* (TryParse, TryGetValue)
 *    - Declaração inline (C# 7+)
 * 
 * ✅ Tuplas (C# 7+)
 *    - Alternativa moderna ao out
 *    - Valores nomeados
 *    - Deconstrução
 *    - Mais limpo para 3+ valores
 *    - var (a, b, c) = Metodo()
 * 
 * ✅ Comparação
 *    - ref vs out vs tuplas
 *    - Quando usar cada um
 *    - Vantagens e desvantagens
 * 
 * ✅ Padrões Comuns
 *    - Try* pattern com out
 *    - Múltiplos retornos com tuplas
 *    - Descarte com _
 * 
 * 💡 Evolução do C#:
 *    - out: Clássico
 *    - Tuplas: Moderno (preferir em novos códigos)
 */