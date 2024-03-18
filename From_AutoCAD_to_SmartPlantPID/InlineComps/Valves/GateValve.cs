using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.Valves
{
    public class GateValve : BasicValve
    {
        public GateValve() { }  
        public GateValve(double x, double y) 
        {
            FormAttribytes(x, y);
        }
    }
}
