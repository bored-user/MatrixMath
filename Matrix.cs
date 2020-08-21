using System.Collections.Generic;
using System.Linq;
using System;

namespace MatrixMath
{
    public class Matrix
    {
        public List<long[]> matrix = new List<long[]>();
        public long rows = 0;
        public long cols = 0;

        /*
            Naming it "identation" is just a joke.
            It actually means the number of spaces in between the matrix members (horzontally speaking)
        */
        private Dictionary<string, int> style = new Dictionary<string, int>() { ["identation"] = 1, ["biggestLength"] = 0 };

        public Matrix(long[] matrix, long rows)
        {
            this.style["biggestLength"] = matrix.OrderBy(n => n.ToString().Length).ToArray().Last().ToString().Length;

            this.cols = matrix.Length / rows;
            this.rows = rows;

            long[] row = new long[this.cols];
            long count = 0;

            foreach (long n in matrix)
            {
                if (count == this.cols)
                {
                    this.matrix.Add((long[])row.Clone());
                    Array.Clear(row, 0, row.Length);
                    count = 0;
                }

                row[count] = n;
                count++;
            }

            this.matrix.Add(row);
        }

        public Matrix()
        {
            Matrix matrix = Program.GetMatrix();
            Console.WriteLine(matrix);
        }

        private static Matrix SumSubWrapper(Matrix matrix1, Matrix matrix2, bool sum)
        {
            Matrix.IsValid(matrix1, matrix2, sum ? "sum" : "sub");

            string res = "";
            long[] row = new long[matrix1.cols];

            for (int i = 0; i < matrix1.rows; i++) // each row
            {
                for (int j = 0; j < matrix1.matrix[i].Count(); j++) // each item in row
                {
                    res += sum ?
                        $"{matrix1.matrix[i][j] + matrix2.matrix[i][j]}," :
                        $"{matrix1.matrix[i][j] - matrix2.matrix[i][j]},";
                }
            }

            return new Matrix(Matrix.Convert(res.Substring(0, res.Length - 1).Split(',')), matrix1.rows);
        }

        public static Matrix Sum(Matrix matrix1, Matrix matrix2)
        {
            return Matrix.SumSubWrapper(matrix1, matrix2, true);
        }

        public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
        {
            return Matrix.SumSubWrapper(matrix1, matrix2, false);
        }

        public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
        {
            Matrix.IsValid(matrix1, matrix2, "mult");
            long[] res;

            

            return new Matrix();
        }

        public static void IsValid(Matrix matrix1, Matrix matrix2, string operation)
        {
            if (operation == "sum" || operation == "sub")
            {
                if (matrix1.cols != matrix2.cols || matrix1.rows != matrix2.rows)
                    throw new FormatException("Both matrices must have the same number of rows and cols.");
            }
            else if(operation == "mult" || operation == "div")
            {
                if (matrix1.rows != matrix2.cols || matrix1.cols != matrix2.rows)
                    throw new FormatException("Matrix 1 cols must be equal to Matrix 2 rows and vice versa.");
            }

        }

        // Matrix will only be valid if its length isn't a prime number
        public static object[] IsValid(string[] matrix, long rows)
        {
            if (matrix.Length % rows != 0) return new object[] { false, "Matrix length and row number aren't multiples" };

            for (long i = 1; i <= matrix.Length; i++)
            {
                for (long j = 1; j <= i; j++)
                {
                    if (i % j == 0)
                        return new object[] { true };
                }
            }

            return new object[] { false, "A matrix with a prime number length isn't a regular matrix and therefore not valid here" };
        }

        public static long[] Convert(string[] matrix)
        {
            long[] res = new long[matrix.Length];
            for (long i = 0; i < matrix.Length; i++)
                res[i] = System.Convert.ToInt64(matrix[i]);

            return res;
        }

        public override string ToString()
        {
            string res = "";
            string padding = "".PadLeft(this.style["identation"], ' ');

            foreach (long[] row in this.matrix)
            {
                res += "|";

                foreach (long n in row)
                    res += n > 0 ?
                        $"{padding}{n.ToString().PadRight(this.style["biggestLength"], ' ')}{padding}" :
                        $"{padding}-{Math.Abs(n).ToString().PadRight(this.style["biggestLength"] - 1, ' ')}{padding}";


                res += "|\n";
            }

            // Removing extra linebreak
            return res.Substring(0, res.Length - 1);
        }
    }
}
