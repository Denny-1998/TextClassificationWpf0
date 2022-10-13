using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextClassification.FileIO
{
    public class FolderProperties
    {

        string PropertiesPath = "..\\..\\..\\..\\TextClassificationWPF\\bin\\Debug\\Categories.txt";
        string folderA = "A";
        string folderB = "B";


        public FolderProperties()
        {
            if (File.Exists(PropertiesPath))
            {
                string[] lines = File.ReadAllLines(PropertiesPath);


                if (lines.Length >= 1)
                {
                    folderA = lines[0];
                    folderB = lines[1];
                }
                
            }
        }



        public string getFolderA ()
        {
            return folderA;
        }

        public string getFolderB()
        {
            return folderB;
        }
    }
}
