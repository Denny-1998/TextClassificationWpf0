using TextClassification.Controller;
using TextClassification.Domain;
using TextClassification.FileIO;
using TextClassification.Foundation;

namespace TestProject
{
    [TestClass]
    public class Testing
    {


        [TestMethod]
        public void TestVectors()
        {

            //arrange
            //train to get vectors of the different texts
            KnowledgeBuilder nb = new KnowledgeBuilder();
            nb.Train();
            Knowledge k = nb.GetKnowledge();

            //save one vector to compare
            Vectors v = k.GetVectors();


            //act 
            List<bool> actualVector = v.GetVectorFromB(0);



            //assert
            //check if a vector is generated 
            Assert.IsNotNull(actualVector[0]);

        }


        [TestMethod]
        public void TestVectorContent()
        {

            //arrange
            KnowledgeBuilder nb = new KnowledgeBuilder();
            nb.Train();
            Knowledge k = nb.GetKnowledge();

            //testfile contains the word about
            string FilePath = "..\\..\\..\\VectorTestFile.txt";
            
            
            
            KnnAlgorithm knn = new KnnAlgorithm(k, 5);
            knn.ReadFile(FilePath);
            knn.GenerateVector();


            
            List<bool> actual = knn.GetVectorForText();
            List<bool> expected = new List<bool>();


            //act
            for (int i = 0; i < actual.Count; i++)
            {
                //the word about is index 31 in the dictionary, so we're manually creating a vector for that
                if (i == 30) expected.Add(true);
                else expected.Add(false);
            }





            //assert
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(actual[i], expected[i]);
            }
            
        }



        [TestMethod]
        public void TestNumberOfVectors()
        {
            //arrange
            //train to get vectors of the different texts
            KnowledgeBuilder nb = new KnowledgeBuilder();
            nb.Train();
            Knowledge k = nb.GetKnowledge();

            //save one vector to compare
            


            //act 
            List<List<bool>> ActualA = new List<List<bool>>();
            List<List<bool>> ActualB = new List<List<bool>>();

            Vectors v = k.GetVectors();

            for (int i = 0; i < v.GetNumberOfVectorsInA(); i++)
            {
                ActualA.Add(v.GetVectorFromA(i));
            }
            for (int i = 0; i < v.GetNumberOfVectorsInB(); i++)
            {
                ActualB.Add(v.GetVectorFromB(i));
            }


            //assert
            //check if vectors are present in list for a and for b
            Assert.IsTrue(ActualA.Count > 0 && ActualB.Count > 0);

        }


        [TestMethod]
        public void TestVectorDistance()
        {   
            // arrange
            //create empty knowledge so the constructor doesnt complain
            Knowledge knowledge = new Knowledge();
            
            //create example vectors 
            List<bool> vector1 = new List<bool>();
            vector1.Add(true);
            vector1.Add(false);

            List<bool> vector2 = new List<bool>();
            vector2.Add(false);
            vector2.Add(false);

            //create instance of knn class
            KnnAlgorithm knn = new KnnAlgorithm(knowledge, 5);
            

            double expected = 1;
            
            // act
            double actual = knn.CompareVectors(vector1, vector2);

            // assert
            Assert.AreEqual(expected, actual);
        }








        [TestMethod]
        public void TestRemovePunctuation()
        {
            KnowledgeBuilder nb = new KnowledgeBuilder();


            nb.Train();

            Knowledge k = nb.GetKnowledge();


            BagOfWords bof = k.GetBagOfWords();

            List<string> entries = bof.GetEntriesInDictionary();


            //check for entry 134 if its "about" or "about:"
            string entry = entries[30];
            Assert.AreEqual("about", entry);

        }




        [TestMethod]
        public void TestWordItemGetWord()
        {
            // arrange
            string expected = "nice";
            WordItem wI = new WordItem("nice", 0);

            // act
            string actual = wI.GetWord();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestStringOperationsGetFileName()
        {
            // deprecated - use 
            // arrange
            string expected = "Suduko";
            string path = "c:\\users\\tha\\documents\\Suduko.txt";

            // act
            string actual = StringOperations.getFileName(path);

            // assert
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        public void TestFileGetAllFileNames()  //does not work anymore because of relative folder path
        {
            // arrange
            string folderA = "ClassA";
            string fileType = "txt";

            string relativeFilePath = "..\\..\\..\\..\\TextClassificationWPF\\bin\\Debug\\";
            List<string> expected = new List<string>();


            expected.Add(relativeFilePath + folderA + "\\bbcsportsfootball." + fileType);
            expected.Add(relativeFilePath + folderA + "\\dailymirrornfl." + fileType);
            expected.Add(relativeFilePath + folderA + "\\independent boxing." + fileType);
            expected.Add(relativeFilePath + folderA + "\\MetroArsenal." + fileType);
            expected.Add(relativeFilePath + folderA + "\\RedBullF1." + fileType);
            expected.Add(relativeFilePath + folderA + "\\sunsportsboxing." + fileType);
            expected.Add(relativeFilePath + folderA + "\\thesun." + fileType);



            // act
            FileAdapter fa = new TextFile(fileType);
            List<string> actual = fa.GetAllFileNames(folderA);

            // assert
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);
        }



        [TestMethod]
        public void TestGetFilePathA()
        {
            // arrange
            string folderA = "ClassA";
            string fileType = "txt";
            string fileName = "filnavn";
            string relativeFilePath = "..\\..\\..\\..\\TextClassificationWPF\\bin\\Debug\\";
            string expected = relativeFilePath + folderA + "\\filnavn." + fileType;

            // act
            TextFile tf = new TextFile(fileType);
            string actual = tf.GetFilePathA(fileName);

            // assert
            Assert.AreEqual(expected, actual);
        }




    }
}





