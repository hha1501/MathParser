namespace MathParser.Expressions
{
    public abstract class Expression
    {
        public NodeType Type { get; protected set; }

        public bool Simplified { get; protected set; }
        public bool ContainVariable { get; protected set; }

        public virtual Expression Simplify()
        {
            Simplified = true;
            return this;
        }

        public abstract EvaluationResult Evaluate(IContext context);

        public Expression(NodeType type)
        {
            Type = type;
        }
    }
}