using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using NAudio.Wave;

namespace Dino_Generator.Model
{

    public class Talk
    {
        private static MediaPlayer mediaPlayer = new MediaPlayer();

        public int NumberOfSentences { get; set; } = 1;
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();

        public Talk()
        {

        }

        public Talk(int numberOfSentences, string pattern = "")
        {
            
            this.NumberOfSentences = numberOfSentences;

            for (int i = 0; i < this.NumberOfSentences; i++)
            {
                var sentence = Sentence.GenerateSentence(pattern);
                if (sentence != null)
                {
                    this.Sentences.Add(sentence);
                }
            }
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Sentence sentence in Sentences)
            {
                sb.Append(sentence.ToString());
            }
            return sb.ToString();
        }

        public void GenerateAudio()
        {
            var waveSources = new List<string>();


            string outFileName = "C:\\tmp\\Dino_" + DateTime.Now.Ticks + ".wav";
            FileStream fs = new FileStream(outFileName, FileMode.Create);
            WaveFileWriter waveFileWriter = null;

            WaveFormat waveFormat = new WaveFormat(45000, 1);

            byte[] buffer = new byte[1024];

            using (waveFileWriter = new WaveFileWriter(fs, waveFormat))
            {
                foreach (var sentence in Sentences)
                {
                    foreach (var phrase in sentence.Phrases)
                    {
                        using (WaveFileReader reader = new WaveFileReader(phrase.WavNameFull))
                        {
                            int read;
                            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                waveFileWriter.Write(buffer, 0, read);
                            }
                        }
                        // Noise
                        string emptyName = AppDomain.CurrentDomain.BaseDirectory + "Resources\\Wav\\pause" +
                                           phrase.PauseAfter + ".wav";
                        using (WaveFileReader reader = new WaveFileReader(emptyName))
                        {
                            int read;
                            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                waveFileWriter.Write(buffer, 0, read);
                            }
                        }
                    }
                    waveFileWriter.Flush();
                }
            }

            // Play 
            PlaySound(outFileName);
        }

        private static void PlaySound(string outFileName)
        {
            mediaPlayer.Open(new Uri(outFileName, UriKind.Absolute));
            mediaPlayer.MediaOpened += (sender, args) => mediaPlayer.Play();

        }

    }
}
