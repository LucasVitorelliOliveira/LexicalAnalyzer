using Irony.Parsing;
using System;

public class DragonDevZGrammar : Grammar
{
    public DragonDevZGrammar() : base(true)
    {
        // ------------------------------------------
        // Terminais básicos
        // ------------------------------------------

        // Comentários entre $...$
        var comentario = new CommentTerminal("Comentário", "$", "$");
        NonGrammarTerminals.Add(comentario);

        // Espaços em branco
        var branco = new RegexBasedTerminal("Espaço em branco", @"[\t \r \n \v \f]+");
        NonGrammarTerminals.Add(branco);

        // Strings e Chars
        var stringLiteral = new StringLiteral("String", "\"");
        var charLiteral = new StringLiteral("Char", "'", StringOptions.IsChar);

        // Números
        var numeroInteiro = new NumberLiteral("Número Inteiro", NumberOptions.IntOnly);
        var numeroFloat = new NumberLiteral("Número Float", NumberOptions.AllowSign | NumberOptions.AllowStartEndDot);

        // Identificador: kame + Letra maiúscula + letras/dígitos
        var variavel = new RegexBasedTerminal("Variável", @"kame[A-Z][A-Za-z0-9]*");

        // Tipo sufixado (ex: :i, :f, :s)
        var tipo = new RegexBasedTerminal("Tipo", @":[ifsbc]");

        // ------------------------------------------
        // Palavras reservadas
        // ------------------------------------------
        var palavrasReservadas = new[]
        {
            "Goku", "Dabura", "kame", "Yamcha", "Freeza",
            "Vegeta", "Bulma", "Kuririn", "Napa", "Majin"
        };

        foreach (var palavra in palavrasReservadas)
            this.MarkReservedWords(palavra);

        foreach (var palavra in palavrasReservadas)
            this.KeyTerms[palavra] = ToTerm(palavra);

        // ------------------------------------------
        // Operadores personalizados
        // ------------------------------------------
        var simbolosCompostos = new[]
        {
            "<<>>",    // Igualdade
            "<!>",     // Diferença
            "<<=",     // Menor ou Igual
            "=>>",     // Maior ou Igual
            "+>",      // Soma
            "<-",      // Subtração
            "*>",      // Multiplicação
            "</",      // Divisão
            ">>",      // Maior que
            "<<",      // Menor que
            "<",       // Atribuição
            "@",       // E lógico
            "#",       // OU lógico
        };

        // Registra os operadores complexos primeiro
        foreach (var op in simbolosCompostos)
            this.KeyTerms[op] = ToTerm(op);

        // ------------------------------------------
        // Símbolos e pontuação
        // ------------------------------------------
        var simbolos = new[] { ";", "/", "\\", "?", "¿" };

        foreach (var simbolo in simbolos)
            this.KeyTerms[simbolo] = ToTerm(simbolo);

        // ------------------------------------------
        // Regra de Root (mínima para funcionar)
        // ------------------------------------------
        var dummy = new NonTerminal("dummy");
        var token = new NonTerminal("token");
        token.Rule = stringLiteral | charLiteral | numeroInteiro | numeroFloat | tipo | variavel;

        // Adiciona todos os KeyTerms dinamicamente
        foreach (var key in this.KeyTerms.Values)
            token.Rule |= key;

        // Repetição de tokens reconhecíveis
        dummy.Rule = MakePlusRule(dummy, token);
        this.Root = dummy;
    }
}