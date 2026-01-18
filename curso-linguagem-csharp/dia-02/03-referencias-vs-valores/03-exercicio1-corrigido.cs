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