namespace Analyser.Lexical
{
    public sealed class Token
    {
        public string Type { get; private set; }
        public string Value { get; private set; }
        public TokenPosition Position { get; private set; }
        
        internal Token(
            string type,
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
            return $@"
                Token: Type: {Type}, 
                Value: {Value}, 
                Position => Index: {Position.Index},
                Line: {Position.Line},
                Column: {Position.Column}
            ";
        }
    }
}