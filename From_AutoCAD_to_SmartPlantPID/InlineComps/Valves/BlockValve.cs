using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.Valves
{
    public class BlockValve : BasicValve
    {
        public BlockValve() { } 
        public BlockValve(double x, double y)
        {
            FormAttribytes(x, y);
        }
    }
}
