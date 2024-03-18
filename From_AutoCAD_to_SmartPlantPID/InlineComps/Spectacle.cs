using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.InlineComps
{
    public class Spectacle : BasicInlineComp
    {
        long PolylineId;
        public bool isLong;
        public bool isClosed;
        public Spectacle() { }
        public Spectacle(long polylineId, bool isLong, bool isClosed, double x, double y) 
        {
            PolylineId = polylineId;
            this.isClosed = isClosed;
            this.isLong = isLong;
            FormAttribytes(x, y);
        }
    }
}
