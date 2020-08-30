namespace Analyzer.Syntactic
{
    public interface IParser
    {
        void Program();
        void Block();
        void VariableDeclaration();
        void CommandDeclaration();
        void AddError(string error);
    }
}