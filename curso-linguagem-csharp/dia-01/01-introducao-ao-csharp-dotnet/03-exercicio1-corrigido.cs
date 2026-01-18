// Exercício 2 Corrigido: Primeiro Projeto Console
// Arquivo: Program.cs

// Usando top-level statements (C# 10+)
// Este é o ponto de entrada da aplicação

Console.WriteLine("Olá! Bem-vindo ao mundo C#!");
Console.WriteLine("Meu nome é: João Silva");
Console.WriteLine("Este é meu primeiro projeto em .NET 8");

/* 
 * EXPLICAÇÃO:
 * 
 * 1. Console.WriteLine() escreve uma linha no terminal
 * 2. Cada chamada adiciona uma quebra de linha automaticamente
 * 3. O texto entre aspas duplas é uma string literal
 * 4. Este código usa "top-level statements" introduzido no C# 9
 *    e aprimorado no C# 10
 * 
 * VERSÃO TRADICIONAL (se você preferir):
 * 
 * using System;
 * 
 * namespace MeuPrimeiroProjeto
 * {
 *     class Program
 *     {
 *         static void Main(string[] args)
 *         {
 *             Console.WriteLine("Olá! Bem-vindo ao mundo C#!");
 *             Console.WriteLine("Meu nome é: João Silva");
 *             Console.WriteLine("Este é meu primeiro projeto em .NET 8");
 *         }
 *     }
 * }
 * 
 * Ambas as formas são válidas! A versão moderna é mais concisa.
 */