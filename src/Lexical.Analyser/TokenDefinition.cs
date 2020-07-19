using System.Text.RegularExpressions;

namespace Lexical.Analyser
{
    public sealed class TokenDefinition
    {
        public string Type { get; private set; }
        public Regex Regex { get; private set; }
        public bool IsIgnored { get; private set; }

        private TokenDefinition(
            Regex regex,
            string type,
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
                string type,
                bool isIgnored = false
            )
            {
                return new TokenDefinition(regex, type, isIgnored);
            }
        }
    }
}