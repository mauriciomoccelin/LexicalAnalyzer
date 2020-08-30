using Xunit;

namespace Analyser.Lexical.Tests
{
    public class Lexer_Test
    {
        private readonly ILexer lexer;

        public Lexer_Test() { lexer = new Lexer(); }

        [Fact]
        public void Tokenize()
        {
            // Act

            // Numeric
        }
    }
}
