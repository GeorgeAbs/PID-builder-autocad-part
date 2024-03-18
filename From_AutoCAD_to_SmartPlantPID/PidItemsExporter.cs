using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using From_AutoCAD_to_SmartPlantPID.Labels.PipingLabels;
using From_AutoCAD_to_SmartPlantPID.Labels;

namespace From_AutoCAD_to_SmartPlantPID
{
    public class PidItemsExporter
    {
        public PidItemsExporter() 
        { 
            
        }

        public void Export(string fullPathForFile)
        {
            XDocument xdoc = new XDocument();
            XElement mainItem = new XElement("MyItems");
            foreach (var label in Form1.onlyDiaLabels)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("OnlyDiaLabel");
                xAttributes.Add(new XAttribute("X", label.X.ToString()));
                xAttributes.Add(new XAttribute("Y", label.Y.ToString()));
                xAttributes.Add(new XAttribute("LabelId", label.LabelId.ToString()));
                xAttributes.Add(new XAttribute("RotationAngle", label.RotationAngle.ToString()));
                xAttributes.Add(new XAttribute("RotationAngleDegree", label.RotationAngleDegree.ToString()));
                xAttributes.Add(new XAttribute("PointOnPipeX", label.PointOnPipeX.ToString()));
                xAttributes.Add(new XAttribute("PointOnPipeY", label.PointOnPipeY.ToString()));
                xAttributes.Add(new XAttribute("Text", label.Text.ToString()));
                xAttributes.Add(new XAttribute("fluid", label.fluid.ToString()));
                xAttributes.Add(new XAttribute("diaInch", label.diaInch.ToString()));
                xAttributes.Add(new XAttribute("diaMM", label.diaMM.ToString()));
                xAttributes.Add(new XAttribute("tagSeqNo", label.tagSeqNo.ToString()));
                xAttributes.Add(new XAttribute("pipingClass", label.pipingClass.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var label in Form1.onlyDiaLabelsH)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("OnlyDiaLabelH");
                xAttributes.Add(new XAttribute("X", label.X.ToString()));
                xAttributes.Add(new XAttribute("Y", label.Y.ToString()));
                xAttributes.Add(new XAttribute("LabelId", label.LabelId.ToString()));
                xAttributes.Add(new XAttribute("RotationAngle", label.RotationAngle.ToString()));
                xAttributes.Add(new XAttribute("RotationAngleDegree", label.RotationAngleDegree.ToString()));
                xAttributes.Add(new XAttribute("PointOnPipeX", label.PointOnPipeX.ToString()));
                xAttributes.Add(new XAttribute("PointOnPipeY", label.PointOnPipeY.ToString()));
                xAttributes.Add(new XAttribute("Text", label.Text.ToString()));
                xAttributes.Add(new XAttribute("fluid", label.fluid.ToString()));
                xAttributes.Add(new XAttribute("diaInch", label.diaInch.ToString()));
                xAttributes.Add(new XAttribute("diaMM", label.diaMM.ToString()));
                xAttributes.Add(new XAttribute("tagSeqNo", label.tagSeqNo.ToString()));
                xAttributes.Add(new XAttribute("pipingClass", label.pipingClass.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var label in Form1.oneRowFullLabels)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("OneRowFullLabel");
                xAttributes.Add(new XAttribute("X", label.X.ToString()));
                xAttributes.Add(new XAttribute("Y", label.Y.ToString()));
                xAttributes.Add(new XAttribute("LabelId", label.LabelId.ToString()));
                xAttributes.Add(new XAttribute("RotationAngle", label.RotationAngle.ToString()));
                xAttributes.Add(new XAttribute("RotationAngleDegree", label.RotationAngleDegree.ToString()));
                xAttributes.Add(new XAttribute("PointOnPipeX", label.PointOnPipeX.ToString()));
                xAttributes.Add(new XAttribute("PointOnPipeY", label.PointOnPipeY.ToString()));
                xAttributes.Add(new XAttribute("Text", label.Text.ToString()));
                xAttributes.Add(new XAttribute("fluid", label.fluid.ToString()));
                xAttributes.Add(new XAttribute("diaInch", label.diaInch.ToString()));
                xAttributes.Add(new XAttribute("diaMM", label.diaMM.ToString()));
                xAttributes.Add(new XAttribute("tagSeqNo", label.tagSeqNo.ToString()));
                xAttributes.Add(new XAttribute("pipingClass", label.pipingClass.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var label in Form1.twoRowsFullLabels)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("TwoRowsFullLabel");
                xAttributes.Add(new XAttribute("X", label.X.ToString()));
                xAttributes.Add(new XAttribute("Y", label.Y.ToString()));
                xAttributes.Add(new XAttribute("LabelId", label.LabelId.ToString()));
                xAttributes.Add(new XAttribute("RotationAngle", label.RotationAngle.ToString()));
                xAttributes.Add(new XAttribute("RotationAngleDegree", label.RotationAngleDegree.ToString()));
                xAttributes.Add(new XAttribute("PointOnPipeX", label.PointOnPipeX.ToString()));
                xAttributes.Add(new XAttribute("PointOnPipeY", label.PointOnPipeY.ToString()));
                xAttributes.Add(new XAttribute("Text", label.Text.ToString()));
                xAttributes.Add(new XAttribute("fluid", label.fluid.ToString()));
                xAttributes.Add(new XAttribute("diaInch", label.diaInch.ToString()));
                xAttributes.Add(new XAttribute("diaMM", label.diaMM.ToString()));
                xAttributes.Add(new XAttribute("tagSeqNo", label.tagSeqNo.ToString()));
                xAttributes.Add(new XAttribute("pipingClass", label.pipingClass.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.notes)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("Note");
                xAttributes.Add(new XAttribute("X", thisItem.X.ToString()));
                xAttributes.Add(new XAttribute("Y", thisItem.Y.ToString()));
                xAttributes.Add(new XAttribute("RotationAngle", thisItem.RotationAngle.ToString()));
                xAttributes.Add(new XAttribute("Text", thisItem.Text.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.noteDroplets)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("NoteDroplet");
                xAttributes.Add(new XAttribute("X", thisItem.X.ToString()));
                xAttributes.Add(new XAttribute("Y", thisItem.Y.ToString()));
                xAttributes.Add(new XAttribute("RotationAngle", thisItem.RotationAngle.ToString()));
                xAttributes.Add(new XAttribute("Text", thisItem.Text.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.roundTools)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("RoundTool");
                xAttributes.Add(new XAttribute("X", thisItem.X.ToString()));
                xAttributes.Add(new XAttribute("Y", thisItem.Y.ToString()));
                xAttributes.Add(new XAttribute("InstrTypeModifier", thisItem.InstrTypeModifier.ToString()));
                xAttributes.Add(new XAttribute("InventaryTag", thisItem.InventaryTag.ToString()));
                xAttributes.Add(new XAttribute("MeasuredVariable", thisItem.MeasuredVariable.ToString()));
                xAttributes.Add(new XAttribute("TagSeqNo", thisItem.TagSeqNo.ToString()));
                xAttributes.Add(new XAttribute("TagSeqNoSuffix", thisItem.TagSeqNoSuffix.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.roundSquareTools)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("RoundSquareTool");
                xAttributes.Add(new XAttribute("X", thisItem.X.ToString()));
                xAttributes.Add(new XAttribute("Y", thisItem.Y.ToString()));
                xAttributes.Add(new XAttribute("InstrTypeModifier", thisItem.InstrTypeModifier.ToString()));
                xAttributes.Add(new XAttribute("InventaryTag", thisItem.InventaryTag.ToString()));
                xAttributes.Add(new XAttribute("MeasuredVariable", thisItem.MeasuredVariable.ToString()));
                xAttributes.Add(new XAttribute("TagSeqNo", thisItem.TagSeqNo.ToString()));
                xAttributes.Add(new XAttribute("TagSeqNoSuffix", thisItem.TagSeqNoSuffix.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.interlockTools)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("InterlockTool");
                xAttributes.Add(new XAttribute("X", thisItem.X.ToString()));
                xAttributes.Add(new XAttribute("Y", thisItem.Y.ToString()));
                xAttributes.Add(new XAttribute("InstrTypeModifier", thisItem.InstrTypeModifier.ToString()));
                xAttributes.Add(new XAttribute("InventaryTag", thisItem.InventaryTag.ToString()));
                xAttributes.Add(new XAttribute("MeasuredVariable", thisItem.MeasuredVariable.ToString()));
                xAttributes.Add(new XAttribute("TagSeqNo", thisItem.TagSeqNo.ToString()));
                xAttributes.Add(new XAttribute("TagSeqNoSuffix", thisItem.TagSeqNoSuffix.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.allTypesBreaks)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("AllTypesBreak");
                xAttributes.Add(new XAttribute("InsertionPointX", thisItem.InsertionPointX.ToString()));
                xAttributes.Add(new XAttribute("InsertionPointY", thisItem.InsertionPointY.ToString()));
                xAttributes.Add(new XAttribute("LeaderConnectionX", thisItem.LeaderConnectionX.ToString()));
                xAttributes.Add(new XAttribute("LeaderConnectionY", thisItem.LeaderConnectionY.ToString()));

                string coords = "";
                foreach(var coord in thisItem.coordinates)
                {
                    coords += coord.ToString() + ";";
                }
                coords.TrimEnd(';');
                xAttributes.Add(new XAttribute("Coordinates", coords));

                string rawCoords = "";
                foreach (var coord in thisItem.rawCoordinates)
                {
                    rawCoords += coord.ToString() + ";";
                }
                rawCoords.TrimEnd(';');
                xAttributes.Add(new XAttribute("RawCoordinates", rawCoords));

                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.primaryPiperuns)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("PrimaryPiperun");
                xAttributes.Add(new XAttribute("ID", thisItem.ID.ToString()));
                xAttributes.Add(new XAttribute("IsEnabled", thisItem.IsEnabled.ToString()));

                string coords = "";
                foreach (var coord in thisItem.coords)
                {
                    coords += coord.ToString() + ";";
                }
                coords.TrimEnd(';');
                xAttributes.Add(new XAttribute("coords", coords));

                string labelIds = "";
                foreach (var coord in thisItem.labelIds)
                {
                    labelIds += coord.ToString() + ";";
                }
                labelIds.TrimEnd(';');
                xAttributes.Add(new XAttribute("labelIds", labelIds));

                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.secondaryPiperuns)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("SecondaryPiperun");
                xAttributes.Add(new XAttribute("ID", thisItem.ID.ToString()));
                xAttributes.Add(new XAttribute("IsEnabled", thisItem.IsEnabled.ToString()));

                string coords = "";
                foreach (var coord in thisItem.coords)
                {
                    coords += coord.ToString() + ";";
                }
                coords.TrimEnd(';');
                xAttributes.Add(new XAttribute("coords", coords));

                string labelIds = "";
                foreach (var coord in thisItem.labelIds)
                {
                    labelIds += coord.ToString() + ";";
                }
                labelIds.TrimEnd(';');
                xAttributes.Add(new XAttribute("labelIds", labelIds));

                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.blockValves)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("BlockValve");
                xAttributes.Add(new XAttribute("X", thisItem.X.ToString()));
                xAttributes.Add(new XAttribute("Y", thisItem.Y.ToString()));
                xAttributes.Add(new XAttribute("pointOnItemX", thisItem.pointOnItemX.ToString()));
                xAttributes.Add(new XAttribute("pointOnItemY", thisItem.pointOnItemY.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.gateValves)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("GateValve");
                xAttributes.Add(new XAttribute("X", thisItem.X.ToString()));
                xAttributes.Add(new XAttribute("Y", thisItem.Y.ToString()));
                xAttributes.Add(new XAttribute("pointOnItemX", thisItem.pointOnItemX.ToString()));
                xAttributes.Add(new XAttribute("pointOnItemY", thisItem.pointOnItemY.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }

            foreach (var thisItem in Form1.reducers)
            {
                List<XAttribute> xAttributes = new List<XAttribute>();
                XElement item = new XElement("Reducer");
                xAttributes.Add(new XAttribute("X", thisItem.X.ToString()));
                xAttributes.Add(new XAttribute("Y", thisItem.Y.ToString()));
                xAttributes.Add(new XAttribute("pointOnItemX", thisItem.pointOnItemX.ToString()));
                xAttributes.Add(new XAttribute("pointOnItemY", thisItem.pointOnItemY.ToString()));
                item.Add(xAttributes);

                mainItem.Add(item);
            }


            xdoc.Add(mainItem);
            xdoc.Save(fullPathForFile);
        }
    }
}
