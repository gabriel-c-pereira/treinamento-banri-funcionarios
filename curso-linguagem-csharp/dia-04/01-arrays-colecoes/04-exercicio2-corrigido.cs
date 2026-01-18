// Exercício 2: Lista Dinâmica
// Objetivo: Usar List<T> com operações de adicionar, remover e buscar

using System;
using System.Collections.Generic;

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("    GERENCIADOR DE NOMES - LIST<T>     ");
Console.WriteLine("═══════════════════════════════════════");

List<string> nomes = new List<string>();
bool continuar = true;

while (continuar)
{
    Console.WriteLine("\n[1] Adicionar nome");
    Console.WriteLine("[2] Remover nome");
    Console.WriteLine("[3] Listar nomes");
    Console.WriteLine("[4] Buscar nome");
    Console.WriteLine("[0] Sair");
    Console.Write("\nEscolha uma opção: ");
    
    string opcao = Console.ReadLine() ?? "";
    
    switch (opcao)
    {
        case "1":
            Console.Write("Digite o nome: ");
            string novoNome = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(novoNome))
            {
                nomes.Add(novoNome);
                Console.WriteLine($"✅ '{novoNome}' adicionado!");
            }
            break;
            
        case "2":
            Console.Write("Digite o nome a remover: ");
            string nomeRemover = Console.ReadLine() ?? "";
            if (nomes.Remove(nomeRemover))
                Console.WriteLine($"✅ '{nomeRemover}' removido!");
            else
                Console.WriteLine($"❌ '{nomeRemover}' não encontrado.");
            break;
            
        case "3":
            Console.WriteLine($"\n📋 Lista de nomes ({nomes.Count}):");
            if (nomes.Count == 0)
                Console.WriteLine("   (vazia)");
            else
                for (int i = 0; i < nomes.Count; i++)
                    Console.WriteLine($"   {i + 1}. {nomes[i]}");
            break;
            
        case "4":
            Console.Write("Digite o nome a buscar: ");
            string nomeBuscar = Console.ReadLine() ?? "";
            int indice = nomes.IndexOf(nomeBuscar);
            if (indice >= 0)
                Console.WriteLine($"✅ '{nomeBuscar}' encontrado na posição {indice + 1}");
            else
                Console.WriteLine($"❌ '{nomeBuscar}' não encontrado.");
            break;
            
        case "0":
            continuar = false;
            Console.WriteLine("Até logo!");
            break;
            
        default:
            Console.WriteLine("❌ Opção inválida!");
            break;
    }
}

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. LIST<T>:
 *    - List<string> nomes = new List<string>();
 *    - Tamanho dinâmico (diferente de array)
 *    - Métodos úteis: Add, Remove, IndexOf, Count
 * 
 * 2. OPERAÇÕES PRINCIPAIS:
 *    - Add(item):          Adiciona ao final
 *    - Remove(item):       Remove primeira ocorrência
 *    - IndexOf(item):      Retorna índice (-1 se não encontrado)
 *    - Count:              Retorna quantidade de elementos
 * 
 * 3. NULL-COALESCING:
 *    - Console.ReadLine() ?? ""
 *    - Retorna string vazia se ReadLine retornar null
 * 
 * 4. SWITCH STATEMENT:
 *    - Estrutura de seleção múltipla
 *    - Mais organizado que múltiplos if-else
 */
