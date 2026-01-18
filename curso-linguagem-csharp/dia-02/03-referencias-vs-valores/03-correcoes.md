# 📝 Correções dos Exercícios

## 🎯 Exercício 1

```csharp
namespace CursoCSharp.Dia02.Referencias;

/// <summary>
/// EXERCÍCIO 1 - Comparando Value Types vs Reference Types
/// 
/// Demonstra a diferença fundamental entre:
/// - Struct (value type) - copia o valor
/// - Class (reference type) - copia a referência
/// </summary>

// =============================================
// STRUCT: Value Type
// =============================================
public struct PontoStruct
{
    public int X { get; set; }
    public int Y { get; set; }

    public PontoStruct(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";
}

// =============================================
// CLASS: Reference Type
// =============================================
public class PontoClass
{
    public int X { get; set; }
    public int Y { get; set; }

    public PontoClass(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaComparacaoTipos
{
    public static void Main()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("    VALUE TYPE vs REFERENCE TYPE");
        Console.WriteLine("═══════════════════════════════════════\n");

        TestarStruct();
        Console.WriteLine();
        TestarClass();
        Console.WriteLine();
        TestarComArray();
        Console.WriteLine();
        TestarComMetodo();
        Console.WriteLine();
        CompararPerformance();
    }

    static void TestarStruct()
    {
        Console.WriteLine("=== STRUCT (Value Type) ===");
        Console.WriteLine("Structs são copiados POR VALOR\n");

        // Criar struct original
        var p1 = new PontoStruct(10, 20);
        Console.WriteLine($"p1 criado: {p1}");

        // COPIAR para p2 (copia o VALOR)
        var p2 = p1;
        Console.WriteLine($"p2 = p1: {p2}");

        // Modificar p2
        p2.X = 999;
        p2.Y = 888;
        Console.WriteLine($"\nApós modificar p2:");
        Console.WriteLine($"p1: {p1} ← ORIGINAL não mudou! ✅");
        Console.WriteLine($"p2: {p2} ← Apenas p2 mudou");

        // Explicação
        Console.WriteLine("\n💡 Por que?");
        Console.WriteLine("   p2 é uma CÓPIA INDEPENDENTE de p1");
        Console.WriteLine("   Cada um tem sua própria memória no Stack");
    }

    static void TestarClass()
    {
        Console.WriteLine("=== CLASS (Reference Type) ===");
        Console.WriteLine("Classes são copiadas POR REFERÊNCIA\n");

        // Criar class original
        var p1 = new PontoClass(10, 20);
        Console.WriteLine($"p1 criado: {p1}");

        // COPIAR para p2 (copia a REFERÊNCIA)
        var p2 = p1;
        Console.WriteLine($"p2 = p1: {p2}");

        // Modificar p2
        p2.X = 999;
        p2.Y = 888;
        Console.WriteLine($"\nApós modificar p2:");
        Console.WriteLine($"p1: {p1} ← MUDOU também! ⚠️");
        Console.WriteLine($"p2: {p2} ← p2 mudou");

        // Explicação
        Console.WriteLine("\n💡 Por que?");
        Console.WriteLine("   p1 e p2 apontam para O MESMO OBJETO na memória");
        Console.WriteLine("   São duas variáveis apontando para o mesmo lugar");
        Console.WriteLine("   Heap: [Objeto: X=999, Y=888]");
        Console.WriteLine("          ↑         ↑");
        Console.WriteLine("         p1        p2");
    }

    static void TestarComArray()
    {
        Console.WriteLine("=== ARRAYS (São Reference Types) ===\n");

        // Array de structs
        var arrayStruct = new PontoStruct[3];
        arrayStruct[0] = new PontoStruct(1, 2);
        arrayStruct[1] = new PontoStruct(3, 4);
        arrayStruct[2] = new PontoStruct(5, 6);

        Console.WriteLine("Array de structs:");
        var copiaArrayStruct = arrayStruct; // Copia a REFERÊNCIA do array
        copiaArrayStruct[0] = new PontoStruct(999, 888);

        Console.WriteLine($"arrayStruct[0]: {arrayStruct[0]} ← Mudou! (array é reference type)");
        Console.WriteLine($"copiaArrayStruct[0]: {copiaArrayStruct[0]}");
        Console.WriteLine("\n💡 Array é sempre reference type, mesmo se contiver structs!");

        // Array de classes
        Console.WriteLine("\nArray de classes:");
        var arrayClass = new PontoClass[2];
        arrayClass[0] = new PontoClass(10, 20);
        arrayClass[1] = new PontoClass(30, 40);

        var copiaArrayClass = arrayClass;
        copiaArrayClass[0].X = 999;

        Console.WriteLine($"arrayClass[0]: {arrayClass[0]} ← Mudou!");
        Console.WriteLine($"copiaArrayClass[0]: {copiaArrayClass[0]}");
    }

    static void TestarComMetodo()
    {
        Console.WriteLine("=== PASSAGEM PARA MÉTODOS ===\n");

        // Struct
        var pontoStruct = new PontoStruct(10, 20);
        Console.WriteLine($"Antes: {pontoStruct}");
        ModificarStruct(pontoStruct);
        Console.WriteLine($"Depois: {pontoStruct} ← Não mudou! (passou cópia)");

        Console.WriteLine();

        // Class
        var pontoClass = new PontoClass(10, 20);
        Console.WriteLine($"Antes: {pontoClass}");
        ModificarClass(pontoClass);
        Console.WriteLine($"Depois: {pontoClass} ← Mudou! (passou referência)");
    }

    static void ModificarStruct(PontoStruct p)
    {
        p.X = 999;
        p.Y = 888;
        Console.WriteLine($"Dentro do método: {p}");
    }

    static void ModificarClass(PontoClass p)
    {
        p.X = 999;
        p.Y = 888;
        Console.WriteLine($"Dentro do método: {p}");
    }

    static void CompararPerformance()
    {
        Console.WriteLine("=== COMPARAÇÃO DE PERFORMANCE ===\n");

        const int iteracoes = 1_000_000;

        // Struct
        var sw = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < iteracoes; i++)
        {
            var p = new PontoStruct(i, i);
            var p2 = p; // Cópia rápida
        }
        sw.Stop();
        Console.WriteLine($"Struct: {sw.ElapsedMilliseconds}ms");

        // Class
        sw.Restart();
        for (int i = 0; i < iteracoes; i++)
        {
            var p = new PontoClass(i, i); // Alocação no heap (mais lenta)
            var p2 = p; // Cópia de referência (rápida)
        }
        sw.Stop();
        Console.WriteLine($"Class: {sw.ElapsedMilliseconds}ms");

        Console.WriteLine("\n💡 Structs pequenos são mais rápidos (stack)");
        Console.WriteLine("   Classes requerem alocação no heap e GC");
    }
}

// =============================================
// VISUALIZAÇÃO DE MEMÓRIA
// =============================================
public class VisualizacaoMemoria
{
    public static void Demonstrar()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("        VISUALIZAÇÃO DE MEMÓRIA");
        Console.WriteLine("═══════════════════════════════════════\n");

        Console.WriteLine("=== STRUCT (Stack) ===\n");
        Console.WriteLine("PontoStruct p1 = new(10, 20);");
        Console.WriteLine("PontoStruct p2 = p1;\n");

        Console.WriteLine("┌─────── STACK ───────┐");
        Console.WriteLine("│ p1: { X=10, Y=20 }  │ ← Valor copiado");
        Console.WriteLine("│ p2: { X=10, Y=20 }  │ ← Cópia independente");
        Console.WriteLine("└─────────────────────┘");

        Console.WriteLine("\np2.X = 999;\n");

        Console.WriteLine("┌─────── STACK ───────┐");
        Console.WriteLine("│ p1: { X=10, Y=20 }  │ ← Não muda!");
        Console.WriteLine("│ p2: { X=999, Y=20 } │ ← Só p2 muda");
        Console.WriteLine("└─────────────────────┘\n");

        Console.WriteLine("═══════════════════════════════════════\n");

        Console.WriteLine("=== CLASS (Heap + Stack) ===\n");
        Console.WriteLine("PontoClass p1 = new(10, 20);");
        Console.WriteLine("PontoClass p2 = p1;\n");

        Console.WriteLine("┌─────── STACK ───────┐    ┌─────── HEAP ────────┐");
        Console.WriteLine("│ p1: 0x1234 ─────────┼───→│ { X=10, Y=20 }     │");
        Console.WriteLine("│ p2: 0x1234 ─────────┼───→│ (mesmo objeto)     │");
        Console.WriteLine("└─────────────────────┘    └────────────────────┘");

        Console.WriteLine("\np2.X = 999;\n");

        Console.WriteLine("┌─────── STACK ───────┐    ┌─────── HEAP ────────┐");
        Console.WriteLine("│ p1: 0x1234 ─────────┼───→│ { X=999, Y=20 }    │");
        Console.WriteLine("│ p2: 0x1234 ─────────┼───→│ (ambos veem isso!) │");
        Console.WriteLine("└─────────────────────┘    └────────────────────┘\n");
    }
}

// =============================================
// RESUMO E GUIDELINES
// =============================================
public class Guidelines
{
    public static void Exibir()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("     QUANDO USAR CADA UM?");
        Console.WriteLine("═══════════════════════════════════════\n");

        Console.WriteLine("✅ USE STRUCT quando:");
        Console.WriteLine("   • Tipo pequeno (≤ 16 bytes recomendado)");
        Console.WriteLine("   • Representa um valor único (Point, Color, DateTime)");
        Console.WriteLine("   • Imutável (readonly)");
        Console.WriteLine("   • Raramente usado em arrays grandes");
        Console.WriteLine("   • Performance crítica (muitas alocações)");
        Console.WriteLine("   Exemplos: int, double, DateTime, Point, Color\n");

        Console.WriteLine("✅ USE CLASS quando:");
        Console.WriteLine("   • Tipo grande (> 16 bytes)");
        Console.WriteLine("   • Representa uma entidade (Person, Car, Order)");
        Console.WriteLine("   • Mutável");
        Console.WriteLine("   • Precisa de herança");
        Console.WriteLine("   • Usa polimorfismo");
        Console.WriteLine("   Exemplos: string, List<T>, Customer, Order\n");

        Console.WriteLine("═══════════════════════════════════════\n");

        Console.WriteLine("⚠️  ARMADILHAS COMUNS:\n");
        Console.WriteLine("1. Struct mutável grande → Use class");
        Console.WriteLine("2. Boxing/Unboxing desnecessário → Use List<T> em vez de ArrayList");
        Console.WriteLine("3. Struct em array → Cuidado com cópias");
        Console.WriteLine("4. Passar struct grande por valor → Use 'in' parameter");
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ Value Types (Struct)
 *    - Armazenados no Stack
 *    - Copiados por valor
 *    - Cada variável tem sua própria cópia
 *    - Mais rápidos para tipos pequenos
 * 
 * ✅ Reference Types (Class)
 *    - Armazenados no Heap
 *    - Copiados por referência
 *    - Múltiplas variáveis podem apontar para o mesmo objeto
 *    - Requerem Garbage Collection
 * 
 * ✅ Diferenças em Comportamento
 *    - Atribuição (=)
 *    - Passagem para métodos
 *    - Arrays
 *    - Performance
 * 
 * ✅ Visualização de Memória
 *    - Stack vs Heap
 *    - Como as referências funcionam
 * 
 * ✅ Guidelines de Uso
 *    - Quando usar struct
 *    - Quando usar class
 *    - Armadilhas comuns
 * 
 * 💡 Este é o conceito mais fundamental de C#!
 *    Entender isso evita MUITOS bugs!
 */
```

---

## 🎯 Exercício 2

```csharp
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
```

---

## 🎯 Exercício 4

```csharp
namespace CursoCSharp.Dia02.Referencias;

/// <summary>
/// EXERCÍCIO 4 - Records para Dados Imutáveis
/// 
/// Demonstra:
/// - Records (C# 9+)
/// - Imutabilidade
/// - Comparação por valor
/// - with expressions
/// - Deconstrução
/// </summary>

// =============================================
// VERSÃO 1: Record Básico
// =============================================
public record Pessoa(string Nome, string CPF, DateTime DataNascimento);

// =============================================
// VERSÃO 2: Record com Properties Calculadas
// =============================================
public record PessoaCompleta(string Nome, string CPF, DateTime DataNascimento)
{
    // Property calculada
    public int Idade
    {
        get
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;
            return idade;
        }
    }

    // Método para criar cópia com nome alterado
    public PessoaCompleta ComNome(string novoNome)
    {
        return this with { Nome = novoNome };
    }

    // Método para verificar maioridade
    public bool EhMaiorDeIdade() => Idade >= 18;

    // Categoria por idade
    public string Categoria => Idade switch
    {
        < 13 => "Criança",
        < 18 => "Adolescente",
        < 60 => "Adulto",
        _ => "Idoso"
    };
}

// =============================================
// VERSÃO 3: Record com Validação
// =============================================
public record PessoaValidada
{
    public string Nome { get; init; }
    public string CPF { get; init; }
    public DateTime DataNascimento { get; init; }

    public PessoaValidada(string nome, string cpf, DateTime dataNascimento)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome não pode ser vazio", nameof(nome));

        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            throw new ArgumentException("CPF inválido", nameof(cpf));

        if (dataNascimento > DateTime.Today)
            throw new ArgumentException("Data de nascimento não pode ser futura", nameof(dataNascimento));

        Nome = nome;
        CPF = cpf;
        DataNascimento = dataNascimento;
    }

    public int Idade
    {
        get
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;
            return idade;
        }
    }

    public PessoaValidada ComNome(string novoNome)
    {
        return new PessoaValidada(novoNome, CPF, DataNascimento);
    }

    public PessoaValidada ComIdade(int novaIdade)
    {
        var novaData = DateTime.Today.AddYears(-novaIdade);
        return new PessoaValidada(Nome, CPF, novaData);
    }
}

// =============================================
// VERSÃO 4: Record Class vs Record Struct
// =============================================

// Record Class (padrão) - Reference Type
public record class PessoaRecordClass(string Nome, int Idade);

// Record Struct (C# 10+) - Value Type
public record struct PessoaRecordStruct(string Nome, int Idade);

// =============================================
// EXEMPLOS AVANÇADOS DE RECORDS
// =============================================

// Record com herança
public record PessoaBase(string Nome, DateTime DataNascimento);
public record Funcionario(string Nome, DateTime DataNascimento, string Cargo, decimal Salario)
    : PessoaBase(Nome, DataNascimento);

// Record com propriedades adicionais
public record Endereco
{
    public string Rua { get; init; }
    public int Numero { get; init; }
    public string Cidade { get; init; }
    public string Estado { get; init; }
    public string CEP { get; init; }

    public Endereco(string rua, int numero, string cidade, string estado, string cep)
    {
        Rua = rua;
        Numero = numero;
        Cidade = cidade;
        Estado = estado;
        CEP = cep;
    }

    // Override ToString para formatação customizada
    public override string ToString()
    {
        return $"{Rua}, {Numero} - {Cidade}/{Estado} - CEP: {CEP}";
    }
}

// Record complexo com outro record
public record Cliente
{
    public string Nome { get; init; }
    public string Email { get; init; }
    public Endereco Endereco { get; init; }
    public DateTime DataCadastro { get; init; }

    public Cliente(string nome, string email, Endereco endereco)
    {
        Nome = nome;
        Email = email;
        Endereco = endereco;
        DataCadastro = DateTime.Now;
    }

    public int AnosCadastrado => (DateTime.Now - DataCadastro).Days / 365;
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaRecords
{
    public static void Main()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("           RECORDS EM C#");
        Console.WriteLine("═══════════════════════════════════════\n");

        TestarRecordBasico();
        Console.WriteLine();

        TestarComparacaoPorValor();
        Console.WriteLine();

        TestarWithExpressions();
        Console.WriteLine();

        TestarDesconstrucao();
        Console.WriteLine();

        TestarRecordCompleto();
        Console.WriteLine();

        TestarRecordValidado();
        Console.WriteLine();

        CompararRecordClassVsRecordStruct();
        Console.WriteLine();

        TestarHeranca();
        Console.WriteLine();

        TestarRecordComplexo();
    }

    static void TestarRecordBasico()
    {
        Console.WriteLine("=== RECORD BÁSICO ===\n");

        // Criação simples
        var pessoa1 = new Pessoa("João Silva", "12345678901", new DateTime(1990, 5, 15));

        // ToString automático (todos os valores)
        Console.WriteLine($"pessoa1: {pessoa1}");
        Console.WriteLine($"Nome: {pessoa1.Nome}");
        Console.WriteLine($"CPF: {pessoa1.CPF}");
        Console.WriteLine($"Data Nascimento: {pessoa1.DataNascimento:dd/MM/yyyy}");
    }

    static void TestarComparacaoPorValor()
    {
        Console.WriteLine("=== COMPARAÇÃO POR VALOR ===\n");

        var pessoa1 = new Pessoa("Maria Santos", "98765432109", new DateTime(1985, 10, 20));
        var pessoa2 = new Pessoa("Maria Santos", "98765432109", new DateTime(1985, 10, 20));
        var pessoa3 = new Pessoa("Pedro Oliveira", "11122233344", new DateTime(1995, 3, 8));

        Console.WriteLine($"pessoa1: {pessoa1}");
        Console.WriteLine($"pessoa2: {pessoa2}");
        Console.WriteLine($"pessoa3: {pessoa3}\n");

        // Comparação por valor (não por referência!)
        Console.WriteLine($"pessoa1 == pessoa2: {pessoa1 == pessoa2} ← Mesmos valores!");
        Console.WriteLine($"pessoa1 == pessoa3: {pessoa1 == pessoa3} ← Valores diferentes");
        Console.WriteLine($"ReferenceEquals(pessoa1, pessoa2): {ReferenceEquals(pessoa1, pessoa2)} ← Objetos diferentes\n");

        Console.WriteLine("💡 Records comparam por VALOR, não por referência!");
        Console.WriteLine("   Classes normais comparam por referência.");
    }

    static void TestarWithExpressions()
    {
        Console.WriteLine("=== with EXPRESSIONS ===\n");

        var pessoa1 = new Pessoa("Ana Costa", "55566677788", new DateTime(1992, 7, 12));
        Console.WriteLine($"Original: {pessoa1}\n");

        // Criar cópia modificando apenas o nome
        var pessoa2 = pessoa1 with { Nome = "Ana Costa Silva" };
        Console.WriteLine($"Com nome alterado: {pessoa2}");
        Console.WriteLine($"Original ainda: {pessoa1}\n");

        // Modificar múltiplas propriedades
        var pessoa3 = pessoa1 with
        {
            Nome = "Ana Beatriz Costa",
            DataNascimento = new DateTime(1993, 8, 20)
        };
        Console.WriteLine($"Múltiplas alterações: {pessoa3}\n");

        Console.WriteLine("💡 'with' cria uma NOVA instância (imutabilidade)");
        Console.WriteLine("   Original permanece inalterado.");
    }

    static void TestarDesconstrucao()
    {
        Console.WriteLine("=== DECONSTRUÇÃO ===\n");

        var pessoa = new Pessoa("Carlos Lima", "99988877766", new DateTime(1988, 12, 25));

        // Deconstruir em variáveis separadas
        var (nome, cpf, data) = pessoa;

        Console.WriteLine($"Pessoa completa: {pessoa}\n");
        Console.WriteLine("Deconstruída:");
        Console.WriteLine($"  Nome: {nome}");
        Console.WriteLine($"  CPF: {cpf}");
        Console.WriteLine($"  Data: {data:dd/MM/yyyy}\n");

        // Descartar valores com _
        var (nomeApenas, _, _) = pessoa;
        Console.WriteLine($"Só o nome: {nomeApenas}");
    }

    static void TestarRecordCompleto()
    {
        Console.WriteLine("=== RECORD COM PROPERTIES CALCULADAS ===\n");

        var pessoa = new PessoaCompleta(
            "Beatriz Alves",
            "44455566677",
            new DateTime(2000, 3, 15)
        );

        Console.WriteLine($"Nome: {pessoa.Nome}");
        Console.WriteLine($"Idade: {pessoa.Idade} anos");
        Console.WriteLine($"Categoria: {pessoa.Categoria}");
        Console.WriteLine($"Maior de idade: {(pessoa.EhMaiorDeIdade() ? "Sim" : "Não")}\n");

        // Usar método ComNome
        var pessoaCasada = pessoa.ComNome("Beatriz Alves Silva");
        Console.WriteLine($"Após casamento: {pessoaCasada.Nome}");
        Console.WriteLine($"Original: {pessoa.Nome} ← Não mudou!");
    }

    static void TestarRecordValidado()
    {
        Console.WriteLine("=== RECORD COM VALIDAÇÃO ===\n");

        try
        {
            var pessoa1 = new PessoaValidada(
                "Ricardo Souza",
                "33344455566",
                new DateTime(1995, 8, 10)
            );
            Console.WriteLine($"✅ Pessoa válida: {pessoa1.Nome}, Idade: {pessoa1.Idade}");
            Console.WriteLine();

            // Criar variação
            var pessoa2 = pessoa1.ComNome("Ricardo Souza Jr.");
            Console.WriteLine($"✅ Com novo nome: {pessoa2.Nome}");
            Console.WriteLine();

            var pessoa3 = pessoa1.ComIdade(30);
            Console.WriteLine($"✅ Com nova idade: {pessoa3.Idade} anos");
            Console.WriteLine();

            // Tentar criar pessoa inválida
            var pessoaInvalida = new PessoaValidada("", "123", DateTime.Today.AddDays(1));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Erro: {ex.Message}");
        }
    }

    static void CompararRecordClassVsRecordStruct()
    {
        Console.WriteLine("=== RECORD CLASS vs RECORD STRUCT ===\n");

        // Record Class (reference type)
        var p1Class = new PessoaRecordClass("João", 30);
        var p2Class = p1Class; // Copia a referência
        Console.WriteLine($"Record Class - p1: {p1Class}");
        Console.WriteLine($"Record Class - p2: {p2Class}");
        Console.WriteLine($"São o mesmo objeto? {ReferenceEquals(p1Class, p2Class)}\n");

        // Record Struct (value type)
        var p1Struct = new PessoaRecordStruct("Maria", 25);
        var p2Struct = p1Struct; // Copia o valor
        Console.WriteLine($"Record Struct - p1: {p1Struct}");
        Console.WriteLine($"Record Struct - p2: {p2Struct}");
        Console.WriteLine($"São o mesmo objeto? {ReferenceEquals(p1Struct, p2Struct)}\n");

        Console.WriteLine("💡 Record Class:");
        Console.WriteLine("   • Reference type (padrão)");
        Console.WriteLine("   • Alocado no Heap");
        Console.WriteLine("   • Comparação por valor");
        Console.WriteLine("   • Ideal para DTOs\n");

        Console.WriteLine("💡 Record Struct:");
        Console.WriteLine("   • Value type");
        Console.WriteLine("   • Alocado no Stack");
        Console.WriteLine("   • Comparação por valor");
        Console.WriteLine("   • Ideal para dados pequenos e imutáveis");
    }

    static void TestarHeranca()
    {
        Console.WriteLine("=== HERANÇA COM RECORDS ===\n");

        var pessoa = new PessoaBase("Fernanda Lima", new DateTime(1992, 6, 18));
        var funcionario = new Funcionario(
            "Carlos Mendes",
            new DateTime(1988, 4, 22),
            "Desenvolvedor",
            8000
        );

        Console.WriteLine($"Pessoa: {pessoa}");
        Console.WriteLine($"Funcionário: {funcionario}\n");

        // with expressions funcionam com herança
        var funcionarioPromovido = funcionario with { Cargo = "Tech Lead", Salario = 12000 };
        Console.WriteLine($"Promovido: {funcionarioPromovido}");
    }

    static void TestarRecordComplexo()
    {
        Console.WriteLine("=== RECORD COMPLEXO ===\n");

        // Criar endereço
        var endereco = new Endereco(
            "Av. Paulista",
            1000,
            "São Paulo",
            "SP",
            "01310-100"
        );

        // Criar cliente com endereço
        var cliente = new Cliente(
            "Paula Rodrigues",
            "paula@email.com",
            endereco
        );

        Console.WriteLine($"Cliente: {cliente.Nome}");
        Console.WriteLine($"Email: {cliente.Email}");
        Console.WriteLine($"Endereço: {cliente.Endereco}");
        Console.WriteLine($"Anos cadastrado: {cliente.AnosCadastrado}\n");

        // Alterar endereço (with aninhado)
        var clienteMudou = cliente with
        {
            Endereco = endereco with { Numero = 2000 }
        };

        Console.WriteLine("Após mudança:");
        Console.WriteLine($"Cliente: {clienteMudou.Nome}");
        Console.WriteLine($"Novo endereço: {clienteMudou.Endereco}\n");
        Console.WriteLine($"Original: {cliente.Endereco} ← Não mudou!");
    }
}

// =============================================
// COMPARAÇÃO: Class vs Record
// =============================================
public class ComparacaoClassVsRecord
{
    // Class tradicional
    public class PessoaClass
    {
        public string Nome { get; set; }
        public int Idade { get; set; }

        // Precisa implementar manualmente
        public override bool Equals(object obj)
        {
            if (obj is not PessoaClass other) return false;
            return Nome == other.Nome && Idade == other.Idade;
        }

        public override int GetHashCode() => HashCode.Combine(Nome, Idade);
        public override string ToString() => $"PessoaClass {{ Nome = {Nome}, Idade = {Idade} }}";
    }

    // Record - tudo automático!
    public record PessoaRecord(string Nome, int Idade);

    public static void Comparar()
    {
        Console.WriteLine("═══ CLASS vs RECORD ═══\n");

        Console.WriteLine("Class:");
        Console.WriteLine("  • Precisa implementar Equals, GetHashCode, ToString");
        Console.WriteLine("  • Mutável por padrão");
        Console.WriteLine("  • Comparação por referência\n");

        Console.WriteLine("Record:");
        Console.WriteLine("  • Equals, GetHashCode, ToString automáticos ✅");
        Console.WriteLine("  • Imutável por padrão (init) ✅");
        Console.WriteLine("  • Comparação por valor ✅");
        Console.WriteLine("  • with expressions ✅");
        Console.WriteLine("  • Deconstrução automática ✅");
        Console.WriteLine("  • Sintaxe concisa ✅");
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ Records (C# 9+)
 *    - Sintaxe concisa para DTOs
 *    - Imutabilidade por padrão (init)
 *    - Comparação por valor automática
 *    - ToString, Equals, GetHashCode automáticos
 * 
 * ✅ with Expressions
 *    - Criar cópias modificadas
 *    - Preserva imutabilidade
 *    - Sintaxe elegante
 * 
 * ✅ Deconstrução
 *    - Extrair valores facilmente
 *    - var (a, b, c) = record
 *    - Descartar com _
 * 
 * ✅ Properties Calculadas
 *    - Idade baseada em data de nascimento
 *    - Categorização dinâmica
 * 
 * ✅ Validação
 *    - Possível em construtores
 *    - Métodos para criar variações
 * 
 * ✅ Record Class vs Record Struct
 *    - Reference type vs Value type
 *    - Quando usar cada um
 * 
 * ✅ Herança
 *    - Records podem herdar de outros records
 *    - with funciona com herança
 * 
 * ✅ Records Complexos
 *    - Records dentro de records
 *    - with aninhado
 * 
 * 💡 QUANDO USAR RECORDS:
 *    • DTOs (Data Transfer Objects)
 *    • Value Objects
 *    • Dados imutáveis
 *    • Comparação por valor necessária
 *    • APIs e serialização
 */
```

---

## 🎯 Exercício 10

```csharp
namespace CursoCSharp.Dia02.Referencias;

/// <summary>
/// EXERCÍCIO 10 - Sistema de Geometria (PROJETO FINAL)
/// 
/// Sistema completo demonstrando:
/// - Structs para pontos (value types)
/// - Records para cores (imutabilidade)
/// - Classes para formas (reference types)
/// - in parameter para performance
/// - Tuplas para múltiplos retornos
/// - Integração de todos os conceitos
/// </summary>

// =============================================
// STRUCT: Ponto2D (Value Type)
// =============================================
public struct Ponto2D
{
    public double X { get; init; }
    public double Y { get; init; }

    public Ponto2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    // Calcular distância para outro ponto (usando 'in' para performance)
    public double DistanciaPara(in Ponto2D outro)
    {
        double dx = X - outro.X;
        double dy = Y - outro.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    // Mover o ponto (retorna novo ponto - imutável)
    public Ponto2D Mover(double deltaX, double deltaY)
    {
        return new Ponto2D(X + deltaX, Y + deltaY);
    }

    // Rotacionar ao redor da origem
    public Ponto2D Rotacionar(double anguloGraus)
    {
        double anguloRad = anguloGraus * Math.PI / 180.0;
        double cos = Math.Cos(anguloRad);
        double sin = Math.Sin(anguloRad);

        return new Ponto2D(
            X * cos - Y * sin,
            X * sin + Y * cos
        );
    }

    public override string ToString() => $"({X:F2}, {Y:F2})";

    // Operadores
    public static Ponto2D operator +(Ponto2D a, Ponto2D b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Ponto2D operator -(Ponto2D a, Ponto2D b)
        => new(a.X - b.X, a.Y - b.Y);
}

// =============================================
// STRUCT: Ponto3D (Value Type)
// =============================================
public struct Ponto3D
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }

    public Ponto3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double DistanciaPara(in Ponto3D outro)
    {
        double dx = X - outro.X;
        double dy = Y - outro.Y;
        double dz = Z - outro.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public Ponto3D Mover(double deltaX, double deltaY, double deltaZ)
    {
        return new Ponto3D(X + deltaX, Y + deltaY, Z + deltaZ);
    }

    public override string ToString() => $"({X:F2}, {Y:F2}, {Z:F2})";
}

// =============================================
// RECORD: Cor (Imutável)
// =============================================
public record Cor(byte R, byte G, byte B)
{
    // Converter para hexadecimal
    public string ToHex() => $"#{R:X2}{G:X2}{B:X2}";

    // Converter para string amigável
    public override string ToString() => $"RGB({R}, {G}, {B})";

    // Cores pré-definidas
    public static Cor Vermelho => new(255, 0, 0);
    public static Cor Verde => new(0, 255, 0);
    public static Cor Azul => new(0, 0, 255);
    public static Cor Amarelo => new(255, 255, 0);
    public static Cor Magenta => new(255, 0, 255);
    public static Cor Ciano => new(0, 255, 255);
    public static Cor Preto => new(0, 0, 0);
    public static Cor Branco => new(255, 255, 255);

    // Criar cor aleatória
    public static Cor Aleatoria()
    {
        var random = new Random();
        return new Cor(
            (byte)random.Next(256),
            (byte)random.Next(256),
            (byte)random.Next(256)
        );
    }

    // Clarear cor
    public Cor Clarear(double fator = 1.2)
    {
        return new Cor(
            (byte)Math.Min(255, R * fator),
            (byte)Math.Min(255, G * fator),
            (byte)Math.Min(255, B * fator)
        );
    }

    // Escurecer cor
    public Cor Escurecer(double fator = 0.8)
    {
        return new Cor(
            (byte)(R * fator),
            (byte)(G * fator),
            (byte)(B * fator)
        );
    }
}

// =============================================
// CLASS: FormaGeometrica (Base)
// =============================================
public abstract class FormaGeometrica
{
    public string Nome { get; set; }
    public Cor CorPreenchimento { get; set; }
    public Ponto2D Centro { get; set; }

    protected FormaGeometrica(string nome, Cor cor, Ponto2D centro)
    {
        Nome = nome;
        CorPreenchimento = cor;
        Centro = centro;
    }

    // Métodos abstratos (devem ser implementados)
    public abstract double CalcularArea();
    public abstract double CalcularPerimetro();

    // Método concreto
    public void Mover(double deltaX, double deltaY)
    {
        Centro = Centro.Mover(deltaX, deltaY);
    }

    public virtual void ExibirInformacoes()
    {
        Console.WriteLine($"  🔹 {Nome}");
        Console.WriteLine($"     Centro: {Centro}");
        Console.WriteLine($"     Cor: {CorPreenchimento}");
        Console.WriteLine($"     Área: {CalcularArea():F2}");
        Console.WriteLine($"     Perímetro: {CalcularPerimetro():F2}");
    }
}

// =============================================
// CLASS: Circulo
// =============================================
public class Circulo : FormaGeometrica
{
    public double Raio { get; set; }

    public Circulo(string nome, Cor cor, Ponto2D centro, double raio)
        : base(nome, cor, centro)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(raio));
        Raio = raio;
    }

    public override double CalcularArea()
        => Math.PI * Raio * Raio;

    public override double CalcularPerimetro()
        => 2 * Math.PI * Raio;

    // Verificar se ponto está dentro do círculo
    public bool ContemPonto(in Ponto2D ponto)
    {
        return Centro.DistanciaPara(ponto) <= Raio;
    }

    public override void ExibirInformacoes()
    {
        base.ExibirInformacoes();
        Console.WriteLine($"     Raio: {Raio:F2}");
        Console.WriteLine($"     Diâmetro: {Raio * 2:F2}");
    }
}

// =============================================
// CLASS: Retangulo
// =============================================
public class Retangulo : FormaGeometrica
{
    public double Largura { get; set; }
    public double Altura { get; set; }

    public Retangulo(string nome, Cor cor, Ponto2D centro, double largura, double altura)
        : base(nome, cor, centro)
    {
        if (largura <= 0)
            throw new ArgumentException("Largura deve ser positiva", nameof(largura));
        if (altura <= 0)
            throw new ArgumentException("Altura deve ser positiva", nameof(altura));

        Largura = largura;
        Altura = altura;
    }

    public override double CalcularArea()
        => Largura * Altura;

    public override double CalcularPerimetro()
        => 2 * (Largura + Altura);

    // Retornar os 4 cantos do retângulo
    public (Ponto2D SuperiorEsquerdo, Ponto2D SuperiorDireito,
            Ponto2D InferiorEsquerdo, Ponto2D InferiorDireito) ObterCantos()
    {
        double meiaLargura = Largura / 2;
        double meiaAltura = Altura / 2;

        return (
            SuperiorEsquerdo: new Ponto2D(Centro.X - meiaLargura, Centro.Y + meiaAltura),
            SuperiorDireito: new Ponto2D(Centro.X + meiaLargura, Centro.Y + meiaAltura),
            InferiorEsquerdo: new Ponto2D(Centro.X - meiaLargura, Centro.Y - meiaAltura),
            InferiorDireito: new Ponto2D(Centro.X + meiaLargura, Centro.Y - meiaAltura)
        );
    }

    // Verificar se é quadrado
    public bool EhQuadrado() => Math.Abs(Largura - Altura) < 0.0001;

    public override void ExibirInformacoes()
    {
        base.ExibirInformacoes();
        Console.WriteLine($"     Largura: {Largura:F2}");
        Console.WriteLine($"     Altura: {Altura:F2}");
        Console.WriteLine($"     Tipo: {(EhQuadrado() ? "Quadrado" : "Retângulo")}");
    }
}

// =============================================
// CLASS: Triangulo
// =============================================
public class Triangulo : FormaGeometrica
{
    public Ponto2D P1 { get; set; }
    public Ponto2D P2 { get; set; }
    public Ponto2D P3 { get; set; }

    public Triangulo(string nome, Cor cor, Ponto2D p1, Ponto2D p2, Ponto2D p3)
        : base(nome, cor, CalcularCentro(p1, p2, p3))
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    private static Ponto2D CalcularCentro(Ponto2D p1, Ponto2D p2, Ponto2D p3)
    {
        return new Ponto2D(
            (p1.X + p2.X + p3.X) / 3,
            (p1.Y + p2.Y + p3.Y) / 3
        );
    }

    public override double CalcularArea()
    {
        // Fórmula de Heron
        double a = P1.DistanciaPara(P2);
        double b = P2.DistanciaPara(P3);
        double c = P3.DistanciaPara(P1);
        double s = (a + b + c) / 2; // semi-perímetro
        return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
    }

    public override double CalcularPerimetro()
    {
        return P1.DistanciaPara(P2) +
               P2.DistanciaPara(P3) +
               P3.DistanciaPara(P1);
    }

    public override void ExibirInformacoes()
    {
        base.ExibirInformacoes();
        Console.WriteLine($"     P1: {P1}");
        Console.WriteLine($"     P2: {P2}");
        Console.WriteLine($"     P3: {P3}");
    }
}

// =============================================
// CLASS: GerenciadorFormas
// =============================================
public class GerenciadorFormas
{
    private List<FormaGeometrica> _formas = new();

    public void Adicionar(FormaGeometrica forma)
    {
        _formas.Add(forma);
        Console.WriteLine($"✅ Forma '{forma.Nome}' adicionada");
    }

    public void Remover(FormaGeometrica forma)
    {
        if (_formas.Remove(forma))
            Console.WriteLine($"✅ Forma '{forma.Nome}' removida");
        else
            Console.WriteLine($"❌ Forma '{forma.Nome}' não encontrada");
    }

    // Retornar estatísticas usando tupla
    public (double AreaTotal, double PerimetroTotal, int Quantidade) ObterEstatisticas()
    {
        return (
            AreaTotal: _formas.Sum(f => f.CalcularArea()),
            PerimetroTotal: _formas.Sum(f => f.CalcularPerimetro()),
            Quantidade: _formas.Count
        );
    }

    public void MoverTodas(double deltaX, double deltaY)
    {
        foreach (var forma in _formas)
        {
            forma.Mover(deltaX, deltaY);
        }
        Console.WriteLine($"✅ {_formas.Count} forma(s) movida(s)");
    }

    public List<FormaGeometrica> BuscarPorCor(Cor cor)
    {
        return _formas.Where(f => f.CorPreenchimento == cor).ToList();
    }

    public List<FormaGeometrica> BuscarPorTipo<T>() where T : FormaGeometrica
    {
        return _formas.OfType<T>().ToList();
    }

    // Encontrar forma mais próxima de um ponto
    public (FormaGeometrica Forma, double Distancia)? EncontrarMaisProxima(in Ponto2D ponto)
    {
        if (_formas.Count == 0)
            return null;

        var maisProxima = _formas
            .Select(f => (Forma: f, Distancia: f.Centro.DistanciaPara(ponto)))
            .OrderBy(x => x.Distancia)
            .First();

        return maisProxima;
    }

    public void ExibirResumo()
    {
        Console.WriteLine("\n═══════════════════════════════════════");
        Console.WriteLine("         RESUMO DAS FORMAS");
        Console.WriteLine("═══════════════════════════════════════");

        if (_formas.Count == 0)
        {
            Console.WriteLine("Nenhuma forma cadastrada.");
            return;
        }

        var (areaTotal, perimetroTotal, quantidade) = ObterEstatisticas();

        Console.WriteLine($"Total de formas: {quantidade}");
        Console.WriteLine($"Área total: {areaTotal:F2}");
        Console.WriteLine($"Perímetro total: {perimetroTotal:F2}");
        Console.WriteLine();

        // Agrupar por tipo
        var porTipo = _formas.GroupBy(f => f.GetType().Name);
        foreach (var grupo in porTipo)
        {
            Console.WriteLine($"  {grupo.Key}: {grupo.Count()}");
        }

        Console.WriteLine("\n═══════════════════════════════════════");
    }

    public void ExibirTodasFormas()
    {
        Console.WriteLine("\n═══ TODAS AS FORMAS ═══\n");

        for (int i = 0; i < _formas.Count; i++)
        {
            Console.WriteLine($"[{i + 1}]");
            _formas[i].ExibirInformacoes();
            Console.WriteLine();
        }
    }
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaSistemaGeometria
{
    public static void Main()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("      SISTEMA DE GEOMETRIA");
        Console.WriteLine("═══════════════════════════════════════\n");

        var gerenciador = new GerenciadorFormas();

        // Criar formas
        CriarFormas(gerenciador);
        Console.WriteLine();

        // Exibir todas
        gerenciador.ExibirTodasFormas();

        // Estatísticas
        gerenciador.ExibirResumo();

        // Operações
        Console.WriteLine("\n═══ OPERAÇÕES ═══\n");

        // Mover todas
        Console.WriteLine("Movendo todas as formas (10, 10):");
        gerenciador.MoverTodas(10, 10);
        Console.WriteLine();

        // Buscar por cor
        Console.WriteLine("Buscando formas vermelhas:");
        var vermelhas = gerenciador.BuscarPorCor(Cor.Vermelho);
        Console.WriteLine($"Encontradas: {vermelhas.Count}");
        foreach (var forma in vermelhas)
        {
            Console.WriteLine($"  - {forma.Nome}");
        }
        Console.WriteLine();

        // Buscar por tipo
        Console.WriteLine("Buscando círculos:");
        var circulos = gerenciador.BuscarPorTipo<Circulo>();
        Console.WriteLine($"Encontrados: {circulos.Count}");
        foreach (var circulo in circulos)
        {
            Console.WriteLine($"  - {circulo.Nome} (Raio: {circulo.Raio:F2})");
        }
        Console.WriteLine();

        // Encontrar mais próxima
        var pontoTeste = new Ponto2D(5, 5);
        Console.WriteLine($"Forma mais próxima de {pontoTeste}:");
        var resultado = gerenciador.EncontrarMaisProxima(pontoTeste);
        if (resultado.HasValue)
        {
            Console.WriteLine($"  {resultado.Value.Forma.Nome} - Distância: {resultado.Value.Distancia:F2}");
        }
        Console.WriteLine();

        // Testar structs e records
        TestarStructsERecords();
    }

    static void CriarFormas(GerenciadorFormas gerenciador)
    {
        Console.WriteLine("═══ CRIANDO FORMAS ═══\n");

        // Círculo 1
        var circulo1 = new Circulo(
            "Círculo Vermelho",
            Cor.Vermelho,
            new Ponto2D(0, 0),
            10
        );
        gerenciador.Adicionar(circulo1);

        // Círculo 2
        var circulo2 = new Circulo(
            "Círculo Azul",
            Cor.Azul,
            new Ponto2D(20, 20),
            5
        );
        gerenciador.Adicionar(circulo2);

        // Retângulo 1
        var retangulo1 = new Retangulo(
            "Retângulo Verde",
            Cor.Verde,
            new Ponto2D(10, 10),
            15,
            10
        );
        gerenciador.Adicionar(retangulo1);

        // Quadrado (retângulo especial)
        var quadrado = new Retangulo(
            "Quadrado Amarelo",
            Cor.Amarelo,
            new Ponto2D(30, 30),
            8,
            8
        );
        gerenciador.Adicionar(quadrado);

        // Triângulo
        var triangulo = new Triangulo(
            "Triângulo Magenta",
            Cor.Magenta,
            new Ponto2D(0, 0),
            new Ponto2D(10, 0),
            new Ponto2D(5, 8.66)
        );
        gerenciador.Adicionar(triangulo);
    }

    static void TestarStructsERecords()
    {
        Console.WriteLine("═══ TESTANDO STRUCTS E RECORDS ═══\n");

        // Structs são value types
        Console.WriteLine("1. Structs (Value Types):");
        var p1 = new Ponto2D(10, 20);
        var p2 = p1; // Cópia do valor
        var p3 = p1.Mover(5, 5);

        Console.WriteLine($"   p1: {p1}");
        Console.WriteLine($"   p2: {p2} ← Cópia independente");
        Console.WriteLine($"   p3: {p3} ← Movido");
        Console.WriteLine($"   Distância p1 → p3: {p1.DistanciaPara(p3):F2}\n");

        // Records são imutáveis
        Console.WriteLine("2. Records (Imutáveis):");
        var cor1 = Cor.Vermelho;
        var cor2 = cor1.Clarear();
        var cor3 = cor1.Escurecer();

        Console.WriteLine($"   cor1: {cor1} ({cor1.ToHex()})");
        Console.WriteLine($"   cor2: {cor2} ({cor2.ToHex()}) ← Clareada");
        Console.WriteLine($"   cor3: {cor3} ({cor3.ToHex()}) ← Escurecida");
        Console.WriteLine($"   cor1 == Cor.Vermelho: {cor1 == Cor.Vermelho} ← Comparação por valor!\n");

        // Ponto3D
        Console.WriteLine("3. Ponto3D:");
        var p3d1 = new Ponto3D(1, 2, 3);
        var p3d2 = new Ponto3D(4, 5, 6);
        Console.WriteLine($"   p3d1: {p3d1}");
        Console.WriteLine($"   p3d2: {p3d2}");
        Console.WriteLine($"   Distância: {p3d1.DistanciaPara(p3d2):F2}");
    }
}

/*
 * CONCEITOS DEMONSTRADOS NO PROJETO FINAL:
 * 
 * ✅ Structs para Pontos (Value Types)
 *    - Ponto2D e Ponto3D
 *    - Pequenos (16-24 bytes)
 *    - Imutáveis (init)
 *    - in parameter para performance
 *    - Operadores sobrecarregados
 * 
 * ✅ Records para Cores (Imutabilidade)
 *    - Cor com RGB
 *    - Cores pré-definidas
 *    - Métodos para clarear/escurecer
 *    - Comparação por valor
 *    - Factory methods
 * 
 * ✅ Classes para Formas (Reference Types)
 *    - Herança (FormaGeometrica base)
 *    - Polimorfismo (abstract methods)
 *    - Objetos complexos
 *    - Múltiplas propriedades
 * 
 * ✅ in Parameter
 *    - DistanciaPara(in Ponto2D outro)
 *    - EncontrarMaisProxima(in Ponto2D ponto)
 *    - Performance sem cópias
 * 
 * ✅ Tuplas
 *    - ObterCantos() retorna 4 pontos
 *    - ObterEstatisticas() retorna múltiplos valores
 *    - EncontrarMaisProxima() retorna forma + distância
 *    - Deconstrução elegante
 * 
 * ✅ Integração Completa
 *    - Structs, Records e Classes trabalhando juntos
 *    - Cada tipo usado adequadamente
 *    - Stack (structs) vs Heap (classes)
 *    - Imutabilidade (records e structs com init)
 * 
 * ✅ SOLID Principles (Preview)
 *    - Single Responsibility
 *    - Open/Closed (extensível via herança)
 *    - Liskov Substitution (polimorfismo)
 * 
 * ✅ Performance
 *    - Structs pequenos no stack (rápido)
 *    - in parameter evita cópias
 *    - Records para dados imutáveis (thread-safe)
 * 
 * 🎯 Este exercício integra TODOS os conceitos do Dia 02:
 *    - Classes e Objetos (Dia 02.1)
 *    - Construtores e Sobrecarga (Dia 02.2)
 *    - Referências vs Valores (Dia 02.3)
 *    - Preview de Herança e Polimorfismo (Dia 03)
 */
```

---

