#nullable enable
using System;
using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public class Expressions
    {
        public string Lexeme { get; }
        public TokenTypeEnum Type { get; }
        
        private Expressions(string lexeme, TokenTypeEnum type)
        {
            Lexeme = lexeme;
            Type = type;
        }

        public override bool Equals(object? other)
        {
            return other != null && other.GetHashCode().Equals(GetHashCode());
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type);
        }

        public static class Factory
        {
            public static Expressions Empty()
            {
                return new Expressions(string.Empty, TokenTypeEnum.Eof);
            }
            
            public static Expressions Create(string lexeme)
            {
                return new Expressions(lexeme, TokenTypeEnum.Undefined);
            }
            
            public static Expressions Create(string lexeme, TokenTypeEnum type)
            {
                return new Expressions(lexeme, type);
            }
        }
    }
}