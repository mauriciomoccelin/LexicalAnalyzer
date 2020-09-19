using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public class Symbols
    {
        public int Scoped { get; }
        public string Lexeme { get; }
        public TokenTypeEnum Type { get; }

        private Symbols(int scoped, string lexeme, TokenTypeEnum type)
        {
            Scoped = scoped;
            Lexeme = lexeme;
            Type = type;
        }
        
        public static class Factory
        {
            public static Symbols CreateForIntType(int scoped,string lexeme)
            {
                return new Symbols(scoped, lexeme, TokenTypeEnum.TypeInt);
            }
            
            public static Symbols CreateForCharType(int scoped,string lexeme)
            {
                return new Symbols(scoped, lexeme, TokenTypeEnum.TypeChar);
            }
            
            public static Symbols CreateForFloatType(int scoped,string lexeme)
            {
                return new Symbols(scoped, lexeme, TokenTypeEnum.TypeReal);
            }
        }
    }
}