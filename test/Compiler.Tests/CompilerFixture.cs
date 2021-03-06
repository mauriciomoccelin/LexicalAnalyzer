using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Analyser.Lexical;
using Analyzer.Syntactic;
using Xunit;

namespace Compiler.Tests
{
    [CollectionDefinition(nameof(CompilerColletion))]
    public class CompilerColletion : ICollectionFixture<CompilerFixture> { }
    public class CompilerFixture : IDisposable
    {
        public string GetFullCodeSample()
        {
            const string code = @"
                int main() {
                    int one,two,three;
                    real result;
                    
                    one = 1;
                    two = 2;
                    three = 3;
                    
                    result = 5 / one + two + three * 8 + 5 * 7;
                    
                    while (one == two) {
                        one = 1;
                    }

                    
                    if (one == two) {
                        three = 4;    
                    }
                    else {
                        three = 3;
                    }
                }
            ";

            return code;
        }

        public ILexer GetLexer()
        {
            ILexer lexer = new Lexer();

            #region Numeric

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\d+"), TokenTypeEnum.Number));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\d+(\.\d{1,2})m?"), TokenTypeEnum.Real));

            #endregion

            #region Operator Tokens

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\^"), TokenTypeEnum.OperatorPotentiation));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\+"), TokenTypeEnum.OperatorSum));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\-"), TokenTypeEnum.OperatorSubtraction));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\*"), TokenTypeEnum.OperatorMultiplication));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\/"), TokenTypeEnum.OperatorDivision));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\="), TokenTypeEnum.Assignment));

            #endregion

            #region Relational Operators

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"[==]+"), TokenTypeEnum.OperatorEquals));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"[<=]+"), TokenTypeEnum.OperatorLessEquals));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"[>=]+"), TokenTypeEnum.OperatorGreaterEquals));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\>"), TokenTypeEnum.OperatorBigger));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\<"), TokenTypeEnum.OperatorSmaller));

            #endregion

            #region Special Characters

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\' '"), TokenTypeEnum.Space, true));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\s+"), TokenTypeEnum.Tab, true));
            
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\t+"), TokenTypeEnum.Tab));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\n]+"), TokenTypeEnum.LineBreak));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\d+"), TokenTypeEnum.LineBreak));

            #endregion

            #region Delimiters

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\{"), TokenTypeEnum.OpenKeys));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\}"), TokenTypeEnum.CloseKeys));

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\("), TokenTypeEnum.OpenParentheses));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\)"), TokenTypeEnum.CloseParentheses));

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\["), TokenTypeEnum.OpenBrackets));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\]"), TokenTypeEnum.CloseBrackets));

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\."), TokenTypeEnum.Dot));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\,"), TokenTypeEnum.Comma));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"\;"), TokenTypeEnum.Semicolon));

            #endregion

            #region Reserved Words

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"main"), TokenTypeEnum.Main));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"var"), TokenTypeEnum.ReservedWordVar));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"while"), TokenTypeEnum.ReservedWordWhile));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"for"), TokenTypeEnum.ReservedWordFor));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"if"), TokenTypeEnum.ReservedWordIf));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"else"), TokenTypeEnum.ReservedWordElse));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"switch"), TokenTypeEnum.ReservedWordSwitch));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"case"), TokenTypeEnum.ReservedWordCase));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"return"), TokenTypeEnum.ReservedWordReturn));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"function"), TokenTypeEnum.ReservedWordFunction));

            #endregion
            
            #region Types

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"int"), TokenTypeEnum.TypeInt));
            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"real"), TokenTypeEnum.TypeReal));

            #endregion
            
            #region Identifiers

            lexer.AddDefinition(TokenDefinition.Factory.Create(new Regex(@"[A-Za-z_][a-zA-Z0-9_]+"), TokenTypeEnum.Identifier));

            #endregion

            return lexer;
        }

        public IParser GetParser(IEnumerable<Token> tokens)
        {
            IParser parser = new Parser(tokens);
            return parser;
        }

        public void Dispose() { }
    }
}