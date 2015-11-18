using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundOut;
using CSCore.Streams;
using NAudio.Wave;

namespace Dino_Generator.Model
{
    public class Phrase
    {

        public static List<string> PhraseTypes = new List<string>()
        {
            "ADJECTIVE",
            "AND",
            "ARE",
            "COUGH",
            "DEMONSTRATION",
            "ENUMERATION",
            "FIXED_PHRASE",
            "ING",
            "NOUN_PLURAL",
            "NOUN_SINGULAR",
            "NUMBER",
            "PREPOSITION",
            "PRONOUN",
            "SCREENSHOT",
            "THE",
            "TO",
            "VERB__PRESENT",
            "VERB_PRESENT",
            "VERB_SINGULAR",
            "VERB_THIRD_PERSON"
        };

        public int PauseAfter { get; set; }

        public string Text { get; set; }
        public string WavName { get; set; }
        public string WavNameFull { get
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Resources\\Wav\\" + WavName + ".wav";
        } }
        public string Type { get; set; }

        public static ObservableCollection<Phrase> AllPhrases = new ObservableCollection<Phrase>();

        public Phrase()
        {
                this.PauseAfter = new Random().Next(1, 3);

        }

        public static List<Phrase> LoadPhrasesFromXml()
        {
            List<Phrase> phrases = new List<Phrase>();


            return phrases;
        }

        public static List<Phrase> LoadFromCsv()
        {
            string csv = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}Resources\\Config\\Dino - Sheet1.csv");
            string[] splitCsv = csv.Split(new string[] {"\r\n"}, StringSplitOptions.None );
            for (int i = 0; i < splitCsv.Length; i++)
            {
                string line = splitCsv[i];
                if (i == 0)
                {
                    continue;
                }
                else
                {
                    Phrase phrase = new Phrase();
                    
                    string[] parts = line.Trim().Split(new string[] {"\t"}, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < parts.Length; j++)
                    {
                        if (j == 0) phrase.Text = parts[j];
                        if (j == 1) phrase.Type = parts[j];
                        if (parts.Length == 3 && j == 2)
                        {
                            phrase.WavName = parts[j];
                        }
                        else if(parts.Length == 2 && j == 1)
                        {
                            phrase.WavName = phrase.Text ;
                        }
                    }
                    AllPhrases.Add(phrase);

                }

            }

            return null;
        } 

        public static Phrase ChoosePhraseByType(string type)
        {


            if (type.Contains("?"))
            {
                string frequency = type.Substring(type.Count() - 2, 2);
                type = type.Substring(0, type.Count()-3);
                if (Utils.GetRealRandomNumberInRange(0, 100) < int.Parse(frequency))
                {
                    return null;
                }
                
            }

            if (type.StartsWith("!"))
            {

                type = type.Replace("!", "");
                int numberOfElements = AllPhrases.Count(p => p.Text == type);
                if (numberOfElements > 1)
                {
                    int index = Utils.GetRealRandomNumberInRange(0, numberOfElements - 1);
                    return AllPhrases.Where(p => p.Text == type).ToList<Phrase>()[index];

                }
                else
                {
                    return AllPhrases.First(p => p.Text == type);
                }
            }

            List<Phrase> phrasesWithType = AllPhrases.Where(p => p.Type == type).ToList<Phrase>();

            int randomIndex = Utils.GetRealRandomNumberInRange(0, phrasesWithType.Count());
            return phrasesWithType.ElementAt(randomIndex);
        }

        public override string ToString()
        {
            return this.Text + " ";
        }
    }   
}
