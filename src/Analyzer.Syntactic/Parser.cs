using System.Collections.Generic;
using System.Linq;
using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public class Parser : IParser
    {
        private readonly IList<string> errors;
        private readonly IEnumerator<Token> tokens;

        public Parser(IEnumerable<Token> tokens)
        {
            errors = new List<string>();
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
        }

        public void VariableDeclaration()
        {
            throw new System.NotImplementedException();
        }

        public void CommandDeclaration()
        {
            throw new System.NotImplementedException();
        }

        public void AddError(string error)
        {
            errors.Add($"Incorrect Syntrax near: {error}");
        }
    }
    
    public static class First
    {
        private static readonly TokenTypeEnum[] types = {
            TokenTypeEnum.TypeInt,
            TokenTypeEnum.TypeChar,
            TokenTypeEnum.TypeFloat,
        };

        private static readonly TokenTypeEnum[] command = {
            TokenTypeEnum.Identifier
        };
        
        public static bool IsVariableDeclaration(this Token token)
        {
            return types.Contains(token.Type);
        }
        
        public static bool IsCommand(this Token token)
        {
            return command.Contains(token.Type);
        }
    }
}