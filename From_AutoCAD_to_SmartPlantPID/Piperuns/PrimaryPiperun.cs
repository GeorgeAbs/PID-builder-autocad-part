using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.Piperuns
{
    public class PrimaryPiperun : Piperun
    {
        public PrimaryPiperun() { }
        public PrimaryPiperun(double[] coords, int id, bool isBeginingOnBreak, bool isEndOnBreak) 
        {
            FormAttributes(coords, id, isBeginingOnBreak, isEndOnBreak);
        }
    }
}
