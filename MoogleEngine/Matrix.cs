namespace Utils {
    public class Matrix {
        bool transposed;
        double[,] elements;

        public Matrix(double[,] elements) {
            this.elements = elements;
        }

        public static Matrix operator +(Matrix a, Matrix b) {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException("Matrix must have same dimentions.");
            double[,] elements = new double[a.Rows,a.Columns];

            for (int i = 0; i < a.Rows; i++){
                for (int j = 0; j < a.Columns; j++)
                {
                    elements[i,j] = a[i,j] + b[i,j];
                }
            }

            return new Matrix(elements);
        }
        public double this[int i, int j]
        {
            get {
                return this.Transposed ? this.elements[j,i] : this.elements[i,j];
            }
            set { 
                if (this.Transposed)
                    this.elements[j,i] = value;
                else
                    this.elements[i,j] = value;
            }
        }

        public bool Transposed { 
            get { return this.transposed; }
        }

        public int Rows {
            get { return this.Transposed ? this.elements.GetLength(1) : this.elements.GetLength(0); }
        }

        public int Columns { 
            get { return this.Transposed ? this.elements.GetLength(0) : this.elements.GetLength(1); }
        }

        public void Transpose() {
            this.transposed = !this.transposed;
        }
    }
}