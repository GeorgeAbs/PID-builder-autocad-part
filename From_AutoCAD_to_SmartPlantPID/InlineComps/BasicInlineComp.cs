using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.InlineComps
{
    public class BasicInlineComp
    {
        public double X;
        public double Y;
        public double pointOnItemX;
        public double pointOnItemY;

        internal void FormAttribytes(double x, double y)
        {
            X = (Math.Ceiling(x) - Form1.minX + Form1.xAllowance) / 1000.0;
            Y = (Math.Ceiling(y) - Form1.minY + Form1.yAllowance) / 1000.0;
        }
    }
}
