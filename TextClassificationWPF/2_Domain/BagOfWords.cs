using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassification.Business;

namespace TextClassification.Domain
{
    public class BagOfWords
    {
        readonly IDictionary<string, int> bagOfWords;

        public BagOfWords()
        {
            bagOfWords = new SortedDictionary<string, int>();
        }

        public void InsertEntry(string word)
        {

            word = word.Trim();
            word = Tokenization.RemovePunctuation(word);
            if (word.Length == 0)
            {
                return;
            }


            if (bagOfWords.ContainsKey(word))       //TODO insert stemming algorithm here
            {
                int count = bagOfWords[word];
                count++;
                bagOfWords[word] = count;
            }
            else
            {
                bagOfWords.Add(word, 1);
            }
        }

        public ICollection<string> GetAllWordsInDictionary()
        {
            return bagOfWords.Keys;
        }

        public List<string> GetEntriesInDictionary()
        {
            List<string> entries = new List<string>();
            foreach (string key in bagOfWords.Keys)
            {
                entries.Add(key);
            }
            return entries;
        }




        public string StemmingAlgorithm(string input)
        {
            //insert stemming algorithm here
            
            return null;
        }

    }
}