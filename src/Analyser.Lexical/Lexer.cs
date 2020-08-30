using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Analyser.Lexical
{
    public sealed class Lexer : ILexer
    {
        private readonly Regex endOfLineRegex;
        private readonly IList<TokenDefinition> tokenDefinitions;

        public Lexer()
        {
            endOfLineRegex = new Regex(@"\}|\}", RegexOptions.Compiled);
            tokenDefinitions = new List<TokenDefinition>();
        }

        public void AddDefinition(
            TokenDefinition tokenDefinition
        )
        {
            tokenDefinitions.Add(tokenDefinition);
        }

        public IEnumerable<Token> Tokenize(
            string source
        )
        {
            int currentIndex = 0;
            int currentLine = 1;
            int currentColumn = 0;

            while (currentIndex < source.Length)
            {
                TokenDefinition matchedDefinition = null;
                int matchLength = 0;

                foreach (var rule in tokenDefinitions)
                {
                    var match = rule.Regex.Match(source, currentIndex);

                    if (match.Success && (match.Index - currentIndex) == 0)
                    {
                        matchedDefinition = rule;
                        matchLength = match.Length;
                        break;
                    }
                }

                if (matchedDefinition == null)
                {
                    throw new Exception(
                        string.Format(
                            "Invalid Token in '{0}' at index {1} (line {2}, column {3}).",
                            source[currentIndex],
                            currentIndex,
                            currentLine,
                            currentColumn
                        )
                    );
                }
                else
                {
                    var value = source.Substring(currentIndex, matchLength);

                    if (!matchedDefinition.IsIgnored)
                        yield return new Token(
                            matchedDefinition.Type,
                            value,
                            new TokenPosition(
                                currentLine,
                                currentIndex,
                                currentColumn
                            )
                        );

                    var endOfLineMatch = endOfLineRegex.Match(value);
                    if (endOfLineMatch.Success)
                    {
                        currentLine += 1;
                        currentColumn = value.Length - (
                            endOfLineMatch.Index + endOfLineMatch.Length
                        );
                    }
                    else
                    {
                        currentColumn += matchLength;
                    }

                    currentIndex += matchLength;
                }
            }

            yield return new Token(
                TokenTypeEnum.Eof,
                null,
                new TokenPosition(
                    currentLine,
                    currentIndex,
                    currentColumn
                )
            );
        }
    }
}