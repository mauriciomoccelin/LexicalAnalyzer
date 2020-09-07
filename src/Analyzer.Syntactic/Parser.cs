using System.Collections.Generic;
using System.Linq;
using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public class Parser : IParser
    {
        private int scope;
        private IList<Symbols> symbols;
        private readonly IList<string> errors;
        private readonly IEnumerator<Token> tokens;

        public Parser(IEnumerable<Token> tokens)
        {
            scope = -1;
            errors = new List<string>();
            symbols = new List<Symbols>();
            this.tokens = tokens?.GetEnumerator() ?? Enumerable.Empty<Token>().GetEnumerator();
        }

        public void Run()
        {
            tokens.MoveNext();
            Program();
        }

        public void Program()
        {
            if (tokens.Current.Type.Equals(TokenTypeEnum.TypeInt))
            {
                tokens.MoveNext();
                if (tokens.Current.Type.Equals(TokenTypeEnum.Main))
                {
                    tokens.MoveNext();
                    if (tokens.Current.Type.Equals(TokenTypeEnum.OpenParentheses))
                    {
                        tokens.MoveNext();
                        if (tokens.Current.Type.Equals(TokenTypeEnum.CloseParentheses))
                        {
                            tokens.MoveNext();
                            Block();
                        }
                        else AddError(tokens.Current);
                    } 
                    else AddError(tokens.Current);
                }
                else AddError(tokens.Current);   
            }
            else AddError(tokens.Current);
        }

        public void Block()
        {
            scope++;
            if (tokens.Current.Type.Equals(TokenTypeEnum.OpenKeys))
            {
                tokens.MoveNext();
                while (tokens.Current.IsVariableDeclaration())
                {
                    VariableDeclaration();
                }
                
                while (tokens.Current.IsCommand())
                {
                    CommandDeclaration();
                }
            }
            else AddError(tokens.Current);

            symbols = symbols.Where(x => !x.Scoped.Equals(scope)).ToList();
            scope--;
        }

        public void VariableDeclaration()
        {
            var type = tokens.Current.Type;
            tokens.MoveNext();
            
            if (tokens.Current.IsIdentifier())
            {
                AddSymbol(type);
                tokens.MoveNext();
                while (tokens.Current.IsComma())
                {
                    tokens.MoveNext();
                    if (tokens.Current.IsIdentifier())
                    {
                        AddSymbol(type);
                        tokens.MoveNext();
                    }
                    else AddError(tokens.Current);
                }

                if (tokens.Current.IsSemicolon())
                {
                    tokens.MoveNext();
                }
                else AddError(tokens.Current);
            }
            else AddError(tokens.Current);
        }

        public void CommandDeclaration()
        {
            if (tokens.Current.IsBasicCommand())
            {
                BasicCommandDeclaration();
            }
            else if (tokens.Current.IsInteractionCommand())
            {
                InteractionCommandDeclaration();
            }
            else if (tokens.Current.IsConditionalCommand())
            {
                ConditionalCommandDeclaration();
            }
            else
            {
                AddError(tokens.Current);
                tokens.MoveNext();
            }
        }

        public void BasicCommandDeclaration()
        {
            if (tokens.Current.IsIdentifier())
            {
                AssignmentDeclaration();
            }
            else if (tokens.Current.IsBlockAssignment())
            {
                Block();
            }
            else AddError(tokens.Current);
        }

        public void AssignmentDeclaration()
        {
            if (tokens.Current.IsIdentifier())
            {
                var symbol = GetSymbol(tokens.Current);
                var firstExpressions = Expressions.Factory.Empty();

                if (symbol != null) firstExpressions = Expressions.Factory.Create(symbol.Lexeme, symbol.Type);
                else AddError(tokens.Current);

                tokens.MoveNext();

                if (tokens.Current.IsAssignment())
                {
                    tokens.MoveNext();

                    var secondExpressions = FirstArithmeticExpressionDeclaration();

                    CheckExpressions(firstExpressions, secondExpressions);

                    if (tokens.Current.IsSemicolon())
                    {
                        tokens.MoveNext();
                    }
                    else AddError(tokens.Current);
                }
                else AddError(tokens.Current);
            }
            else AddError(tokens.Current);
        }

        public void InteractionCommandDeclaration()
        {
            throw new System.NotImplementedException();
        }

        public void ConditionalCommandDeclaration()
        {
            throw new System.NotImplementedException();
        }
        
        public Expressions FirstArithmeticExpressionDeclaration()
        {
            var first = TermDeclaration();
            var second = SecondArithmeticExpressionDeclaration();
                        
            if (second is null) return first;
            
            var third = Expressions.Factory.Create(tokens.Current.Value, second.Type);
            return third;
        }

        private Expressions TermDeclaration()
        {
            var first = FactorDeclaration();

            while (tokens.Current.IsFactorInExpression())
            {
                tokens.MoveNext();
                var second = FactorDeclaration();
                
                var third = Expressions.Factory.Create(tokens.Current.Value, second.Type);

                first = third;
            }

            return first;
        }

        private Expressions FactorDeclaration()
        {
            throw new System.NotImplementedException();
        }

        private Expressions SecondArithmeticExpressionDeclaration()
        {
            throw new System.NotImplementedException();
        }

        public void AddError(Token token)
        {
            errors.Add($"Incorrect Syntrax near: {token}");
        }
        
        public void CheckExpressions(Expressions first, Expressions second)
        {
            if (!first.Equals(second)) AddError(tokens.Current);
        }

        private Symbols GetSymbol(Token token)
        {
            var symbol = scope < 0
                ? symbols.First(x => string.Equals(x.Lexeme, token.Value))
                : symbols.First(x => string.Equals(x.Lexeme, token.Value) && x.Scoped.Equals(scope));

            return symbol;
        }

        private void AddSymbol(TokenTypeEnum type)
        {
            switch (type)
            {
                case TokenTypeEnum.TypeInt:
                    symbols.Add(Symbols.Factory.CreateForIntType(scope, tokens.Current.Value));
                    break;
                case TokenTypeEnum.TypeChar:
                    symbols.Add(Symbols.Factory.CreateForCharType(scope, tokens.Current.Value));
                    break;
                case TokenTypeEnum.TypeFloat:
                    symbols.Add(Symbols.Factory.CreateForFloatType(scope, tokens.Current.Value));
                    break;
                default:
                    AddError(tokens.Current);
                    break;
            }
        }
    }
}
