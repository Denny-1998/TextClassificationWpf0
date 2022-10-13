using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification.Business
{

    public class Tokenization
    {
        private const int SMALLESTWORDLENGTH = 3;

        public static List<string> Tokenize(string originalText)
        {
            List<string> words = new List<string>();
            String [] tokens = originalText.Split(' ');

            
            foreach (string token in tokens)
            {
                
                if (IsAShortWord(token)){
                    // skip
                }
                else
                {
                    string cleanWord = RemovePunctuation(token);
                    cleanWord = cleanWord.ToLower();
                    words.Add(cleanWord);
                }
            }
            return words;
        }



        public static bool IsAShortWord(string token)
        {
            if (token.Length < SMALLESTWORDLENGTH)
            {
                return true;
            }
            return false;
        }

        public static string RemovePunctuation(string token)
        {
            //punctuation to remove 
            string[] charsToRemove = new string[] { ",", "“", "”", "\"", "!", ".", "?", "\n", "'", ";", "(", ")", "‘", "’", ":" };


            //loop through the list of chars to remove
            for (int i = 0; i<charsToRemove.Length; i++)
            {
                //replace caracters with nothing 
                token = token.Replace(charsToRemove[i], "");

            }

            return token;
        }
        

    }
}
