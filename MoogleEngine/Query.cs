using System;
using MoogleEngine.Math;

namespace MoogleEngine {
    public class Query {

        Vector tf;
        Vector tfidf;
        public Query(string query) {
            this.Text = query;
            this.Words = new string[]{};
            this.SkippedWords = new string[]{};
            this.MustWords = new string[]{};
            this.WeightedWords = new Dictionary<string, int>();

            this.CheckOperators();

            double[] tfValues = new double[this.Words.Length];


            // Calculando la frecuencia de cada termino del query
            for (int i = 0; i < this.Words.Length; i++) {
                double tf = this.Words[i].Count() / this.Words.Length;
                tfValues[i] = tf;
            }    

            this.tf = new Vector(tfValues);
            this.tfidf = new Vector(new double[this.Words.Length]);
        }

        private void CheckOperators() {
            string[] splittedText = this.Text.Split(' ');
            List<string> skipped = new List<string>();
            List<string> must = new List<string>();

            for (int i = 0; i < splittedText.Length; i++) {
                if (splittedText[i].Contains('!')) {
                    splittedText[i] = splittedText[i].ToLower().Replace("!", "");
                    skipped.Add(splittedText[i]);
                } 
                else if (splittedText[i].Contains('*')) {
                    int weightMultiplier = splittedText[i].ToCharArray().Count(c => c == '*');
                    splittedText[i] = splittedText[i].ToLower().Replace("*", "");
                    try {
                        this.WeightedWords[splittedText[i]] += weightMultiplier;
                    } catch (KeyNotFoundException) {
                        this.WeightedWords[splittedText[i]] = weightMultiplier + 1;
                    }
                }
                else if (splittedText[i].Contains('^')) {
                    splittedText[i] = splittedText[i].ToLower().Replace("^", "");
                    must.Add(splittedText[i]);
                }
            }

            this.Words = splittedText;
            this.SkippedWords = skipped.ToArray<string>();
            this.MustWords = must.ToArray<string>();
        }

        public string[] MustWords {
            get;

            private set;
        }
        public Dictionary<string, int> WeightedWords {
            get;

            private set;
        }
        public string[] SkippedWords {
            get;

            private set;
        }

        public string Text {
            get;
        }

        public string[] Words {
            get;

            private set;
        }

        public Vector TF {
            get { return this.tf; }
        }

        public Vector Weight {
            get { return this.tfidf; }

            set { this.tfidf = value; }
        }
    }
}