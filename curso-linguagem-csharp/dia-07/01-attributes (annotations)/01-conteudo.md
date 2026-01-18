# Aula sobre Attributes em C#

E aí, pessoal! Sejam bem-vindos à nossa aula sobre **Attributes em C#**! 🚀

Se você já se deparou com aquelas coisinhas entre colchetes, tipo `[Serializable]`, `[Obsolete]`, ou `[JsonProperty("name")]` no código de alguém e ficou se perguntando "Que bruxaria é essa?", você está no lugar certo.

Vamos desmistificar os *attributes* e ver como eles podem deixar nosso código mais poderoso e organizado.

---

## 1. O que são Attributes? 🤔

Pense nos *attributes* como **etiquetas** ou **anotações** que você coloca em partes do seu código (classes, métodos, propriedades, etc.) para adicionar informações extras a eles.

Essas informações não mudam a *lógica* do seu código no momento da execução, mas elas são usadas por outras ferramentas, o compilador ou até mesmo por outras partes do seu próprio código em tempo de execução para fazer algo especial.

É como colocar uma etiqueta "Frágil" numa caixa. A etiqueta não muda o que está dentro da caixa, mas avisa quem for manuseá-la que um cuidado especial é necessário.

---

## 2. Por que usar Attributes? 💡

Eles servem para um monte de coisas! Os usos mais comuns são:

*   **Serialização/Desserialização:** Dizer a uma biblioteca (como JSON.NET) como mapear propriedades de uma classe para um formato de dados (tipo JSON ou XML).
*   **Validação:** Definir regras para os dados (ex: `[Required]`, `[MaxLength(50)]`).
*   **Documentação/Metadados:** Marcar código como obsoleto (`[Obsolete]`) ou dar descrições para ferramentas de documentação.
*   **Frameworks:** O Entity Framework, ASP.NET Core e muitos outros *frameworks* usam *attributes* extensivamente para configurar o comportamento (ex: mapeamento para colunas de banco de dados).

---

## 3. Exemplos Práticos (A Bruxa Solta) 🧙‍♂️

Vamos ver alguns exemplos comuns que você provavelmente já viu ou verá no dia a dia.

### Exemplo 1: `[Obsolete]` (O Aposentado)

Este é o *attribute* mais simples e talvez o mais comum. Ele diz ao compilador que um método (ou classe/propriedade) não deve mais ser usado.

```csharp
public class Calculadora
{
    // Este método ainda funciona, mas o compilador vai te dar um aviso (warning)
    [Obsolete("Este método está obsoleto. Use SomarNumeros(a, b) no lugar.")]
    public int Add(int a, int b)
    {
        return a + b;
    }

    public int SomarNumeros(int a, int b)
    {
        return a + b;
    }
}
```

Quando alguém tentar usar o método `Add()`:

```csharp
var calc = new Calculadora();
// Aqui, o Visual Studio/compilador vai mostrar um aviso (warning)
int resultado = calc.Add(10, 20);
```

### Exemplo 2: `[Serializable]` (A Mala Pronta)

Em algumas aplicações mais antigas ou específicas (como Unity), você precisa dizer ao .NET que uma classe pode ser convertida em um fluxo de bytes (serializada) para ser salva em um arquivo ou enviada pela rede.

```csharp
// Colocamos a etiqueta na classe
[Serializable]
public class ConfiguracoesUsuario
{
    public string NomeUsuario { get; set; }
    public int Nivel { get; set; }
    public DateTime UltimoLogin { get; set; }
}
```

### Exemplo 3: Serialização JSON (O Mapeamento)

Este é super comum em APIs REST. Você usa attributes para garantir que o nome da propriedade no seu código C# combine com o nome do campo no JSON.
Usando a biblioteca `Newtonsoft.Json` (ou `System.Text.Json`):

```csharp
public class Produto
{
    // No C#, a propriedade é 'Id', mas no JSON que recebemos, o nome do campo é 'product_id'
    [JsonProperty("product_id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Nome { get; set; }

    // Aqui não precisa de attribute porque o nome é o mesmo nos dois lugares
    public decimal Preco { get; set; }
}
```

## 4. Criando Seus Próprios Attributes (O Nível Hard) 👩‍💻

A parte mais legal é que você pode criar suas próprias etiquetas personalizadas! Isso é útil para criar frameworks internos ou para adicionar lógica específica ao seu projeto.

### Passo 1: Criar a classe do Attribute

Você precisa criar uma classe que herda de `System.Attribute`. O nome da sua classe deve terminar com a palavra `Attribute` (por convenção), mas quando você for usá-la no código, essa parte final é opcional.
Vamos criar um attribute para marcar quais métodos em nossa aplicação precisam de logging (registro de atividades).

```csharp
// A classe herda de Attribute
public class LoggableMethodAttribute : Attribute
{
    public string Descricao { get; }

    public LoggableMethodAttribute(string descricao)
    {
        Descricao = descricao;
    }
}
```

### Passo 2: Usar o Attribute no código

Agora podemos etiquetar nossos métodos:

```csharp
public class ServicoDePagamento
{
    [LoggableMethod("Processando um novo pagamento com cartão de crédito.")]
    public void ProcessarPagamento(decimal valor)
    {
        // ... lógica do pagamento ...
    }

    // Este método não será logado por um sistema que procura por esse attribute
    public void EnviarEmailConfirmacao()
    {
        // ...
    }
}
```

### Passo 3: Ler o Attribute em tempo de execução (Reflection)

Para que esse attribute tenha alguma utilidade, precisamos de um código que o procure e o leia. Usamos uma técnica avançada do C# chamada *Reflection*.
Reflection é basicamente a capacidade do C# de inspecionar a si mesmo em tempo de execução (ler os metadados).

```csharp
using System;
using System.Reflection;

public class AttributeReader
{
    public static void Run()
    {
        // 1. Pegamos o tipo da nossa classe ServicoDePagamento
        Type tipoServico = typeof(ServicoDePagamento);

        // 2. Iteramos sobre todos os métodos públicos dessa classe
        MethodInfo[] metodos = tipoServico.GetMethods();

        foreach (var metodo in metodos)
        {
            // 3. Verificamos se o método tem o nosso attribute personalizado
            var attribute = metodo.GetCustomAttribute<LoggableMethodAttribute>();

            if (attribute != null)
            {
                // Se tiver, lemos a descrição que guardamos nele!
                Console.WriteLine($"Método '{metodo.Name}' precisa de log. Descrição: {attribute.Descricao}");
            }
        }
    }
}

// Saída esperada:
// Método 'ProcessarPagamento' precisa de log. Descrição: Processando um novo pagamento com cartão de crédito.
```

## 5. Resumo Rápido 📝
- Attributes são etiquetas de metadados `[EntreColchetes]`.
- Eles não mudam a lógica do código, mas dão instruções para outras coisas.
- Super usados em frameworks para *serialização*, *validação* e *configuração*.
- Você pode criar os seus próprios herdando de `System.Attribute`.
- Para ler attributes personalizados, você usa *Reflection*.

E é isso! Attributes são uma ferramenta poderosa que adiciona uma camada extra de expressividade e funcionalidade ao seu código C#.
