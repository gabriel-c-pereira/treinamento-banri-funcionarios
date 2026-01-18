public static class Utilitarios
{
    // Método genérico para encontrar máximo
    public static T EncontrarMaximo<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length == 0)
            throw new ArgumentException("Array não pode ser vazio");

        T maximo = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i].CompareTo(maximo) > 0)
                maximo = array[i];
        }
        return maximo;
    }

    // Método genérico para trocar valores
    public static void Trocar<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    // Método genérico para verificar se contém
    public static bool Contem<T>(T[] array, T valor) where T : IEquatable<T>
    {
        foreach (T item in array)
        {
            if (item.Equals(valor))
                return true;
        }
        return false;
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        int[] numeros = { 3, 1, 4, 1, 5 };
        int max = Utilitarios.EncontrarMaximo(numeros);
        Console.WriteLine($"Máximo: {max}"); // 5

        string[] palavras = { "maçã", "banana", "laranja" };
        bool contem = Utilitarios.Contem(palavras, "banana");
        Console.WriteLine($"Contém banana: {contem}"); // true

        int x = 10, y = 20;
        Utilitarios.Trocar(ref x, ref y);
        Console.WriteLine($"x: {x}, y: {y}"); // x: 20, y: 10
    }
}
