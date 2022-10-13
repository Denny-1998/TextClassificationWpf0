using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TextClassification.Business;
using TextClassification.Domain;
using TextClassification.FileIO;

namespace TextClassification.Controller
{
    public class KnnAlgorithm
    {

        int k = 5; //k value for calculation


        private Knowledge Knowledge;
        private List<string> TokensInText;
        private List<bool> VectorForText;



        public KnnAlgorithm(Knowledge kn, int kValue)
        {
            this.Knowledge = kn;
            this.k = kValue;
        }


        public List<bool> GetVectorForText()
        {
            return VectorForText;
        }


        public void ReadFile(string Path)
        {
            string text = File.ReadAllText(Path);
            
            List<string> Tokens = TokenizeFile(text);

            TokensInText = Tokens; ;
        }


        private List<string> TokenizeFile(string text)
        {
            List<string> Tokens = Tokenization.Tokenize(text);
            return Tokens;
        }


        
        public void GenerateVector()
        {
            List<bool> vector = new List<bool>();

            BagOfWords bow = Knowledge.GetBagOfWords();

            foreach (string key in bow.GetEntriesInDictionary())
            {
                
                if (TokensInText.Contains(key))
                {
                    vector.Add(true);
                }
                else
                {
                    vector.Add(false);
                }
            }
            VectorForText = vector;
        }

        



        public double CompareVectors(List<bool> Vector1, List<bool> Vector2)
        {
            double distance = 0;


            if (Vector1.Count != Vector2.Count) {
                MessageBox.Show("vectors are not the same length");
            }
            else
            {
                int sum = 0;


                for (int i = 0; i < Vector1.Count; i++)
                {
                    int dist;
                    bool comp = Vector1[i] ^ Vector2[i];    //xor operation (same as the absolute value of vector 1 - vector 2)
                    
                    if (comp) dist = 1;
                    else dist = 0;

                    sum += dist;       //works because 0^2 still is 0 and 1^2 is 1 (^2 becomes redundant)
                }

                distance = Math.Sqrt(sum);

            } 
            return distance;

        }


        //checks if our text has any correspondance with the training texts
        public bool CheckIfVectorsContainTrue()
        {
            bool containTrue = false;
            foreach (bool coordinate in VectorForText)
            {
                if (coordinate) containTrue = true;
            }
            return containTrue;
        }



        public void CalculateNearestNeighbor()
        {
            Vectors V = Knowledge.GetVectors();
            List<List<bool>> VectorsInA = new List<List<bool>>();
            List<List<bool>> VectorsInB = new List<List<bool>>();

            
            
            //generate list of distances
            List<NearestNeighbor> Distance = new List<NearestNeighbor>();
            List<NearestNeighbor> NearestDistances = new List<NearestNeighbor>();


            if (CheckIfVectorsContainTrue())
            {


                //make a list of all vectors in A
                for (int i = 0; i < V.GetNumberOfVectorsInA(); i++)
                {
                    VectorsInA.Add(V.GetVectorFromA(i));
                }

                //make a list of all vectors in B
                for (int i = 0; i < V.GetNumberOfVectorsInB(); i++)
                {
                    VectorsInB.Add(V.GetVectorFromB(i));
                }



                //combine vectors from a and from b into one list
                foreach (List<bool> vector in VectorsInA)
                {
                    double dist = CompareVectors(VectorForText, vector);
                    NearestNeighbor n = new NearestNeighbor('A', dist);
                    Distance.Add(n);
                }
                foreach (List<bool> vector in VectorsInB)
                {
                    double dist = CompareVectors(VectorForText, vector);
                    NearestNeighbor n = new NearestNeighbor('B', dist);
                    Distance.Add(n);
                }



                //sort distances after shortest distance
                Distance.Sort();


                //list the k lowest distances 
                for (int i = 0; i < k; i++)
                {
                    NearestDistances.Add(Distance[i]);
                }




                //count the neighbors belonging to class a and b 
                int countA = 0;
                int countB = 0;

                for (int i = 0; i < NearestDistances.Count; i++)
                {
                    char Folder = NearestDistances[i].Folder;

                    if (Folder.Equals('A')) countA++;
                    else if (Folder.Equals('B')) countB++;
                }





                //get properties to show what each folder is supposed to contain (categories.txt)
                FolderProperties fp = new FolderProperties();
                string a = fp.getFolderA();
                string b = fp.getFolderB();

                double percent = CalculatePercentage(countA, countB);
                string percentStr = string.Format("{0:N2}%", percent);
                
                
                //show result in messagebox
                if (NearestDistances[0].Distance == 0)
                {
                    char folder = NearestDistances[0].Folder;
                    string result;
                    if (folder.Equals('A')) result = a;
                    else if (folder.Equals('B')) result = b;
                    else result = "";
                    

                    MessageBox.Show("text is classified as: \n" + result + "\n it is contained in the training folder" + "\n\n\nNeighbors from A: " + countA + "\nNeighbors from B: " + countB);
                }
                else if (countA < countB)
                {
                    MessageBox.Show("text is classified as: \n" + a + "\n" + percentStr + "\n\n\nNeighbors from A: " + countA + "\nNeighbors from B: " + countB);
                }
                else if (countB < countA)
                {
                    MessageBox.Show("text is classified as: \n" + b + "\n" + percentStr + "\n\n\nNeighbors from A: " + countA + "\nNeighbors from B: " + countB);
                }
                else if (countA == countB)
                {
                    MessageBox.Show("Not sure.\n" + percentStr + "\n\n\nNeighbors from A: " + countA + "\nNeighbors from B: " + countB);
                }
                else
                {
                    MessageBox.Show("something went wrong");
                }
            }
            else MessageBox.Show("Not sure, no correspondance found");


        }


        public double CalculatePercentage(double count1, double count2)
        {
            double percent = 0;
            double percent100 = count1 + count2;

            if (count2 < count1)
            {
                percent = count1 / percent100;
            }
            else if (count2 > count1)
            {
                percent =count2 / percent100;
            } else if (count2 == count1)
            {
                percent = 0.5;
            }

            return percent * 100;

        }
    }









    internal class NearestNeighbor : IComparable<NearestNeighbor>
    {
        public char Folder { get; set; }
        public double Distance { get; set; }

        public NearestNeighbor(char Folder, double Distance)
        {
            this.Folder = Folder;
            this.Distance = Distance;
        }

        public int CompareTo(NearestNeighbor that)
        {
            if (this.Distance < that.Distance) return -1;
            if (this.Distance == that.Distance) return 0;
            return 1;
        }

    }
}
