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