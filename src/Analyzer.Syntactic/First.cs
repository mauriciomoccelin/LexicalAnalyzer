using System.Collections.Generic;
using System.Linq;
using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public static class First
    {
        private static readonly TokenTypeEnum[] types = 
        {
            TokenTypeEnum.TypeInt,
            TokenTypeEnum.TypeChar,
            TokenTypeEnum.TypeReal,
        };

        private static readonly TokenTypeEnum[] commandBasic =
        {
            TokenTypeEnum.OpenKeys,
            TokenTypeEnum.Identifier
        };
        
        private static readonly TokenTypeEnum[] command = 
        {
            TokenTypeEnum.OpenKeys,
            TokenTypeEnum.Identifier,
            TokenTypeEnum.ReservedWordFor,
            TokenTypeEnum.InteractionWhile
        };
        
        private static readonly TokenTypeEnum[] factor = 
        {
            TokenTypeEnum.OperatorMultiplication,
            TokenTypeEnum.OperatorDivision
        };
        
        private static readonly TokenTypeEnum[] arithmeticOperation = 
        {
            TokenTypeEnum.OperatorSum,
            TokenTypeEnum.OperatorSubtraction
        };

        private static readonly TokenTypeEnum[] relationalOperation =
        {
            TokenTypeEnum.OperatorEquals,
            TokenTypeEnum.OperatorLessEquals,
            TokenTypeEnum.OperatorGreaterEquals,
            TokenTypeEnum.OperatorBigger,
            TokenTypeEnum.OperatorSmaller,
        };
        
        public static bool IsVariableDeclaration(this Token token)
        {
            return types.Contains(token.Type);
        }
        
        public static bool IsIdentifier(this Token token)
        {
            return token.Type == TokenTypeEnum.Identifier;
        }

        public static bool IsComma(this Token token)
        {
            return token.Type == TokenTypeEnum.Comma;
        }
        
        public static bool IsSemicolon(this Token token)
        {
            return token.Type == TokenTypeEnum.Semicolon;
        }
        
        public static bool IsCommand(this Token token)
        {
            return command.Contains(token.Type);
        }
        
        public static bool IsBasicCommand(this Token token)
        {
            return commandBasic.Contains(token.Type);
        }
        
        public static bool IsInteractionCommand(this Token token)
        {
            return token.Type == TokenTypeEnum.InteractionWhile;
        }

        public static bool IsConditionalCommand(this Token token)
        {
            return token.Type == TokenTypeEnum.ConditionalIf;
        }

        public static bool IsAssignment(this Token token)
        {
            return token.Type == TokenTypeEnum.Assignment;
        }
        
        public static bool IsBlockAssignment(this Token token)
        {
            return token.Type == TokenTypeEnum.OpenKeys;
        }
        
        public static bool IsFactorInExpression(this Token token)
        {
            return factor.Contains(token.Type);
        }
        
        public static bool IsArithmeticOperation(this Token token)
        {
            return arithmeticOperation.Contains(token.Type);
        }

        public static bool IsRelationalOperation(this Token token)
        {
            return relationalOperation.Contains(token.Type);
        }
    }
}