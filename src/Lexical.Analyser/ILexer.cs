using System.Collections.Generic;

namespace Lexical.Analyser
{
    public interface ILexer
    {
        IEnumerable<Token> Tokenize(string source);
        void AddDefinition(TokenDefinition tokenDefinition);
    }
}