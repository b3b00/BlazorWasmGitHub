using System;
using System.Collections.Generic;
using expressionparser.model;
using LiveBlazorWasm.Client;
using LiveBlazorWasm.Client.Formula;
using sly.parser;
using sly.parser.generator;
using Xunit;

namespace GrpahTest
{
    public class TranslationTests
    {
        [Fact]
        public void AutoTranslate()
        {
            Referential from = new Referential(-10, -6, 10, 6);
            Referential to = new Referential(-10, -6, 10, 6);

            var t = from.TranslateTo(0, 0, to);
            Assert.True(t.InBound);
            Assert.Equal(0, t.X);
            Assert.Equal(0, t.Y);
        }

        [Fact]
        public void SimpleTranslateNoExtend()
        {
            Referential from = new Referential(-10, -6, 10, 6);
            Referential to = new Referential(0, 0, 20, 12);

            var t = from.TranslateTo(0, 0, to);
            Assert.True(t.InBound);
            Assert.Equal(10, t.X);
            Assert.Equal(6, t.Y);
        }

        [Fact]
        public void TranslateAndExtend()
        {
            Referential from = new Referential(-10, -6, 10, 6);
            Referential to = new Referential(0, 0, 750, 500);

            var t = from.TranslateTo(0, 0, to);
            Assert.True(t.InBound);
            Assert.Equal(375, t.X);
            Assert.Equal(250, t.Y);
        }

        [Fact]
        public void TranslateAndExtendAndReverseY()
        {
            Referential from = new Referential(-10, -6, 10, 6);
            Referential to = new Referential(0, 0, 750, 500);

            var t = from.TranslateTo(0, 6, to, reverseY: true);
            Assert.True(t.InBound);
            Assert.Equal(375, t.X);
            Assert.Equal(0, t.Y);

            t = from.TranslateTo(-5, 3, to, reverseY: true);
            Assert.True(t.InBound);
            Assert.Equal(375f / 2f, t.X);
            Assert.Equal(500 / 4f, t.Y);
        }

        [Fact]
        public void Test()
        {
            Referential from = new Referential(-10, -6, 10, 6);
            Referential to = new Referential(0, 0, 750, 500);

            var t = from.TranslateTo(-1.200005f, -1.7280217f, to);
            Assert.True(t.InBound);
        }

        [Fact]
        public void TestCube()
        {
            Referential range = new Referential(-1.7f, -5, 1.7f, 5);
            Referential display = new Referential(0, 0, 750, 500);
            float x = 0.0f;
            float y = x * x * x;
            var pos = range.TranslateTo(x, y, display, reverseY: true);
            ;
            Assert.True(pos.InBound);
            Assert.Equal(375f, pos.X);
            Assert.Equal(250f, pos.Y);
        }

        [Fact]
        public void TestParser()
        {
            Parser<SimpleExpressionToken, Expression> Parser = null;
            if (Parser == null)
            {
                var StartingRule = $"{typeof(FormulaParser).Name}_expressions";
                var parserInstance = new FormulaParser();
                var builder = new ParserBuilder<SimpleExpressionToken, Expression>();
                var bp = builder.BuildParser(parserInstance, ParserType.EBNF_LL_RECURSIVE_DESCENT, StartingRule);
                Assert.True(bp.IsOk);
                var r = bp.Result.Parse("x * 0.5 + 1.0");
                Assert.True(r.IsOk);

                r = bp.Result.Parse("x * 0.5 + 1");
                Assert.True(r.IsOk);

                r = bp.Result.Parse("2 * x  - 2");
                Assert.True(r.IsOk);

                r = bp.Result.Parse("2 * sin(x)");
                Assert.True(r.IsOk);
            }
        }

        [Fact]
        public void TestParserService()
        {
            var service = new ParserService();
            var result = service.Parse("2 x");
            Assert.True(result.IsOk);
            var r = result.Result.Evaluate(new ExpressionContext(new Dictionary<string, double>() { { "x", 2.0 } }));
            Assert.NotNull(r);
            Assert.True(r.HasValue);
            Assert.Equal(4, r.Value);

            service = new ParserService();
            result = service.Parse("sin( 48 x )");
            Assert.True(result.IsOk);
            r = result.Result.Evaluate(new ExpressionContext(new Dictionary<string, double>() { { "x", Math.PI } }));
            Assert.NotNull(r);
            Assert.True(r.HasValue);
            Assert.True(Math.Abs(r.Value) < 0.0001);
            
        }
    }
}