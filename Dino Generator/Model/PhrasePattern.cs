using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Dino_Generator.Model
{
    public class PhrasePattern
    {

        public static ObservableCollection<PhrasePattern> AllPatterns = new ObservableCollection<PhrasePattern>();
        public static string PatternFileName = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Config\\patterns.csv";

        public string PhraseString { get; set; }
        public List<string> PhraseSegments => PhraseString.Split('/').ToList();


        public PhrasePattern()
        {

        }

        public PhrasePattern(string phraseString)
        {
            this.PhraseString = phraseString;
        }

        public static void LoadAllPhrasePatterns()
        {
            string text = File.ReadAllText(PatternFileName);
            List<PhrasePattern> patterns = new List<PhrasePattern>();
            foreach (var line in text.Split(new string[] { "\r\n"}, StringSplitOptions.RemoveEmptyEntries))
            {
                patterns.Add(new PhrasePattern(line.Trim()));
            }
            AllPatterns = new ObservableCollection<PhrasePattern>(patterns);
        }

        public override string ToString()
        {
            return PhraseString;
        }
    }
}
