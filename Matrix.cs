using System.Collections.Generic;
using System.Linq;
using System;

namespace MatrixMath
{
    public class Matrix
    {
        private long[,] matrix;
        private long rows;
        private long cols;

        /*
            Naming it "identation" is just a joke.
            It actually means the number of spaces in between the matrix members (horzontally speaking)
        */
        private Dictionary<string, int> style = new Dictionary<string, int>() { ["identation"] = 1 };

        public Matrix(string matrixStr, long rows, bool substr = true)
        {

            string[] matrixArr = substr ? matrixStr.Substring(0, matrixStr.Length - 1).Split(',') : matrixStr.Split(',');
            long[] matrix = new long[matrixArr.Length];
            for (long i = 0; i < matrixArr.Length; i++)
                matrix[i] = System.Convert.ToInt64(matrixArr[i]);

            // Storing the length of the biggest number for formatting later
            this.style["biggestLength"] = matrix.OrderBy(n => n.ToString().Length).ToArray().Last().ToString().Length;
            this.cols = matrix.Length / rows;
            this.rows = rows;
            this.matrix = new long[this.rows, this.cols];

            long row = 0,
                col = 0;

            foreach (long n in matrix)
            {
                if (col == this.cols)
                {
                    col = 0;
                    row++;
                }

                this.matrix[row, col] = n;
                col++;
            }
        }

        /*
            Addition and subtraction operations are very similar,
            that's the reason for this wrapper existance
        */
        private static Matrix SumSubWrapper(Matrix matrix1, Matrix matrix2, bool sum)
        {
            Matrix.IsValid(matrix1, matrix2, sum ? "sum" : "sub");
            string res = "";

            for (int row = 0; row < matrix1.rows; row++)
            {
                for (int col = 0; col < matrix1.cols; col++)
                {
                    res += sum ?
                        $"{matrix1.matrix[row, col] + matrix2.matrix[row, col]}," :
                        $"{matrix1.matrix[row, col] - matrix2.matrix[row, col]},";
                }
            }

            return new Matrix(res, matrix1.rows);
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
            string res = "";

            for (long row = 0; row < matrix1.rows; row++)
            {
                for (long col = 0; col < matrix2.cols; col++)
                {
                    long rowRes = 0;
                    for (long i = 0; i < matrix1.cols; i++)
                        rowRes += matrix1.matrix[row, i] * matrix2.matrix[i, col];

                    res += $"{rowRes},";
                }
            }

            return new Matrix(res, matrix1.rows);
        }

        // Turn every column into a row and every row into a column
        public Matrix Transpose()
        {
            string res = "";

            for (long col = 0; col < this.cols; col++)
            {
                for (long row = 0; row < this.rows; row++)
                {
                    res += $"{this.matrix[row, col]},";
                }
            }

            return new Matrix(res, this.rows);
        }

        public Matrix Negate()
        {
            string res = "";

            for (long row = 0; row < this.rows; row++)
            {
                for (long col = 0; col < this.cols; col++)
                {
                    res += $"{this.matrix[row, col] * -1},";
                }
            }

            return new Matrix(res, this.rows);
        }

        public bool IsSymmetric()
        {
            return this.rows == this.cols && this.ToString() == this.Transpose().ToString();
        }

        public bool IsAntiSymmetric()
        {
            return this.rows == this.cols && this.Negate().ToString() == this.Transpose().ToString();
        }

        public long Determinant()
        {
            Matrix.IsValid(this, "det");

            return 0;
        }

        public static Matrix Identity(long dimension)
        {
            string res = "";

            for (long row = 0; row < dimension; row++)
            {
                for (long col = 0; col < dimension; col++)
                {
                    res += row == col ? "1," : "0,";
                }
            }

            return new Matrix(res, dimension);
        }

        public static void IsValid(Matrix matrix1, Matrix matrix2, string operation)
        {
            switch (operation)
            {
                case "sum":
                case "sub":
                    if (matrix1.cols != matrix2.cols || matrix1.rows != matrix2.rows)
                        throw new FormatException("Both matrices must have the same number of rows and cols.");
                    break;

                case "mult":
                case "div":
                    if (matrix1.rows != matrix2.cols || matrix1.cols != matrix2.rows)
                        throw new FormatException("Matrix 1 cols must be equal to Matrix 2 rows and vice versa.");
                    break;
            }
        }

        public static void IsValid(Matrix matrix1, string operation)
        {
            switch (operation)
            {
                case "det":
                    if (matrix1.rows != matrix1.cols)
                        throw new FormatException("Determinants can be calculated only with square matrices.");
                    break;
            }
        }

        // First element = bool indicating if it is or not valid
        // Second element = reason why it isn't
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

        public override string ToString()
        {
            /*
                Format:

                T  1  2  3  4 T
                |  5  6  7  8 |
                |  9 10 11 12 |
                ┴ 13 14 15 16 ┴
            */

            string res = "",
                padding = "".PadLeft(this.style["identation"], ' ');
            string Format(long n)
            {
                return Math.Abs(n) == n ?
                        $"{padding}{n.ToString().PadRight(this.style["biggestLength"], ' ')}{padding}" :
                        $"{padding}-{Math.Abs(n).ToString().PadRight(this.style["biggestLength"] - 1, ' ')}{padding}"; ;
            };
            long n;

            res += "T";
            for (long col = 0; col < this.cols; col++)
            {
                n = this.matrix[0, col];
                res += Format(n);
            }
            res += "T\n";

            for (long row = 1; row < this.rows - 1; row++)
            {
                res += "|";

                for (long col = 0; col < this.cols; col++)
                {
                    n = this.matrix[row, col];
                    res += Format(n);
                }

                res += "|\n";
            }

            res += "┴";
            for (long col = 0; col < this.cols; col++)
            {
                n = this.matrix[this.rows - 1, col];
                res += Format(n);
            }
            res += "┴";

            return res;
        }
    }
}
