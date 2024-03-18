using Autodesk.AutoCAD.Interop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.ToolItems
{
    public class GeneralTool
    {
        public string InventaryTag = "";
        public string InstrTypeModifier = "";
        public string MeasuredVariable = "";
        public string TagSeqNo = "";
        public string TagSeqNoSuffix = "";

        public double X;
        public double Y;

        public GeneralTool()
        {
            
        }
        public void FormAttribytes(double x, double y, List<AcadMText> texts)
        {
            X = (Math.Ceiling(x) - Form1.minX + Form1.xAllowance)/ 1000.0;
            Y = (Math.Ceiling(y) - Form1.minY + Form1.xAllowance )/ 1000.0;
            InventaryTag = texts[3].TextString;
            TagSeqNo = texts[2].TextString;
            var rawMeasureVar = texts[1].TextString.Substring(2);

            if (rawMeasureVar.Substring(rawMeasureVar.Length - 2) == "IC")
            {
                MeasuredVariable = rawMeasureVar.Replace("IC", "");
                InstrTypeModifier = "IC";
            }
            else if (rawMeasureVar.Substring(rawMeasureVar.Length - 2) == "AD")
            {
                MeasuredVariable = rawMeasureVar.Replace("AD", "");
                InstrTypeModifier = "AD";
            }
            else
            {
                MeasuredVariable = rawMeasureVar[0].ToString();
                InstrTypeModifier = rawMeasureVar.Substring(1);
            }
        }
    }
}
