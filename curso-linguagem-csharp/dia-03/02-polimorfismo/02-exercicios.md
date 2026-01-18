# 🎯 Exercícios - Polimorfismo

> **Tempo estimado**: 2 horas  
> **Dificuldade**: Intermediário

---

## 📝 Instruções Gerais

- Crie um projeto Console para cada exercício
- Use polimorfismo, upcasting/downcasting e pattern matching
- Teste com múltiplos tipos diferentes
- Comente seu código

---

## 🟢 Exercício 1: Calculadora de Formas

Crie uma hierarquia de formas geométricas usando polimorfismo.

**Requisitos:**
- Classe base `Forma` com método `virtual double CalcularArea()`
- Classes derivadas: `Circulo`, `Retangulo`, `Triangulo`
- Lista de formas e calcular área total usando polimorfismo

```csharp
List<Forma> formas = new()
{
    new Circulo(5),
    new Retangulo(4, 6),
    new Triangulo(3, 4)
};
```

---

## 🟢 Exercício 2: Sistema de Pagamentos

Implemente diferentes métodos de pagamento usando polimorfismo.

**Requisitos:**
- Classe base `Pagamento` com método `virtual bool Processar(decimal valor)`
- Classes: `CartaoCredito`, `Pix`, `Boleto`, `PayPal`
- Use pattern matching para aplicar taxas diferentes
- CartaoCredito: taxa 2.5%
- Pix: sem taxa
- Boleto: taxa fixa R$ 2,00
- PayPal: taxa 3.5%

---

## 🟡 Exercício 3: Zoológico com Pattern Matching

Crie um sistema de zoológico usando polimorfismo e pattern matching moderno.

**Requisitos:**
- Classe base `Animal` com propriedades `Nome`, `Idade`, `Peso`
- Classes derivadas: `Mamifero`, `Ave`, `Reptil`, `Peixe`
- Método `string ObterHabitat()` polimórfico
- Use `switch` com pattern matching para alimentação:
  - Mamífero grande (>50kg): "Ração especial 5kg"
  - Mamífero pequeno: "Ração padrão 1kg"
  - Ave: "Sementes e frutas"
  - Réptil: "Carne ou insetos"
  - Peixe: "Ração aquática"

---

## 🟡 Exercício 4: Sistema de Notificações

Implemente um sistema de notificações usando polimorfismo.

**Requisitos:**
- Interface `INotificacao` com método `Enviar(string mensagem, string destinatario)`
- Classes: `Email`, `SMS`, `Push`, `WhatsApp`
- Classe `GerenciadorNotificacoes` que recebe lista de `INotificacao`
- Use pattern matching para validar destinatário:
  - Email: validar formato
  - SMS: validar número (11 dígitos)
  - Push: validar token
  - WhatsApp: validar número internacional

---

## 🟡 Exercício 5: Veículos e Downcasting

Crie hierarquia de veículos e pratique casting seguro.

**Requisitos:**
- Classe base `Veiculo` com propriedades comuns
- Classes: `Carro`, `Moto`, `Caminhao`, `Bicicleta`
- Método específico em cada classe:
  - `Carro.AbrirPortaMalas()`
  - `Moto.EmpinarMoto()`
  - `Caminhao.CarregarCarga(int toneladas)`
  - `Bicicleta.TrocarMarcha(int marcha)`
- Use `is`, `as` e pattern matching para casting seguro
- Lista de `Veiculo` e chame métodos específicos

---

## 🟡 Exercício 6: Hierarquia de Funcionários

Sistema de RH com diferentes tipos de funcionários.

**Requisitos:**
- Classe base `Funcionario` com `CalcularSalario()` virtual
- `FuncionarioHorista`: salário = horas * valorHora
- `FuncionarioMensalista`: salário fixo
- `FuncionarioComissionado`: salário base + comissões
- `Estagiario`: bolsa fixa (sem herança de Funcionario)
- Use pattern matching para calcular INSS:
  - Até R$ 1.320: 7.5%
  - R$ 1.320 a R$ 2.571: 9%
  - R$ 2.571 a R$ 3.856: 12%
  - Acima R$ 3.856: 14%

---

## 🔴 Exercício 7: Sistema de Arquivos

Simule um sistema de arquivos com polimorfismo.

**Requisitos:**
- Classe abstrata `ItemSistema` com propriedades `Nome`, `Tamanho`, `DataCriacao`
- Classes: `Arquivo`, `Pasta`, `Atalho`, `ArquivoCompactado`
- `Pasta` pode conter outros `ItemSistema` (composição)
- Métodos polimórficos:
  - `long CalcularTamanhoTotal()` (Pasta soma seus itens)
  - `void Abrir()`
  - `void Renomear(string novoNome)`
- Use pattern matching para operações:
  - Arquivo de texto: abrir em editor
  - Arquivo de imagem: abrir em visualizador
  - Pasta: listar conteúdo
  - Atalho: seguir para destino

---

## 🔴 Exercício 8: Sistema de Transporte Público

Crie sistema de transporte com diferentes tipos de veículos.

**Requisitos:**
- Classe base `TransportePublico`
- Classes: `Onibus`, `Metro`, `Trem`, `VLT`, `BRT`
- Propriedades: capacidade, linha, tarifa
- Método `CalcularTarifa(int distancia, bool estudante, bool idoso)`
- Pattern matching para descontos:
  - Estudante: 50%
  - Idoso: gratuito
  - Distância > 20km: +30%
- Método `CalcularTempoViagem(int distancia)` considerando velocidade média

---

## 🔴 Exercício 9: RPG - Sistema de Combate

Sistema de combate para RPG usando polimorfismo avançado.

**Requisitos:**
- Classe base `Personagem` com HP, MP, Ataque, Defesa
- Classes: `Guerreiro`, `Mago`, `Arqueiro`, `Clerigo`
- Método `virtual int Atacar(Personagem alvo)`
- Cada classe tem ataque especial diferente
- Use pattern matching para calcular dano:
  - Guerreiro vs Mago: +20% dano
  - Mago vs Guerreiro: -10% dano
  - Arqueiro vs Voador: +30% dano
- Sistema de crítico (10% chance, dano x2)
- Habilidades especiais:
  - Guerreiro: Fúria (aumenta ataque 50% por 3 turnos)
  - Mago: Explosão (dano em área)
  - Arqueiro: Flecha perfurante (ignora defesa)
  - Clérigo: Cura (restaura HP)

---

## 🔴 Exercício 10: Sistema de E-commerce com Polimorfismo (PROJETO FINAL)

Sistema completo de e-commerce usando todos os conceitos de polimorfismo.

### Requisitos:

#### 1. Hierarquia de Produtos
```csharp
public abstract class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal PrecoBase { get; set; }
    public abstract decimal CalcularPrecoFinal();
    public abstract string ObterDescricaoDetalhada();
}
```

**Classes derivadas:**
- `ProdutoFisico`: peso, dimensões, frete
- `ProdutoDigital`: tamanho do arquivo, formato, link download
- `Servico`: duração, profissional responsável
- `Assinatura`: periodicidade, benefícios

#### 2. Sistema de Descontos Polimórfico
```csharp
public abstract class Desconto
{
    public abstract decimal Aplicar(decimal valor);
}
```

**Tipos:**
- `DescontoPercentual`: percentual fixo
- `DescontoProgressivo`: aumenta com valor
- `DescontoCupom`: código de desconto
- `DescontoQuantidade`: desconto por volume

#### 3. Formas de Pagamento
- `PagamentoCartao`: parcelas, juros
- `PagamentoPix`: desconto 5%
- `PagamentoBoleto`: vencimento, código de barras
- `PagamentoCriptomoeda`: cotação, carteira

#### 4. Cálculo de Frete (Pattern Matching)
```csharp
public decimal CalcularFrete(Produto produto, string cep) => produto switch
{
    ProdutoFisico { Peso: < 1 } => 10m,
    ProdutoFisico { Peso: < 5 } => 20m,
    ProdutoFisico { Peso: >= 5 } => 30m + (produto.Peso * 2),
    ProdutoDigital => 0m,
    _ => 15m
};
```

#### 5. Carrinho de Compras
- Adicionar/remover produtos polimórficos
- Calcular subtotal, descontos, frete, total
- Aplicar cupons
- Validar estoque

#### 6. Sistema de Entrega
- `EntregaNormal`: 7-10 dias
- `EntregaExpressa`: 2-3 dias (+50%)
- `EntregaAgendada`: escolher data/hora
- `RetiradaLoja`: gratuita

#### 7. Features Avançadas
- [ ] Use pattern matching para calcular prazo de entrega por região
- [ ] Implemente sistema de cashback (varia por tipo de produto)
- [ ] Sistema de avaliações polimórfico
- [ ] Filtros de busca usando pattern matching
- [ ] Rastreamento de pedido (estados: pendente, pago, enviado, entregue)
- [ ] Sistema de reembolso (regras diferentes por tipo de produto)

#### 8. Relatórios
```csharp
// Use pattern matching para relatórios
public string GerarRelatorio(Pedido pedido) => pedido.Status switch
{
    StatusPedido.Pendente when pedido.DataCriacao.AddHours(24) < DateTime.Now 
        => "Pagamento pendente há mais de 24h",
    StatusPedido.Pago when pedido.Produtos.Any(p => p is ProdutoDigital) 
        => "Liberar download imediatamente",
    StatusPedido.Enviado { Rastreio: not null } 
        => $"Rastreio: {pedido.Rastreio}",
    _ => "Status normal"
};
```

### Testes Obrigatórios:

1. **Criar carrinho com produtos mistos**
```csharp
var carrinho = new Carrinho();
carrinho.Adicionar(new ProdutoFisico { Nome = "Notebook", Preco = 3000 });
carrinho.Adicionar(new ProdutoDigital { Nome = "E-book", Preco = 50 });
carrinho.Adicionar(new Assinatura { Nome = "Netflix", Preco = 30 });
```

2. **Aplicar descontos diferentes**
3. **Testar diferentes formas de pagamento**
4. **Calcular frete por tipo de produto**
5. **Gerar relatórios usando pattern matching**

### Estrutura Esperada:
- 15+ classes
- 3+ interfaces
- 500-700 linhas de código
- Uso extensivo de polimorfismo
- Pattern matching em pelo menos 5 lugares
- Comentários explicativos

---

## 📚 Dicas

- Use `virtual`/`override` para comportamento customizável
- Prefira `is` com pattern matching ao invés de casting direto
- Use `as` quando o cast pode falhar e você quer tratar null
- Pattern matching com `switch` é mais elegante que múltiplos `if`
- Upcasting é sempre seguro, downcasting requer validação

---

## 🎯 Critérios de Avaliação

- ✅ Uso correto de polimorfismo
- ✅ Casting seguro (is/as)
- ✅ Pattern matching moderno
- ✅ Código limpo e organizado
- ✅ Tratamento de casos especiais
- ✅ Comentários explicativos

Boa sorte! 🚀
