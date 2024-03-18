using Autodesk.AutoCAD.Interop.Common;
using From_AutoCAD_to_SmartPlantPID.ToolItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID
{
   public class RoundTool : GeneralTool
   {
        public RoundTool() { }
        public RoundTool(double x, double y, List<AcadMText> texts) 
        {
            FormAttribytes(x, y, texts);
        }
   }
}
