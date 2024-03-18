using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID
{
    public class Piperun
    {
        public double[] coords;
        public int ID;
        public List<int> labelIds = new List<int>();
        public bool IsEnabled = true;
        public void FormAttributes(double[] coords, int id, bool isBeginingOnBreak, bool isEndOnBreak)
        {
            double[] RoundedCoords = new double[coords.Length];
            for (int i = 0; i < coords.Length; i += 2)
            {
                RoundedCoords[i] = (Math.Ceiling(coords[i]) - Form1.minX + Form1.xAllowance) / 1000;
                RoundedCoords[i + 1] = (Math.Ceiling(coords[i + 1]) - Form1.minY + Form1.yAllowance) / 1000;
            }

            if (isBeginingOnBreak && isEndOnBreak)
            {
                this.coords = new double[RoundedCoords.Length - 4];

                for (int i = 2; i < RoundedCoords.Length - 2; i += 2)
                {
                    this.coords[i - 2] = RoundedCoords[i];
                    this.coords[i + 1 - 2] = RoundedCoords[i + 1];
                }

            }
            else if (isBeginingOnBreak || isEndOnBreak)
            {
                this.coords = new double[RoundedCoords.Length - 2];

                if (isBeginingOnBreak)
                {
                    for (int i = 2; i < RoundedCoords.Length; i += 2)
                    {
                        this.coords[i - 2] = RoundedCoords[i];
                        this.coords[i + 1 - 2] = RoundedCoords[i + 1];
                    }
                }

                else
                {
                    for (int i = 0; i < RoundedCoords.Length - 2; i += 2)
                    {
                        this.coords[i] = RoundedCoords[i];
                        this.coords[i + 1] = RoundedCoords[i + 1];
                    }
                }
            }

            else
            {
                this.coords = new double[RoundedCoords.Length];
                for (int i = 0; i < RoundedCoords.Length; i += 2)
                {
                    this.coords[i] = RoundedCoords[i];
                    this.coords[i + 1] = RoundedCoords[i + 1];
                }
            }

            ID = id;
        }

        public void NewCoordsWithNewEnd(double newEndX, double newEndY)
        {
            double[] tempCoords = new double[coords.Length + 2];
            for (int i = 0; i < coords.Length; i++)
            {
                tempCoords[i] = coords[i];
            }
            tempCoords[tempCoords.Length - 2] = newEndX;
            tempCoords[tempCoords.Length - 1] = newEndY;
            coords = null;
            this.coords = tempCoords;
        }

        public void NewCoordsWithNewBegining(double newBeginingX, double newBeginingY)
        {
            double[] tempCoords = new double[coords.Length + 2];
            for (int i = 0; i < coords.Length; i++)
            {
                tempCoords[i + 2] = coords[i];
            }
            tempCoords[0] = newBeginingX;
            tempCoords[1] = newBeginingY;
            coords = null;
            this.coords = tempCoords;
        }

        public void MergePiperuns(Piperun runToBeMergedWithCurrent, bool isMainRunConnectionPointBegining, bool isToBeMergedRunConnectionPointBegining)
        {
            double[] tempCoords = new double[coords.Length + runToBeMergedWithCurrent.coords.Length];
            if (isMainRunConnectionPointBegining && isToBeMergedRunConnectionPointBegining)
            {
                for (int i = 0; i < coords.Length; i++)
                {
                    tempCoords[i + runToBeMergedWithCurrent.coords.Length] = coords[i];
                }

                for (int i = 0; i < runToBeMergedWithCurrent.coords.Length; i++)
                {
                    tempCoords[i] = runToBeMergedWithCurrent.coords[i];
                }
            }

            if (!isMainRunConnectionPointBegining && isToBeMergedRunConnectionPointBegining)
            {
                for (int i = 0; i < coords.Length; i++)
                {
                    tempCoords[i] = coords[i];
                }

                for (int i = 0; i < runToBeMergedWithCurrent.coords.Length; i++)
                {
                    tempCoords[i + coords.Length] = runToBeMergedWithCurrent.coords[i];
                }
            }

            if (isMainRunConnectionPointBegining && !isToBeMergedRunConnectionPointBegining)
            {
                for (int i = 0; i < coords.Length; i++)
                {
                    tempCoords[i + runToBeMergedWithCurrent.coords.Length] = coords[i];
                }

                for (int i = runToBeMergedWithCurrent.coords.Length - 1; i >=0 ; i-=2)
                {
                    tempCoords[runToBeMergedWithCurrent.coords.Length - 1 - i] = runToBeMergedWithCurrent.coords[i - 1];
                    tempCoords[runToBeMergedWithCurrent.coords.Length - 1 - i + 1] = runToBeMergedWithCurrent.coords[i];
                }
            }

            if (!isMainRunConnectionPointBegining && !isToBeMergedRunConnectionPointBegining)
            {
                for (int i = 0; i < coords.Length; i++)
                {
                    tempCoords[i] = coords[i];
                }

                for (int i = runToBeMergedWithCurrent.coords.Length - 1; i >= 0; i-=2)
                {
                    tempCoords[coords.Length + runToBeMergedWithCurrent.coords.Length - 1 - i] = runToBeMergedWithCurrent.coords[i - 1];
                    tempCoords[coords.Length + runToBeMergedWithCurrent.coords.Length - 1 - i + 1] = runToBeMergedWithCurrent.coords[i];
                }
            }

            coords = null;
            this.coords = tempCoords;
            runToBeMergedWithCurrent.IsEnabled = false;
        }
    }
}
