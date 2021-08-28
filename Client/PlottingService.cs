using System;
using System.Threading.Tasks;

namespace LiveBlazorWasm.Client
{
    public delegate Task MoveToCallback(double x, double y);
    public delegate Task LineToCallback(double x1, double y1, double x2, double y2); 
    
    public class PlottingService
    {
        public MoveToCallback MoveTo { get; set; }
        
        public LineToCallback LineTo { get; set; }

        public PlottingService(MoveToCallback moveTo, LineToCallback lineTo)
        {
            LineTo = lineTo;
            MoveTo = moveTo;
        }
        
        public async Task Draw(Func<double, double?> function, long width, long height)
            {
                Referential range = new Referential(-10f, -6f, 10f, 6f);
                Referential display = new Referential(0, 0, width, height);
                // await _context.SetStrokeStyleAsync(color);
                // await _context.SetLineDashAsync(new float[] { });
                // await _context.BeginPathAsync();
        
                Translation prevPos = null;
        
                for (double x = range.FromX; x < range.ToX; x += (range.RangeX) / display.RangeX)
                {
                    var y = function(x);
        
                    Translation pos = new Translation() { InBound = false };
        
                    if (y.HasValue)
                    {
                        pos = range.TranslateTo(x, y.Value, display, reverseY: true);
                    }
        
                    if (prevPos == null)
                    {
                        if (pos.InBound)
                        {
                            prevPos = pos;
                        }
                    }
                    else
                    {
                        if (pos.InBound)
                        {
                            //await MoveTo(prevPos.X, prevPos.Y);
                            await LineTo(prevPos.X,prevPos.Y,pos.X, pos.Y);
                            prevPos = pos;
                        }
                        else
                        {
                            prevPos = null;
                        }
                    }
                }
            }
    }
}