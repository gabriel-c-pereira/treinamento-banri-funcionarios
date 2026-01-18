namespace CursoCSharp.Dia02.Construtores;

/// <summary>
/// EXERCÍCIO 7 - Círculo com Constantes e Sobrecarga
/// 
/// Demonstra:
/// - Constantes (const e readonly)
/// - Method overloading
/// - Sobrecarga de operações
/// - Properties calculadas
/// </summary>

// =============================================
// VERSÃO 1: Implementação básica
// =============================================
public class Circulo
{
    // Constante PI (valor não pode mudar)
    public const double PI = 3.14159265359;

    // Property
    public double Raio { get; set; }

    // Construtor
    public Circulo(double raio)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(raio));

        Raio = raio;
    }

    // Métodos sobrecarregados para cálculo de área

    // 1. Área do círculo atual
    public double CalcularArea()
    {
        return PI * Raio * Raio;
    }

    // 2. Área de um círculo com raio específico (método estático)
    public static double CalcularArea(double raio)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(raio));

        return PI * raio * raio;
    }

    // 3. Área de múltiplos círculos
    public static double CalcularArea(params double[] raios)
    {
        double areaTotal = 0;
        foreach (var raio in raios)
        {
            if (raio <= 0)
                throw new ArgumentException("Todos os raios devem ser positivos");

            areaTotal += CalcularArea(raio);
        }
        return areaTotal;
    }

    // Métodos sobrecarregados para perímetro

    public double CalcularPerimetro()
    {
        return 2 * PI * Raio;
    }

    public static double CalcularPerimetro(double raio)
    {
        return 2 * PI * raio;
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"⭕ Círculo");
        Console.WriteLine($"   Raio: {Raio:F2}");
        Console.WriteLine($"   Área: {CalcularArea():F2}");
        Console.WriteLine($"   Perímetro: {CalcularPerimetro():F2}");
    }
}

// =============================================
// VERSÃO 2: Com mais sobrecarga e funcionalidades
// =============================================
public class CirculoAvancado
{
    // Constante estática
    public const double PI = Math.PI;

    // Readonly (definido uma vez, no construtor)
    public readonly string Unidade;

    // Properties
    public double Raio { get; set; }
    public string Cor { get; set; }

    // Properties calculadas
    public double Diametro => Raio * 2;
    public double Area => CalcularArea();
    public double Perimetro => CalcularPerimetro();

    // Construtores sobrecarregados
    public CirculoAvancado(double raio, string unidade = "cm", string cor = "Preto")
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(raio));

        Raio = raio;
        Unidade = unidade; // readonly só pode ser definido aqui
        Cor = cor;
    }

    // Métodos de instância

    public double CalcularArea()
    {
        return PI * Raio * Raio;
    }

    public double CalcularPerimetro()
    {
        return 2 * PI * Raio;
    }

    // Redimensionar - versões sobrecarregadas

    // 1. Por fator multiplicativo
    public void Redimensionar(double fator)
    {
        if (fator <= 0)
            throw new ArgumentException("Fator deve ser positivo", nameof(fator));

        Raio *= fator;
        Console.WriteLine($"✅ Círculo redimensionado por fator {fator:F2}");
    }

    // 2. Para um novo raio absoluto
    public void Redimensionar(double novoRaio, bool absoluto)
    {
        if (!absoluto)
        {
            // Se não é absoluto, trata como fator
            Redimensionar(novoRaio);
            return;
        }

        if (novoRaio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(novoRaio));

        Raio = novoRaio;
        Console.WriteLine($"✅ Círculo redimensionado para raio {novoRaio:F2} {Unidade}");
    }

    // 3. Redimensionar para área específica
    public void RedimensionarParaArea(double areaDesejada)
    {
        if (areaDesejada <= 0)
            throw new ArgumentException("Área deve ser positiva", nameof(areaDesejada));

        // Calcular novo raio: raio = sqrt(area / PI)
        double novoRaio = Math.Sqrt(areaDesejada / PI);
        Raio = novoRaio;
        Console.WriteLine($"✅ Círculo redimensionado para área {areaDesejada:F2} {Unidade}²");
    }

    // Métodos estáticos sobrecarregados

    // Comparar áreas
    public static double CompararAreas(CirculoAvancado c1, CirculoAvancado c2)
    {
        return c1.Area - c2.Area;
    }

    // Criar círculo a partir de área
    public static CirculoAvancado CriarPorArea(double area, string unidade = "cm")
    {
        double raio = Math.Sqrt(area / PI);
        return new CirculoAvancado(raio, unidade);
    }

    // Criar círculo a partir de perímetro
    public static CirculoAvancado CriarPorPerimetro(double perimetro, string unidade = "cm")
    {
        double raio = perimetro / (2 * PI);
        return new CirculoAvancado(raio, unidade);
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"⭕ Círculo {Cor}");
        Console.WriteLine($"   Raio: {Raio:F2} {Unidade}");
        Console.WriteLine($"   Diâmetro: {Diametro:F2} {Unidade}");
        Console.WriteLine($"   Área: {Area:F2} {Unidade}²");
        Console.WriteLine($"   Perímetro: {Perimetro:F2} {Unidade}");
    }
}

// =============================================
// VERSÃO 3: Com operadores sobrecarregados
// =============================================
public class CirculoComOperadores
{
    public const double PI = Math.PI;
    public double Raio { get; init; }

    public CirculoComOperadores(double raio)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo");
        Raio = raio;
    }

    // Properties calculadas
    public double Area => PI * Raio * Raio;
    public double Perimetro => 2 * PI * Raio;

    // Sobrecarga de operadores

    // Operador + (soma de áreas, retorna novo círculo)
    public static CirculoComOperadores operator +(CirculoComOperadores c1, CirculoComOperadores c2)
    {
        double areaTotal = c1.Area + c2.Area;
        double novoRaio = Math.Sqrt(areaTotal / PI);
        return new CirculoComOperadores(novoRaio);
    }

    // Operador - (diferença de áreas)
    public static CirculoComOperadores operator -(CirculoComOperadores c1, CirculoComOperadores c2)
    {
        double areaDiferenca = Math.Abs(c1.Area - c2.Area);
        double novoRaio = Math.Sqrt(areaDiferenca / PI);
        return new CirculoComOperadores(novoRaio);
    }

    // Operador * (multiplica raio por escalar)
    public static CirculoComOperadores operator *(CirculoComOperadores c, double escalar)
    {
        return new CirculoComOperadores(c.Raio * escalar);
    }

    // Operador / (divide raio por escalar)
    public static CirculoComOperadores operator /(CirculoComOperadores c, double escalar)
    {
        if (escalar == 0)
            throw new DivideByZeroException("Não é possível dividir por zero");
        return new CirculoComOperadores(c.Raio / escalar);
    }

    // Operadores de comparação
    public static bool operator >(CirculoComOperadores c1, CirculoComOperadores c2)
        => c1.Area > c2.Area;

    public static bool operator <(CirculoComOperadores c1, CirculoComOperadores c2)
        => c1.Area < c2.Area;

    public static bool operator ==(CirculoComOperadores c1, CirculoComOperadores c2)
        => Math.Abs(c1.Area - c2.Area) < 0.0001; // Comparação com tolerância

    public static bool operator !=(CirculoComOperadores c1, CirculoComOperadores c2)
        => !(c1 == c2);

    public override bool Equals(object obj)
        => obj is CirculoComOperadores c && this == c;

    public override int GetHashCode()
        => Raio.GetHashCode();

    public override string ToString()
        => $"Círculo (R={Raio:F2}, A={Area:F2})";
}

// =============================================
// VERSÃO 4: Record com métodos sobrecarregados
// =============================================
public record CirculoRecord(double Raio)
{
    public const double PI = Math.PI;

    // Validação no construtor
    public CirculoRecord(double raio) : this(raio)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo");
    }

    // Properties calculadas
    public double Area => PI * Raio * Raio;
    public double Perimetro => 2 * PI * Raio;
    public double Diametro => Raio * 2;

    // Métodos sobrecarregados

    // Escalar por fator
    public CirculoRecord Escalar(double fator)
    {
        return this with { Raio = Raio * fator };
    }

    // Escalar para área específica
    public CirculoRecord EscalarParaArea(double areaDesejada)
    {
        double novoRaio = Math.Sqrt(areaDesejada / PI);
        return this with { Raio = novoRaio };
    }

    // Escalar para perímetro específico
    public CirculoRecord EscalarParaPerimetro(double perimetroDesejado)
    {
        double novoRaio = perimetroDesejado / (2 * PI);
        return this with { Raio = novoRaio };
    }

    // Factory methods sobrecarregados
    public static CirculoRecord Criar(double raio) => new(raio);
    public static CirculoRecord CriarPorArea(double area) => new(Math.Sqrt(area / PI));
    public static CirculoRecord CriarPorPerimetro(double perimetro) => new(perimetro / (2 * PI));
    public static CirculoRecord CriarPorDiametro(double diametro) => new(diametro / 2);
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaCirculo
{
    public static void Main()
    {
        Console.WriteLine("=== VERSÃO 1: BÁSICA ===\n");
        TestarVersaoBasica();

        Console.WriteLine("\n=== VERSÃO 2: AVANÇADA ===\n");
        TestarVersaoAvancada();

        Console.WriteLine("\n=== VERSÃO 3: COM OPERADORES ===\n");
        TestarVersaoComOperadores();

        Console.WriteLine("\n=== VERSÃO 4: RECORD ===\n");
        TestarVersaoRecord();
    }

    static void TestarVersaoBasica()
    {
        // Criar círculo
        var circulo = new Circulo(5);
        circulo.ExibirInformacoes();
        Console.WriteLine();

        // Método estático - área de um círculo com raio 10
        double area = Circulo.CalcularArea(10);
        Console.WriteLine($"Área de círculo com raio 10: {area:F2}");

        // Método estático - área total de múltiplos círculos
        double areaTotal = Circulo.CalcularArea(5, 10, 15);
        Console.WriteLine($"Área total de 3 círculos: {areaTotal:F2}");
    }

    static void TestarVersaoAvancada()
    {
        // Criar círculos
        var c1 = new CirculoAvancado(5, "cm", "Vermelho");
        c1.ExibirInformacoes();
        Console.WriteLine();

        // Redimensionar por fator
        c1.Redimensionar(2); // Dobra o raio
        c1.ExibirInformacoes();
        Console.WriteLine();

        // Redimensionar para raio absoluto
        c1.Redimensionar(10, true);
        c1.ExibirInformacoes();
        Console.WriteLine();

        // Redimensionar para área específica
        c1.RedimensionarParaArea(100);
        c1.ExibirInformacoes();
        Console.WriteLine();

        // Factory methods
        var c2 = CirculoAvancado.CriarPorArea(50, "m");
        Console.WriteLine("Círculo criado por área:");
        c2.ExibirInformacoes();
        Console.WriteLine();

        var c3 = CirculoAvancado.CriarPorPerimetro(31.4159, "km");
        Console.WriteLine("Círculo criado por perímetro:");
        c3.ExibirInformacoes();
    }

    static void TestarVersaoComOperadores()
    {
        var c1 = new CirculoComOperadores(5);
        var c2 = new CirculoComOperadores(10);

        Console.WriteLine($"C1: {c1}");
        Console.WriteLine($"C2: {c2}");
        Console.WriteLine();

        // Operações
        var c3 = c1 + c2; // Soma de áreas
        Console.WriteLine($"C1 + C2 = {c3}");

        var c4 = c2 - c1; // Diferença de áreas
        Console.WriteLine($"C2 - C1 = {c4}");

        var c5 = c1 * 2; // Dobra o raio
        Console.WriteLine($"C1 * 2 = {c5}");

        var c6 = c2 / 2; // Divide o raio
        Console.WriteLine($"C2 / 2 = {c6}");
        Console.WriteLine();

        // Comparações
        Console.WriteLine($"C1 > C2: {c1 > c2}");
        Console.WriteLine($"C1 < C2: {c1 < c2}");
        Console.WriteLine($"C1 == C2: {c1 == c2}");
    }

    static void TestarVersaoRecord()
    {
        // Criar por diferentes métodos
        var c1 = CirculoRecord.Criar(5);
        var c2 = CirculoRecord.CriarPorArea(78.54);
        var c3 = CirculoRecord.CriarPorPerimetro(31.4159);
        var c4 = CirculoRecord.CriarPorDiametro(20);

        Console.WriteLine($"C1: {c1} - Área: {c1.Area:F2}");
        Console.WriteLine($"C2: {c2} - Área: {c2.Area:F2}");
        Console.WriteLine($"C3: {c3} - Perímetro: {c3.Perimetro:F2}");
        Console.WriteLine($"C4: {c4} - Diâmetro: {c4.Diametro:F2}");
        Console.WriteLine();

        // Escalar
        var c5 = c1.Escalar(2);
        Console.WriteLine($"C1 escalado 2x: {c5} - Área: {c5.Area:F2}");

        var c6 = c1.EscalarParaArea(100);
        Console.WriteLine($"C1 para área 100: {c6} - Área: {c6.Area:F2}");

        // Imutabilidade com with
        var c7 = c1 with { Raio = 15 };
        Console.WriteLine($"\nOriginal C1: {c1}");
        Console.WriteLine($"Modificado C7: {c7}");
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ Constantes
 *    - const PI: Valor fixo em tempo de compilação
 *    - readonly Unidade: Valor fixo após construção
 * 
 * ✅ Method Overloading
 *    - CalcularArea(): 3 versões diferentes
 *    - Redimensionar(): 3 versões diferentes
 *    - Factory methods: 4 formas de criar
 * 
 * ✅ Operator Overloading (Versão 3)
 *    - Aritméticos: +, -, *, /
 *    - Comparação: >, <, ==, !=
 *    - Permite sintaxe natural: c1 + c2
 * 
 * ✅ Properties Calculadas
 *    - Area, Perimetro, Diametro
 *    - Sempre atualizadas com o raio
 * 
 * ✅ Factory Methods (Versão 2 e 4)
 *    - CriarPorArea, CriarPorPerimetro, CriarPorDiametro
 *    - Nomes descritivos da intenção
 * 
 * ✅ Imutabilidade (Versão 4)
 *    - Record com init
 *    - Métodos retornam novos círculos
 *    - with expressions
 * 
 * ✅ Boas Práticas
 *    - Validação em construtores
 *    - Métodos estáticos para operações sem estado
 *    - Sobrecarga para flexibilidade
 *    - Nomes descritivos
 */