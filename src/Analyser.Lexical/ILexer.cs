using System.Collections.Generic;

namespace Analyser.Lexical
{
    public interface ILexer
    {
        IEnumerable<Token> Tokenize(string source);
        void AddDefinition(TokenDefinition tokenDefinition);
    }
}