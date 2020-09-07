using Analyser.Lexical;

namespace Analyzer.Syntactic
{
    public interface IParser
    {
        void Run();
        void Program();
        void Block();
        void VariableDeclaration();
        void CommandDeclaration();
        void BasicCommandDeclaration();
        void InteractionCommandDeclaration();
        void ConditionalCommandDeclaration();
        void AssignmentDeclaration();
        Expressions FirstArithmeticExpressionDeclaration();
        void CheckExpressions(Expressions first, Expressions second);
        void AddError(Token token);
    }
}