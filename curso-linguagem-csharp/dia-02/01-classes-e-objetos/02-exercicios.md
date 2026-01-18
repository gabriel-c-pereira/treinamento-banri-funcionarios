# 📝 Exercícios - Classes e Objetos

## 🎯 Objetivo

Praticar a criação de classes, uso de properties, métodos, encapsulamento e access modifiers.

---

## ✏️ Exercício 1: Classe Pessoa Simples

**Dificuldade**: ⭐ Iniciante

Crie uma classe `Pessoa` com as seguintes características:

**Properties**:
- `Nome` (string)
- `Idade` (int)
- `Email` (string)

**Métodos**:
- `ApresentarSe()` - exibe: "Olá, meu nome é [Nome] e tenho [Idade] anos"
- `EhMaiorDeIdade()` - retorna `true` se idade >= 18

**Teste**: Crie 3 pessoas diferentes e chame os métodos.

---

## ✏️ Exercício 2: Conta Bancária com Encapsulamento

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `ContaBancaria` que demonstre **encapsulamento**:

**Properties**:
- `NumeroConta` (string, somente leitura)
- `Titular` (string)
- `Saldo` (decimal, somente leitura pública)

**Métodos**:
- `Depositar(decimal valor)` - adiciona ao saldo (validar > 0)
- `Sacar(decimal valor)` - remove do saldo (validar saldo suficiente)
- `Transferir(ContaBancaria destino, decimal valor)` - transfere entre contas
- `ExibirExtrato()` - mostra informações da conta

**Validações**:
- Não permitir saldo negativo
- Não permitir depósito/saque de valores <= 0

---

## ✏️ Exercício 3: Produto de E-commerce

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `Produto` para um e-commerce:

**Properties**:
- `Nome` (string)
- `Preco` (decimal, validar >= 0)
- `QuantidadeEstoque` (int, private set)
- `Categoria` (string)
- `CodigoProduto` (int, readonly, auto-incremento)
- `ValorTotalEstoque` (calculada: preço * quantidade)

**Métodos**:
- `AdicionarEstoque(int quantidade)`
- `RemoverEstoque(int quantidade)` - retorna bool (sucesso/falha)
- `AplicarDesconto(decimal percentual)` - valida 0-100%
- `ExibirDetalhes()`

**Extras**:
- Use um campo static para gerar códigos únicos
- Implemente validações robustas

---

## ✏️ Exercício 4: Classe Estática - Calculadora

**Dificuldade**: ⭐ Iniciante

Crie uma classe **static** `Calculadora` com métodos para:

- `Somar(double a, double b)`
- `Subtrair(double a, double b)`
- `Multiplicar(double a, double b)`
- `Dividir(double a, double b)` - validar divisão por zero
- `Potencia(double base, double expoente)`
- `RaizQuadrada(double numero)` - validar >= 0
- `Porcentagem(double valor, double percentual)`

Teste todos os métodos sem criar instância da classe.

---

## ✏️ Exercício 5: Aluno e Notas

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `Aluno`:

**Properties**:
- `Nome` (string)
- `Matricula` (string, readonly)
- `Nota1`, `Nota2`, `Nota3` (decimal, validar 0-10)
- `Media` (calculada: (nota1 + nota2 + nota3) / 3)
- `Situacao` (calculada: "Aprovado" se média >= 7, "Recuperação" se >= 5, "Reprovado")

**Métodos**:
- `AlterarNota(int numeroNota, decimal novoValor)` - 1, 2 ou 3
- `ExibirBoletim()`

**Validações**:
- Notas devem estar entre 0 e 10

---

## ✏️ Exercício 6: Funcionário com Salário

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `Funcionario`:

**Properties**:
- `Nome` (string)
- `CPF` (string, readonly)
- `Cargo` (string)
- `SalarioBruto` (decimal, private set)
- `SalarioLiquido` (calculada: bruto - descontos)

**Métodos**:
- `AumentarSalario(decimal percentual)` - valida 0-100%
- `CalcularINSS()` - retorna decimal (8% do bruto)
- `CalcularIR()` - retorna decimal (15% do bruto se > 3000)
- `ExibirContracheque()`

**Extras**:
- Use const para taxas de INSS e IR

---

## ✏️ Exercício 7: Veículo

**Dificuldade**: ⭐⭐ Intermediário

Crie uma classe `Veiculo`:

**Properties**:
- `Marca`, `Modelo`, `Cor` (strings)
- `Ano` (int)
- `VelocidadeAtual` (int, private set, padrão 0)
- `VelocidadeMaxima` (int, readonly)
- `Ligado` (bool, private set, padrão false)

**Métodos**:
- `Ligar()` - só liga se desligado
- `Desligar()` - só desliga se velocidade = 0
- `Acelerar(int incremento)` - não ultrapassar velocidade máxima
- `Frear(int decremento)` - não ficar negativo
- `ExibirStatus()`

**Validações**:
- Só acelera se estiver ligado
- Velocidade entre 0 e velocidade máxima

---

## ✏️ Exercício 8: Retângulo

**Dificuldade**: ⭐ Iniciante

Crie uma classe `Retangulo`:

**Properties**:
- `Largura` (double, validar > 0)
- `Altura` (double, validar > 0)
- `Area` (calculada: largura * altura)
- `Perimetro` (calculado: 2 * (largura + altura))
- `EhQuadrado` (calculada: largura == altura)

**Métodos**:
- `ExibirDimensoes()`
- `Redimensionar(double novaLargura, double novaAltura)`
- `Comparar(Retangulo outro)` - retorna qual é maior por área

---

## ✏️ Exercício 9: Sistema de Livros

**Dificuldade**: ⭐⭐⭐ Avançado

Crie duas classes: `Livro` e `Biblioteca`:

**Classe Livro**:
- `Titulo`, `Autor`, `ISBN` (strings)
- `AnoPublicacao` (int)
- `NumeroPaginas` (int)
- `Disponivel` (bool, private set, padrão true)
- `Emprestar()` - marca como indisponível
- `Devolver()` - marca como disponível
- `ExibirDetalhes()`

**Classe Biblioteca** (static):
- Lista de livros (static field private)
- `AdicionarLivro(Livro livro)`
- `RemoverLivro(string isbn)`
- `BuscarPorTitulo(string titulo)` - retorna lista
- `BuscarPorAutor(string autor)` - retorna lista
- `ListarDisponiveis()`
- `ListarTodos()`
- `QuantidadeLivros()` - retorna int

**Teste**:
- Adicione pelo menos 5 livros
- Faça buscas
- Empreste e devolva livros

---

## ✏️ Exercício 10: Sistema de Pedidos (PROJETO FINAL)

**Dificuldade**: ⭐⭐⭐ Avançado

Crie um sistema completo de pedidos com 3 classes:

**Classe ItemPedido**:
- `NomeProduto` (string)
- `PrecoUnitario` (decimal)
- `Quantidade` (int)
- `Subtotal` (calculado: preço * quantidade)

**Classe Cliente**:
- `Nome`, `Email`, `Telefone` (strings)
- `CPF` (string, readonly)
- `DataCadastro` (DateTime, readonly)

**Classe Pedido**:
- `NumeroPedido` (int, static auto-incremento)
- `Cliente` (Cliente)
- `Itens` (List<ItemPedido>)
- `DataPedido` (DateTime, readonly)
- `Status` (string: "Pendente", "Pago", "Enviado", "Entregue")
- `ValorTotal` (calculado: soma dos subtotais)
- `TemDesconto` (bool)
- `PercentualDesconto` (decimal)
- `ValorFinal` (calculado: total - desconto)

**Métodos da Pedido**:
- `AdicionarItem(ItemPedido item)`
- `RemoverItem(string nomeProduto)` - retorna bool
- `AplicarDesconto(decimal percentual)` - valida 0-50%
- `AlterarStatus(string novoStatus)` - valida status permitidos
- `ExibirResumo()`
- `ExibirDetalhado()` - com todos os itens

**Métodos Static**:
- `CriarPedido(Cliente cliente)` - factory method

**Validações**:
- Não adicionar itens com quantidade <= 0
- Não adicionar itens com preço <= 0
- Só permitir status válidos
- Desconto entre 0-50%

**Teste Completo**:
```csharp
// Criar cliente
var cliente = new Cliente
{
    Nome = "João Silva",
    Email = "joao@email.com",
    Telefone = "(11) 98765-4321",
    CPF = "123.456.789-00"
};

// Criar pedido
var pedido = Pedido.CriarPedido(cliente);

// Adicionar itens
pedido.AdicionarItem(new ItemPedido
{
    NomeProduto = "Notebook",
    PrecoUnitario = 3500,
    Quantidade = 1
});

pedido.AdicionarItem(new ItemPedido
{
    NomeProduto = "Mouse",
    PrecoUnitario = 85,
    Quantidade = 2
});

// Aplicar desconto
pedido.AplicarDesconto(10);  // 10%

// Alterar status
pedido.AlterarStatus("Pago");

// Exibir
pedido.ExibirDetalhado();
```

---

## 📊 Critérios de Avaliação

Para cada exercício, verifique:

✅ **Funcionalidade**: Código funciona corretamente?  
✅ **Encapsulamento**: Fields privados, properties públicas?  
✅ **Validações**: Trata casos inválidos?  
✅ **Nomenclatura**: Segue convenções C#?  
✅ **Organização**: Código limpo e legível?  
✅ **Comentários**: Código está documentado?  

---

## 🎯 Dicas

1. **Comece Simples**: Faça os exercícios na ordem
2. **Teste Muito**: Teste casos normais e extremos
3. **Valide Sempre**: Nunca confie na entrada do usuário
4. **Use Properties**: Evite fields públicos
5. **Encapsule**: Proteja seus dados
6. **Nomeie Bem**: Nomes claros e descritivos
7. **Documente**: Comente código complexo
8. **Refatore**: Melhore depois de funcionar

---

## 🚀 Desafios Extras

### Fácil
1. Adicione ToString() override em todas as classes
2. Implemente IEquatable nas classes principais
3. Adicione validação de CPF/Email real

### Médio
4. Crie um sistema de log para operações
5. Implemente histórico de transações
6. Adicione persistência em arquivo JSON

### Difícil
7. Crie um sistema completo de e-commerce
8. Implemente padrão Repository
9. Adicione eventos para mudanças de estado

---

**Boa sorte nos exercícios! 💪**

*Lembre-se: A prática leva à perfeição. Quanto mais classes criar, melhor você entenderá POO!*
