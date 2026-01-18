# 📝 Exercícios - Construtores e Sobrecarga

## 🎯 Objetivo

Praticar construtores, overloading, chaining, optional parameters e method overloading.

---

## ✏️ Exercício 1: Livro com Múltiplos Construtores

**Dificuldade**: ⭐ Iniciante

Crie uma classe `Livro` com múltiplos construtores:

**Properties**:
- `Titulo` (string)
- `Autor` (string)
- `Paginas` (int)
- `ISBN` (string)
- `AnoPublicacao` (int)

**Construtores**:
1. Completo (recebe todos os dados)
2. Com título, autor e páginas
3. Com título e autor
4. "Padrão" (sem parâmetros) - valores padrão


**Use constructor chaining** para evitar duplicação!

**Método**: `ExibirInformacoes()`

---

## ✏️ Exercício 2: Calculadora com Method Overloading

**Dificuldade**: ⭐ Iniciante

Crie uma classe `Calculadora` com métodos sobrecarregados:

**Métodos Somar**:
- `Somar(int a, int b)` → retorna int
- `Somar(double a, double b)` → retorna double
- `Somar(int a, int b, int c)` → retorna int
- `Somar(params int[] numeros)` → retorna int (soma array)

**Métodos Multiplicar**:
- `Multiplicar(int a, int b)` → retorna int
- `Multiplicar(double a, double b)` → retorna double
- `Multiplicar(int a, int b, int c)` → retorna int

Teste todos os overloads!

---

## ✏️ Exercício 3: Retângulo com Validação

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `Retangulo`:

**Properties**:
- `Largura` (double)
- `Altura` (double)
- `Area` (calculada)
- `Perimetro` (calculado)

**Construtores**:
1. `Retangulo(double largura, double altura)` - retângulo
2. `Retangulo(double lado)` - quadrado (largura = altura = lado)
3. `Retangulo()` - largura e altura = 1

**Validação**: Largura e altura devem ser > 0

**Métodos**:
- `Redimensionar(double novaLargura, double novaAltura)`
- `Redimensionar(double fator)` - overload que multiplica por fator
- `ExibirDimensoes()`

---

## ✏️ Exercício 4: Configuração com Optional Parameters

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `ConfiguracaoBancoDados`:

**Properties**:
- `Host` (string)
- `Port` (int)
- `Usuario` (string)
- `Senha` (string)
- `Timeout` (int)
- `UsarSSL` (bool)

**Construtor com Optional Parameters**:
```csharp
public ConfiguracaoBancoDados(
    string host,
    int port,
    string usuario,
    string senha,
    int timeout = 30,
    bool usarSSL = false)
```

**Métodos**:
- `ObterStringConexao()` - retorna string formatada
- `TestarConexao()` - simula teste
- `ExibirConfiguracao()`

Teste com **named arguments** e diferentes combinações!

---

## ✏️ Exercício 5: Funcionário com Constructor Chaining

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `Funcionario`:

**Properties**:
- `Nome` (string)
- `CPF` (string)
- `Cargo` (string)
- `Salario` (decimal)
- `DataAdmissao` (DateTime)

**Construtores encadeados**:
1. `Funcionario(nome, cpf, salario, cargo, dataAdmissao)` - completo
2. `Funcionario(nome, cpf, salario, cargo)` - admissão hoje
3. `Funcionario(nome, cpf, salario)` - admissão hoje, cargo "Geral"

Todos devem chamar o construtor mais completo!

**Métodos**:
- `CalcularTempoEmpresa()` - retorna anos
- `AumentarSalario(decimal valor)` - overload 1
- `AumentarSalario(double percentual)` - overload 2

---

## ✏️ Exercício 6: Endereço com Object Initializer

**Dificuldade**: ⭐ Iniciante

Crie uma classe `Endereco`:

**Properties**:
- `Rua`, `Numero`, `Complemento`
- `Bairro`, `Cidade`, `Estado`, `CEP`

**Construtor**: Apenas obrigatórios (rua, numero, cidade, estado, cep)

**Método**: `ObterEnderecoCompleto()` - retorna string formatada

**Teste**: Crie endereços usando **object initializer** para complemento e bairro.

---

## ✏️ Exercício 7: Círculo com Sobrecarga

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `Circulo`:

**Constantes**: `PI = 3.14159`

**Property**: `Raio` (double)

**Construtores**:
1. `Circulo()` - raio = 1
2. `Circulo(double raio)` - com validação > 0

**Properties Calculadas**:
- `Area` - π * r²
- `Circunferencia` - 2 * π * r
- `Diametro` - 2 * r

**Métodos Sobrecarregados**:
- `Comparar(Circulo outro)` - retorna qual é maior
- `Comparar(double raio)` - compara com raio dado
- `Redimensionar(double novoRaio)`
- `Redimensionar(double fator, bool multiplicar)` - se true multiplica, se false divide

---

## ✏️ Exercício 8: Contato com Validação

**Dificuldade**: ⭐⭐⭐ Avançado

Crie uma classe `Contato`:

**Properties**:
- `Nome` (string)
- `Email` (string, validar @)
- `Telefone` (string, validar formato)
- `Tipo` (string: "Pessoal", "Trabalho", "Outro")
- `DataCadastro` (DateTime, readonly)

**Construtores**:
1. `Contato(nome, email, telefone, tipo)` - completo
2. `Contato(nome, email, telefone)` - tipo "Pessoal"
3. `Contato(nome, email)` - tipo "Pessoal", sem telefone

**Validações no construtor**:
- Nome não vazio
- Email contém @
- Tipo válido (Pessoal, Trabalho, Outro)

**Métodos**:
- `AlterarEmail(string novoEmail)` - com validação
- `AlterarTelefone(string novoTelefone)` - com validação
- `ExibirCartao()` - formato vCard

---

## ✏️ Exercício 9: Sistema de Produtos (Com Primary Constructor - C# 12)

**Dificuldade**: ⭐⭐⭐ Avançado

Se estiver usando C# 12, crie uma classe `Produto` com **primary constructor**:

```csharp
public class Produto(string nome, decimal preco, int estoque)
{
    // Implemente properties calculadas
    // Implemente métodos
}
```

**Properties**:
- Nome, Preco, Estoque (do primary constructor)
- `CodigoProduto` (auto-incremento static)
- `ValorEstoque` (calculado)
- `Disponivel` (calculado: estoque > 0)

**Métodos**:
- `AdicionarEstoque(int quantidade)`
- `RemoverEstoque(int quantidade)`
- `AplicarDesconto(decimal percentual)` - overload 1
- `AplicarDesconto(decimal valor, bool isPercentual)` - overload 2

**Extra**: Se não tiver C# 12, use construtores tradicionais.

---

## ✏️ Exercício 10: Sistema de Reservas (PROJETO FINAL)

**Dificuldade**: ⭐⭐⭐ Avançado

Crie um sistema de reservas com 3 classes:

### Classe `Cliente`
**Properties**:
- `Nome`, `CPF`, `Email`, `Telefone`
- `DataCadastro` (readonly)

**Construtores**:
1. Com CPF (obrigatório)
2. Com CPF e nome
3. Com todos os dados

### Classe `QuartoHotel`
**Properties**:
- `Numero` (int)
- `Tipo` (string: "Single", "Duplo", "Suite")
- `PrecoDiaria` (decimal)
- `Ocupado` (bool, private set)

**Construtores com Optional Parameters**:
```csharp
public QuartoHotel(int numero, string tipo = "Single", decimal preco = 100)
```

**Métodos**:
- `Ocupar()` e `Desocupar()`
- `CalcularTotal(int dias)`
- `CalcularTotal(int dias, decimal desconto)` - overload

### Classe `Reserva`
**Properties**:
- `NumeroReserva` (static auto-incremento)
- `Cliente` (Cliente)
- `Quarto` (QuartoHotel)
- `CheckIn` (DateTime)
- `CheckOut` (DateTime)
- `DiasEstadia` (calculado)
- `ValorTotal` (calculado)
- `Status` (string: "Pendente", "Confirmada", "Cancelada", "Finalizada")

**Construtores**:
1. Com cliente, quarto e datas
2. Com cliente, quarto, checkIn e quantidadeDias (calcula checkout)

**Métodos**:
- `Confirmar()`, `Cancelar()`, `Finalizar()`
- `AplicarDesconto(decimal percentual)`
- `ExibirResumo()`
- `ExibirDetalhado()`

**Validações**:
- CheckOut > CheckIn
- Quarto não ocupado
- Status válido

**Teste Completo**:
```csharp
// Criar cliente
var cliente = new Cliente("123.456.789-00")
{
    Nome = "João Silva",
    Email = "joao@email.com",
    Telefone = "(11) 98765-4321"
};

// Criar quarto
var quarto = new QuartoHotel(
    numero: 101,
    tipo: "Suite",
    preco: 350
);

// Criar reserva
var reserva = new Reserva(
    cliente: cliente,
    quarto: quarto,
    checkIn: DateTime.Today,
    quantidadeDias: 3
);

reserva.Confirmar();
reserva.AplicarDesconto(10);
reserva.ExibirDetalhado();
```

---

## 📊 Critérios de Avaliação

✅ **Constructor Chaining**: Usa `: this(...)` corretamente?  
✅ **Overloading**: Múltiplas assinaturas funcionam?  
✅ **Optional Parameters**: Usa valores padrão adequados?  
✅ **Named Arguments**: Melhora legibilidade?  
✅ **Validações**: Trata casos inválidos nos construtores?  
✅ **Encapsulamento**: Mantém dados protegidos?  

---

## 🎯 Dicas

1. **Constructor Chaining**: Sempre chame o construtor mais completo
2. **Validação**: Sempre valide no construtor principal
3. **Optional Parameters**: Coloque valores sensatos como padrão
4. **Overloading**: Diferencie claramente cada versão
5. **Named Arguments**: Use quando melhorar legibilidade
6. **Object Initializer**: Combine com construtores

---

## 🚀 Desafios Extras

### Iniciante
1. Adicione ToString() override em todas as classes
2. Implemente Equals() e GetHashCode()
3. Adicione mais validações

### Intermediário
4. Crie factory methods (static Create methods)
5. Implemente fluent interface (métodos retornam this)
6. Adicione logging nas operações

### Avançado
7. Use expression-bodied members em tudo
8. Implemente IEquatable<T>
9. Crie builder pattern para classes complexas

---

**Boa sorte! 💪**

*Lembre-se: Construtores bem projetados facilitam muito o uso da classe!*
