# 📝 Exercícios - Herança

## 🎯 Objetivo

Praticar herança, override de métodos, uso de `base` e modificadores de acesso.

---

## ✏️ Exercício 1: Hierarquia de Animais

**Dificuldade**: ⭐ Iniciante

Crie uma hierarquia simples:

**Classe Base**: `Animal`
- Properties: `Nome`, `Idade`
- Método: `EmitirSom()` (virtual)

**Classes Derivadas**:
- `Cachorro`: Override `EmitirSom()` → "Au au!"
- `Gato`: Override `EmitirSom()` → "Miau!"
- `Passaro`: Override `EmitirSom()` → "Piu piu!"

**Teste**: Crie uma lista de animais e chame `EmitirSom()` para todos.

---

## ✏️ Exercício 2: Sistema de Funcionários Básico

**Dificuldade**: ⭐⭐ Intermediário

**Classe Base**: `Funcionario`
- Properties: `Nome`, `Salario`, `DataAdmissao`
- Método: `CalcularBonus()` → retorna 10% do salário

**Classes Derivadas**:
- `Gerente`: Bonus de 20% + R$ 1000 fixo
- `Desenvolvedor`: Bonus de 15% + (HorasExtras * 50)

**Requisitos**:
- Use `base` para chamar construtor
- Override `CalcularBonus()`
- Método `ExibirInformacoes()` em todas

---

## ✏️ Exercício 3: Formas Geométricas

**Dificuldade**: ⭐⭐ Intermediário

**Classe Base**: `Forma`
- Método virtual: `CalcularArea()`
- Método virtual: `CalcularPerimetro()`

**Classes Derivadas**:
- `Circulo` (Raio)
- `Retangulo` (Largura, Altura)
- `Triangulo` (Base, Altura)

**Extra**: Método `Redimensionar(double fator)` que aumenta/diminui a forma.

---

## ✏️ Exercício 4: Contas Bancárias

**Dificuldade**: ⭐⭐ Intermediário

**Classe Base**: `ContaBancaria`
- Properties: `Numero`, `Titular`, `Saldo`
- Métodos: `Depositar()`, `Sacar()` (virtual)

**Classes Derivadas**:
- `ContaCorrente`: Taxa de R$ 2 por saque
- `ContaPoupanca`: Limite de 3 saques por mês
- `ContaEmpresarial`: Sem limite de saque, mas taxa de 1%

**Teste**: Criar contas e simular operações.

---

## ✏️ Exercício 5: Veículos

**Dificuldade**: ⭐⭐ Intermediário

**Classe Base**: `Veiculo`
- Properties: `Marca`, `Modelo`, `Ano`
- Métodos virtuais: `Acelerar()`, `Frear()`

**Classes Derivadas**:
- `Carro` (NumeroPortas): Acelera até 180 km/h
- `Moto` (Cilindrada): Acelera até 220 km/h
- `Caminhao` (CapacidadeCarga): Acelera até 120 km/h

**Requisitos**:
- Método `ExibirDados()`
- Validação de velocidade máxima

---

## ✏️ Exercício 6: Produtos

**Dificuldade**: ⭐⭐⭐ Avançado

**Classe Base**: `Produto`
- Properties: `Nome`, `Preco`, `Estoque`
- Método virtual: `CalcularValorTotal()`

**Classes Derivadas**:
- `ProdutoFisico`: Tem peso, calcula frete
- `ProdutoDigital`: Tem tamanho em MB, sem frete
- `ProdutoPerecivel`: Tem data de validade, desconto se próximo do vencimento

**Requisitos**:
- `ProdutoFisico.CalcularFrete(string cep)`
- `ProdutoDigital.CalcularTamanhoDownload()`
- `ProdutoPerecivel.CalcularDesconto()`

---

## ✏️ Exercício 7: Sistema de Pagamentos

**Dificuldade**: ⭐⭐⭐ Avançado

**Classe Base**: `Pagamento`
- Properties: `Valor`, `Data`, `Status`
- Método virtual: `ProcessarPagamento()`

**Classes Derivadas**:
- `PagamentoCartao`: Tem número do cartão, parcelas
- `PagamentoPix`: Tem chave PIX, instantâneo
- `PagamentoBoleto`: Tem código de barras, data de vencimento

**Requisitos**:
- Validação específica por tipo
- Cálculo de juros (cartão parcelado)
- Status: Pendente, Aprovado, Recusado

---

## ✏️ Exercício 8: Hierarquia de Pessoas

**Dificuldade**: ⭐⭐⭐ Avançado

**Classe Base**: `Pessoa`
- Properties: `Nome`, `CPF`, `DataNascimento`
- Property calculada: `Idade`

**Derivada 1**: `PessoaFisica` : Pessoa
- Property: `RG`

**Derivada 2**: `PessoaJuridica` : Pessoa
- Properties: `CNPJ`, `RazaoSocial`

**Derivadas Finais**:
- `Cliente` : PessoaFisica
- `Funcionario` : PessoaFisica
- `Empresa` : PessoaJuridica

**Teste**: Criar lista heterogênea e processar.

---

## ✏️ Exercício 9: Instrumentos Musicais

**Dificuldade**: ⭐⭐ Intermediário

**Classe Base**: `InstrumentoMusical`
- Properties: `Nome`, `Marca`, `Preco`
- Método virtual: `Tocar()`

**Classes Derivadas**:
- `InstrumentoCorda` (NumCordas): Violão, Guitarra
- `InstrumentoSopro` (Material): Flauta, Saxofone
- `InstrumentoPercussao` (Tamanho): Bateria, Bongo

**Requisitos**:
- Método `Afinar()` específico por tipo
- Método `CalcularManutencao()` baseado no tipo

---

## ✏️ Exercício 10: Sistema de E-commerce (PROJETO FINAL)

**Dificuldade**: ⭐⭐⭐⭐ Muito Avançado

Crie um sistema completo de e-commerce usando herança:

### Hierarquia de Classes

**1. Classe Base: `Produto`**
```csharp
- Id, Nome, Descricao, Preco, Estoque
- CalcularPrecoFinal() (virtual)
- AtualizarEstoque(int quantidade)
- ExibirDetalhes() (virtual)
```

**2. Derivadas de Produto:**

`ProdutoFisico` : Produto
- Peso, Dimensoes (Largura, Altura, Profundidade)
- CalcularFrete(string cepDestino)
- Override CalcularPrecoFinal() → adiciona frete

`ProdutoDigital` : Produto
- TamanhoMB, LinkDownload
- GerarLinkDownload()
- Override CalcularPrecoFinal() → sem frete

`ProdutoPerecivel` : ProdutoFisico
- DataValidade
- DiasParaVencimento (property calculada)
- Override CalcularPrecoFinal() → desconto se próximo do vencimento

**3. Classe Base: `Cliente`**
```csharp
- Id, Nome, Email, CPF
- Enderecos (List<Endereco>)
- CalcularDesconto() (virtual)
```

**4. Derivadas de Cliente:**

`ClienteRegular` : Cliente
- DataCadastro
- CalcularDesconto() → 0%

`ClienteVIP` : Cliente
- PontosFidelidade
- CalcularDesconto() → 10% + 0.01% por ponto

`ClientePremium` : Cliente
- DataAssinatura, ValorMensalidade
- CalcularDesconto() → 20% + frete grátis

**5. Classe: `Pedido`**
```csharp
- NumeroPedido, Cliente, Data
- Itens (List<ItemPedido>)
- CalcularSubtotal()
- CalcularDesconto() → usa Cliente.CalcularDesconto()
- CalcularTotal()
- ExibirResumo()
```

**6. Classe: `ItemPedido`**
```csharp
- Produto, Quantidade, PrecoUnitario
- CalcularSubtotal()
```

### Requisitos do Sistema

1. **Gerenciador de Produtos**: Adicionar, remover, buscar
2. **Gerenciador de Clientes**: Cadastrar, buscar, atualizar
3. **Processador de Pedidos**: Criar, calcular totais, aplicar descontos
4. **Relatórios**:
   - Produtos mais vendidos
   - Clientes com mais compras
   - Receita total por tipo de produto

### Testes

```csharp
// 1. Criar produtos
var livro = new ProdutoFisico("Livro C#", 89.90m, 0.5, ...);
var curso = new ProdutoDigital("Curso C#", 199.90m, 500);
var leite = new ProdutoPerecivel("Leite", 5.99m, DateTime.Now.AddDays(3));

// 2. Criar clientes
var clienteRegular = new ClienteRegular("João", "joao@email.com", "123");
var clienteVIP = new ClienteVIP("Maria", "maria@email.com", "456") 
{
    PontosFidelidade = 1000
};

// 3. Criar pedido
var pedido = new Pedido(clienteVIP);
pedido.AdicionarItem(livro, 2);
pedido.AdicionarItem(curso, 1);

// 4. Calcular e exibir
pedido.ExibirResumo();
```

---

## 📊 Critérios de Avaliação

✅ **Herança**: Hierarquia clara e lógica?  
✅ **Override**: Métodos sobrescritos corretamente?  
✅ **base**: Usado apropriadamente?  
✅ **Encapsulamento**: Properties privadas/protegidas?  
✅ **Polimorfismo**: Lista heterogênea funciona?  
✅ **Validação**: Dados validados?  
✅ **Clean Code**: Código limpo e documentado?  

---

## 🎯 Dicas

1. **Comece simples**: Implemente a classe base primeiro
2. **Teste cada classe**: Crie Main() para testar
3. **Use base**: Reutilize código da classe pai
4. **Protected**: Use para membros que derivadas precisam
5. **Virtual**: Marque métodos que devem ser customizáveis
6. **Documentação**: Comente o porquê das decisões

---

## 🚀 Desafios Extras

### Iniciante
1. Adicionar mais tipos de animais
2. Implementar ToString() em todas as classes
3. Criar método de comparação (salário, área, etc)

### Intermediário
4. Implementar padrão Repository para Produtos
5. Adicionar histórico de transações nas contas
6. Sistema de notificações para produtos perecíveis

### Avançado
7. Implementar carrinho de compras com sessão
8. Sistema de cupons de desconto
9. Integração com API de frete (simulado)
10. Dashboard com estatísticas usando LINQ

---

**Boa sorte! 💪**

*Lembre-se: Herança é poderosa, mas use com sabedoria. Prefira composição quando a relação não for claramente "é um".*
