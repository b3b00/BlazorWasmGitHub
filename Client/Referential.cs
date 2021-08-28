using System;

namespace LiveBlazorWasm.Client
{

    public class Translation
    {
        public double X;
        public double Y;
        public bool InBound;
    }
    public class Referential
    {
        public double FromX { get; set; }
        
        public double FromY { get; set; }
        
        public double ToX { get; set; }

        public double ToY { get; set; }

        public double RangeX => ToX - FromX;
        
        public double RangeY => ToY - FromY;
        
        

        public Translation Out(double x, double y)
        {
            return new Translation()
            {
                InBound = false,
                X = x,
                Y = y
            };
        }
        
        public Translation In(float x, float y)
        {
            return new Translation()
            {
                InBound = true,
                X = x,
                Y = y
            };
        }

        public Referential(float fromX, float fromY, float toX, float toY)
        {
            FromX = fromX;
            ToX = toX;
            FromY = fromY;
            ToY = toY;
        }

        public bool InBound(double x, double y)
        {
            return x >= FromX && x <= ToX && y >= FromY && y <= ToY;
        }
        
        public Translation TranslateTo(double x, double y, Referential referential, bool reverseY = false) 
        {
            if (!InBound(x, y))
            {
                return Out(x, y);
            }

            var xRatio = referential.RangeX / RangeX;
            var yRatio = referential.RangeY / RangeY;

            double pX = (x + Math.Abs(FromX)) / RangeX;
            double pY = (y + Math.Abs(FromY)) / RangeY;
            
            var nx = referential.FromX + referential.RangeX * pX;
            var ny = referential.FromY + referential.RangeY * pY;
            if (reverseY)
            {
                ny = referential.ToY - ny;
            }

            return  new Translation()
            {
                X = nx,
                Y = ny,
                InBound = referential.InBound(nx,ny)
            };
        }

        public override string ToString()
        {
            return $"({FromX},{FromY}) -> ({ToX},{ToY})";
        }
    }
}