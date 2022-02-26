using System;
using MoogleEngine.Math;

namespace MoogleEngine {
    public class Document {
        
        float score;
        string snippet;
        string[] cleanContent; 

        Vector tf;

        public Document(string content, string path) {
            this.Content = content;
            this.Path = path.Split('/').Last().Split('.')[0];

            // Limpiamos los caracteres "extrannos" del contenido del documento
            cleanContent = content.Split(" @$/#.-:&*+=[]?!(){},''\">_<;%\\".ToCharArray());
            double[] tfValues = new double[cleanContent.Length];

            // Guardamos el tf de cada termino del documento (frecuencia del termino / frecuencia maxima)
            for (int i = 0; i < cleanContent.Length; i++) {
                tfValues[i] = cleanContent[i].ToLower().Count() / cleanContent.Length;
            }

            this.tf = new Vector(tfValues);            
            this.snippet = "";
            this.score = 0.0f;
        }

        public float Score {
            get { return this.score; }
        }

        public string Content {
            get;

            private set;
        }

        public string[] CleanContent {
            get { return this.cleanContent; }
        }

        public string Snippet {
            get { return this.snippet; }
        }

        public string Path {
            get;

            private set;
        }

        public Vector TF {
            get { return this.tf; }
        }
    }
}
