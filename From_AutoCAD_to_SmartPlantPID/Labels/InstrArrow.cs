using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.Labels
{
    public class InstrArrow : BasicLabel
    {
        public InstrArrow() { }

        public InstrArrow(double x, double y, double rotationRadians) 
        {
            FormBasicAttributes(x, y, rotationRadians, "", -1);
        }

    }
}
