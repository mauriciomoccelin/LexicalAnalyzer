using System.Collections.Generic;
using System.Linq;
using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public class Parser : IParser
    {
        private int scope;
        private IList<Symbol> symbols;
        private readonly IList<string> errors;
        private readonly IEnumerator<Token> tokens;

        public Parser(IEnumerable<Token> tokens)
        {
            scope = -1;
            errors = new List<string>();
            symbols = new List<Symbol>();
            this.tokens = tokens?.GetEnumerator() ?? Enumerable.Empty<Token>().GetEnumerator();
        }
        
        public void Program()
        {
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
                    else AddError(tokens.Current.ToString());
                } 
                else AddError(tokens.Current.ToString());
            }
            else AddError(tokens.Current.ToString());
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
            else AddError(tokens.Current.ToString());

            symbols = symbols.Where(x => !x.Scoped.Equals(scope)).ToList();
            scope--;
        }

        public void VariableDeclaration()
        {
            if (tokens.Current.IsIdentifier())
            {
                AddSymbol();
                tokens.MoveNext();
                while (tokens.Current.IsComma())
                {
                    tokens.MoveNext();
                    if (tokens.Current.IsIdentifier())
                    {
                        AddSymbol();
                        tokens.MoveNext();
                    }
                    else AddError(tokens.Current.ToString());
                }

                if (tokens.Current.Type == TokenTypeEnum.Semicolon)
                {
                    tokens.MoveNext();
                }
                else AddError(tokens.Current.ToString());
            }
            else AddError(tokens.Current.ToString());
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
            else AddError(tokens.Current.ToString());
        }

        public void BasicCommandDeclaration()
        {
            
        }
        
        public void InteractionCommandDeclaration()
        {
            
        }

        public void ConditionalCommandDeclaration()
        {
            
        }

        public void AddError(string error)
        {
            errors.Add($"Incorrect Syntrax near: {error}");
        }
        
        private void AddSymbol()
        {
            switch (tokens.Current.Type)
            {
                case TokenTypeEnum.TypeInt:
                    symbols.Add(Symbol.Factory.CreateForIntType(scope, tokens.Current.Value));
                    break;
                case TokenTypeEnum.TypeChar:
                    symbols.Add(Symbol.Factory.CreateForCharType(scope, tokens.Current.Value));
                    break;
                case TokenTypeEnum.TypeFloat:
                    symbols.Add(Symbol.Factory.CreateForFloatType(scope, tokens.Current.Value));
                    break;
                default:
                    AddError(tokens.Current.ToString());
                    break;
            }
        }
    }
}