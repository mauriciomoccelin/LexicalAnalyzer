using System.Collections.Generic;
using System.Linq;
using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public class Parser : IParser
    {
        private int scope;
        private int typeExpression;
        private IList<Symbols> symbols;
        private readonly IList<string> errors;
        private readonly IEnumerator<Token> tokens;

        public Parser(IEnumerable<Token> tokens)
        {
            scope = -1;
            typeExpression = 0;
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
            tokens.MoveNext();
            if (tokens.Current.Type.Equals(TokenTypeEnum.OpenParentheses))
            {
                tokens.MoveNext();
                var expressions = RelationalExpressionDeclaration();
                // TODO: How to generate validate expression

                if (tokens.Current.Type.Equals(TokenTypeEnum.CloseParentheses))
                {
                    tokens.MoveNext();
                    CommandDeclaration();
                }
                else AddError(tokens.Current);
                
                // TODO: How to generate return to the initial call
            }
            else AddError(tokens.Current);
            
            throw new System.NotImplementedException();
        }

        public void ConditionalCommandDeclaration()
        {
            if (tokens.Current.Type.Equals(TokenTypeEnum.ReservedWordIf))
            {
                tokens.MoveNext();
                if (tokens.Current.Type.Equals(TokenTypeEnum.OpenParentheses))
                {
                    tokens.MoveNext();
                    var expressions = RelationalExpressionDeclaration();
                    // TODO: How to skip to the else

                    if (tokens.Current.Type.Equals(TokenTypeEnum.CloseParentheses))
                    {
                        tokens.MoveNext();
                        CommandDeclaration();
                        
                        if (tokens.Current.Type.Equals(TokenTypeEnum.ReservedWordElse))
                        {
                            // TODO: How to jump to the end or something else
                            
                            tokens.MoveNext();
                            CommandDeclaration();
                        }
                    }
                    else AddError(tokens.Current);
                }
                else AddError(tokens.Current);
            }
            else AddError(tokens.Current);
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
            var type = TokenTypeEnum.Undefined;
            Expressions first;
            
            if (tokens.Current.Equals(TokenTypeEnum.OpenParentheses))
            {
                tokens.MoveNext();
                first = FirstArithmeticExpressionDeclaration();
                if (tokens.Current.Equals(TokenTypeEnum.CloseParentheses))
                {
                    tokens.MoveNext();
                } else AddError(tokens.Current);

                return first;
            }

            var lexeme = tokens.Current.Value;
            if (tokens.Current.IsAssignment())
            {
                var symbol = GetSymbol(tokens.Current);
                if (symbol is null) AddError(tokens.Current);

                type = symbol.Type;
                tokens.MoveNext();
            }
            else switch (tokens.Current.Type)
            {
                case TokenTypeEnum.Number:
                    type = TokenTypeEnum.Number;
                    tokens.MoveNext();
                    break;
                case TokenTypeEnum.Real:
                    type = TokenTypeEnum.Real;
                    tokens.MoveNext();
                    break;
                case TokenTypeEnum.Char:
                    type = TokenTypeEnum.Char;
                    tokens.MoveNext();
                    break;
                default:
                    AddError(tokens.Current);
                    break;
            }

            first = Expressions.Factory.Create(lexeme, type);
            return first;
        }

        private string RelationalExpressionDeclaration()
        {
            var first = FirstArithmeticExpressionDeclaration();
            var operation = RelationalOperationDeclaration();
            var second = FirstArithmeticExpressionDeclaration();
            
            // TODO: How to build this relational operation?

            var third = Expressions.Factory.Create(CreateTypeExpression());

            return third.Lexeme;
        }
        
        private TokenTypeEnum RelationalOperationDeclaration()
        {
            var operation = TokenTypeEnum.Undefined;

            if (tokens.Current.IsRelationalOperation())
            {
                operation = tokens.Current.Type;
                tokens.MoveNext();
            } else AddError(tokens.Current);

            return operation;
        }

        private Expressions SecondArithmeticExpressionDeclaration()
        {
            var operation = TokenTypeEnum.Undefined;
            Expressions first;
            Expressions second;
            
            if (tokens.Current.IsArithmeticOperation())
            {
                operation = tokens.Current.Type;
                tokens.MoveNext();
                first = TermDeclaration();
                second = SecondArithmeticExpressionDeclaration();
            }
            else return null; // λ

            if (second is null)
            {
                first = Expressions.Factory.Create(first.Lexeme, operation);
                return first;
            }

            if (first.Type.Equals(TokenTypeEnum.TypeInt) && second.Type.Equals(TokenTypeEnum.TypeReal))
            {
                first = Expressions.Factory.Create(first.Lexeme, TokenTypeEnum.TypeReal);
            }
            else if (first.Type.Equals(TokenTypeEnum.TypeReal) && first.Type.Equals(TokenTypeEnum.TypeInt))
            {
                second = Expressions.Factory.Create(second.Lexeme, TokenTypeEnum.TypeReal);
            }

            return Expressions.Factory.Create(CreateTypeExpression(), second.Type);
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

        private string CreateTypeExpression()
        {
            return "TypeExpression" + typeExpression++;
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
                case TokenTypeEnum.TypeReal:
                    symbols.Add(Symbols.Factory.CreateForFloatType(scope, tokens.Current.Value));
                    break;
                default:
                    AddError(tokens.Current);
                    break;
            }
        }
    }
}
