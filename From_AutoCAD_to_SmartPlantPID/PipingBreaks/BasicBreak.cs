using Autodesk.AutoCAD.Interop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.PipingBreaks
{
    public class BasicBreak
    {
        public double LeaderConnectionX;
        public double LeaderConnectionY;
        public double InsertionPointX;
        public double InsertionPointY;
        public double[] coordinates;
        public double[] rawCoordinates;
        public void FormAttribytes(double[] coordinates)
        {
            rawCoordinates = coordinates;
            this.coordinates = new double[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i+= 2) 
            {
                this.coordinates[i] = (Math.Ceiling(coordinates[i]) - Form1.minX + Form1.xAllowance) / 1000;
                this.coordinates[i+1] = (Math.Ceiling(coordinates[i+1]) - Form1.minY + Form1.yAllowance) / 1000;
            }
        }

        /*public static bool IsPolylineLeader(AcadLWPolyline thisBreak, )
        {

        }*/
    }
}
