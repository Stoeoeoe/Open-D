using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dino_Generator.Model;

namespace Dino_Generator.Model
{
    public class Sentence
    {
        public int Length { get; set; }
        public List<Phrase> Phrases { get; set; } = new List<Phrase>();
        public PhrasePattern Pattern { get; set; }


        public Sentence()
        {
            
        }

        public static Sentence GenerateSentence(string pattern)
        {
            Sentence sentence = new Sentence();
            if (PhrasePattern.AllPatterns.Count == 0)
            {
                MessageBox.Show("No Phrase Patterns defined. Please define some.", "No phrase patterns available",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return null;
            }
            if (pattern == "")
            {
                int randomIndex = Utils.GetRealRandomNumberInRange(0, PhrasePattern.AllPatterns.Count);
                sentence.Pattern = PhrasePattern.AllPatterns.ElementAt(randomIndex);

            }
            else if (PhrasePattern.AllPatterns.Any(p => p.PhraseString == pattern) == false)
            {
                sentence.Pattern = new PhrasePattern(pattern);
            }
            else
            {
                sentence.Pattern = PhrasePattern.AllPatterns.FirstOrDefault(p => p.PhraseString == pattern);
            }

            for (int i = 0; i < sentence.Pattern.PhraseSegments.Count; i++)
            {
                string phraseType = sentence.Pattern.PhraseSegments[i];
                if (phraseType.Contains("|"))
                {
                    string[] orPhraseTypes = phraseType.Split('|');
                    phraseType = orPhraseTypes[Utils.GetRealRandomNumberInRange(0, orPhraseTypes.Length - 1)];
                }

                string type = phraseType;
                Phrase phrase = Phrase.ChoosePhraseByType(type);
                if(phrase != null)
                    sentence.Phrases.Add(phrase);                
            }

            return sentence;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Phrases.Count; i++)
            {
                
                if (i == 0)
                {
                    string text = Phrases[0].ToString();
                   sb.Append(text[0].ToString().ToUpper() + text.Substring(1, text.Length-1) + " ");
                }
                else
                {
                    sb.Append(Phrases[i].ToString());
                }
            }
            sb.Append(". ");
            return sb.ToString();
        }
    }
}
