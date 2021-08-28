using System;
using expressionparser.model;

namespace LiveBlazorWasm.Client.Formula.model
{
    public class BinaryOperation : Expression
    {
        private readonly Expression LeftExpresion;
        private readonly SimpleExpressionToken Operator;
        private readonly Expression RightExpression;


        public BinaryOperation(Expression left, SimpleExpressionToken op, Expression right)
        {
            LeftExpresion = left;
            Operator = op;
            RightExpression = right;
        }

        public double? Evaluate(ExpressionContext context)
        {
            var left = LeftExpresion.Evaluate(context);
            var right = RightExpression.Evaluate(context);

            if (left.HasValue && right.HasValue)
                switch (Operator)
                {
                    case SimpleExpressionToken.PLUS:
                    {
                        return left.Value + right.Value;
                    }
                    case SimpleExpressionToken.MINUS:
                    {
                        return left.Value - right.Value;
                    }
                    case SimpleExpressionToken.TIMES:
                    {
                        return left.Value * right.Value;
                    }
                    case SimpleExpressionToken.DIVIDE:
                    {
                        return left.Value / right.Value;
                    }
                    case SimpleExpressionToken.EXP:
                    {
                        return Math.Pow(left.Value,right.Value);
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