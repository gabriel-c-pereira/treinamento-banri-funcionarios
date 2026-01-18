# 📦 Arrays e Coleções Genéricas

> **Tempo estimado**: 2 horas  
> **Nível**: Intermediário

## 🎯 O que você aprenderá

- Arrays (fixos) vs Coleções (dinâmicas)
- `List<T>`, `Dictionary<TKey, TValue>`, `HashSet<T>`
- `Queue<T>`, `Stack<T>`
- Quando usar cada coleção
- Performance e complexidade

---

## 📊 Arrays

### Arrays Unidimensionais

```csharp
// Declaração e inicialização
int[] numeros = new int[5];  // Array de tamanho fixo
int[] numeros2 = { 1, 2, 3, 4, 5 };  // Inicialização inline
int[] numeros3 = new int[] { 1, 2, 3 };  // Explícita

// Acesso
numeros[0] = 10;
int primeiro = numeros[0];

// Propriedades
int tamanho = numeros.Length;  // 5

// Iteração
foreach (int num in numeros)
    Console.WriteLine(num);
```

### Arrays Multidimensionais

```csharp
// Array bidimensional (matriz)
int[,] matriz = new int[3, 3];
matriz[0, 0] = 1;
matriz[1, 1] = 5;

// Inicialização inline
int[,] matriz2 = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Array jagged (array de arrays)
int[][] jagged = new int[3][];
jagged[0] = new int[] { 1, 2 };
jagged[1] = new int[] { 3, 4, 5 };
jagged[2] = new int[] { 6 };
```

---

## 📋 List\<T> - A Coleção Mais Usada

```csharp
// Criação
List<int> numeros = new List<int>();
List<string> nomes = new() { "João", "Maria" };  // C# 9+

// Adicionar
numeros.Add(10);
numeros.AddRange(new[] { 20, 30, 40 });
numeros.Insert(0, 5);  // Insere na posição 0

// Acessar
int primeiro = numeros[0];
int ultimo = numeros[numeros.Count - 1];

// Buscar
bool existe = numeros.Contains(20);
int index = numeros.IndexOf(30);  // -1 se não encontrar
int elemento = numeros.Find(x => x > 25);  // Primeiro que atende

// Remover
numeros.Remove(20);  // Remove primeira ocorrência
numeros.RemoveAt(0);  // Remove por índice
numeros.RemoveAll(x => x < 15);  // Remove todos que atendem
numeros.Clear();  // Remove todos

// Ordenar
numeros.Sort();  // Crescente
numeros.Sort((a, b) => b.CompareTo(a));  // Decrescente
numeros.Reverse();  // Inverte ordem

// Propriedades úteis
int quantidade = numeros.Count;
int capacidade = numeros.Capacity;
```

### Quando usar List\<T>?

✅ **Use List quando:**
- Precisa de tamanho dinâmico
- Precisa acessar por índice
- Ordem dos elementos importa
- Aceita duplicatas

---

## 🗂️ Dictionary\<TKey, TValue> - Chave-Valor

```csharp
// Criação
Dictionary<string, int> idades = new();
Dictionary<int, string> usuarios = new()
{
    { 1, "João" },
    { 2, "Maria" }
};

// Adicionar
idades.Add("João", 25);
idades["Maria"] = 30;  // Adiciona ou atualiza

// Acessar
int idadeJoao = idades["João"];
bool temMaria = idades.ContainsKey("Maria");
bool temIdade30 = idades.ContainsValue(30);

// Acesso seguro
if (idades.TryGetValue("Pedro", out int idadePedro))
    Console.WriteLine($"Pedro tem {idadePedro} anos");
else
    Console.WriteLine("Pedro não encontrado");

// Remover
idades.Remove("João");

// Iterar
foreach (var kvp in idades)
    Console.WriteLine($"{kvp.Key}: {kvp.Value} anos");

// Apenas chaves ou valores
foreach (string nome in idades.Keys)
    Console.WriteLine(nome);

foreach (int idade in idades.Values)
    Console.WriteLine(idade);
```

### Quando usar Dictionary?

✅ **Use Dictionary quando:**
- Precisa buscar rapidamente por chave (O(1))
- Tem pares chave-valor
- Chaves são únicas
- Ex: cache, índices, lookups

---

## 🎯 HashSet\<T> - Sem Duplicatas

```csharp
// Criação
HashSet<int> numeros = new();
HashSet<string> emails = new() { "joao@email.com", "maria@email.com" };

// Adicionar (ignora duplicatas)
numeros.Add(10);
numeros.Add(20);
numeros.Add(10);  // Não adiciona (já existe)

Console.WriteLine(numeros.Count);  // 2

// Verificar
bool existe = numeros.Contains(10);

// Operações de conjunto
HashSet<int> a = new() { 1, 2, 3 };
HashSet<int> b = new() { 3, 4, 5 };

// União
a.UnionWith(b);  // a = { 1, 2, 3, 4, 5 }

// Interseção
a.IntersectWith(b);  // a = { 3 }

// Diferença
a.ExceptWith(b);  // a = { 1, 2 }

// Diferença simétrica
a.SymmetricExceptWith(b);  // a = { 1, 2, 4, 5 }

// Subconjunto?
bool isSubset = a.IsSubsetOf(b);
bool isSuperset = a.IsSupersetOf(b);
```

### Quando usar HashSet?

✅ **Use HashSet quando:**
- Não quer duplicatas
- Precisa de operações de conjunto
- Ordem não importa
- Ex: tags, permissões, emails únicos

---

## 🚶 Queue\<T> - Fila (FIFO)

```csharp
// Criação
Queue<string> fila = new();

// Adicionar (no final)
fila.Enqueue("Cliente 1");
fila.Enqueue("Cliente 2");
fila.Enqueue("Cliente 3");

// Remover (do início) - FIFO
string proximo = fila.Dequeue();  // "Cliente 1"

// Ver próximo sem remover
string proximoSemRemover = fila.Peek();  // "Cliente 2"

// Quantidade
int quantidade = fila.Count;

// Verificar se existe
bool existe = fila.Contains("Cliente 3");

// Iterar (não remove)
foreach (string cliente in fila)
    Console.WriteLine(cliente);
```

### Quando usar Queue?

✅ **Use Queue quando:**
- First In, First Out (FIFO)
- Ex: fila de atendimento, processamento de tarefas

---

## 📚 Stack\<T> - Pilha (LIFO)

```csharp
// Criação
Stack<string> pilha = new();

// Adicionar (no topo)
pilha.Push("Ação 1");
pilha.Push("Ação 2");
pilha.Push("Ação 3");

// Remover (do topo) - LIFO
string ultima = pilha.Pop();  // "Ação 3"

// Ver topo sem remover
string topoSemRemover = pilha.Peek();  // "Ação 2"

// Quantidade
int quantidade = pilha.Count;

// Iterar (não remove)
foreach (string acao in pilha)
    Console.WriteLine(acao);
```

### Quando usar Stack?

✅ **Use Stack quando:**
- Last In, First Out (LIFO)
- Ex: desfazer/refazer, navegação, parsing

---

## 📊 Comparação de Performance

| Operação | List | Dictionary | HashSet | Queue | Stack |
|----------|------|------------|---------|-------|-------|
| **Adicionar no final** | O(1)* | O(1)* | O(1)* | O(1) | O(1) |
| **Adicionar no início** | O(n) | - | - | - | - |
| **Buscar por valor** | O(n) | O(1)* | O(1)* | O(n) | O(n) |
| **Buscar por índice** | O(1) | - | - | - | - |
| **Remover** | O(n) | O(1)* | O(1)* | O(1) | O(1) |
| **Contains** | O(n) | O(1)* | O(1)* | O(n) | O(n) |

*Em média. Pode ser O(n) se precisar redimensionar.

---

## 🔍 Qual Coleção Usar?

```csharp
// Precisa de índice e ordem? → List<T>
List<Produto> produtos = new();

// Precisa buscar por chave rapidamente? → Dictionary<TKey, TValue>
Dictionary<int, Cliente> clientes = new();

// Precisa garantir unicidade? → HashSet<T>
HashSet<string> emailsUnicos = new();

// Precisa FIFO (fila)? → Queue<T>
Queue<Pedido> filaPedidos = new();

// Precisa LIFO (pilha)? → Stack<T>
Stack<string> historicoNavegacao = new();

// Precisa ordenação automática? → SortedSet<T> ou SortedDictionary
SortedSet<int> numerosOrdenados = new();
```

---

## 💡 Exemplo Completo

```csharp
public class SistemaVendas
{
    // Lista de produtos (ordem importa, índice útil)
    private List<Produto> _produtos = new();
    
    // Índice por ID (busca rápida)
    private Dictionary<int, Produto> _produtosPorId = new();
    
    // Categorias únicas
    private HashSet<string> _categorias = new();
    
    // Fila de processamento
    private Queue<Pedido> _filaPedidos = new();
    
    // Histórico de navegação
    private Stack<string> _historicoTelas = new();
    
    public void AdicionarProduto(Produto produto)
    {
        _produtos.Add(produto);
        _produtosPorId[produto.Id] = produto;
        _categorias.Add(produto.Categoria);
    }
    
    public Produto BuscarPorId(int id)
    {
        return _produtosPorId.TryGetValue(id, out var produto) 
            ? produto 
            : null;
    }
    
    public void EnfileirarPedido(Pedido pedido)
    {
        _filaPedidos.Enqueue(pedido);
    }
    
    public Pedido ProcessarProximoPedido()
    {
        return _filaPedidos.Count > 0 
            ? _filaPedidos.Dequeue() 
            : null;
    }
    
    public void NavegarPara(string tela)
    {
        _historicoTelas.Push(tela);
    }
    
    public string Voltar()
    {
        return _historicoTelas.Count > 0 
            ? _historicoTelas.Pop() 
            : "Home";
    }
}
```

---

## 🎓 Resumo

✅ **Array**: tamanho fixo, rápido, sintaxe simples  
✅ **List\<T>**: dinâmica, indexável, mais usada  
✅ **Dictionary\<K,V>**: busca O(1) por chave  
✅ **HashSet\<T>**: sem duplicatas, operações de conjunto  
✅ **Queue\<T>**: FIFO, fila  
✅ **Stack\<T>**: LIFO, pilha  

➡️ **Próximo**: Lambda Expressions
