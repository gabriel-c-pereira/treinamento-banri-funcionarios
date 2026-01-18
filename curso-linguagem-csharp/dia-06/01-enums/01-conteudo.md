# 01 - Enums

## 📚 Introdução

No mundo da programação, frequentemente nos deparamos com a necessidade de representar um conjunto fixo e limitado de opções. Por exemplo, os dias da semana, os meses do ano, ou os status de um pedido (pendente, enviado, entregue). Embora pudéssemos usar números simples (como 0 para pendente e 1 para enviado), isso logo se torna confuso. O que significa o número 2 no seu código? É aí que entram os Enums (Enumerações), uma ferramenta do C# projetada para tornar seu código mais legível e menos propenso a erros, dando nomes claros a esses valores.

## 🎯 Objetivos

- Compreender a finalidade e a sintaxe básica dos `enums` (enumerações)
- Aprender a declarar e definir membros de um `enum`
- Saber como converter entre `enums` e seus tipos integrais subjacentes (como `int`)
- Entender a melhor forma de utilizar `enums` para tornar o código mais legível e seguro

## 📂 O que são `Enums`?

Imagine que você está construindo um sistema de gerenciamento de tarefas. Você precisa acompanhar se uma tarefa está "A Fazer", "Em Andamento" ou "Concluída".
Em vez de espalhar "números mágicos" ou strings pelo seu código:

```csharp
// Código confuso (Não faça isso!)
if (statusTarefa == 1)
{
    Console.WriteLine("Tarefa em andamento!");
}
```

Um enum permite que você crie um conjunto de constantes nomeadas que se comportam como valores únicos e descritivos. Ele é basicamente uma lista de opções pré-definidas.
O principal objetivo do enum é substituir esses números ou textos soltos por nomes claros e que se autoexplicam, melhorando muito a legibilidade e a manutenção do seu código.
Pense no enum como uma "gaveta organizada" onde você guarda apenas as opções válidas para um determinado cenário.


### ✏️ Sintaxe e Declaração

Um enum é declarado usando a palavra-chave enum, geralmente dentro de um namespace ou diretamente dentro da classe Program (fora do método Main).

```csharp
public enum StatusPedido
    {
        Pendente,
        Processando,
        Enviado,
        Entregue,
        Cancelado
    }
```

### ⚙️ Uso Básico

Para usar um enum, você referencia o nome do tipo (StatusPedido) seguido de um ponto e o nome do membro (Pendente).

```csharp
class Program
{
    static void Main(string[] args)
    {
        StatusPedido meuStatus;
        meuStatus = StatusPedido.Pendente;

        Console.WriteLine($"O status atual do pedido é: {meuStatus}");

        // Verificando o valor
        if (meuStatus == StatusPedido.Pendente)
        {
            Console.WriteLine("Ação necessária: Iniciar processamento.");
        }
    }
}
```

## 🔄 Valores Subjacentes (Integrais)

Por padrão, os membros de um enum são automaticamente atribuídos a valores inteiros, começando do zero (0).

```csharp
public enum StatusPedido
{
    // Pendente = 0 (por padrão)
    Pendente,
    // Processando = 1
    Processando,
    // Enviado = 2
    Enviado,
    // Entregue = 3
    Entregue,
    // Cancelado = 4
    Cancelado
}
```

Você pode converter explicitamente um membro do enum para o seu valor inteiro subjacente (casting):

```csharp
StatusPedido status = StatusPedido.Entregue;
int valorNumerico = (int)status; // valorNumerico será 3

Console.WriteLine($"Status: {status}, Valor numérico: {valorNumerico}");
```

Você também pode converter um inteiro de volta para o enum:

```csharp
int codigoRecebido = 1;
StatusPedido statusRecebido = (StatusPedido)codigoRecebido; // statusRecebido será StatusPedido.Processando
```

### 🔢 Atribuindo Valores Personalizados

Você pode definir valores inteiros específicos para cada membro, ou apenas para alguns:


```csharp
public enum Prioridade
{
    Baixa = 1,
    Media = 2,
    Alta = 3,
    Urgente = 10 // Pula valores
}
```

Se você atribuir um valor apenas ao primeiro membro, os seguintes continuarão automaticamente a partir desse valor:

```csharp
public enum Mes
{
    Janeiro = 1, // Janeiro = 1
    Fevereiro,   // Fevereiro = 2
    Marco        // Marco = 3
}
```

## ⚠️ Boas Práticas

1. **Use Nomes Significativos**: Escolha nomes de `enum` e membros que descrevam claramente o propósito
2. **Use nomes no singular**: É recomendado é usar nomes no singular (ex: StatusPedido em vez de StatusPedidos)
3. **Evite Mudar Valores Existentes**: Se você mudar o valor numérico de um membro após o código ter sido compilado e usado em produção (por exemplo, em um banco de dados), você pode quebrar a lógica existente. Priorizem adicione novos membros ao final ao invés de modificar se possível.

