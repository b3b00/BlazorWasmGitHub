namespace expressionparser.model
{
    public interface Expression
    {
        double? Evaluate(ExpressionContext context);
    }
}