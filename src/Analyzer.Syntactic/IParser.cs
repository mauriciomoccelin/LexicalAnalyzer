using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public interface IParser
    {
        void Program();
        void Block();
        void VariableDeclaration();
        void CommandDeclaration();
        void BasicCommandDeclaration();
        void InteractionCommandDeclaration();
        void ConditionalCommandDeclaration();
        void AssignmentDeclaration();
        Expressions ArithmeticExpression();
        void CheckExpressions(Expressions first, Expressions second);
        void AddError(Token token);
    }
}