using System;
using System.Collections.Generic;
using expressionparser.model;
using sly.lexer;
using sly.parser;
using sly.parser.generator;

namespace LiveBlazorWasm.Client.Formula
{
    public class ParserService
    {
        public ParserService()
        {
            Parser = GetParser();
        }

        private static Parser<SimpleExpressionToken, Expression> Parser;

        private static List<Token<SimpleExpressionToken>> postProcess(List<Token<SimpleExpressionToken>> tokens)
        {
            var mayLeft = new List<SimpleExpressionToken>()
            {
                SimpleExpressionToken.INT, SimpleExpressionToken.DOUBLE, SimpleExpressionToken.IDENTIFIER,
                SimpleExpressionToken.RPAREN
            };

            var mayRight = new List<SimpleExpressionToken>()
            {
                SimpleExpressionToken.INT, SimpleExpressionToken.DOUBLE, SimpleExpressionToken.LPAREN,
                SimpleExpressionToken.IDENTIFIER, SimpleExpressionToken.FUNCTION
            };

            Func<SimpleExpressionToken, bool> mayOmmitLeft = (SimpleExpressionToken tokenid) =>
                mayLeft.Contains(tokenid);

            Func<SimpleExpressionToken, bool> mayOmmitRight = (SimpleExpressionToken tokenid) =>
                mayRight.Contains(tokenid);


            List<Token<SimpleExpressionToken>> newTokens = new List<Token<SimpleExpressionToken>>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (i >= 1 &&
                    mayOmmitRight(tokens[i].TokenID) && mayOmmitLeft(tokens[i - 1].TokenID))
                {
                    newTokens.Add(new Token<SimpleExpressionToken>()
                    {
                        TokenID = SimpleExpressionToken.TIMES
                    });
                }

                newTokens.Add(tokens[i]);
            }

            return newTokens;
        }


        private Parser<SimpleExpressionToken, Expression> GetParser()
        {
            if (Parser == null)
            {
                var StartingRule = $"{typeof(FormulaParser).Name}_expressions";
                var parserInstance = new FormulaParser();
                var builder = new ParserBuilder<SimpleExpressionToken, Expression>();
                var bp = builder.BuildParser(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT, StartingRule,
                    lexerPostProcess: postProcess);
                if (bp.IsOk)
                {
                    Parser = bp.Result;
                }
            }

            return Parser;
        }

        public ParseResult<SimpleExpressionToken, Expression> Parse(string source)
        {
            return Parser.Parse(source);
        }
    }
}