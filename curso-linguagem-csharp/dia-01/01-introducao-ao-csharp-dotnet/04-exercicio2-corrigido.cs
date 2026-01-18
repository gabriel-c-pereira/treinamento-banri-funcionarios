// Exercício 5 Corrigido: Trabalhando com Argumentos
// Arquivo: Program.cs

// args está disponível automaticamente em top-level statements

if (args.Length == 0)
{
    Console.WriteLine("Nenhum argumento foi passado.");
    Console.WriteLine();
    Console.WriteLine("Uso: dotnet run -- argumento1 argumento2 ...");
}
else
{
    Console.WriteLine($"Você passou {args.Length} argumento(s):");
    Console.WriteLine();
    
    // Método 1: for tradicional
    for (int i = 0; i < args.Length; i++)
    {
        Console.WriteLine($"[{i}] {args[i]}");
    }
    
    /* Método 2: foreach (alternativa)
    int indice = 0;
    foreach (string arg in args)
    {
        Console.WriteLine($"[{indice}] {arg}");
        indice++;
    }
    */
}

/*
 * COMO EXECUTAR:
 * 
 * dotnet run -- teste argumento1 argumento2
 * 
 * O "--" separa os argumentos do dotnet dos argumentos da aplicação
 * 
 * CONCEITOS IMPORTANTES:
 * 
 * 1. args é um array de strings (string[])
 * 2. args.Length retorna o número de elementos
 * 3. Arrays em C# começam no índice 0
 * 4. String interpolation: $"texto {variavel}"
 * 5. Estrutura condicional: if-else
 * 6. Loop for: for (inicialização; condição; incremento)
 * 
 * EXEMPLO DE SAÍDA:
 * 
 * $ dotnet run -- teste argumento1 argumento2
 * Você passou 3 argumento(s):
 * 
 * [0] teste
 * [1] argumento1
 * [2] argumento2
 */