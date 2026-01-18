// Exercício 10 Corrigido: Sistema Completo de Gerenciamento de Notas
// Arquivo: Program.cs

using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("═══════════════════════════════════════════════════════════");
Console.WriteLine("          SISTEMA DE GERENCIAMENTO DE NOTAS               ");
Console.WriteLine("═══════════════════════════════════════════════════════════");
Console.WriteLine();

// Estrutura de dados
List<string> nomes = new List<string>();
List<double> notas = new List<double>();

string opcao;

do
{
    // Menu principal
    Console.WriteLine("╔═══════════════════════════════════╗");
    Console.WriteLine("║          MENU PRINCIPAL           ║");
    Console.WriteLine("╠═══════════════════════════════════╣");
    Console.WriteLine("║ 1 - Adicionar aluno               ║");
    Console.WriteLine("║ 2 - Listar todos os alunos        ║");
    Console.WriteLine("║ 3 - Buscar aluno por nome         ║");
    Console.WriteLine("║ 4 - Calcular média da turma       ║");
    Console.WriteLine("║ 5 - Mostrar estatísticas          ║");
    Console.WriteLine("║ 6 - Alunos aprovados/reprovados   ║");
    Console.WriteLine("║ 7 - Remover aluno                 ║");
    Console.WriteLine("║ 8 - Editar nota                   ║");
    Console.WriteLine("║ 0 - Sair                          ║");
    Console.WriteLine("╚═══════════════════════════════════╝");
    Console.Write("\nEscolha uma opção: ");
    opcao = Console.ReadLine()?.Trim() ?? "";
    Console.WriteLine();

    switch (opcao)
    {
        case "1":  // Adicionar aluno
            AdicionarAluno();
            break;

        case "2":  // Listar todos
            ListarAlunos();
            break;

        case "3":  // Buscar por nome
            BuscarAluno();
            break;

        case "4":  // Calcular média
            CalcularMedia();
            break;

        case "5":  // Estatísticas
            MostrarEstatisticas();
            break;

        case "6":  // Aprovados/Reprovados
            MostrarAprovados();
            break;

        case "7":  // Remover aluno
            RemoverAluno();
            break;

        case "8":  // Editar nota
            EditarNota();
            break;

        case "0":  // Sair
            Console.WriteLine("👋 Encerrando sistema...");
            break;

        default:
            Console.WriteLine("❌ Opção inválida!");
            break;
    }

    Console.WriteLine();

} while (opcao != "0");

// ═══════════════════════════════════════════════════════════
// FUNÇÕES DO SISTEMA
// ═══════════════════════════════════════════════════════════

void AdicionarAluno()
{
    Console.WriteLine("─── ADICIONAR ALUNO ───");
    
    // Nome
    Console.Write("Nome do aluno: ");
    string nome = Console.ReadLine()?.Trim() ?? "";
    
    if (string.IsNullOrWhiteSpace(nome))
    {
        Console.WriteLine("❌ Nome não pode ser vazio!");
        return;
    }
    
    // Verificar se já existe
    if (nomes.Any(n => n.Equals(nome, StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine("⚠️  Aluno já cadastrado!");
        return;
    }
    
    // Nota
    Console.Write("Nota (0-10): ");
    if (!double.TryParse(Console.ReadLine(), out double nota) || 
        nota < 0 || nota > 10)
    {
        Console.WriteLine("❌ Nota inválida! Deve estar entre 0 e 10.");
        return;
    }
    
    // Adicionar
    nomes.Add(nome);
    notas.Add(nota);
    
    Console.WriteLine($"✅ Aluno '{nome}' adicionado com sucesso!");
    Console.WriteLine($"   Nota: {nota:F2} - Situação: {ObterSituacao(nota)}");
}

void ListarAlunos()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    Console.WriteLine("─── LISTA DE ALUNOS ───");
    Console.WriteLine();
    Console.WriteLine($"{"#",-4} {"Nome",-20} {"Nota",-8} {"Situação",-15}");
    Console.WriteLine(new string('─', 50));
    
    for (int i = 0; i < nomes.Count; i++)
    {
        string situacao = ObterSituacao(notas[i]);
        string emoji = ObterEmoji(notas[i]);
        
        Console.WriteLine($"{i + 1,-4} {nomes[i],-20} {notas[i],-8:F2} {emoji} {situacao}");
    }
    
    Console.WriteLine(new string('─', 50));
    Console.WriteLine($"Total de alunos: {nomes.Count}");
}

void BuscarAluno()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    Console.Write("Digite o nome do aluno: ");
    string busca = Console.ReadLine()?.Trim() ?? "";
    
    // Busca case-insensitive e parcial
    var resultados = new List<int>();
    for (int i = 0; i < nomes.Count; i++)
    {
        if (nomes[i].Contains(busca, StringComparison.OrdinalIgnoreCase))
        {
            resultados.Add(i);
        }
    }
    
    if (resultados.Count == 0)
    {
        Console.WriteLine($"❌ Nenhum aluno encontrado com '{busca}'.");
        return;
    }
    
    Console.WriteLine($"\n🔍 Encontrado(s) {resultados.Count} aluno(s):");
    Console.WriteLine();
    
    foreach (int i in resultados)
    {
        Console.WriteLine($"Nome: {nomes[i]}");
        Console.WriteLine($"Nota: {notas[i]:F2}");
        Console.WriteLine($"Situação: {ObterSituacao(notas[i])}");
        Console.WriteLine();
    }
}

void CalcularMedia()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    // Calcular média
    double soma = 0;
    for (int i = 0; i < notas.Count; i++)
    {
        soma += notas[i];
    }
    double media = soma / notas.Count;
    
    Console.WriteLine("─── MÉDIA DA TURMA ───");
    Console.WriteLine($"Total de alunos: {nomes.Count}");
    Console.WriteLine($"Média geral: {media:F2}");
    Console.WriteLine($"Situação da turma: {ObterSituacao(media)}");
}

void MostrarEstatisticas()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    // Calcular estatísticas
    double menorNota = notas[0];
    double maiorNota = notas[0];
    double soma = notas[0];
    string alunoMenorNota = nomes[0];
    string alunoMaiorNota = nomes[0];
    
    for (int i = 1; i < notas.Count; i++)
    {
        if (notas[i] < menorNota)
        {
            menorNota = notas[i];
            alunoMenorNota = nomes[i];
        }
        
        if (notas[i] > maiorNota)
        {
            maiorNota = notas[i];
            alunoMaiorNota = nomes[i];
        }
        
        soma += notas[i];
    }
    
    double media = soma / notas.Count;
    
    // Contar aprovados/reprovados
    int aprovados = 0;
    int reprovados = 0;
    int recuperacao = 0;
    
    foreach (double nota in notas)
    {
        if (nota >= 7.0)
            aprovados++;
        else if (nota >= 5.0)
            recuperacao++;
        else
            reprovados++;
    }
    
    // Mostrar estatísticas
    Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
    Console.WriteLine("║               ESTATÍSTICAS DA TURMA                       ║");
    Console.WriteLine("╠═══════════════════════════════════════════════════════════╣");
    Console.WriteLine($"║ Total de alunos:           {nomes.Count,3}                         ║");
    Console.WriteLine($"║                                                           ║");
    Console.WriteLine($"║ Média geral:               {media,6:F2}                       ║");
    Console.WriteLine($"║ Menor nota:                {menorNota,6:F2} ({alunoMenorNota,-15})   ║");
    Console.WriteLine($"║ Maior nota:                {maiorNota,6:F2} ({alunoMaiorNota,-15})   ║");
    Console.WriteLine($"║                                                           ║");
    Console.WriteLine($"║ ✅ Aprovados (≥ 7.0):      {aprovados,3} ({aprovados * 100.0 / nomes.Count,5:F1}%)              ║");
    Console.WriteLine($"║ ⚠️  Recuperação (5.0-6.9): {recuperacao,3} ({recuperacao * 100.0 / nomes.Count,5:F1}%)              ║");
    Console.WriteLine($"║ ❌ Reprovados (< 5.0):     {reprovados,3} ({reprovados * 100.0 / nomes.Count,5:F1}%)              ║");
    Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
}

void MostrarAprovados()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    // Separar por categoria
    var aprovados = new List<(string nome, double nota)>();
    var recuperacao = new List<(string nome, double nota)>();
    var reprovados = new List<(string nome, double nota)>();
    
    for (int i = 0; i < nomes.Count; i++)
    {
        if (notas[i] >= 7.0)
            aprovados.Add((nomes[i], notas[i]));
        else if (notas[i] >= 5.0)
            recuperacao.Add((nomes[i], notas[i]));
        else
            reprovados.Add((nomes[i], notas[i]));
    }
    
    // Mostrar aprovados
    Console.WriteLine("✅ APROVADOS (≥ 7.0):");
    if (aprovados.Count > 0)
    {
        foreach (var aluno in aprovados)
        {
            Console.WriteLine($"   • {aluno.nome,-20} Nota: {aluno.nota:F2}");
        }
    }
    else
    {
        Console.WriteLine("   (nenhum)");
    }
    Console.WriteLine();
    
    // Mostrar recuperação
    Console.WriteLine("⚠️  RECUPERAÇÃO (5.0 - 6.9):");
    if (recuperacao.Count > 0)
    {
        foreach (var aluno in recuperacao)
        {
            Console.WriteLine($"   • {aluno.nome,-20} Nota: {aluno.nota:F2}");
        }
    }
    else
    {
        Console.WriteLine("   (nenhum)");
    }
    Console.WriteLine();
    
    // Mostrar reprovados
    Console.WriteLine("❌ REPROVADOS (< 5.0):");
    if (reprovados.Count > 0)
    {
        foreach (var aluno in reprovados)
        {
            Console.WriteLine($"   • {aluno.nome,-20} Nota: {aluno.nota:F2}");
        }
    }
    else
    {
        Console.WriteLine("   (nenhum)");
    }
}

void RemoverAluno()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    ListarAlunos();
    Console.WriteLine();
    Console.Write("Digite o número do aluno para remover (0 para cancelar): ");
    
    if (!int.TryParse(Console.ReadLine(), out int indice) || 
        indice < 0 || indice > nomes.Count)
    {
        Console.WriteLine("❌ Número inválido!");
        return;
    }
    
    if (indice == 0)
    {
        Console.WriteLine("Operação cancelada.");
        return;
    }
    
    indice--;  // Ajustar para índice 0-based
    
    // Confirmar remoção
    Console.Write($"Tem certeza que deseja remover '{nomes[indice]}'? (S/N): ");
    string confirmacao = Console.ReadLine()?.Trim().ToUpper() ?? "";
    
    if (confirmacao == "S" || confirmacao == "SIM")
    {
        string nomeRemovido = nomes[indice];
        nomes.RemoveAt(indice);
        notas.RemoveAt(indice);
        Console.WriteLine($"✅ Aluno '{nomeRemovido}' removido com sucesso!");
    }
    else
    {
        Console.WriteLine("Operação cancelada.");
    }
}

void EditarNota()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    ListarAlunos();
    Console.WriteLine();
    Console.Write("Digite o número do aluno para editar a nota (0 para cancelar): ");
    
    if (!int.TryParse(Console.ReadLine(), out int indice) || 
        indice < 0 || indice > nomes.Count)
    {
        Console.WriteLine("❌ Número inválido!");
        return;
    }
    
    if (indice == 0)
    {
        Console.WriteLine("Operação cancelada.");
        return;
    }
    
    indice--;  // Ajustar para índice 0-based
    
    Console.WriteLine($"\nAluno: {nomes[indice]}");
    Console.WriteLine($"Nota atual: {notas[indice]:F2}");
    Console.Write("Nova nota (0-10): ");
    
    if (!double.TryParse(Console.ReadLine(), out double novaNota) || 
        novaNota < 0 || novaNota > 10)
    {
        Console.WriteLine("❌ Nota inválida! Deve estar entre 0 e 10.");
        return;
    }
    
    double notaAntiga = notas[indice];
    notas[indice] = novaNota;
    
    Console.WriteLine($"✅ Nota atualizada!");
    Console.WriteLine($"   Anterior: {notaAntiga:F2} ({ObterSituacao(notaAntiga)})");
    Console.WriteLine($"   Nova: {novaNota:F2} ({ObterSituacao(novaNota)})");
}

// ═══════════════════════════════════════════════════════════
// FUNÇÕES AUXILIARES
// ═══════════════════════════════════════════════════════════

string ObterSituacao(double nota)
{
    return nota switch
    {
        >= 9.0 => "Excelente",
        >= 7.0 => "Aprovado",
        >= 5.0 => "Recuperação",
        _ => "Reprovado"
    };
}

string ObterEmoji(double nota)
{
    return nota switch
    {
        >= 9.0 => "🏆",
        >= 7.0 => "✅",
        >= 5.0 => "⚠️",
        _ => "❌"
    };
}

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA - PROJETO COMPLETO
 * ═══════════════════════════════════════════════════════════
 * 
 * Este projeto integra TODOS os conceitos do Dia 1:
 * 
 * 1. VARIÁVEIS E TIPOS DE DADOS:
 *    - string: nomes, opções
 *    - double: notas
 *    - int: índices, contadores
 *    - bool: confirmações
 * 
 * 2. ESTRUTURAS DE CONTROLE:
 *    - if/else: validações
 *    - switch: menu principal
 *    - switch expression: classificações
 *    - for: iteração sobre arrays
 *    - foreach: iteração simplificada
 *    - do-while: loop do menu
 *    - while: validações repetidas
 * 
 * 3. COLEÇÕES:
 *    - List<T>: listas dinâmicas
 *    - Arrays paralelos (nomes + notas)
 *    - Tuplas: (string nome, double nota)
 * 
 * 4. FUNÇÕES (LOCAL FUNCTIONS):
 *    - void: ações sem retorno
 *    - string: funções que retornam texto
 *    - Parâmetros e retornos
 * 
 * ═══════════════════════════════════════════════════════════
 * PADRÕES E TÉCNICAS UTILIZADAS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. ESTRUTURA DO MENU:
 * 
 *    do
 *    {
 *        // Mostrar opções
 *        // Ler escolha
 *        // Processar com switch
 *    } while (opcao != "0");
 *    
 *    Vantagens:
 *    - Sempre mostra menu pelo menos uma vez
 *    - Loop contínuo até usuário sair
 *    - Código organizado
 * 
 * 2. LISTAS PARALELAS:
 * 
 *    List<string> nomes = new();
 *    List<double> notas = new();
 *    
 *    nomes[0] corresponde a notas[0]
 *    nomes[1] corresponde a notas[1]
 *    ...
 *    
 *    Alternativa (melhor):
 *    - Criar uma classe Aluno
 *    - List<Aluno> alunos
 *    (Veremos no Dia 2!)
 * 
 * 3. VALIDAÇÃO DE ENTRADA:
 * 
 *    if (!double.TryParse(input, out double valor) || 
 *        valor < 0 || valor > 10)
 *    {
 *        // Entrada inválida
 *        return;
 *    }
 *    
 *    Componentes:
 *    - TryParse: converte e valida tipo
 *    - Validação de range: valor < 0 || valor > 10
 *    - Early return: sai da função se inválido
 * 
 * 4. BUSCA EM LISTA:
 * 
 *    Método 1 (manual):
 *    for (int i = 0; i < lista.Count; i++)
 *    {
 *        if (lista[i] == valor)
 *            return i;
 *    }
 *    
 *    Método 2 (LINQ - Dia 4):
 *    lista.Any(x => x == valor)
 *    lista.FirstOrDefault(x => x == valor)
 * 
 * 5. FORMATAÇÃO DE STRINGS:
 * 
 *    {valor,-20}  → Alinha à esquerda, 20 caracteres
 *    {valor,20}   → Alinha à direita, 20 caracteres
 *    {valor:F2}   → 2 casas decimais: 7.50
 *    {valor:P1}   → Percentual: 75.0%
 *    
 *    Exemplo:
 *    double nota = 7.5;
 *    Console.WriteLine($"Nota: {nota,-8:F2}");
 *    // Output: "Nota: 7.50    "
 * 
 * 6. NULL-COALESCING OPERATOR (??):
 * 
 *    string input = Console.ReadLine() ?? "";
 *                                      ↑
 *                   Se for null, usa ""
 *    
 *    Útil para evitar NullReferenceException:
 *    string nome = obterNome() ?? "Desconhecido";
 * 
 * 7. NULL-CONDITIONAL OPERATOR (?.):
 * 
 *    string input = Console.ReadLine()?.Trim() ?? "";
 *                                     ↑
 *                   Só chama Trim() se não for null
 *    
 *    Equivalente a:
 *    string temp = Console.ReadLine();
 *    string input = temp != null ? temp.Trim() : "";
 * 
 * ═══════════════════════════════════════════════════════════
 * MELHORIAS POSSÍVEIS (Para o Futuro)
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. USAR CLASSE (DIA 2):
 * 
 *    class Aluno
 *    {
 *        public string Nome { get; set; }
 *        public double Nota { get; set; }
 *        public string Situacao => ObterSituacao(Nota);
 *    }
 *    
 *    List<Aluno> alunos = new();
 * 
 * 2. PERSISTÊNCIA DE DADOS (DIA 6):
 * 
 *    - Salvar em arquivo JSON
 *    - Carregar ao iniciar
 *    - Manter dados entre execuções
 * 
 * 3. LINQ (DIA 4):
 * 
 *    var aprovados = alunos.Where(a => a.Nota >= 7.0);
 *    var media = alunos.Average(a => a.Nota);
 *    var melhorAluno = alunos.OrderByDescending(a => a.Nota).First();
 * 
 * 4. TRATAMENTO DE EXCEÇÕES (DIA 5):
 * 
 *    try
 *    {
 *        // Código que pode falhar
 *    }
 *    catch (Exception ex)
 *    {
 *        Console.WriteLine($"Erro: {ex.Message}");
 *    }
 * 
 * 5. INTERFACE GRÁFICA:
 * 
 *    - Windows Forms
 *    - WPF
 *    - Blazor (web)
 * 
 * ═══════════════════════════════════════════════════════════
 * ESTRUTURA DE CÓDIGO LIMPO
 * ═══════════════════════════════════════════════════════════
 * 
 * ✅ Organização:
 *    1. Variáveis globais no topo
 *    2. Loop principal do menu
 *    3. Funções específicas
 *    4. Funções auxiliares
 * 
 * ✅ Nomenclatura:
 *    - Funções: PascalCase (AdicionarAluno)
 *    - Variáveis: camelCase (nomeAluno)
 *    - Constantes: UPPER_CASE (MAX_NOTA)
 * 
 * ✅ Responsabilidade única:
 *    - Cada função faz UMA coisa
 *    - Funções pequenas e focadas
 *    - Reutilização de código
 * 
 * ✅ Validações:
 *    - Sempre validar entrada do usuário
 *    - Mensagens claras de erro
 *    - Early returns para casos especiais
 * 
 * ✅ Feedback ao usuário:
 *    - Emojis para visual feedback
 *    - Mensagens descritivas
 *    - Confirmações para ações destrutivas
 * 
 * ═══════════════════════════════════════════════════════════
 * EXERCÍCIOS DE EXTENSÃO
 * ═══════════════════════════════════════════════════════════
 * 
 * Tente implementar:
 * 
 * 1. Ordenação:
 *    - Ordenar alunos por nome (A-Z)
 *    - Ordenar por nota (maior primeiro)
 * 
 * 2. Relatórios:
 *    - Gerar relatório em texto
 *    - Mostrar gráfico ASCII das notas
 * 
 * 3. Múltiplas notas:
 *    - Cada aluno tem várias notas
 *    - Calcular média por aluno
 * 
 * 4. Disciplinas:
 *    - Gerenciar múltiplas disciplinas
 *    - Cada disciplina tem seus alunos e notas
 * 
 * 5. Importar/Exportar:
 *    - Ler de arquivo CSV
 *    - Exportar para CSV/JSON
 * 
 * 6. Histórico:
 *    - Registrar todas as alterações
 *    - Permitir desfazer ações
 * 
 * ═══════════════════════════════════════════════════════════
 */