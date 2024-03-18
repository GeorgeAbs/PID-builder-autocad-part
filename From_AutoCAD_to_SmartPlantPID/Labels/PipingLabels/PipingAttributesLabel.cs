using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.Labels.PipingLabels
{
    
    public class PipingAttributesLabel : BasicLabel
    {
        public string fluid = "";
        public string diaInch = "";
        public string diaMM = "";
        public string tagSeqNo = "";
        public string pipingClass = "";

        public void FormPipingAttributes(string pipingLabelText)
        {
            string[] attributes = pipingLabelText.Split('-');
            fluid = attributes[2].Replace(" ", "").Replace("\n", "").ToUpper();
            diaInch = attributes[3].Replace(" ", "").Replace("\n", "").Replace("H", "").Replace("h", "").Split('/')[0];
            diaMM = attributes[3].Replace(" ", "").Replace("\n", "").Replace("H", "").Replace("h", "").Split('/')[1];
            tagSeqNo = attributes[4].Replace(" ", "").Replace("\n", "");
            pipingClass = attributes[5].Replace(" ", "").Replace("\n", "").ToUpper();
        }

        public void FormPipingOnlyDiaLabel(string pipingLabelText)
        {
            string[] attributes = pipingLabelText.Split('-');
            diaInch = attributes[0].Replace(" ", "").Replace("\n", "").Replace("H", "").Replace("h", "").Split('/')[0];
        }
    }
}
