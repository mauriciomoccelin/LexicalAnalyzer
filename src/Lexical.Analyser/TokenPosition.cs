namespace Lexical.Analyser
{
    public sealed class TokenPosition
    {
        public int Line { get; private set; }
        public int Index { get; private set; }
        public int Column { get; private set; }

        internal TokenPosition(
            int line,
            int index,
            int column
        )
        {
            Line = line;
            Index = index;
            Column = column;
        }
    }
}