using LiveBlazorWasm.Client.Formula;

namespace expressionparser.model
{
    public class UnaryOperation : Expression
    {
        private readonly SimpleExpressionToken Operator;
        private readonly Expression RightExpression;

        public UnaryOperation(SimpleExpressionToken op, Expression right)
        {
            Operator = op;
            RightExpression = right;
        }

        public double? Evaluate(ExpressionContext context)
        {
            var right = RightExpression.Evaluate(context);

            if (right.HasValue)
                switch (Operator)
                {
                    case SimpleExpressionToken.PLUS:
                    {
                        return +right.Value;
                    }
                    case SimpleExpressionToken.MINUS:
                    {
                        return -right.Value;
                    }
                    default:
                    {
                        return null;
                    }
                }
            return null;
        }
    }
}