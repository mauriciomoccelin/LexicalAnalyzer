namespace Analyser.Lexical
{
    public sealed class Token
    {
        public TokenTypeEnum Type { get; private set; }
        public string Value { get; private set; }
        public TokenPosition Position { get; private set; }
        
        internal Token(
            TokenTypeEnum type,
            string value,
            TokenPosition position
        )
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public override string ToString()
        {
            var type = $"Token: Type: {Type}";
            var value = $"Value: {Value}";
            var position = $"Position => Index: {Position.Index}";
            var line = $"Line: {Position.Line}";
            var column = $"Column: {Position.Column}";
            
            return $"{type}, {value}, {position}, {line}, {column}";
        }
    }
}