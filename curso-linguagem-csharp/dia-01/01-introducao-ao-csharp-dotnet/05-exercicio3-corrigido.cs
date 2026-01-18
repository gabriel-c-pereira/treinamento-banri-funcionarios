// Exercício 7 Corrigido: Gerenciando Pacotes NuGet
// Arquivo: Program.cs

// Importação necessária para usar Newtonsoft.Json
using Newtonsoft.Json;

// Criando um objeto anônimo
var pessoa = new 
{
    Nome = "Maria",
    Idade = 25,
    Cidade = "São Paulo",
    Email = "maria@example.com"
};

// Convertendo o objeto para JSON com formatação indentada
string json = JsonConvert.SerializeObject(pessoa, Formatting.Indented);

// Exibindo o resultado
Console.WriteLine("Objeto convertido para JSON:");
Console.WriteLine(json);

Console.WriteLine();
Console.WriteLine("──────────────────────────────────────");
Console.WriteLine();

// Desserializando de volta para objeto
var pessoaDeserializada = JsonConvert.DeserializeObject(json);
Console.WriteLine("Objeto desserializado:");
Console.WriteLine(pessoaDeserializada);

/*
 * PASSOS PARA EXECUTAR ESTE CÓDIGO:
 * 
 * 1. Adicionar o pacote NuGet:
 *    dotnet add package Newtonsoft.Json
 * 
 * 2. Compilar e executar:
 *    dotnet run
 * 
 * ──────────────────────────────────────
 * 
 * CONCEITOS IMPORTANTES:
 * 
 * 1. PACOTES NUGET:
 *    - Bibliotecas de terceiros reutilizáveis
 *    - Gerenciadas pelo dotnet CLI
 *    - Listadas no arquivo .csproj
 * 
 * 2. NEWTONSOFT.JSON (Json.NET):
 *    - Biblioteca popular para trabalhar com JSON
 *    - Métodos principais:
 *      - SerializeObject: objeto → JSON
 *      - DeserializeObject: JSON → objeto
 * 
 * 3. OBJETOS ANÔNIMOS:
 *    - Criados com "new { }" 
 *    - Úteis para dados temporários
 *    - Propriedades inferidas automaticamente
 * 
 * 4. USING STATEMENT:
 *    - Importa namespaces
 *    - Permite usar classes sem nome completo
 *    - Newtonsoft.Json em vez de Newtonsoft.Json.JsonConvert
 * 
 * ──────────────────────────────────────
 * 
 * SAÍDA ESPERADA:
 * 
 * Objeto convertido para JSON:
 * {
 *   "Nome": "Maria",
 *   "Idade": 25,
 *   "Cidade": "São Paulo",
 *   "Email": "maria@example.com"
 * }
 * 
 * ──────────────────────────────────────
 * 
 * Objeto desserializado:
 * {
 *   "Nome": "Maria",
 *   "Idade": 25,
 *   "Cidade": "São Paulo",
 *   "Email": "maria@example.com"
 * }
 * 
 * ──────────────────────────────────────
 * 
 * ALTERNATIVA COM System.Text.Json (Built-in .NET):
 * 
 * using System.Text.Json;
 * 
 * var options = new JsonSerializerOptions { WriteIndented = true };
 * string json = JsonSerializer.Serialize(pessoa, options);
 * 
 * Newtonsoft.Json é mais flexível, mas System.Text.Json é mais rápido
 * e já vem com o .NET (não precisa instalar pacote)
 */