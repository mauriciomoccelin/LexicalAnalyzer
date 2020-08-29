using Analyser.Lexical;
using FluentAssertions;
using System;
using System.Text.RegularExpressions;
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

            lexer.AddDefinition(
                TokenDefinition.Factory.Create(
                    new Regex(@"\d+"), "NUMBER"
                )
            );

            lexer.AddDefinition(
                TokenDefinition.Factory.Create(
                    new Regex(@"\d+(\.\d{1,2})m?"), "DECIMAL"
                )
            );

            // Types

            lexer.AddDefinition(
                TokenDefinition.Factory.Create(
                    new Regex(@"\d+(\.\d{1,2})m?"), "TYPE_INT"
                )
            );

            lexer.AddDefinition(
                TokenDefinition.Factory.Create(
                    new Regex(@"\d+(\.\d{1,2})m?"), "TYPE_REAL"
                )
            );

            var tokens = lexer.Tokenize("35");

            tokens.Should().HaveCount(2);
        }
    }
}
