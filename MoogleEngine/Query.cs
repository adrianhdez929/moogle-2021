using System;
using MoogleEngine.Math;

namespace MoogleEngine {
    public class Query {

        Vector tf;
        Vector tfidf;
        public Query(string query) {
            this.Text = query;
            this.Words = query.Split(' ');
            double[] tfValues = new double[this.Words.Length];

            // Calculando la frecuencia de cada termino del query
            for (int i = 0; i < this.Words.Length; i++) {
                double tf = this.Words[i].Count() / this.Words.Length;
                tfValues[i] = tf;
            }    

            this.tf = new Vector(tfValues);
            this.tfidf = new Vector(new double[this.Words.Length]);
        }

        public string Text {
            get;
        }

        public string[] Words {
            get;
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