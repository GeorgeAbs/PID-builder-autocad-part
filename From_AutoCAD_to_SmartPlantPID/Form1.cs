using Autodesk.AutoCAD.Interop.Common;
using From_AutoCAD_to_SmartPlantPID.Additional;
using From_AutoCAD_to_SmartPlantPID.InlineComps;
using From_AutoCAD_to_SmartPlantPID.Labels.PipingLabels;
using From_AutoCAD_to_SmartPlantPID.Piperuns;
using From_AutoCAD_to_SmartPlantPID.PipingBreaks;
using From_AutoCAD_to_SmartPlantPID.ToolItems;
using From_AutoCAD_to_SmartPlantPID.Valves;
using Llama;
using Plaice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using From_AutoCAD_to_SmartPlantPID.Labels;
using System.Numerics;

namespace From_AutoCAD_to_SmartPlantPID
{
    public partial class Form1 : Form
    {
        Autodesk.AutoCAD.Interop.AcadDocument acadDocs;
        Autodesk.AutoCAD.Interop.AcadApplication acadApp;
        List<Autodesk.AutoCAD.Interop.Common.AcadCircle> circles = new List<Autodesk.AutoCAD.Interop.Common.AcadCircle>();
        List<Autodesk.AutoCAD.Interop.Common.AcadLWPolyline> polylines = new List<Autodesk.AutoCAD.Interop.Common.AcadLWPolyline>();
        List<Autodesk.AutoCAD.Interop.Common.AcadText> sTexts = new List<Autodesk.AutoCAD.Interop.Common.AcadText>();
        List<Autodesk.AutoCAD.Interop.Common.AcadMText> mTexts = new List<Autodesk.AutoCAD.Interop.Common.AcadMText>();
        List<AcadHatch> hatches = new List<AcadHatch>();
        string pathToDWG;

        public static List<RoundTool> roundTools = new List<RoundTool>();
        public static List<RoundSquareTool> roundSquareTools = new List<RoundSquareTool>();
        public static List<InterlockTool> interlockTools = new List<InterlockTool>();
        public static List<BlockValve> blockValves = new List<BlockValve>();
        public static List<GateValve> gateValves = new List<GateValve>();
        public static List<Note> notes = new List<Note>();
        public static List<Reducer> reducers = new List<Reducer>();
        public static List<PrimaryPiperun> primaryPiperuns = new List<PrimaryPiperun>();
        public static List<SecondaryPiperun> secondaryPiperuns = new List<SecondaryPiperun>();
        public static List<TagSeqNoBreak> tagSeqNoBreaks = new List<TagSeqNoBreak>();
        public static List<AllTypesBreak> allTypesBreaks = new List<AllTypesBreak>();
        public static List<Spectacle> spectacles = new List<Spectacle>();
        public static List<TwoRowsFullLabel> twoRowsFullLabels = new List<TwoRowsFullLabel> ();
        public static List<OneRowFullLabel> oneRowFullLabels = new List<OneRowFullLabel> ();
        public static List<NoteDroplet> noteDroplets = new List<NoteDroplet>();
        public static List<OnlyDiaLabel> onlyDiaLabels = new List<OnlyDiaLabel> ();
        public static List<OnlyDiaLabelH> onlyDiaLabelsH = new List<OnlyDiaLabelH> ();
        public static List<Flange> flanges = new List<Flange>();
        public static List<InstrArrow> instrArrows = new List<InstrArrow>();

        public static double minX = 666666666666666666;
        public static double minY = 666666666666666666;
        public static double xAllowance = 25;
        public static double yAllowance = 25;

        public int LabelId = 0;
        public Form1()
        {
            InitializeComponent();
            Enums.CreateLists();
            foreach(var text in Enums.PossibleNotesText)
            {
                //richTextBox1.Text += text + "\n";
            }
            foreach (var text in Enums.NotTools)
            {
                //richTextBox1.Text += text + "\n";
            }
        }

        private void choseYourDWG_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathToDWG = openFileDialog1.FileName;
                openDWG();
                CollectPrimitives(acadDocs);
                CreareInstrArrows();
                //CreateNoteDroplets();
                //CreateSpectacles();
                //CreatePipingBreaks();
                CreateTools();
                //CreateValves();
                //CreateNotes();
                ///CreateReducer();
                //CreatePiperuns();
                //CreateFlanges();
                //no demands ExtendPiperunForNearestPiperun();
                //CreatePipingLabels();
                //FindLabelForPiperuns();
                //////////////////////////////////////////MergePiperuns();

                richTextBox1.Text += "\n" + 
                    "primary pipes " + primaryPiperuns.Count + "\n" +
                    "secondary pipes " + secondaryPiperuns.Count + "\n" +
                    "breaks " + allTypesBreaks.Count + "\n" +
                    "oneRowFullLabel " + oneRowFullLabels.Count + "\n" +
                    "twoRowFullLabel " + twoRowsFullLabels.Count + "\n" +
                    "noteDroplets " + noteDroplets.Count + "\n" +
                    "notes " + notes.Count + "\n" +
                    "block valves " + blockValves.Count + "\n" +
                    "gate valves " + gateValves.Count + "\n" +
                    "block valves " + blockValves.Count + "\n" +
                    "round tools " + roundTools.Count + "\n" +
                    "round square tools " + roundTools.Count + "\n" +
                    "interlock tools " + interlockTools.Count + "\n" +
                    "spectacles " + spectacles.Count + "\n";
                PlaceItemsToPID();
                var exp = new PidItemsExporter();
                exp.Export(openFileDialog1.FileName.Replace(".dwg","") + ".myPid");

                acadDocs.Close(false);
                acadApp.Quit();
            }
        }

        void openDWG()
        {
            acadApp = new Autodesk.AutoCAD.Interop.AcadApplication();
            while (true)
            {
                try
                {
                    acadApp = new Autodesk.AutoCAD.Interop.AcadApplication();
                    break;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }

            while (true)
            {
                try
                {
                    //acadApp.Application.Visible = true;
                    break;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }

            
            while (true)
            {
                try
                {
                    acadDocs = acadApp.Documents.Open(pathToDWG);
                    break;
                }
                catch
                {
                    Thread.Sleep(100);

                }
            }

            Autodesk.AutoCAD.Interop.Common.AcadLayout acLayout;
            int acadLayouts = 0;
            while (true)
            {
                try
                {
                    acadLayouts = acadDocs.Layouts.Count;
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
            bool notModel = true;
            while (true)
            {
                try
                {
                    acLayout = acadDocs.Layouts.Item(0);
                    richTextBox1.Text += acadDocs.ActiveLayout.Name;
                    acadDocs.ActiveLayout = acLayout;
                    richTextBox1.Text += acadDocs.ActiveLayout.Name;
                    break;
                }
                catch 
                {
                    Thread.Sleep(100);
                }
            }
        }

        void CollectPrimitives(Autodesk.AutoCAD.Interop.AcadDocument acadDocs)
        {
            int q = 0;
            int y = 0;
            bool b = false;

            while (true)
            {
                try
                {
                    y = acadDocs.ModelSpace.Count;
                    break;
                }
                catch
                {
                    Thread.Sleep(100);

                }
            }

            for (int i = 0; i < y; i++)
            {
                while(true)
                {
                    try
                    {
                        acadDocs.ModelSpace.Item(i).GetBoundingBox(out object minPoint, out object maxPoint);
                        double[] doubleObj = (double[])minPoint;
                        if (doubleObj[0] < minX)
                        {
                            minX = doubleObj[0];
                        }
                        if (doubleObj[1] < minY)
                        {
                            minY = doubleObj[1];
                        }

                        //richTextBox1.Text += acadDocs.ModelSpace.Item(i).ObjectName + "\n";
                        if (acadDocs.ModelSpace.Item(i).ObjectName == "AcDbCircle")
                        {
                            var circle = acadDocs.ModelSpace.Item(i) as Autodesk.AutoCAD.Interop.Common.AcadCircle;
                            circles.Add(circle);
                            /*if (circle.Radius > 7 && circle.Radius < 8)
                            {
                                circles.Add(circle);
                            }*/
                            
                            break;
                        }

                        else if (acadDocs.ModelSpace.Item(i).ObjectName == "AcDbPolyline")
                        {
                            polylines.Add(acadDocs.ModelSpace.Item(i) as Autodesk.AutoCAD.Interop.Common.AcadLWPolyline);
                            //richTextBox1.Text += (acadDocs.ModelSpace.Item(i) as Autodesk.AutoCAD.Interop.Common.AcadLWPolyline).Coordinates + "\n";
                            break;
                        }

                        else if (acadDocs.ModelSpace.Item(i).ObjectName == "AcDbText")
                        {
                            sTexts.Add(acadDocs.ModelSpace.Item(i) as Autodesk.AutoCAD.Interop.Common.AcadText);
                            break;
                        }
                        else if (acadDocs.ModelSpace.Item(i).ObjectName == "AcDbMText")
                        {
                            mTexts.Add(acadDocs.ModelSpace.Item(i) as Autodesk.AutoCAD.Interop.Common.AcadMText);
                            break;
                        }

                        else if (acadDocs.ModelSpace.Item(i).ObjectName == "AcDbHatch")
                        {
                            hatches.Add(acadDocs.ModelSpace.Item(i) as Autodesk.AutoCAD.Interop.Common.AcadHatch);
                            break;
                        }
                        break;
                    }
                    catch { Thread.Sleep(100); }
                }
                
            }

            minX = Math.Ceiling(minX);
            minY = Math.Ceiling(minY);
            //richTextBox1.Text += circles.Count + " " + polylines.Count + " " + sTexts.Count + " " + mTexts.Count;
        }

        void CreareInstrArrows()
        {
            foreach (var poly in polylines.Where(x => x.Coordinates.Length == 6 && Math.Abs(x.Length - 8.5) < 0.4 && x.Lineweight == ACAD_LWEIGHT.acLnWt013))
            {
                double point1X = poly.Coordinates[0];
                double point1Y = poly.Coordinates[1];
                double point2X = poly.Coordinates[2];
                double point2Y = poly.Coordinates[3];
                double point3X = poly.Coordinates[4];
                double point3Y = poly.Coordinates[5];

                double xMidle = (point1X + point3X) / 2;
                double yMidle = (point1Y + point3Y) / 2;


                if (Math.Abs(CalculateDistance(point1X, point1Y, point2X, point2Y) - 4.25) < 0.2 &&
                    Math.Abs(CalculateDistance(point1X, point1Y, point3X, point3Y) - 4) < 0.2)
                {
                    double rotation = 0;
                    if (Math.Abs(xMidle - point2X) > 2 && point2X > xMidle)
                    {
                        rotation = -1 * Math.PI;
                    }

                    else if (Math.Abs(xMidle - point2X) > 2 && point2X < xMidle)
                    {
                        rotation = 0;
                    }

                    else if (Math.Abs(xMidle - point2X) < 2 && point2Y > yMidle)
                    {
                        rotation = -1 * Math.PI / 2;
                    }

                    else if (Math.Abs(xMidle - point2X) < 2 && point2Y < yMidle)
                    {
                        rotation = Math.PI / 2;
                    }

                    instrArrows.Add(new InstrArrow(point2X, point2Y, rotation));
                }
            }
        }

        void CreateSpectacles()
        {
            foreach (var polyline in polylines.Where(xxx => xxx.Coordinates.Length == 4 && Math.Abs(xxx.Length - 5.625) < 0.2))
            {
                double x1 = polyline.Coordinates[0];
                double y1 = polyline.Coordinates[1];
                double x2 = polyline.Coordinates[2];
                double y2 = polyline.Coordinates[3];

                foreach (var circle in circles.Where(x => Math.Abs(x.Radius - 1.25) < 0.2))
                {
                    double dist1 = CalculateDistance(x1, y1, circle.Center[0], circle.Center[1]);
                    double dist2 = CalculateDistance(x2, y2, circle.Center[0], circle.Center[1]);
                    if (Math.Abs(dist1 - 1.25) < 0.2 || Math.Abs(dist2 - 1.25) < 0.2)
                    {
                        double insertionX = 0;
                        double insertionY = 0;
                        bool isClosed = false;
                        bool isLong = false;
                        if (Math.Abs(y1 - y2) < 0.2)
                        {
                            if (polyline.Coordinates[0] > polyline.Coordinates[2])
                            {
                                insertionX = polyline.Coordinates[0] - 3.75;
                            }

                            else
                            {
                                insertionX = polyline.Coordinates[0] + 3.75;
                            }

                            insertionY = polyline.Coordinates[1];
                        }

                        else
                        {
                            if (polyline.Coordinates[1] > polyline.Coordinates[3])
                            {
                                insertionY = polyline.Coordinates[1] - 3.75;
                            }

                            else
                            {
                                insertionY = polyline.Coordinates[1] + 3.75;
                            }

                            insertionX = polyline.Coordinates[0];
                        }

                        foreach(var hatch in hatches)
                        {
                            object minPoint;
                            object maxPoint;
                            hatch.GetBoundingBox(out minPoint, out maxPoint);
                            double[] minPointDoubles = (double[])minPoint;
                            double[] maxPointDoubles = (double[])maxPoint;
                            if (Math.Abs((maxPointDoubles[0] + minPointDoubles[0]) / 2 - circle.Center[0]) < 0.2 &&
                                Math.Abs((maxPointDoubles[1] + minPointDoubles[1]) / 2 - circle.Center[1]) < 0.2)
                            {
                                isClosed = true;
                            }
                        }

                        foreach(var pol in polylines.Where(x => x.Coordinates.Length == 4 && Math.Abs(x.Length - 2.5) < 0.2))
                        {
                            if (Math.Abs(CalculateDistance(pol.Coordinates[0], pol.Coordinates[1], circle.Center[0], circle.Center[1]) - 3.75) < 0.3 ||
                                Math.Abs(CalculateDistance(pol.Coordinates[2], pol.Coordinates[3], circle.Center[0], circle.Center[1]) - 3.75) < 0.3)
                            {
                                isLong = true;
                            }
                        }

                        spectacles.Add(new Spectacle(polyline.ObjectID, isLong, isClosed, insertionX, insertionY));
                        break;
                    }
                }

            }
        }

        void CreateFlanges()
        {
            foreach (var polyline in polylines.Where(xxx => xxx.Coordinates.Length == 4 && Math.Abs(xxx.Length - 3.75) < 0.2))
            {
                double x = (polyline.Coordinates[0] + polyline.Coordinates[2]) / 2;
                double y = (polyline.Coordinates[1] + polyline.Coordinates[3]) / 2;
                double insertionX = (Math.Ceiling((polyline.Coordinates[0] + polyline.Coordinates[2]) / 2) - Form1.minX + Form1.xAllowance) / 1000.0;
                double insertionY = (Math.Ceiling((polyline.Coordinates[1] + polyline.Coordinates[3]) / 2) - Form1.minY + Form1.yAllowance) / 1000.0;
                foreach (var pipe in secondaryPiperuns)
                {
                    double end1X = pipe.coords[0];
                    double end1Y = pipe.coords[1];
                    double end2X = pipe.coords[pipe.coords.Length - 2];
                    double end2Y = pipe.coords[pipe.coords.Length - 1];

                    if (CalculateDistance(end1X, end1Y, insertionX, insertionY) < 0.00015 ||
                        CalculateDistance(end2X, end2Y, insertionX, insertionY) < 0.00015)
                    {
                        flanges.Add(new Flange(x, y));
                    }
                }
            }
        }

        void CreatePipingBreaks()
        {
            foreach (var polyline in polylines)
            {
                if (polyline.Lineweight == ACAD_LWEIGHT.acLnWt013 && polyline.Closed)
                {
                    if ((polyline.Coordinates.Length == 6 && (polyline.Length > 8 && polyline.Length < 10)) ||
                            (polyline.Coordinates.Length == 8 && (polyline.Length > 13 && polyline.Length < 15)))
                    {
                        allTypesBreaks.Add(new AllTypesBreak(polyline.Coordinates));
                    }
                }
            }
        }

        void CreateTools()
        {
            foreach(var circle in circles)
            {
                var radius = circle.Radius;
                if (circle.Radius > 7 && circle.Radius < 8)
                {
                    List<AcadMText> texts = new List<AcadMText>();
                    double X = circle.Center[0];
                    double Y = circle.Center[1];

                    foreach (var mText in mTexts)
                    {
                        if (mText.InsertionPoint[0] > X - radius && mText.InsertionPoint[0] < X + radius)
                        {
                            if (mText.InsertionPoint[1] > Y - radius && mText.InsertionPoint[1] < Y + radius)
                            {
                                //richTextBox1.Text += mText.TextString + "\n";
                                texts.Add(mText);
                            }

                        }
                    }

                    if (texts.Count == 4)
                    {
                        texts.Sort((x, y) => y.InsertionPoint[1].CompareTo(x.InsertionPoint[1]));
                        string measureVariable = texts[1].TextString.Substring(2);
                        if (!Enums.NotTools.Contains(measureVariable))
                        {
                            if (IsCircleInSquare(X, Y))
                            {
                                roundSquareTools.Add(new RoundSquareTool(X, Y, texts));

                            }

                            else
                            {
                                roundTools.Add(new RoundTool(X, Y, texts));
                            }
                        }
                            
                    }
                    
                    

                    
                }
            }

            foreach (var polyline in polylines/*.Where(xxx => xxx != null)*/)
            {
                if (polyline.Coordinates.Length == 8)
                {
                    List<AcadMText> texts = new List<AcadMText>();
                    double meanX = ((double)polyline.Coordinates[0] + (double)polyline.Coordinates[2] + (double)polyline.Coordinates[4] + (double)polyline.Coordinates[6]) / 4.0;
                    double meanY = ((double)polyline.Coordinates[1] + (double)polyline.Coordinates[3] + (double)polyline.Coordinates[5] + (double)polyline.Coordinates[7]) / 4.0;
                    if ((Math.Abs((double)polyline.Coordinates[0] - meanX ) < 0.1 && Math.Abs((double)polyline.Coordinates[4] - meanX) < 0.1 && Math.Abs((double)polyline.Coordinates[3] - meanY) < 0.1 && Math.Abs((double)polyline.Coordinates[7] - meanY) < 0.1) ||
                        (Math.Abs((double)polyline.Coordinates[0] - meanX) < 0.1 && Math.Abs((double)polyline.Coordinates[4] - meanX) < 0.1 && Math.Abs((double)polyline.Coordinates[3] - meanY) < 0.1 && Math.Abs((double)polyline.Coordinates[7] - meanY) < 0.1))
                    {
                        foreach (var mText in mTexts)
                        {
                            if (mText.InsertionPoint[0] > meanX - 7.47 && mText.InsertionPoint[0] < meanX + 7.47)
                            {
                                if (mText.InsertionPoint[1] > meanY - 7.47 && mText.InsertionPoint[1] < meanY + 7.47)
                                {
                                    //richTextBox1.Text += mText.TextString + "\n";
                                    texts.Add(mText);
                                }

                            }
                        }
                    }

                    if (texts.Count == 4)
                    {
                        texts.Sort((x, y) => y.InsertionPoint[1].CompareTo(x.InsertionPoint[1]));
                        if (texts[1].TextString.Length < 2) continue;

                        string measureVariable = texts[1].TextString.Substring(2);
                        
                        if (!Enums.NotTools.Contains(measureVariable))
                        {
                            /*foreach (var text in texts)
                            {
                                richTextBox1.Text += text.TextString + "\n";
                            }
                            richTextBox1.Text += "\n";*/
                            interlockTools.Add(new InterlockTool(meanX, meanY, texts));
                        }

                    }
                }
            }
        }

        void CreateValves()
        {
            
            foreach (var polyline in polylines/*.Where(xxx => xxx != null)*/)
            {
                if (polyline.Coordinates.Length == 8)
                {
                    double meanX = ((double)polyline.Coordinates[0] + (double)polyline.Coordinates[2] + (double)polyline.Coordinates[4] + (double)polyline.Coordinates[6]) / 4.0;
                    double meanY = ((double)polyline.Coordinates[1] + (double)polyline.Coordinates[3] + (double)polyline.Coordinates[5] + (double)polyline.Coordinates[7]) / 4.0;
                    
                    if (Math.Abs(CalculateDistance((double)polyline.Coordinates[0],(double)polyline.Coordinates[1], (double)polyline.Coordinates[2], (double)polyline.Coordinates[3]) - 3.75) < 0.2  &&
                        Math.Abs(CalculateDistance((double)polyline.Coordinates[2], (double)polyline.Coordinates[3], (double)polyline.Coordinates[4], (double)polyline.Coordinates[5]) - 8.38) < 0.2)
                    {
                        bool isGate = false;
                        foreach (var smallPolyline in polylines.Where(x => x.Coordinates.Length == 4 && Math.Abs(x.Length-3.75) < 1)) 
                        {
                            if (Math.Abs((smallPolyline.Coordinates[0] + smallPolyline.Coordinates[2]) / 2 - meanX) < 0.2 &&
                                Math.Abs((smallPolyline.Coordinates[1] + smallPolyline.Coordinates[3]) / 2 - meanY) < 0.2)
                            {
                                isGate = true;
                            }
                        }

                        if (isGate)
                            gateValves.Add(new GateValve(meanX, meanY));

                        else
                            blockValves.Add(new BlockValve(meanX, meanY));

                    }
                }
            }
        }

        void CreateNotes()
        {
            List<AcadMText> stringsToBigNote = new List<AcadMText>();
            double bigNoteX = -999999999;
            double bigNoteY = -999999999;
            foreach (var mText in mTexts)
            {
                if (Enums.IsTextNote(mText.TextString))
                {
                    //richTextBox1.Text += mText.TextString + "\n";
                    notes.Add(new Note(mText.InsertionPoint[0], mText.InsertionPoint[1], mText.Rotation, mText.TextString));
                }

                if (mText.TextString.Contains(')'))
                {
                    var testString = mText.TextString.Substring(0, mText.TextString.IndexOf(')'));
                    if (testString.Where(x => !IsDigit(x)).Count() == 0)
                    {
                        bigNoteX = mText.InsertionPoint[0];
                        bigNoteY = mText.InsertionPoint[1];
                        stringsToBigNote.Add(mText);
                    }
                }
            }
            #region big note
            if (bigNoteX != -999999999)
            {
                foreach (var mText in mTexts)
                {
                    if (mText.InsertionPoint[0] >= bigNoteX)
                    {
                        if (!stringsToBigNote.Contains(mText))
                        {
                            stringsToBigNote.Add(mText);
                        }
                    }
                }
            }
            

            if (stringsToBigNote.Count > 0)
            {
                bigNoteY -= 50;
                stringsToBigNote.Sort((a, b) => b.InsertionPoint[1].CompareTo(a.InsertionPoint[1]));
                string noteText = "";
                foreach (var str in stringsToBigNote)
                {
                    noteText += str.TextString + "\n";
                }
                notes.Add(new Note(bigNoteX, bigNoteY, 0, noteText));
            }
            #endregion
        }

        void CreateReducer()
        {
            foreach (var polyline in polylines/*.Where(xxx => xxx != null)*/)
            {
                if (polyline.Coordinates.Length == 6)
                {
                    double meanX = ((double)polyline.Coordinates[0] + (double)polyline.Coordinates[2] + (double)polyline.Coordinates[4]) / 3.0;
                    double meanY = ((double)polyline.Coordinates[1] + (double)polyline.Coordinates[3] + (double)polyline.Coordinates[5]) / 3.0;

                    double insertX = (double)polyline.Coordinates[2];
                    double insertY = (double)polyline.Coordinates[3];

                    double side1 = CalculateDistance((double)polyline.Coordinates[0], (double)polyline.Coordinates[1], (double)polyline.Coordinates[2], (double)polyline.Coordinates[3]);
                    double side2 = CalculateDistance((double)polyline.Coordinates[2], (double)polyline.Coordinates[3], (double)polyline.Coordinates[4], (double)polyline.Coordinates[5]);
                    double side3 = CalculateDistance((double)polyline.Coordinates[0], (double)polyline.Coordinates[1], (double)polyline.Coordinates[4], (double)polyline.Coordinates[5]);
                    if ((side1 + side2 + side3) > 18 && (side1 + side2 + side3) < 20 && (Math.Abs(side1 - 7.8) < 0.4 || Math.Abs(side1 - 3.7) < 0.4))
                    {
                       reducers.Add(new Reducer(insertX, insertY));
                    }
                }
            }
        }

        void CreatePiperuns()
        {
            int id = 0;
            List<AcadLWPolyline> firtApproachRuns = new List<AcadLWPolyline>();
            foreach (var polyline in polylines/*.Where(xxx => xxx != null)*/)
            {
                if (polyline.Lineweight == ACAD_LWEIGHT.acLnWt018 || polyline.Lineweight == ACAD_LWEIGHT.acLnWt035)
                {
                    if (polyline.Length > 5.8)
                    {
                        if (!polyline.Closed)
                        {
                            if (IsOrto(polyline) || 
                                IsEndOnBreak(polyline.Coordinates[polyline.Coordinates.Length - 2], polyline.Coordinates[polyline.Coordinates.Length - 1]) ||
                                IsBeginingOnBreak(polyline.Coordinates[0], polyline.Coordinates[1]))
                            {
                                if (!IsAlmostClosed(polyline))
                                {
                                    if (polyline.Lineweight == ACAD_LWEIGHT.acLnWt018)
                                    {
                                        secondaryPiperuns.Add(new SecondaryPiperun((double[])polyline.Coordinates, id, IsBeginingOnBreak(polyline.Coordinates[0], polyline.Coordinates[1]), IsEndOnBreak(polyline.Coordinates[polyline.Coordinates.Length - 2], polyline.Coordinates[polyline.Coordinates.Length - 1])));
                                    }

                                    else
                                    {
                                        primaryPiperuns.Add(new PrimaryPiperun((double[])polyline.Coordinates, id, IsBeginingOnBreak(polyline.Coordinates[0], polyline.Coordinates[1]), IsEndOnBreak(polyline.Coordinates[polyline.Coordinates.Length - 2], polyline.Coordinates[polyline.Coordinates.Length - 1])));
                                    }

                                    id++;
                                }
                            }
                        }
                    }
                }
            }
        }

        void CreatePipingLabels()
        {
            foreach (var mText in mTexts.Where(x => x.TextString[0] == 'L'))
            {
                if (mText.TextString.Split('-').Length == 7)
                {
                    oneRowFullLabels.Add(new OneRowFullLabel(mText.InsertionPoint[0], mText.InsertionPoint[1], mText.Rotation, mText.TextString, LabelId));
                    LabelId++;
                }

                else if (mText.TextString.Split('-').Length == 5)
                {
                    string labelText = mText.TextString;
                    double x = mText.InsertionPoint[0];
                    double y = mText.InsertionPoint[1];

                    if (Math.Abs(mText.Rotation * 180 / Math.PI - 0) < 3)
                    {
                        foreach (var mText2 in mTexts.Where(x2 => x2.TextString.Split('-').Length == 3))
                        {
                            if (Math.Abs(mText2.Rotation * 180 / Math.PI - 0) < 3)
                            {
                                if (CalculateDistance(x, y, mText2.InsertionPoint[0], mText2.InsertionPoint[1]) < 6)
                                {
                                    labelText += mText2.TextString;
                                    twoRowsFullLabels.Add(new TwoRowsFullLabel(mText2.InsertionPoint[0], mText2.InsertionPoint[1], mText2.Rotation, labelText, LabelId));
                                    LabelId++;
                                }
                            }
                        }
                    }

                    else if (Math.Abs(mText.Rotation * 180 / Math.PI - 90) < 3)
                    {
                        foreach (var mText2 in mTexts.Where(x2 => x2.TextString.Split('-').Length == 3))
                        {
                            if (Math.Abs(mText2.Rotation * 180 / Math.PI - 90) < 3)
                            {
                                if (CalculateDistance(x, y, mText2.InsertionPoint[0], mText2.InsertionPoint[1]) < 6)
                                {
                                    labelText += mText2.TextString;
                                    twoRowsFullLabels.Add(new TwoRowsFullLabel(mText2.InsertionPoint[0], mText2.InsertionPoint[1], mText2.Rotation, labelText, LabelId));
                                    LabelId++;
                                }
                            }
                        }
                    }
                }

            }

            foreach (var mText in mTexts.Where(x => x.TextString.Contains("mm") && x.TextString.Contains("\"") && x.TextString.Split('/').Length == 2 && !x.TextString.Contains('-')))
            {
                if (mText.TextString.Contains('h'))
                {
                    onlyDiaLabelsH.Add(new OnlyDiaLabelH(mText.InsertionPoint[0], mText.InsertionPoint[1], mText.Rotation, mText.TextString, LabelId));
                    LabelId++;
                }

                else
                {
                    onlyDiaLabels.Add(new OnlyDiaLabel(mText.InsertionPoint[0], mText.InsertionPoint[1], mText.Rotation, mText.TextString, LabelId));
                    LabelId++;
                }
                
            }
        }

        void CreateNoteDroplets()
        {
            foreach (var circle in circles.Where(x => Math.Abs(x.Radius - 2.5) < 0.2))
            {
                bool ok = false;
                double rotation = 0;
                string noteDropletText = "";
                double cirlceCenterX = circle.Center[0];
                double cirlceCenterY = circle.Center[1];
                double circleRadius = circle.Radius;

                
                foreach(var text in mTexts)
                {
                    rotation = text.Rotation;
                    noteDropletText = text.TextString;
                    double textX = text.InsertionPoint[0];
                    double textY = text.InsertionPoint[1];
                    if (Math.Abs(cirlceCenterX - textX) < circleRadius &&
                        Math.Abs(cirlceCenterY - textY) < circleRadius)
                    {
                        foreach(var polyline in polylines.Where(x => x.Coordinates.Length == 6 && Math.Abs(x.Length - 8.1) < 1 && !x.Closed))
                        {
                            ok = true;


                            goto CreateNoteDroplet;
                        }
                    }
                }

                CreateNoteDroplet:

                if (ok)
                    noteDroplets.Add(new NoteDroplet(cirlceCenterX, cirlceCenterY, rotation, noteDropletText));
            }


        }

        bool IsCircleInSquare(double x, double y)
        {
            foreach (var polyline in polylines/*.Where(xxx => xxx != null)*/)
            {
                if (polyline.Coordinates.Length == 8)
                {
                    double meanX = ((double)polyline.Coordinates[0] + (double)polyline.Coordinates[2] + (double)polyline.Coordinates[4] + (double)polyline.Coordinates[6]) / 4.0;
                    double meanY = ((double)polyline.Coordinates[1] + (double)polyline.Coordinates[3] + (double)polyline.Coordinates[5] + (double)polyline.Coordinates[7]) / 4.0;
                    if (Math.Abs(meanX - x) < 2 && Math.Abs(meanY - y) < 2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        bool IsOrto(AcadLWPolyline polyline)
        {
            double oldX;
            double oldY;

            for (int i = 2; i < polyline.Coordinates.Length; i+=2)
            {
                oldX = polyline.Coordinates[i-2];
                oldY = polyline.Coordinates[i-1];
                double thisX = polyline.Coordinates[i];
                double thisY = polyline.Coordinates[i+1];
                if (Math.Abs(oldX - thisX) > 0.05 && Math.Abs(oldY - thisY) > 0.05)
                {
                    return false;
                }
            }

            return true;
        }

        bool IsAlmostClosed(AcadLWPolyline polyline)
        {
            /*double minX = 66666666666666;
            double minY = 66666666666666;

            double maxX = -66666666666666;
            double maxY = -66666666666666;

            for (int i = 0; i < polyline.Coordinates - 1; i += 2)
            {
                if ((double)polyline.Coordinates[i] < minX)
                {
                    minX = (double)polyline.Coordinates[i];
                }

                if ((double)polyline.Coordinates[i+1] < minY)
                {
                    minY = (double)polyline.Coordinates[i+1];
                }

                if ((double)polyline.Coordinates[i] > maxX)
                {
                    maxX = (double)polyline.Coordinates[i];
                }

                if ((double)polyline.Coordinates[i + 1] < maxY)
                {
                    maxY = (double)polyline.Coordinates[i + 1];
                }
            }

            if ((maxX + minX) / )

            return true;*/
            object minPoint;
            object maxPoint;
            polyline.GetBoundingBox(out minPoint, out maxPoint);
            double[] minPointCoords = (double[])minPoint;
            double[] maxPointCoords = (double[])maxPoint;

            double diagonal = CalculateDistance(minPointCoords[0], minPointCoords[1], maxPointCoords[0], maxPointCoords[1]);

            if (polyline.Length / diagonal > 2.5)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        bool IsEndOnBreak(double endX, double endY)
        {
            foreach(var allTypesBreak in allTypesBreaks)
            {
                for (int i = 0; i < allTypesBreak.rawCoordinates.Length; i += 2)
                {
                    if (allTypesBreak.rawCoordinates.Length == 6)
                    {
                        if (CalculateDistance(endX, endY, allTypesBreak.rawCoordinates[i], allTypesBreak.rawCoordinates[i+1]) < 0.3)
                        {
                            return true;
                        }
                    }

                    if (allTypesBreak.rawCoordinates.Length == 8)
                    {
                        if (CalculateDistance(endX, endY, allTypesBreak.rawCoordinates[i], allTypesBreak.rawCoordinates[i + 1]) < 1.9)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        bool IsBeginingOnBreak(double beginingX, double beginingY)
        {
            foreach (var allTypesBreak in allTypesBreaks)
            {
                for (int i = 0; i < allTypesBreak.rawCoordinates.Length; i += 2)
                {
                    if (allTypesBreak.rawCoordinates.Length == 6)
                    {
                        if (CalculateDistance(beginingX, beginingY, allTypesBreak.rawCoordinates[i], allTypesBreak.rawCoordinates[i + 1]) < 0.3)
                        {
                            return true;
                        }
                    }

                    if (allTypesBreak.rawCoordinates.Length == 8)
                    {
                        if (CalculateDistance(beginingX, beginingY, allTypesBreak.rawCoordinates[i], allTypesBreak.rawCoordinates[i + 1]) < 1.9)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        void FindLabelForPiperuns()
        {
            foreach(SecondaryPiperun run in secondaryPiperuns)
            {
                FindLabeForPiperunsMainPart(run);
            }

            foreach (PrimaryPiperun run in primaryPiperuns)
            {
                FindLabeForPiperunsMainPart(null, run);
            }
        }

        void FindLabeForPiperunsMainPart(SecondaryPiperun secRun = null, PrimaryPiperun primRun = null)
        {
            Piperun run = null;
            if (secRun != null) run = secRun;
            if (primRun != null) run = primRun;
            for (int i = 0; i < run.coords.Length - 2; i += 2)
            {
                if (Math.Abs(run.coords[i + 1] - run.coords[i + 3]) < 0.001)
                {
                    foreach (OneRowFullLabel label in oneRowFullLabels.Where(x => Math.Abs(x.RotationAngleDegree - 0) < 3))
                    {
                        if ((label.Y > run.coords[i + 1] && Math.Abs(label.Y - run.coords[i + 1]) < 0.003))
                        {
                            if (((label.X > run.coords[i] && label.X < run.coords[i + 2]) || (label.X < run.coords[i] && label.X > run.coords[i + 2])))
                            {
                                run.labelIds.Add(label.LabelId);
                                label.PointOnPipeX = label.X;
                                label.PointOnPipeY = run.coords[i + 1];
                            }
                        }
                    }

                    foreach (TwoRowsFullLabel label in twoRowsFullLabels.Where(x => Math.Abs(x.RotationAngleDegree - 0) < 3))
                    {
                        if ((label.Y > run.coords[i + 1] && Math.Abs(label.Y - run.coords[i + 1]) < 0.003))
                        {
                            if (((label.X > run.coords[i] && label.X < run.coords[i + 2]) || (label.X < run.coords[i] && label.X > run.coords[i + 2])))
                            {
                                run.labelIds.Add(label.LabelId);
                                label.PointOnPipeX = label.X;
                                label.PointOnPipeY = run.coords[i + 1];
                            }

                        }
                    }

                    foreach (OnlyDiaLabel label in onlyDiaLabels.Where(x => Math.Abs(x.RotationAngleDegree - 0) < 3))
                    {
                        if ((label.Y > run.coords[i + 1] && Math.Abs(label.Y - run.coords[i + 1]) < 0.003))
                        {
                            if (((label.X > run.coords[i] && label.X < run.coords[i + 2]) || (label.X < run.coords[i] && label.X > run.coords[i + 2])))
                            {
                                run.labelIds.Add(label.LabelId);
                                label.PointOnPipeX = label.X;
                                label.PointOnPipeY = run.coords[i + 1];
                            }

                        }
                    }

                    foreach (OnlyDiaLabelH label in onlyDiaLabelsH.Where(x => Math.Abs(x.RotationAngleDegree - 0) < 3))
                    {
                        if ((label.Y > run.coords[i + 1] && Math.Abs(label.Y - run.coords[i + 1]) < 0.003))
                        {
                            if (((label.X > run.coords[i] && label.X < run.coords[i + 2]) || (label.X < run.coords[i] && label.X > run.coords[i + 2])))
                            {
                                run.labelIds.Add(label.LabelId);
                                label.PointOnPipeX = label.X;
                                label.PointOnPipeY = run.coords[i + 1];
                            }

                        }
                    }

                }

                if (Math.Abs(run.coords[i] - run.coords[i + 2]) < 0.001)
                {
                    foreach (OneRowFullLabel label in oneRowFullLabels.Where(x => Math.Abs(x.RotationAngleDegree - 90) < 3))
                    {
                        if ((label.X < run.coords[i] && Math.Abs(label.X - run.coords[i]) < 0.003))
                        {
                            if (((label.Y > run.coords[i + 1] && label.Y < run.coords[i + 3]) || (label.Y < run.coords[i + 1] && label.Y > run.coords[i + 3])))
                            {
                                run.labelIds.Add(label.LabelId);
                                label.PointOnPipeX = run.coords[i];
                                label.PointOnPipeY = label.Y;
                            }
                        }
                    }

                    foreach (TwoRowsFullLabel label in twoRowsFullLabels.Where(x => Math.Abs(x.RotationAngleDegree - 90) < 3))
                    {
                        if ((label.X < run.coords[i] && Math.Abs(label.X - run.coords[i]) < 0.003))
                        {
                            if (((label.Y > run.coords[i + 1] && label.Y < run.coords[i + 3]) || (label.Y < run.coords[i + 1] && label.Y > run.coords[i + 3])))
                            {
                                run.labelIds.Add(label.LabelId);
                                label.PointOnPipeX = run.coords[i];
                                label.PointOnPipeY = label.Y;
                            }
                        }
                    }

                    foreach (OnlyDiaLabel label in onlyDiaLabels.Where(x => Math.Abs(x.RotationAngleDegree - 90) < 3))
                    {
                        if ((label.X < run.coords[i] && Math.Abs(label.X - run.coords[i]) < 0.003))
                        {
                            if (((label.Y > run.coords[i + 1] && label.Y < run.coords[i + 3]) || (label.Y < run.coords[i + 1] && label.Y > run.coords[i + 3])))
                            {
                                run.labelIds.Add(label.LabelId);
                                label.PointOnPipeX = run.coords[i];
                                label.PointOnPipeY = label.Y;
                            }
                        }
                    }

                    foreach (OnlyDiaLabelH label in onlyDiaLabelsH.Where(x => Math.Abs(x.RotationAngleDegree - 90) < 3))
                    {
                        if ((label.X < run.coords[i] && Math.Abs(label.X - run.coords[i]) < 0.003))
                        {
                            if (((label.Y > run.coords[i + 1] && label.Y < run.coords[i + 3]) || (label.Y < run.coords[i + 1] && label.Y > run.coords[i + 3])))
                            {
                                run.labelIds.Add(label.LabelId);
                                label.PointOnPipeX = run.coords[i];
                                label.PointOnPipeY = label.Y;
                            }
                        }
                    }
                }

            }
        }

        void ExtendPiperunForNearestPiperun()
        {
            double extendedLength = 0.011;
            foreach(var currentRun in secondaryPiperuns.Where(x => x.coords.Length >= 4))
            {
                double x1 = currentRun.coords[0];
                double y1 = currentRun.coords[1];
                double x12 = currentRun.coords[2];
                double y12 = currentRun.coords[3];
                double x2 = currentRun.coords[currentRun.coords.Length - 2];
                double y2 = currentRun.coords[currentRun.coords.Length - 1];
                double x22 = currentRun.coords[currentRun.coords.Length - 4];
                double y22 = currentRun.coords[currentRun.coords.Length - 3];

                foreach (var testRun in secondaryPiperuns.Where(x => x.coords.Length >= 4).Where(x => !x.Equals(currentRun)))
                {
                    double testX1 = testRun.coords[0];
                    double testY1 = testRun.coords[1];
                    double testX12 = testRun.coords[2];
                    double testY12 = testRun.coords[3];
                    double testX2 = testRun.coords[testRun.coords.Length - 2];
                    double testY2 = testRun.coords[testRun.coords.Length - 1];
                    double testX22 = testRun.coords[testRun.coords.Length - 4];
                    double testY22 = testRun.coords[testRun.coords.Length - 3];

                    if ((Math.Abs(x1 - testX1) < 0.002 && Math.Abs(y1 - y12) > 0.003 && Math.Abs(testY1 - testY12) > 0.003) ||
                        (Math.Abs(x1 - testX2) < 0.002 && Math.Abs(y1 - y12) > 0.003 && Math.Abs(testY2 - testY22) > 0.003) ||
                        (Math.Abs(x2 - testX1) < 0.002 && Math.Abs(y2 - y22) > 0.003 && Math.Abs(testY1 - testY12) > 0.003) ||
                        (Math.Abs(x2 - testX2) < 0.002 && Math.Abs(y2 - y22) > 0.003 && Math.Abs(testY2 - testY22) > 0.003) ||
                        (Math.Abs(y1 - testY1) < 0.002 && Math.Abs(x1 - x12) > 0.003 && Math.Abs(testX1 - testX12) > 0.003) ||
                        (Math.Abs(y1 - testY2) < 0.002 && Math.Abs(x1 - x12) > 0.003 && Math.Abs(testX2 - testX22) > 0.003) ||
                        (Math.Abs(y2 - testY1) < 0.002 && Math.Abs(x2 - x22) > 0.003 && Math.Abs(testX1 - testX12) > 0.003) ||
                        (Math.Abs(y2 - testY2) < 0.002 && Math.Abs(x2 - x22) > 0.003 && Math.Abs(testX2 - testX22) > 0.003))
                    {
                        //richTextBox1.Text += "case " + CalculateDistance(x1, y1, testX1, testY1) + " " + CalculateDistance(x1, y1, testX2, testY2) + " " + CalculateDistance(x2, y2, testX1, testY1) + " " + CalculateDistance(x2, y2, testX2, testY2) + "\n";

                        if (CalculateDistance(x1, y1, testX1, testY1) < extendedLength)
                        {
                            currentRun.NewCoordsWithNewBegining(testX1, testY1);
                        }

                        else if (CalculateDistance(x1, y1, testX2, testY2) < extendedLength)
                        {
                            currentRun.NewCoordsWithNewBegining(testX2, testY2);
                        }

                        else if (CalculateDistance(x2, y2, testX1, testY1) < extendedLength)
                        {
                            currentRun.NewCoordsWithNewEnd(testX1, testY1);
                        }

                        else if (CalculateDistance(x2, y2, testX2, testY2) < extendedLength)
                        {
                            currentRun.NewCoordsWithNewEnd(testX2, testY2);
                        }
                    }
                    
                }
            }

            foreach (var currentRun in primaryPiperuns.Where(x => x.coords.Length >= 4))
            {
                double x1 = currentRun.coords[0];
                double y1 = currentRun.coords[1];
                double x12 = currentRun.coords[2];
                double y12 = currentRun.coords[3];
                double x2 = currentRun.coords[currentRun.coords.Length - 2];
                double y2 = currentRun.coords[currentRun.coords.Length - 1];
                double x22 = currentRun.coords[currentRun.coords.Length - 4];
                double y22 = currentRun.coords[currentRun.coords.Length - 3];

                foreach (var testRun in primaryPiperuns.Where(x => x.coords.Length >= 4).Where(x => !x.Equals(currentRun)))
                {
                    double testX1 = testRun.coords[0];
                    double testY1 = testRun.coords[1];
                    double testX12 = testRun.coords[2];
                    double testY12 = testRun.coords[3];
                    double testX2 = testRun.coords[testRun.coords.Length - 2];
                    double testY2 = testRun.coords[testRun.coords.Length - 1];
                    double testX22 = testRun.coords[testRun.coords.Length - 4];
                    double testY22 = testRun.coords[testRun.coords.Length - 3];

                    if ((Math.Abs(x1 - testX1) < 0.002 && Math.Abs(y1 - y12) > 0.003 && Math.Abs(testY1 - testY12) > 0.003) ||
                        (Math.Abs(x1 - testX2) < 0.002 && Math.Abs(y1 - y12) > 0.003 && Math.Abs(testY2 - testY22) > 0.003) ||
                        (Math.Abs(x2 - testX1) < 0.002 && Math.Abs(y2 - y22) > 0.003 && Math.Abs(testY1 - testY12) > 0.003) ||
                        (Math.Abs(x2 - testX2) < 0.002 && Math.Abs(y2 - y22) > 0.003 && Math.Abs(testY2 - testY22) > 0.003) ||
                        (Math.Abs(y1 - testY1) < 0.002 && Math.Abs(x1 - x12) > 0.003 && Math.Abs(testX1 - testX12) > 0.003) ||
                        (Math.Abs(y1 - testY2) < 0.002 && Math.Abs(x1 - x12) > 0.003 && Math.Abs(testX2 - testX22) > 0.003) ||
                        (Math.Abs(y2 - testY1) < 0.002 && Math.Abs(x2 - x22) > 0.003 && Math.Abs(testX1 - testX12) > 0.003) ||
                        (Math.Abs(y2 - testY2) < 0.002 && Math.Abs(x2 - x22) > 0.003 && Math.Abs(testX2 - testX22) > 0.003))
                    {
                        //richTextBox1.Text += "case " + CalculateDistance(x1, y1, testX1, testY1) + " " + CalculateDistance(x1, y1, testX2, testY2) + " " + CalculateDistance(x2, y2, testX1, testY1) + " " + CalculateDistance(x2, y2, testX2, testY2) + "\n";

                        if (CalculateDistance(x1, y1, testX1, testY1) < extendedLength)
                        {
                            currentRun.NewCoordsWithNewBegining(testX1, testY1);
                        }

                        else if (CalculateDistance(x1, y1, testX2, testY2) < extendedLength)
                        {
                            currentRun.NewCoordsWithNewBegining(testX2, testY2);
                        }

                        else if (CalculateDistance(x2, y2, testX1, testY1) < extendedLength)
                        {
                            currentRun.NewCoordsWithNewEnd(testX1, testY1);
                        }

                        else if (CalculateDistance(x2, y2, testX2, testY2) < extendedLength)
                        {
                            currentRun.NewCoordsWithNewEnd(testX2, testY2);
                        }
                    }

                }
            }
        }

        void MergePiperuns()
        {
            foreach(var currentRun in secondaryPiperuns.Where(x => x.labelIds.Count == 0 && x.IsEnabled))
            {
                double x1 = currentRun.coords[0];
                double y1 = currentRun.coords[1];
                double x2 = currentRun.coords[currentRun.coords.Length - 2];
                double y2 = currentRun.coords[currentRun.coords.Length - 1];

                foreach(var testRun in secondaryPiperuns.Where(x => !x.Equals(currentRun) && x.labelIds.Count == 0 && x.IsEnabled)) 
                {
                    double testX1 = testRun.coords[0];
                    double testY1 = testRun.coords[1];
                    double testX2 = testRun.coords[testRun.coords.Length - 2];
                    double testY2 = testRun.coords[testRun.coords.Length - 1];

                    if (CalculateDistance(x1, y1, testX1, testY1) < 0.001)
                    {
                        currentRun.MergePiperuns(testRun, true, true);
                    }

                    else if (CalculateDistance(x1, y1, testX2, testY2) < 0.001)
                    {
                        currentRun.MergePiperuns(testRun, true, false);
                    }

                    else if (CalculateDistance(x2, y2, testX1, testY1) < 0.001)
                    {
                        currentRun.MergePiperuns(testRun, false, true);
                    }

                    else if (CalculateDistance(x2, y2, testX2, testY2) < 0.001)
                    {
                        currentRun.MergePiperuns(testRun, false, false);
                    }
                }
            }

            foreach (var currentRun in primaryPiperuns.Where(x => x.labelIds.Count == 0 && x.IsEnabled))
            {
                double x1 = currentRun.coords[0];
                double y1 = currentRun.coords[1];
                double x2 = currentRun.coords[currentRun.coords.Length - 2];
                double y2 = currentRun.coords[currentRun.coords.Length - 1];

                foreach (var testRun in primaryPiperuns.Where(x => !x.Equals(currentRun) && x.labelIds.Count == 0))
                {
                    double testX1 = testRun.coords[0];
                    double testY1 = testRun.coords[1];
                    double testX2 = testRun.coords[testRun.coords.Length - 2];
                    double testY2 = testRun.coords[testRun.coords.Length - 1];

                    if (CalculateDistance(x1, y1, testX1, testY1) < 0.001)
                    {
                        currentRun.MergePiperuns(testRun, true, true);
                    }

                    else if (CalculateDistance(x1, y1, testX2, testY2) < 0.001)
                    {
                        currentRun.MergePiperuns(testRun, true, false);
                    }

                    else if (CalculateDistance(x2, y2, testX1, testY1) < 0.001)
                    {
                        currentRun.MergePiperuns(testRun, false, true);
                    }

                    else if (CalculateDistance(x2, y2, testX2, testY2) < 0.001)
                    {
                        currentRun.MergePiperuns(testRun, false, false);
                    }
                }
            }
        }

        bool IsDigit(char testChar)
        {
            int i = 0;
            return int.TryParse(testChar.ToString(), out i);
        }






        void PlaceItemsToPID()
        {
            Plaice.Placement placement = new Plaice.Placement();
            foreach (var primaryPiperun in primaryPiperuns.Where(x => x.coords.Count() >= 4 && x.IsEnabled))
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Routing\\Process Lines\\Primary Piping.sym");
                item.Commit();
                PlaceRunInputs placeRunInputs = new PlaceRunInputs();
                for (int i = 0; i < primaryPiperun.coords.Count(); i += 2)
                {
                    placeRunInputs.AddPoint(primaryPiperun.coords[i], primaryPiperun.coords[i + 1]);
                }
                placement.PIDPlaceRun(item, placeRunInputs);

                LMPipeRun lMPipeRun = placement.PIDDataSource.GetPipeRun(item.Id);
                PlacePipingLabels(placement, lMPipeRun, null, primaryPiperun);
            }

            foreach (var secondaryPiperun in secondaryPiperuns.Where(x => x.coords.Count() >= 4 && x.IsEnabled))
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Routing\\Process Lines\\Secondary Piping.sym");
                item.Commit();
                PlaceRunInputs placeRunInputs = new PlaceRunInputs();
                for (int i = 0; i < secondaryPiperun.coords.Count(); i += 2)
                {
                    placeRunInputs.AddPoint(secondaryPiperun.coords[i], secondaryPiperun.coords[i + 1]);
                }
                placement.PIDPlaceRun(item, placeRunInputs);

                LMPipeRun lMPipeRun = placement.PIDDataSource.GetPipeRun(item.Id);
                PlacePipingLabels(placement, lMPipeRun, secondaryPiperun);
            }

            foreach (var roundTool in roundTools)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Instrumentation\\Tools\\Instrument.sym");
                item.Commit();
                placement.PIDPlaceSymbol("\\_New_catalog\\Instrumentation\\Tools\\Instrument.sym", roundTool.X, roundTool.Y, null, null, item);
                LMInstrument instr = placement.PIDDataSource.GetInstrument(item.Id);
                placement.PIDDataSource.BeginTransaction();
                instr.Attributes["InstrumentTypeModifier"].set_Value(roundTool.InstrTypeModifier);
                instr.Attributes["MeasuredVariableCode"].set_Value(roundTool.MeasuredVariable);
                instr.Attributes["InventoryTag"].set_Value(roundTool.InventaryTag);
                instr.Attributes["TagSequenceNo"].set_Value(roundTool.TagSeqNo);
                placement.PIDDataSource.CommitTransaction();
                instr.Commit();
            }

            foreach (var roundSquareTool in roundSquareTools)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Instrumentation\\Tools\\DCS Accessible.sym");
                item.Commit();
                placement.PIDPlaceSymbol("\\_New_catalog\\Instrumentation\\Tools\\DCS Accessible.sym", roundSquareTool.X, roundSquareTool.Y, null, null, item);
                LMInstrument instr = placement.PIDDataSource.GetInstrument(item.Id);
                placement.PIDDataSource.BeginTransaction();
                instr.Attributes["InstrumentTypeModifier"].set_Value(roundSquareTool.InstrTypeModifier);
                instr.Attributes["MeasuredVariableCode"].set_Value(roundSquareTool.MeasuredVariable);
                instr.Attributes["InventoryTag"].set_Value(roundSquareTool.InventaryTag);
                instr.Attributes["TagSequenceNo"].set_Value(roundSquareTool.TagSeqNo);
                placement.PIDDataSource.CommitTransaction();
                instr.Commit();
            }

            foreach (var interlockTool in interlockTools)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Instrumentation\\Tools\\Interlock.sym");
                item.Commit();
                placement.PIDPlaceSymbol("\\_New_catalog\\Instrumentation\\Tools\\Interlock.sym", interlockTool.X, interlockTool.Y, null, null, item);
                LMInstrument instr = placement.PIDDataSource.GetInstrument(item.Id);
                placement.PIDDataSource.BeginTransaction();
                instr.Attributes["InstrumentTypeModifier"].set_Value(interlockTool.InstrTypeModifier);
                instr.Attributes["MeasuredVariableCode"].set_Value(interlockTool.MeasuredVariable);
                instr.Attributes["InventoryTag"].set_Value(interlockTool.InventaryTag);
                instr.Attributes["TagSequenceNo"].set_Value(interlockTool.TagSeqNo);
                placement.PIDDataSource.CommitTransaction();
                instr.Commit();
            }

            foreach (var blockValve in blockValves)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Valves\\2 Way Common\\BLOCKVALVE.sym");
                item.Commit();
                placement.PIDPlaceSymbol("\\_New_catalog\\Piping\\Valves\\2 Way Common\\BLOCKVALVE.sym", blockValve.X, blockValve.Y);
            }

            foreach (var gateValve in gateValves)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Valves\\2 Way Common\\GATE VALVE.sym");
                item.Commit();
                placement.PIDPlaceSymbol("\\_New_catalog\\Piping\\Valves\\2 Way Common\\GATE VALVE.sym", gateValve.X, gateValve.Y);
            }

            foreach (var note in notes)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Labels\\Note_25.sym");
                item.Commit();
                var symbol = placement.PIDPlaceSymbol("\\_New_catalog\\Labels\\Note_25.sym", note.X, note.Y, null, note.RotationAngle, item);
                LMItemNote lmNote = placement.PIDDataSource.GetItemNote(item.Id);
                placement.PIDDataSource.BeginTransaction();
                lmNote.Attributes["Note.Body"].set_Value(note.Text);
                placement.PIDDataSource.CommitTransaction();
            }

            foreach (var reducer in reducers)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Fittings\\Diameter Change\\REDUCER.sym");
                item.Commit();
                var symbol = placement.PIDPlaceSymbol("\\_New_catalog\\Piping\\Fittings\\Diameter Change\\REDUCER.sym", reducer.X, reducer.Y, null, null, item);
                LMPipingComp comp = placement.PIDDataSource.GetPipingComp(item.Id);
                placement.PIDPlaceLabel("\\_New_catalog\\Piping\\Labels - Piping Components\\Red Nominal Diameter.sym", new double[6] { reducer.X, reducer.Y, reducer.X, reducer.Y, reducer.X, reducer.Y }, false, 0, comp.Representations.Nth[1]);
            }

            foreach (var noteDroplet in noteDroplets)
            {
                var itemNote = placement.PIDCreateItem("\\_New_catalog\\Labels\\Note.sym");
                itemNote.Commit();
                var symbol = placement.PIDPlaceSymbol("\\_New_catalog\\Labels\\Note.sym", noteDroplet.X, noteDroplet.Y, null, noteDroplet.RotationAngle, itemNote);
                LMItemNote lmNote = placement.PIDDataSource.GetItemNote(itemNote.Id);
                placement.PIDDataSource.BeginTransaction();
                lmNote.Attributes["Note.Body"].set_Value(noteDroplet.Text);
                placement.PIDDataSource.CommitTransaction();
                lmNote.Commit();
            }

            foreach (var spectacle in spectacles)
            {
                if (spectacle.isClosed && !spectacle.isLong)
                {
                    var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Fittings\\Spectacle Blanks and Spacers\\Blank Closed Spectacle.sym");
                    item.Commit();
                    placement.PIDPlaceSymbol("\\_New_catalog\\Piping\\Fittings\\Spectacle Blanks and Spacers\\Blank Closed Spectacle.sym", spectacle.X, spectacle.Y);
                }

                else if (spectacle.isClosed && spectacle.isLong)
                {
                    var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Fittings\\Spectacle Blanks and Spacers\\BLANK INST SPACER STORED.sym");
                    item.Commit();
                    placement.PIDPlaceSymbol("\\_New_catalog\\Piping\\Fittings\\Spectacle Blanks and Spacers\\BLANK INST SPACER STORED.sym", spectacle.X, spectacle.Y);
                }

                else if(!spectacle.isClosed && !spectacle.isLong)
                {
                    var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Fittings\\Spectacle Blanks and Spacers\\Blank Open Spectacle.sym");
                    item.Commit();
                    placement.PIDPlaceSymbol("\\_New_catalog\\Piping\\Fittings\\Spectacle Blanks and Spacers\\Blank Open Spectacle.sym", spectacle.X, spectacle.Y);
                }

                else if(!spectacle.isClosed && spectacle.isLong)
                {
                    var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Fittings\\Spectacle Blanks and Spacers\\SPACER INST BLANK STORED.sym");
                    item.Commit();
                    placement.PIDPlaceSymbol("\\_New_catalog\\Piping\\Fittings\\Spectacle Blanks and Spacers\\SPACER INST BLANK STORED.sym", spectacle.X, spectacle.Y);
                }

            }

            foreach( var flange in flanges)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Piping\\Fittings\\Flanges and Unions\\Flange.sym");
                item.Commit();
                placement.PIDPlaceSymbol("\\_New_catalog\\Piping\\Fittings\\Flanges and Unions\\Flange.sym", flange.X, flange.Y);
            }

            foreach (var instrArrow in instrArrows)
            {
                var item = placement.PIDCreateItem("\\_New_catalog\\Instrumentation\\Labels\\Direction of signal.sym");
                item.Commit();
                placement.PIDPlaceSymbol("\\_New_catalog\\Instrumentation\\Labels\\Direction of signal.sym", instrArrow.X, instrArrow.Y, false, instrArrow.RotationAngle);
            }
        }

        void PlacePipingLabels(Plaice.Placement placement, LMPipeRun lMPipeRun, SecondaryPiperun secRun = null, PrimaryPiperun primRun = null)
        {
            Piperun run = null;
            if (secRun != null) run = secRun;
            if (primRun != null) run = primRun;


            //placement.PIDPlaceLabel("\\_New_catalog\\Piping\\Labels - Piping Segments\\Flow Direction.sym", new double[6] { run.coords[0], run.coords[1], run.coords[0], run.coords[1], run.coords[0], run.coords[1] }, lMPipeRun.Representations.Nth[2]);
            foreach (int labelId in run.labelIds)
            {
                foreach (OneRowFullLabel label in oneRowFullLabels)
                {
                    if (label.LabelId == labelId)
                    {
                        placement.PIDDataSource.BeginTransaction();
                        lMPipeRun.Attributes["OperFluidCode"].set_Value(label.fluid);
                        lMPipeRun.Attributes["TagSequenceNo"].set_Value(label.tagSeqNo);
                        lMPipeRun.Attributes["PipingMaterialsClass"].set_Value(label.pipingClass);
                        lMPipeRun.Attributes["NominalDiameter"].set_Value(label.diaInch);
                        lMPipeRun.Attributes["mmNominalDia"].set_Value(label.diaMM);
                        placement.PIDDataSource.CommitTransaction();
                        lMPipeRun.Commit();

                        var ll = placement.PIDPlaceLabel("\\_New_catalog\\Piping\\Labels - Piping Segments\\Line Number.sym", new double[6] { label.PointOnPipeX, label.PointOnPipeY, label.PointOnPipeX, label.PointOnPipeY, label.X, label.Y }, false, label.RotationAngle, lMPipeRun.Representations.Nth[2]);
                        ll.Commit();
                    }

                }

                foreach (TwoRowsFullLabel label in twoRowsFullLabels)
                {
                    if (label.LabelId == labelId)
                    {
                        placement.PIDDataSource.BeginTransaction();
                        lMPipeRun.Attributes["OperFluidCode"].set_Value(label.fluid);
                        lMPipeRun.Attributes["TagSequenceNo"].set_Value(label.tagSeqNo);
                        lMPipeRun.Attributes["PipingMaterialsClass"].set_Value(label.pipingClass);
                        lMPipeRun.Attributes["NominalDiameter"].set_Value(label.diaInch);
                        lMPipeRun.Attributes["mmNominalDia"].set_Value(label.diaMM);
                        placement.PIDDataSource.CommitTransaction();
                        lMPipeRun.Commit();

                        var ll = placement.PIDPlaceLabel("\\_New_catalog\\Piping\\Labels - Piping Segments\\Line Number 2 rows.sym", new double[6] { label.PointOnPipeX, label.PointOnPipeY, label.PointOnPipeX, label.PointOnPipeY, label.X, label.Y }, false, label.RotationAngle, lMPipeRun.Representations.Nth[2]);
                        ll.Commit();

                    }

                }

                foreach (OnlyDiaLabel label in onlyDiaLabels)
                {
                    if (label.LabelId == labelId)
                    {
                        placement.PIDDataSource.BeginTransaction();
                        lMPipeRun.Attributes["NominalDiameter"].set_Value(label.diaInch);
                        placement.PIDDataSource.CommitTransaction();
                        lMPipeRun.Commit();

                        var ll = placement.PIDPlaceLabel("\\_New_catalog\\Piping\\Labels - Piping Components\\DN.sym", new double[6] { label.PointOnPipeX, label.PointOnPipeY, label.PointOnPipeX, label.PointOnPipeY, label.X, label.Y }, false, label.RotationAngle, lMPipeRun.Representations.Nth[2]);
                        ll.Commit();

                    }

                }

                foreach (OnlyDiaLabelH label in onlyDiaLabelsH)
                {
                    if (label.LabelId == labelId)
                    {
                        placement.PIDDataSource.BeginTransaction();
                        lMPipeRun.Attributes["NominalDiameter"].set_Value(label.diaInch);
                        placement.PIDDataSource.CommitTransaction();
                        lMPipeRun.Commit();

                        var ll = placement.PIDPlaceLabel("\\_New_catalog\\Piping\\Labels - Piping Components\\DN_hold.sym", new double[6] { label.PointOnPipeX, label.PointOnPipeY, label.PointOnPipeX, label.PointOnPipeY, label.X, label.Y }, false, label.RotationAngle, lMPipeRun.Representations.Nth[2]);
                        ll.Commit();

                    }

                }

            }
            var itemNote = placement.PIDCreateItem("\\_New_catalog\\Labels\\Note_25.sym");
            itemNote.Commit();
            var symbol = placement.PIDPlaceSymbol("\\_New_catalog\\Labels\\Note_25.sym", run.coords[0], run.coords[1], null, 0.0, itemNote);
            LMItemNote lmNote = placement.PIDDataSource.GetItemNote(itemNote.Id);
            placement.PIDDataSource.BeginTransaction();
            lmNote.Attributes["Note.Body"].set_Value("ID: " + run.ID);
            placement.PIDDataSource.CommitTransaction();
            lmNote.Commit();
        }
    }
}
