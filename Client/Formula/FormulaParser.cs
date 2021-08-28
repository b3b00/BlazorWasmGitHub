using System.Collections.Generic;
using System.Linq;
using expressionparser.model;
using LiveBlazorWasm.Client.Formula.model;
using sly.lexer;
using sly.parser.generator;
using sly.parser.parser;

namespace LiveBlazorWasm.Client.Formula
{
    public class FormulaParser
    
    {
        [Operation((int) SimpleExpressionToken.PLUS, Affix.InFix, Associativity.Right, 10)]
        [Operation((int) SimpleExpressionToken.MINUS, Affix.InFix, Associativity.Left, 10)]
        public Expression BinaryTermExpression(Expression left, Token<SimpleExpressionToken> operation, Expression right)
        {
            return new BinaryOperation(left, operation.TokenID, right);
        }


        [Operation((int) SimpleExpressionToken.TIMES, Affix.InFix, Associativity.Right, 50)]
        [Operation((int) SimpleExpressionToken.DIVIDE, Affix.InFix, Associativity.Left, 50)]
        public Expression BinaryFactorExpression(Expression left, Token<SimpleExpressionToken> operation, Expression right)
        {
            return new BinaryOperation(left, operation.TokenID, right);
        }


        [Operation((int) SimpleExpressionToken.MINUS, Affix.PreFix, Associativity.Right, 100)]
        public Expression PreFixExpression(Token<SimpleExpressionToken> operation, Expression value)
        {
            return new UnaryOperation(SimpleExpressionToken.MINUS,value);
        }
        
        [Operation((int) SimpleExpressionToken.EXP, Affix.InFix, Associativity.Left, 90)]
        public Expression ExpExpression(Expression left, Token<SimpleExpressionToken> operation, Expression right)
        {
            return new BinaryOperation(left, operation.TokenID, right);
        }

        [Operand]
        [Production("operand : primary_value")]
        public Expression OperandValue(Expression value)
        {
            return value;
        }

        
        [Production("primary_value : IDENTIFIER")]
        public Expression OperandVariable(Token<SimpleExpressionToken> identifier)
        {
            return new Variable(identifier.Value);
        }

        [Production("primary_value : DOUBLE")]
        [Production("primary_value : INT")]
        public Expression OperandInt(Token<SimpleExpressionToken> value)
        {
            return new Number(value.DoubleValue);
        }

        [Production("primary_value : LPAREN FormulaParser_expressions RPAREN")]
        public Expression OperandParens(Token<SimpleExpressionToken> lparen, Expression value, Token<SimpleExpressionToken> rparen)
        {
            return value;
        }

        [Production(
            "primary_value : FUNCTION LPAREN[d] FormulaParser_expressions (COMMA FormulaParser_expressions)* RPAREN[d]")]
        public Expression FunctionCall(Token<SimpleExpressionToken> funcName, Expression first,
            List<Group<SimpleExpressionToken, Expression>> others)
        {
            List<Expression> parameters = new List<Expression>();
            parameters.Add(first);
            parameters.AddRange(others.Select(x => x.Value(0)));
            return new FunctionCall(funcName.Value, parameters);
        }
    }
}