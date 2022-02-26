using MoogleEngine.Math;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace MoogleEngine {
    public class ScoreComparer : IComparer<SearchItem> {
        public int Compare(SearchItem? x, SearchItem? y) {
            if (x == null || y == null)
                return 0;

            return x.Score.CompareTo(y.Score);
        }
    }
    public class DocumentInfo {
        int queryMatches;

        public int QueryMatches 
        {
            get {
                return this.queryMatches;
            }

            set {
                this.queryMatches = value;
            }
        }
    }
    public static class Moogle {
        public static SearchResult Query(string query) {
            // Modifique este método para responder a la búsqueda
            Dictionary<int, DocumentInfo> docInfo = new Dictionary<int, DocumentInfo>();
            
            Document[] docs = LoadFilesContent(docInfo);

            string[] fileNames = Directory.GetFiles("../Content");
            string[] queryWords = query.Split(' ');

            Matrix tfidf = TFIDF(query, docs, docInfo);
            
            double[] results = GetResults(tfidf);

            List<SearchItem> items = new List<SearchItem>();
            string suggestion = "";

            // Ahora mismo no funciona
            for (int i = 0; i < results.Length; i++) {
                if (results[i] != 0) {
                    string name = fileNames[i].Split('/').Last<string>();
                    items.Add(new SearchItem(name, GetSnippet(query, docs[i].Content), (float)results[i]));
                }
            }

            items.Sort(new ScoreComparer());
            items.Reverse();
            Array.Sort(results);
            Array.Reverse(results);

            float[] scores = CalculateScore(items, results, query, docInfo);

            // System.Console.WriteLine(tfidf.ToString());

            // for(int i = 0; i < queryWords.Length; i++) {
            //     for (int j = 0; j < content.Length; j++) {
            //         if (tfidf[i, j] != 0)
            //             System.Console.WriteLine("{0} - {1}: {2}", queryWords[i], fileNames[j], tfidf[i, j]);
            //     }
            // }

            // SearchItem[] items = new SearchItem[3] {
            //     new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
            //     new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
            //     new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
            // };

            return new SearchResult(items.ToArray(), suggestion);
        }
        
        public static Document[] LoadFilesContent(Dictionary<int, DocumentInfo> docInfo) {
            string[] fileNames = Directory.GetFiles("../Content");
            Document[] documents = new Document[fileNames.Length];

            for (int i = 0; i < fileNames.Length; i++)
            {
                StreamReader reader = new StreamReader(fileNames[i]);
                string fileContent = reader.ReadToEnd();                

                documents[i] = new Document(fileContent, fileNames[i]);
                docInfo[i] = new DocumentInfo();
            }

            return documents;
        }

        public static float[] CalculateScore(
            List<SearchItem> items,
            double[] results, 
            string query, 
            Dictionary<int, DocumentInfo> docInfo
        ) {
            float BASE_SCORE = 1.0f;
            float[] scores = new float[results.Length];
            int querySize = query.Split(' ').Length;

            for (int i = 0; i < items.Count; i++) {
                
            }

            return scores;
        }
        public static Matrix TFIDF(string query, Document[] docs, Dictionary<int, DocumentInfo> docInfo) {
            string[] words = query.Split(' ');
            int[] docOcurrencies = new int[words.Length];
            
            double[,] tfidfMatrix = new double[words.Length, docs.Length];

            for (int i = 0; i < words.Length; i++) {
                double[] tf = new double[docs.Length];
                
                for (int j = 0; j < docs.Length; j++) {
                    // Eliminando los restantes signos de puntuacion                    
                    int documentCount = 0;

                    foreach(string w in docs[j].CleanContent)
                        if (w.ToLower() == words[i].ToLower())
                            documentCount++;
                    
                    tf[j] = (double)documentCount / (double)docs[j].Content.Length;

                    if (documentCount > 0) {
                        docOcurrencies[i]++;
                        docInfo[j].QueryMatches++;
                    }
                }
                
                Vector tfVector = new Vector(tf);
                double idf = System.Math.Log10((double)docs.Length / (double)(1 + docOcurrencies[i]));

                Vector tfidfVector = idf * tfVector;

                for (int j = 0; j < tfidfVector.Size; j++) {
                    tfidfMatrix[i, j] = tfidfVector[j];
                }
            }

            return new Matrix(tfidfMatrix);
        }

        public static double[] GetResults(Matrix tfidf) {
            double[] tfidfSum = new double[tfidf.Columns];

            for (int i = 0; i < tfidf.Columns; i++) {
                for (int j = 0; j < tfidf.Rows; j++) {
                    tfidfSum[i] += tfidf[j, i];
                }
            }

            return tfidfSum;
        }

        public static string GetSnippet(string query, string content) {
            string snippet = "";
            string[] queryWords = query.Split(' ');

            for (int i = 0; i < queryWords.Length; i++) {
                foreach(string word in content.Split(" @$/#.-:&*+=[]?!(){},''\">_<;%\\".ToCharArray())) {
                    if (word.ToLower().Contains(queryWords[i].ToLower())) {
                        snippet += word + " ";
                        break;
                    }
                }
            }

            return snippet;
        }
    }
}
