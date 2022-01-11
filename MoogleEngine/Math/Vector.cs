using System.Text;

namespace MoogleEngine.Math;
public class Vector {
    private double[] elements;

    public Vector(double[] elements) {
        if (elements == null)
            throw new ArgumentException("The input vector can't be null");

        this.elements = elements;
    }

    public int Size {
        get { return this.elements.Length; }
    }

    public override string ToString()
    {
        StringBuilder vector_sb = new StringBuilder($"Vector ({this.Size}):");
        vector_sb.AppendLine();

        for (int i = 0; i < this.Size; i++) {
            if (i == 0)
                vector_sb.AppendFormat("{0, -3}", "(");
            
            vector_sb.AppendFormat("{0, -4}", elements[i]);

            if (i == this.Size - 1)
            vector_sb.Append(")");
        }

        return vector_sb.ToString();
    }

    public double this[int i] {
        get {
            return this.elements[i];
        }

        set {
            this.elements[i] = value;
        }
    }

    public static Vector Sum(Vector a, Vector b) {
        CheckNullVector(a);
        CheckNullVector(b);

        if (a.Size != b.Size)
            throw new ArgumentException("Incompatible dimensions");

        double[] sum = new double[a.Size];

        for (int i = 0; i < sum.Length; i++)
        {
            sum[i] = a[i] + b[i];
        }

        return new Vector(sum);
    }

    public static Vector ScalarProduct(double n, Vector a) {
        CheckNullVector(a);

        double[] s_product = new double[a.Size];

        for (int i = 0; i < s_product.Length; i++)
        {
            s_product[i] = n * a[i];
        }

        return new Vector(s_product);
    }

    public static Vector operator *(double n, Vector a) {
        return Vector.ScalarProduct(n, a);
    }

    private static void CheckNullVector(Vector v)
    {
        // Verificar los valores de entrada
        if (v == null)
        {
            throw new ArgumentException("Operands cannot be null");
        }
    }
}
