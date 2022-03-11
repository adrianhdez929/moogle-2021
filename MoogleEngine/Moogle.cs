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
        public static SearchResult Query(string pQuery) {
            Dictionary<int, DocumentInfo> docInfo = new Dictionary<int, DocumentInfo>();
            
            Document[] docs = LoadFilesContent(docInfo);
            Query query = new Query(pQuery);

            string[] fileNames = Directory.GetFiles("../Content");

            Matrix tfidf = TFIDF(query, docs, docInfo);

            System.Console.WriteLine(tfidf);
            
            double[] results = GetResults(tfidf);

            List<SearchItem> items = new List<SearchItem>();

            for (int i = 0; i < results.Length; i++) {
                if (results[i] != 0) {
                    items.Add(new SearchItem(docs[i].Path, GetSnippet(query, docs[i].Content), (float)results[i]));
                }
            }

            items.Sort(new ScoreComparer());
            items.Reverse();
            Array.Sort(results);
            Array.Reverse(results);

            string suggestion = GetSuggestion(query, items, docs, tfidf);

            return new SearchResult(items.ToArray(), suggestion);
        }

        public static string GetSuggestion(Query query, List<SearchItem> result, Document[] docCollection, Matrix tfidfMatrix) {
            string suggestion = "";

            for (int i = 0; i < docCollection.Length; i++) {
                if (docCollection[i].Path == result.First().Title.Split('.').First()) {
                    for (int j = 0; j < query.Words.Length; j++) {
                        if (tfidfMatrix[j, i] != (double)0) {
                            suggestion += query.Words[j] + " ";
                        }
                    }
                }
            }

            return suggestion;
        }
        
        public static Document[] LoadFilesContent(Dictionary<int, DocumentInfo> docInfo) {
            string[] fileNames = Directory.GetFiles("../Content");
            Document[] documents = new Document[fileNames.Length - 1];
            int currentIndex = 0;

            for (int i = 0; i < fileNames.Length; i++) {
                if (fileNames[i] == "../Content/.gitignore")
                    continue;

                StreamReader reader = new StreamReader(fileNames[i]);
                string fileContent = reader.ReadToEnd();

                documents[currentIndex] = new Document(fileContent, fileNames[i]);
                docInfo[currentIndex] = new DocumentInfo();

                currentIndex++;
            }

            return documents;
        }
        public static Matrix TFIDF(Query query, Document[] docs, Dictionary<int, DocumentInfo> docInfo) {
            string[] words = query.Words;
            int[] docOcurrencies = new int[words.Length];

            double[,] tfidfMatrix = new double[words.Length, docs.Length];

            for (int i = 0; i < words.Length; i++) {
                double[] tf = new double[docs.Length];
                
                for (int j = 0; j < docs.Length; j++) {
                    int documentCount = 0;

                    foreach(string w in docs[j].CleanContent) {
                        if (w.ToLower() == words[i].ToLower())
                            documentCount++;
                    }

                    //System.Console.WriteLine(words[i] + " " + documentCount + " " + docs[j].CleanContent.Length);
                    
                    tf[j] = (double)documentCount / (double)docs[j].CleanContent.Length;

                    if (documentCount > 0) {
                        docOcurrencies[i]++;
                        docInfo[j].QueryMatches++;
                    }
                }
                
                Vector tfVector = new Vector(tf);

                if (docOcurrencies[i] == 0)
                    docOcurrencies[i]++;

                double idf = System.Math.Log10((double)docs.Length / (double)(docOcurrencies[i]));

                //System.Console.WriteLine(idf);

                Vector tfidfVector = idf * tfVector;

                try {
                    tfidfVector = (double)query.WeightedWords[words[i]] * tfidfVector;
                } catch (KeyNotFoundException) {}

                for (int j = 0; j < tfidfVector.Size; j++) {
                    tfidfMatrix[i, j] = tfidfVector[j];
                }
            }

            for (int i = 0; i < docs.Length; i++) {
                foreach (string word in docs[i].CleanContent) {
                    if (query.SkippedWords.Contains(word.ToLower())) {
                        for (int j = 0; j < words.Length; j++) {
                            tfidfMatrix[j, i] = 0;
                        }
                        break;
                    }
                }
            }

            // implementar el must

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

        public static string GetSnippet(Query query, string content) {
            string snippet = "";
            string[] queryWords = query.Words;

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
