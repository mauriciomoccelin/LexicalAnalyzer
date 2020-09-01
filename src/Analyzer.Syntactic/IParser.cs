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
        void AddError(string error);
    }
}