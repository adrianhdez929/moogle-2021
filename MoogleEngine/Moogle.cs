using MoogleEngine.Math;
using System.Text.RegularExpressions;
using System.IO;

namespace MoogleEngine {
    public static class Moogle {
        public static SearchResult Query(string query) {
            // Modifique este método para responder a la búsqueda
            string[] content = LoadFilesContent();

            string[] fileNames = Directory.GetFiles("../Content");
            string[] queryWords = query.Split(' ');

            Matrix tfidf = TFIDF(query, content);

            System.Console.WriteLine(tfidf.ToString());

            for(int i = 0; i < queryWords.Length; i++) {
                for (int j = 0; j < content.Length; j++) {
                    if (tfidf[i, j] != 0)
                        System.Console.WriteLine("{0} - {1}: {2}", queryWords[i], fileNames[j], tfidf[i, j]);
                }
            }

            SearchItem[] items = new SearchItem[3] {
                new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
                new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
                new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
            };

            return new SearchResult(items, query);
        }
        
        public static string[] LoadFilesContent() {
            string[] fileNames = Directory.GetFiles("../Content");
            string[] content = new string[fileNames.Length];

            for (int i = 0; i < content.Length; i++)
            {
                StreamReader reader = new StreamReader(fileNames[i]);
                string fileContent = reader.ReadToEnd();                

                content[i] = fileContent;
            }

            return content;
        }

        public static Matrix TFIDF(string query, string[] documentsContent) {
            string[] words = query.Split(' ');
            int[] docOcurrencies = new int[words.Length];
            
            double[,] tfidfMatrix = new double[words.Length, documentsContent.Length];

            for (int i = 0; i < words.Length; i++) {
                double[] tf = new double[documentsContent.Length];
                
                for (int j = 0; j < documentsContent.Length; j++) {
                    // Eliminando los restantes signos de puntuacion                    
                    string[] currentContent = documentsContent[j].Split(" @$/#.-:&*+=[]?!(){},''\">_<;%\\".ToCharArray());
                    int documentCount = 0;

                    foreach(string w in currentContent)
                        if (w.ToLower().Contains(words[i].ToLower()))
                            documentCount++;
                    
                    tf[j] = (double)documentCount / (double)currentContent.Length;

                    if (documentCount > 0)
                        docOcurrencies[i]++;
                }
                
                Vector tfVector = new Vector(tf);
                double idf = System.Math.Log10((double)documentsContent.Length / (double)(1 + docOcurrencies[i]));

                Vector tfidfVector = idf * tfVector;

                for (int j = 0; j < tfidfVector.Size; j++) {
                    tfidfMatrix[i, j] = tfidfVector[j];
                }
            }

            return new Matrix(tfidfMatrix);
        }
    }
}
