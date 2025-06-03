// Alunos: 
// Lucas Vitorelli de Oliveira 2211600019
// Mateus Henrique Silva Rizzo 2211600241
// Mauricio Cortez 2211600068
using System;
using Irony.Parsing;

class Program
{
    static void Main()
    {
        var grammar = new DragonDevZGrammar();
        var parser = new Parser(grammar);

        string input = @"
                    Goku?¿/
                    kameNum1:i;
                    kameResult < kameNum1 +> 3;
                    Freeza?kameResult¿;
                    Vegeta?kameResult <<>> 10¿ Majin /
                    kameResult < kameNum1 <- 10;
                    \\ Bulma /
                    kameResult < kameNum1 </ 3;
                \\
                ";

        Console.WriteLine("Expressão de entrada:");
        Console.WriteLine(input);
        Console.WriteLine("\n--- Tokens Reconhecidos ---");

        var tree = parser.Parse(input);
        
        if (tree == null || tree.Tokens == null)
        {
            Console.WriteLine("Falha na análise léxica: árvore ou tokens nulos.");
            if (tree?.ParserMessages != null)
            {
                foreach (var err in tree.ParserMessages)
                    Console.WriteLine($"Erro: {err.Message}");
            }
            return;
        }

        foreach (var token in tree.Tokens)
        {
            Console.WriteLine($"{token.Text} - {token.Terminal.Name}");
        }

        if (tree.HasErrors())
        {
            Console.WriteLine("\nErros:");
            foreach (var msg in tree.ParserMessages)
                Console.WriteLine(msg.Message);
        }
    }
}