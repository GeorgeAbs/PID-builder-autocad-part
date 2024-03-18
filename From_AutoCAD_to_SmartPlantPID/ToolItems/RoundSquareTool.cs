using Autodesk.AutoCAD.Interop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.ToolItems
{
    public class RoundSquareTool : GeneralTool
    {
        public RoundSquareTool() { }
        public RoundSquareTool(double x, double y, List<AcadMText> texts) 
        {
            FormAttribytes(x, y, texts);
        }
    }
}
