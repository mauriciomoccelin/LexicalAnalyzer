using System.Linq;
using Analyser.Lexical;
using FluentAssertions;
using Xunit;

namespace Compiler.Tests
{
    [Collection(nameof(CompilerColletion))]
    public class Lexer_Test
    {
        private readonly CompilerFixture fixture;
        public Lexer_Test(CompilerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Compiler_AnalyserLexical_BeSuccess()
        {
            // Arrange
            var code = fixture.GetFullCodeSample();
            var lexer = fixture.GetLexer();

            // Act
            var tokens = lexer.Tokenize(code);
            
            // Assert
            var enumerable = tokens as Token[] ?? tokens.ToArray();
            enumerable.Any(x => x.Type.Equals(TokenTypeEnum.Eof)).Should().BeTrue();
        }
        
        [Fact]
        public void Compiler_AnalyserSyntatic_BeSuccess()
        {
            // Arrange
            var code = fixture.GetFullCodeSample();
            var lexer = fixture.GetLexer();
            var tokens = lexer.Tokenize(code);
            var parser = fixture.GetParser(tokens);
            
            // Act
            var parserResult = parser.Invoking(p => p.Run());
            
            // Assert
            parserResult.Should().NotThrow();
        }
    }
}
