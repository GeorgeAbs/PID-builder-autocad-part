using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.InlineComps
{
    public class Flange : BasicInlineComp
    {
        public Flange() 
        { 

        }

        public Flange(double x, double y)
        {
            FormAttribytes(x, y);
        }
    }
}
