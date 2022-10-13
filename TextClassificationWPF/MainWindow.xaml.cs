using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextClassification.Controller;
using TextClassification.Domain;
using TextClassification.FileIO;

namespace TextClassificationWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ListItems> items;
        Knowledge Knowledge;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartLearning_Click(object sender, RoutedEventArgs e)
        {
            //start time
            DateTimeOffset now = DateTimeOffset.UtcNow; 

            KnowledgeBuilder nb = new KnowledgeBuilder();

            // initiate the learning process
            nb.Train();

            // getting the (whole) knowledge found in ClassA and in ClassB
            Knowledge = nb.GetKnowledge();


            // get a part of the knowledge - here just for debugging
            BagOfWords bof = Knowledge.GetBagOfWords();

            //end time
            DateTimeOffset end = DateTimeOffset.UtcNow;

            long unixTimeMilliseconds = end.ToUnixTimeMilliseconds()-now.ToUnixTimeMilliseconds(); 
           
            TrainingTime.Text = unixTimeMilliseconds + " ms"; 



            List<string> entries = bof.GetEntriesInDictionary();

            //List object
            items = new List<ListItems>();


            //looping through all entries and adding them to the list obj
            for (int i = 0; i < entries.Count; i++) 
            {
                items.Add(new ListItems() { Title = entries[i] });
            }

            //Adding the list to the xaml
            //Itemlist = items;

            ListOfItems.ItemsSource = items;

        }



        public class ListItems
        {
            public ListItems()
            {
                this.Title = "";
            }

            public string Title { get; set; }
            
        }



        private void btnUploadText_Click(object sender, RoutedEventArgs e)
        {
           
            string FilePath = tbFilePath.Text;
            bool cont = false;

            string kTextBox = kValueTb.Text;
            int kValue;

            try
            {
                kValue = Int32.Parse(kTextBox);
            } 
            catch
            {
                MessageBox.Show("please input a valid K-Value");
                kValue = 5;         //just a default value
            }


            //check if training is started first
            if (Knowledge == null)
            {
                MessageBox.Show("Please start training first.");
            }
            else
            {

                //check if filepath is empty 
                if (FilePath.Equals(""))
                {
                    MessageBox.Show("please enter a file path.");
                }
                else
                {

                    //check if file exists
                    if (File.Exists(FilePath))
                    {

                        //do knn calculations

                        KnnAlgorithm knn = new KnnAlgorithm(Knowledge, kValue);
                        knn.ReadFile(FilePath);
                        knn.GenerateVector();

                        knn.CalculateNearestNeighbor();

                    }
                    else
                    {
                        MessageBox.Show("please enter a valid file path.");
                    }
                }
            }
        }
    }
}
