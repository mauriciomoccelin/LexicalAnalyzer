using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public class Symbol
    {
        public int Scoped { get; }
        public string Lexeme { get; }
        public TokenTypeEnum Type { get; }

        private Symbol(int scoped, string lexeme, TokenTypeEnum type)
        {
            Scoped = scoped;
            Lexeme = lexeme;
            Type = type;
        }
        
        public static class Factory
        {
            public static Symbol CreateForIntType(int scoped,string lexeme)
            {
                return new Symbol(scoped, lexeme, TokenTypeEnum.TypeInt);
            }
            
            public static Symbol CreateForCharType(int scoped,string lexeme)
            {
                return new Symbol(scoped, lexeme, TokenTypeEnum.TypeChar);
            }
            
            public static Symbol CreateForFloatType(int scoped,string lexeme)
            {
                return new Symbol(scoped, lexeme, TokenTypeEnum.TypeFloat);
            }
        }
    }
}