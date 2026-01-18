// Exercício 8 Corrigido: Jogo de Adivinhação
// Arquivo: Program.cs

using System;

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("     JOGO DE ADIVINHAÇÃO          ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

// Configuração do jogo
Random random = new Random();
int numeroSecreto = random.Next(1, 101);  // 1 a 100
int tentativas = 0;
int maxTentativas = 7;
bool acertou = false;

Console.WriteLine("🎲 Pensei em um número entre 1 e 100!");
Console.WriteLine($"Você tem {maxTentativas} tentativas para acertar.");
Console.WriteLine();

// Loop principal do jogo
while (tentativas < maxTentativas && !acertou)
{
    tentativas++;
    Console.Write($"Tentativa {tentativas}/{maxTentativas}: ");
    
    // Validação de entrada
    if (!int.TryParse(Console.ReadLine(), out int palpite))
    {
        Console.WriteLine("❌ Digite um número válido!");
        tentativas--;  // Não conta como tentativa
        continue;
    }
    
    // Validação de range
    if (palpite < 1 || palpite > 100)
    {
        Console.WriteLine("❌ O número deve estar entre 1 e 100!");
        tentativas--;  // Não conta como tentativa
        continue;
    }
    
    // Verificação do palpite
    if (palpite == numeroSecreto)
    {
        acertou = true;
        Console.WriteLine($"🎉 PARABÉNS! Você acertou em {tentativas} tentativa(s)!");
    }
    else if (palpite < numeroSecreto)
    {
        int diferenca = numeroSecreto - palpite;
        
        if (diferenca <= 5)
            Console.WriteLine("🔥 MUITO QUENTE! O número é MAIOR!");
        else if (diferenca <= 15)
            Console.WriteLine("🌡️  Quente! O número é maior.");
        else
            Console.WriteLine("❄️  Frio! O número é muito maior.");
    }
    else  // palpite > numeroSecreto
    {
        int diferenca = palpite - numeroSecreto;
        
        if (diferenca <= 5)
            Console.WriteLine("🔥 MUITO QUENTE! O número é MENOR!");
        else if (diferenca <= 15)
            Console.WriteLine("🌡️  Quente! O número é menor.");
        else
            Console.WriteLine("❄️  Frio! O número é muito menor.");
    }
    
    Console.WriteLine();
}

// Resultado final
if (!acertou)
{
    Console.WriteLine($"😞 Você perdeu! O número era {numeroSecreto}.");
}

Console.WriteLine();

// ═══════════════════════════════════════════════════════════
// VERSÃO 2: Com do-while (jogar novamente)
// ═══════════════════════════════════════════════════════════

bool jogarNovamente;

do
{
    Console.WriteLine("═══════════════════════════════════");
    Console.WriteLine("     NOVA PARTIDA                  ");
    Console.WriteLine("═══════════════════════════════════");
    Console.WriteLine();
    
    numeroSecreto = random.Next(1, 101);
    tentativas = 0;
    acertou = false;
    
    while (tentativas < maxTentativas && !acertou)
    {
        tentativas++;
        Console.Write($"Tentativa {tentativas}/{maxTentativas}: ");
        
        if (int.TryParse(Console.ReadLine(), out int palpite) && 
            palpite >= 1 && palpite <= 100)
        {
            if (palpite == numeroSecreto)
            {
                acertou = true;
                Console.WriteLine($"🎉 Você acertou em {tentativas} tentativa(s)!");
            }
            else if (palpite < numeroSecreto)
            {
                Console.WriteLine("📈 O número é MAIOR!");
            }
            else
            {
                Console.WriteLine("📉 O número é MENOR!");
            }
        }
        else
        {
            Console.WriteLine("❌ Entrada inválida!");
            tentativas--;
        }
        
        Console.WriteLine();
    }
    
    if (!acertou)
    {
        Console.WriteLine($"😞 Game Over! O número era {numeroSecreto}.");
        Console.WriteLine();
    }
    
    // Perguntar se quer jogar novamente
    Console.Write("Jogar novamente? (S/N): ");
    string resposta = Console.ReadLine()?.Trim().ToUpper() ?? "N";
    jogarNovamente = resposta == "S" || resposta == "SIM";
    Console.WriteLine();
    
} while (jogarNovamente);

Console.WriteLine("Obrigado por jogar! 👋");

// ═══════════════════════════════════════════════════════════
// VERSÃO 3: Com histórico de tentativas
// ═══════════════════════════════════════════════════════════

Console.WriteLine();
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("  VERSÃO COM HISTÓRICO             ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

numeroSecreto = random.Next(1, 101);
tentativas = 0;
acertou = false;
List<int> historico = new List<int>();  // Armazena todos os palpites

while (tentativas < maxTentativas && !acertou)
{
    tentativas++;
    
    // Mostrar histórico
    if (historico.Count > 0)
    {
        Console.Write("Palpites anteriores: ");
        Console.WriteLine(string.Join(", ", historico));
    }
    
    Console.Write($"Tentativa {tentativas}/{maxTentativas}: ");
    
    if (int.TryParse(Console.ReadLine(), out int palpite) && 
        palpite >= 1 && palpite <= 100)
    {
        // Verificar se já tentou esse número
        if (historico.Contains(palpite))
        {
            Console.WriteLine("⚠️  Você já tentou esse número!");
            tentativas--;
            continue;
        }
        
        historico.Add(palpite);
        
        if (palpite == numeroSecreto)
        {
            acertou = true;
            Console.WriteLine($"🎉 ACERTOU em {tentativas} tentativa(s)!");
            
            // Estatísticas
            int menorPalpite = historico.Min();
            int maiorPalpite = historico.Max();
            Console.WriteLine($"Menor palpite: {menorPalpite}");
            Console.WriteLine($"Maior palpite: {maiorPalpite}");
        }
        else
        {
            string dica = palpite < numeroSecreto ? "MAIOR" : "MENOR";
            Console.WriteLine($"O número é {dica}!");
        }
    }
    else
    {
        Console.WriteLine("❌ Entrada inválida!");
        tentativas--;
    }
    
    Console.WriteLine();
}

if (!acertou)
{
    Console.WriteLine($"😞 Acabaram as tentativas! Era {numeroSecreto}.");
}

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. LOOP WHILE:
 * 
 *    while (condição)
 *    {
 *        // Executa enquanto condição for true
 *    }
 *    
 *    Características:
 *    - Testa condição ANTES de executar
 *    - Pode executar 0 vezes se condição inicialmente falsa
 *    - Ideal quando não se sabe quantas iterações
 * 
 *    Exemplo:
 *    int tentativas = 0;
 *    while (tentativas < 5)
 *    {
 *        Console.WriteLine(tentativas);
 *        tentativas++;
 *    }
 *    // Output: 0, 1, 2, 3, 4
 * 
 * 2. LOOP DO-WHILE:
 * 
 *    do
 *    {
 *        // Executa pelo menos uma vez
 *    } while (condição);
 *    
 *    Características:
 *    - Testa condição DEPOIS de executar
 *    - SEMPRE executa pelo menos 1 vez
 *    - Ideal para menus e validações
 * 
 *    Exemplo:
 *    string resposta;
 *    do
 *    {
 *        Console.Write("Continuar? (S/N): ");
 *        resposta = Console.ReadLine();
 *    } while (resposta != "S" && resposta != "N");
 * 
 * 3. CONDIÇÕES COMPOSTAS:
 * 
 *    while (tentativas < maxTentativas && !acertou)
 *                   ↑                        ↑
 *            Condição 1           Condição 2
 *    
 *    Operadores lógicos:
 *    && (AND) - Ambas devem ser true
 *    || (OR)  - Pelo menos uma deve ser true
 *    !  (NOT) - Inverte o valor booleano
 *    
 *    Tabela verdade AND:
 *    true  && true  → true
 *    true  && false → false
 *    false && true  → false
 *    false && false → false
 *    
 *    Tabela verdade OR:
 *    true  || true  → true
 *    true  || false → true
 *    false || true  → true
 *    false || false → false
 * 
 * 4. CLASSE RANDOM:
 * 
 *    Random random = new Random();
 *    
 *    Métodos:
 *    random.Next()           → 0 a int.MaxValue
 *    random.Next(100)        → 0 a 99
 *    random.Next(1, 101)     → 1 a 100
 *    random.NextDouble()     → 0.0 a 1.0
 *    
 *    Exemplo:
 *    int dado = random.Next(1, 7);     // 1 a 6
 *    double porcentagem = random.NextDouble() * 100;
 * 
 * 5. CONTINUE vs BREAK:
 * 
 *    continue - Pula para próxima iteração
 *    break    - Sai do loop imediatamente
 *    
 *    Exemplo:
 *    for (int i = 0; i < 10; i++)
 *    {
 *        if (i % 2 == 0)
 *            continue;  // Pula números pares
 *        
 *        if (i == 7)
 *            break;     // Para quando chegar em 7
 *        
 *        Console.WriteLine(i);
 *    }
 *    // Output: 1, 3, 5
 * 
 * ═══════════════════════════════════════════════════════════
 * DIFERENÇAS: WHILE vs DO-WHILE vs FOR
 * ═══════════════════════════════════════════════════════════
 * 
 * WHILE:
 * ------
 * while (contador < 10)
 * {
 *     Console.WriteLine(contador);
 *     contador++;
 * }
 * 
 * Uso: Quando não sabe quantas iterações
 * Executa: 0 ou mais vezes
 * Testa: ANTES de executar
 * 
 * 
 * DO-WHILE:
 * ---------
 * do
 * {
 *     Console.WriteLine(contador);
 *     contador++;
 * } while (contador < 10);
 * 
 * Uso: Quando precisa executar pelo menos 1 vez
 * Executa: 1 ou mais vezes
 * Testa: DEPOIS de executar
 * 
 * 
 * FOR:
 * ----
 * for (int i = 0; i < 10; i++)
 * {
 *     Console.WriteLine(i);
 * }
 * 
 * Uso: Quando sabe exatamente quantas iterações
 * Executa: 0 ou mais vezes
 * Testa: ANTES de executar
 * 
 * ═══════════════════════════════════════════════════════════
 * ARMADILHAS COMUNS (COMMON PITFALLS)
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. LOOP INFINITO:
 * 
 *    ❌ ERRADO:
 *    int i = 0;
 *    while (i < 10)
 *    {
 *        Console.WriteLine(i);
 *        // ESQUECEU de incrementar i!
 *    }
 *    
 *    ✅ CORRETO:
 *    int i = 0;
 *    while (i < 10)
 *    {
 *        Console.WriteLine(i);
 *        i++;  // ← Incrementa!
 *    }
 * 
 * 2. CONDIÇÃO SEMPRE VERDADEIRA:
 * 
 *    ❌ ERRADO:
 *    while (true)  // Loop infinito!
 *    {
 *        // Sem break, roda para sempre
 *    }
 *    
 *    ✅ CORRETO:
 *    while (true)
 *    {
 *        if (condicaoSaida)
 *            break;  // ← Saída
 *    }
 * 
 * 3. OFF-BY-ONE ERROR:
 * 
 *    ❌ ERRADO:
 *    int tentativas = 0;
 *    while (tentativas <= 5)  // Executa 6 vezes!
 *    {
 *        tentativas++;
 *    }
 *    
 *    ✅ CORRETO:
 *    int tentativas = 0;
 *    while (tentativas < 5)   // Executa 5 vezes
 *    {
 *        tentativas++;
 *    }
 * 
 * ═══════════════════════════════════════════════════════════
 * PADRÕES DE USO COMUNS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. VALIDAÇÃO DE ENTRADA:
 * 
 *    int numero;
 *    while (true)
 *    {
 *        Console.Write("Digite um número: ");
 *        if (int.TryParse(Console.ReadLine(), out numero))
 *            break;  // Entrada válida, sai do loop
 *        
 *        Console.WriteLine("Entrada inválida!");
 *    }
 * 
 * 2. MENU INTERATIVO:
 * 
 *    string opcao;
 *    do
 *    {
 *        Console.WriteLine("1 - Jogar");
 *        Console.WriteLine("2 - Configurações");
 *        Console.WriteLine("0 - Sair");
 *        opcao = Console.ReadLine();
 *        
 *        // Processar opção...
 *        
 *    } while (opcao != "0");
 * 
 * 3. PROCESSAMENTO ATÉ SENTINELA:
 * 
 *    string linha;
 *    while ((linha = Console.ReadLine()) != "fim")
 *    {
 *        // Processar linha
 *        Console.WriteLine($"Você digitou: {linha}");
 *    }
 * 
 * 4. JOGO COM GAME LOOP:
 * 
 *    bool jogoAtivo = true;
 *    while (jogoAtivo)
 *    {
 *        // 1. Processar entrada
 *        // 2. Atualizar estado
 *        // 3. Renderizar
 *        // 4. Verificar condições de saída
 *        
 *        if (jogadorPerdeu || jogadorDesistiu)
 *            jogoAtivo = false;
 *    }
 * 
 * ═══════════════════════════════════════════════════════════
 * OTIMIZAÇÕES E PERFORMANCE
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. EVITE OPERAÇÕES PESADAS DENTRO DO LOOP:
 * 
 *    ❌ LENTO:
 *    while (condicao)
 *    {
 *        var lista = ObterLista();  // Chama função toda vez!
 *        // ...
 *    }
 *    
 *    ✅ RÁPIDO:
 *    var lista = ObterLista();      // Chama uma vez só
 *    while (condicao)
 *    {
 *        // Usa lista
 *    }
 * 
 * 2. USE BREAK PARA SAÍDA ANTECIPADA:
 * 
 *    while (condicao)
 *    {
 *        if (encontrouResultado)
 *            break;  // Sai imediatamente
 *        
 *        // Não precisa continuar procurando
 *    }
 * 
 * ═══════════════════════════════════════════════════════════
 * DICAS E BOAS PRÁTICAS
 * ═══════════════════════════════════════════════════════════
 * 
 * ✅ Use while quando não sabe quantas iterações
 * ✅ Use do-while para menus e validações
 * ✅ Use for quando sabe quantas iterações
 * ✅ Sempre garanta que o loop pode terminar
 * ✅ Use break para sair antecipadamente
 * ✅ Use continue para pular iteração
 * ✅ Comente loops complexos
 * 
 * ❌ Evite loops infinitos sem break
 * ❌ Evite modificar contador de forma imprevisível
 * ❌ Evite muitos níveis de loops aninhados
 * ❌ Evite operações pesadas dentro do loop
 * 
 * ═══════════════════════════════════════════════════════════
 */