# Exercício 1: O Verificador de Versão Simples

O objetivo é usar um attribute para marcar classes de software com um número de versão e, em seguida, escrever um pequeno código para ler essa versão.

## Passo a passo:
1. Crie uma classe `VersaoAttribute` que herde de `Attribute` e que receba um `string` (o número da versão) como parâmetro no construtor.
2. Aplique este `attribute` a duas classes diferentes do seu projeto (ex: `GerenciadorUsuarios`, `ServicoEstoque`).
3. Escreva uma função que receba um `Type` (o tipo da classe, por exemplo `typeof(GerenciadorUsuarios)`) e use *Reflection* (`GetCustomAttribute<T>`) para imprimir a versão que você declarou.

```csharp
// 1. Crie o attribute aqui...
// Ex: [Versao("1.0.0")]

public class GerenciadorUsuarios
{
    // ...
}

public class ServicoEstoque
{
    // ...
}

// 3. Implemente a função de leitura aqui...
// public void ImprimirVersao(Type tipo) { ... }
```

# Exercício 2: O Serializador de Campos Privados

Bibliotecas de serialização (como JSON.NET) geralmente só serializam propriedades públicas. Neste exercício, você criará um attribute para forçar a serialização de um campo privado.

## Passo a passo:
1. Crie uma classe `SerializarCampoAttribute` (vazia, apenas para servir de marcador).
2. Crie uma classe `DadosSecretos` com um campo privado (`private string token = "XYZ";`).
3. Aplique o `[SerializarCampo]` nesse campo privado.
4. Escreva uma função de serialização manual que use Reflection (`GetFields(BindingFlags.NonPublic | BindingFlags.Instance)`) para encontrar todos os campos privados que possuem o seu `attribute` e imprima o nome e o valor deles no console.

```csharp
// 1. Crie o attribute SerializarCampoAttribute aqui...

public class DadosSecretos
{
    // 3. Aplique o attribute aqui:
    private string token = "meu_token_secreto_123";
    public string PublicData { get; set; } = "Dado publico";
}

// 4. Implemente a função de serialização manual aqui...
// public void Serializar(DadosSecretos obj) { ... }
```

# Exercício 3: O Validador de Requerido Personalizado

O objetivo é simular o attribute `[Required]` usado em validações de formulários (como no ASP.NET Core).

## Passo a passo:
1. Crie um `attribute` `RequeridoAttribute` (vazio).
2. Crie uma classe `CadastroUsuario` com propriedades `Nome`, `Email` e `Idade`.
3. Aplique o `[Requerido]` nas propriedades `Nome` e `Email`.
4. Escreva uma função `ValidarObjeto(object obj)` que:
- - Itera por todas as propriedades do objeto.
- - Verifica se a propriedade tem o `RequeridoAttribute`.
- - Se tiver, verifica se o valor da propriedade é `null` ou vazio (`string.IsNullOrEmpty`).
- - Se for nulo/vazio, imprime uma mensagem de erro (`"O campo X é obrigatório!"`).

```csharp
// 1. Crie o attribute RequeridoAttribute aqui...

public class CadastroUsuario
{
    // 3. Aplique o attribute aqui:
    public string Nome { get; set; }
    // Aplique o attribute aqui:
    public string Email { get; set; }
    public int Idade { get; set; }
}

// 4. Implemente a função ValidarObjeto(object obj) aqui...
// Use-a assim:
// var usuarioInvalido = new CadastroUsuario { Idade = 25 };
// ValidarObjeto(usuarioInvalido);
```