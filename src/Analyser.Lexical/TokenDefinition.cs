using System.Text.RegularExpressions;

namespace Analyser.Lexical
{
    public sealed class TokenDefinition
    {
        public TokenTypeEnum Type { get; private set; }
        public Regex Regex { get; private set; }
        public bool IsIgnored { get; private set; }

        private TokenDefinition(
            Regex regex,
            TokenTypeEnum type,
            bool isIgnored
        )
        {
            Type = type;
            Regex = regex;
            IsIgnored = isIgnored;
        }

        public static class Factory
        {
            public static TokenDefinition Create(
                Regex regex,
                TokenTypeEnum type,
                bool isIgnored = false
            )
            {
                return new TokenDefinition(regex, type, isIgnored);
            }
        }
    }
}