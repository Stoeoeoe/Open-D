using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Xml;
using System.Xml.Serialization;
using Dino_Generator.Model;

namespace Dino_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public MainWindow()
        {
            InitializeComponent();

            Phrase.LoadFromCsv();
            PhrasePattern.LoadAllPhrasePatterns();

            // Test all phrases
            foreach (Phrase phrase in Phrase.AllPhrases)
            {
                if (File.Exists(phrase.WavNameFull) == false)
                {
                    MessageBox.Show($"{phrase.Text}/{phrase.Type}/{phrase.WavName} does not exist");
                }
            }

            foreach (var phrasePattern in PhrasePattern.AllPatterns)
            {
                foreach (string phraseSegment in phrasePattern.PhraseSegments)
                {
                    
                }
            }


            comboBox.ItemsSource = PhrasePattern.AllPatterns;
            patternsGrid.ItemsSource = PhrasePattern.AllPatterns;
            phrasesGrid.ItemsSource = Phrase.AllPhrases;
            typeColumn.ItemsSource = Phrase.PhraseTypes;
        }

        private void PatternsGridOnAddingNewItem(object sender, AddingNewItemEventArgs addingNewItemEventArgs)
        {
            
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            Talk talk = null;

            // Specific phrase pattern
            if (specifiedPatternBox.Text != "")
            {
                try
                {
                    talk = new Talk(1, specifiedPatternBox.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("Phrase pattern contains an error. Please check again.", "Phrase pattern errorneous",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {

                int numberOfSentences = 1;

                try
                {
                    numberOfSentences = int.Parse(sentenceNumber.Text.Trim());

                }
                catch (Exception)
                {

                }

                if ((bool)checkBox.IsChecked == false)
                {
                    talk = new Talk(numberOfSentences);
                }
                else
                {
                    string selectedPattern = comboBox.SelectedValue == null ? comboBox.Items[0].ToString() : comboBox.SelectedValue.ToString();
                    talk = new Talk(numberOfSentences, selectedPattern);

                }

            }
            DinoBox.Text = talk?.ToString();
            talk?.GenerateAudio();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var phrasePattern in PhrasePattern.AllPatterns)
            {
                sb.AppendLine(phrasePattern.PhraseString);
            }
            File.WriteAllText(PhrasePattern.PatternFileName, sb.ToString());
        }

        private void SavePhrase(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var phrase in Phrase.AllPhrases)
            {
                sb.AppendLine($"{phrase.Text}\t{phrase.Type}\t{phrase.WavName}");
            }
            File.WriteAllText(PhrasePattern.PatternFileName, sb.ToString());
        }
    }
}
