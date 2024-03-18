using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace From_AutoCAD_to_SmartPlantPID.Additional
{
    public static class Enums
    {
        public static List<string> NotTools = new List<string>();
        public static List<string> PossibleNotesText = new List<string>();
        public static void CreateLists()
        {
            NotTools = File.ReadAllLines(Directory.GetCurrentDirectory() + "/NotTools.txt", Encoding.GetEncoding(1251)).ToList();
            PossibleNotesText = File.ReadAllLines(Directory.GetCurrentDirectory() + "/PossibleNotesText.txt", Encoding.GetEncoding(1251)).ToList();
        }

        public static bool IsTextNote(string testText)
        {
            foreach(string text in PossibleNotesText)
            {
                if (testText.ToUpper().Contains(text.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
