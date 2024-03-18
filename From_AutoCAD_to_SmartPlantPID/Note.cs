using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID
{

    public class Note
    {
        public double X;
        public double Y;
        public double RotationAngle;
        public string Text = "";

        public Note() { }
        public Note(double x, double y, double rotationAngle, string text)
        {
            X = (x - Form1.minX + Form1.xAllowance) / 1000.0;
            Y = (y - Form1.minY + Form1.xAllowance) / 1000.0;
            RotationAngle = rotationAngle /*/ 180.0 * Math.PI*/; 
            Text = text.ToUpper();
        }
    }
}
