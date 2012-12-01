using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Dictionary {
    /// <summary>
    /// Interaction logic for DictionaryView.xaml
    /// </summary>
    public partial class DictionaryDisplay : UserControl {
        static string dictionary_url = "http://oxforddictionaries.com/definition/american_english/";


        public DictionaryDisplay(string word) {
            InitializeComponent();
            this.dictionary_term.Content = word;
            //this.dictionary_definition.Content = new TextBlock {
            //    Text = getDefinition(word), TextWrapping = TextWrapping.Wrap
            //};
            this.dictionary_definition.Text = getDefinition(word);
        }

        public static string getDefinition(string word) {

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(dictionary_url + word);
            request.UserAgent = "Dictionary Web Crawler";

            //get html source page
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string htmltext = reader.ReadToEnd();
            reader.Close();

            //create a navigator to walk through the html
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(htmltext);

            //parse out the definition of word
            WordDefinition wd = parseDefinition(word, htmldoc);

            //write formatted definition to text file
            int counter = 1;
            string retvalue = "";
            foreach (List<string> l in wd.getDefinition()) {
                retvalue += counter.ToString() + ". ";
                int inner_counter = 0;
                foreach (string s in l) {
                    if (inner_counter == 0) {
                        retvalue += s;
                    }
                    else {
                        retvalue += "\n\t - " + s;
                    }
                    inner_counter++;
                }
                retvalue += "\n\n";
                counter++;
            }
            return retvalue;
        }

        /**
         * 
         */
        private static WordDefinition parseDefinition(String word, HtmlAgilityPack.HtmlDocument htmldoc) {
            List<List<string>> definitions = new List<List<string>>();
            foreach (HtmlAgilityPack.HtmlNode node in htmldoc.DocumentNode.Descendants("ul").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("sense-entry"))) {
                List<string> l = new List<string>();
                foreach (HtmlAgilityPack.HtmlNode n in node.Descendants("span").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("definition"))) {
                    l.Add(Regex.Replace(n.InnerHtml, "<!.*?>", string.Empty));
                }
                definitions.Add(l);
            }

            WordDefinition wd = new WordDefinition(word, definitions);
            return wd;
        }

        class WordDefinition {
            string word;
            List<List<string>> definition;

            public WordDefinition(string word, List<List<string>> definition) {
                this.word = word;
                this.definition = definition;
            }

            public string getWord() {
                return word;
            }

            public List<List<string>> getDefinition() {
                return this.definition;
            }
        }
    }
}
