# Exercícios — Leitura e Escrita de Arquivos

## Exercício 1: Dias da Semana

**Objetivo:** Crie um `enum` chamado `DiaSemana` que represente os dias da semana, começando de domingo a sábado. Escreva um programa que declare uma variável do tipo `DiaSemana` e imprima na tela o valor do dia da semana e seu valor numérico correspondente.

**Requisitos:**
- Domingo deve ter o valor 0.
- Imprimir uma frase como: `"Hoje é Terça-feira, que corresponde ao número 2."`

---

## Exercício 2: Controle de Acesso

**Objetivo:** Crie um `enum` chamado `NivelAcesso` com os seguintes níveis e valores personalizados:
- `Visitante` (1)
- `Usuario` (5)
- `Editor` (10)
- `Administrador` (50)

Escreva um programa que receba um nível de acesso numérico (simule a leitura de um número inteiro, por exemplo, `10`) e use casting para converter esse número para o `enum` `NivelAcesso`, imprimindo o resultado na tela.

---

## Exercício 3: Status de Semáforo

**Objetivo:** Crie um `enum` chamado `Semaforo` com os estados `Vermelho`, `Amarelo` e `Verde`. Escreva um método chamado `ExibirAcao` que aceite um parâmetro do tipo `Semaforo`. Dentro do método, use uma estrutura switch para imprimir a ação apropriada para cada cor:
- `Vermelho`: "Parar"
- `Amarelo`: "Atenção"
- `Verde`: "Seguir"

Chame essa função três vezes na sua Main com cada um dos estados.

