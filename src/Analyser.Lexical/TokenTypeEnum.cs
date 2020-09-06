namespace Analyser.Lexical
{
    public enum TokenTypeEnum
    {
        Eof = -1,
        Main = 0,
        OpenParentheses = 1,
        CloseParentheses = 2,
        OpenKeys = 3,
        CloseKeys = 4,
        TypeInt = 5,
        TypeChar = 6,
        TypeFloat = 7,
        Identifier = 8,
        Comma = 9,
        Semicolon = 10,
        ConditionalIf = 11,
        InteractionWhile = 12,
        Assignment = 13
    }
}