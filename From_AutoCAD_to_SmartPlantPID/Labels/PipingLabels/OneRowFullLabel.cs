using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.Labels.PipingLabels
{
    public class OneRowFullLabel : PipingAttributesLabel
    {
        public OneRowFullLabel() { }
        public OneRowFullLabel(double x, double y, double rotationAngle, string text, int id) 
        {
            FormBasicAttributes(x, y, rotationAngle, text, id);
            FormPipingAttributes(text);
        }
    }
}
