using System.Collections.Generic;
using System.Linq;
using System;

namespace MatrixMath
{
    public class Matrix
    {
        private decimal[,] matrix;
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
            decimal[] matrix = new decimal[matrixArr.Length];
            for (long i = 0; i < matrixArr.Length; i++)
                matrix[i] = System.Convert.ToDecimal(matrixArr[i]);

            // Storing the length of the biggest number for formatting later
            this.style["biggestLength"] = matrix.OrderBy(n => n.ToString().Length).ToArray().Last().ToString().Length;
            this.cols = matrix.Length / rows;
            this.rows = rows;
            this.matrix = new decimal[this.rows, this.cols];

            long row = 0,
                col = 0;

            foreach (decimal n in matrix)
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
            Matrix.IsValid(sum ? "sum" : "sub", matrix1, matrix2);
            string res = "";

            for (long row = 0; row < matrix1.rows; row++)
                for (long col = 0; col < matrix1.cols; col++)
                    res += sum ?
                        $"{matrix1.matrix[row, col] + matrix2.matrix[row, col]}," :
                        $"{matrix1.matrix[row, col] - matrix2.matrix[row, col]},";

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
            Matrix.IsValid("mult", matrix1, matrix2);
            string res = "";

            for (long row = 0; row < matrix1.rows; row++)
            {
                for (long col = 0; col < matrix2.cols; col++)
                {
                    decimal rowRes = 0;
                    for (long i = 0; i < matrix1.cols; i++)
                        rowRes += matrix1.matrix[row, i] * matrix2.matrix[i, col];

                    res += $"{rowRes},";
                }
            }

            return new Matrix(res, matrix1.rows);
        }

        public static Matrix Divide(Matrix matrix1, Matrix matrix2)
        {
            Matrix.IsValid("div", matrix1, matrix2);

            return Matrix.Multiply(matrix1, matrix2.Inverse());
        }

        public decimal Determinant()
        {
            Matrix.IsValid("det", this);

            if (this.cols == 1) return this.matrix[0, 0];

            decimal det = 0;
            for (long col = 0; col < this.cols; col++)
            {
                decimal n = this.matrix[0, col];
                Matrix baseMatrix = Matrix.RemoveLine(Matrix.RemoveLine(this, 0, false), col, true);
                det = (col + 1) % 2 == 0 ? det - n * baseMatrix.Determinant() : det + n * baseMatrix.Determinant();
            }

            return det;
        }

        private static Matrix RemoveLine(Matrix matrix, long idx, bool removeCol)
        {
            string matrixStr = "";

            for (long row = 0; row < matrix.rows; row++)
                for (long col = 0; col < matrix.cols; col++)
                {
                    if ((removeCol && col == idx) || (!removeCol && row == idx)) continue;
                    matrixStr += $"{matrix.matrix[row, col]},";
                }

            return new Matrix(matrixStr, removeCol ? matrix.rows : matrix.rows - 1);
        }

        // Turn every column into a row and every row into a column
        public Matrix Transpose()
        {
            string res = "";

            for (long col = 0; col < this.cols; col++)
                for (long row = 0; row < this.rows; row++)
                    res += $"{this.matrix[row, col]},";

            return new Matrix(res, this.cols);
        }

        public Matrix Negate()
        {
            string res = "";

            foreach (decimal n in this.matrix)
                res += $"{n * -1},";

            return new Matrix(res, this.rows);
        }

        public Matrix Inverse()
        {
            Matrix.IsValid("inv", this);

            string cofactors = "",
                inverse = "";
            decimal det = this.Determinant();
            int change = 0;

            for (long row = 0; row < this.rows; row++)
                for (long col = 0; col < this.cols; col++)
                {
                    decimal minorDet = Matrix.RemoveLine(Matrix.RemoveLine(this, row, false), col, true).Determinant();
                    cofactors += $"{(change == 0 ? minorDet : minorDet * -1)},";
                    change ^= 1;
                }

            Matrix adjugate = new Matrix(cofactors, this.rows).Transpose();
            foreach (decimal n in adjugate.matrix)
                inverse += $"{n * 1 / det},";

            return new Matrix(inverse, this.rows);
        }

        public bool IsSymmetric()
        {
            return this.rows == this.cols && this == this.Transpose();
        }

        public bool IsAntiSymmetric()
        {
            return this.rows == this.cols && this.Negate() == this.Transpose();
        }

        public static Matrix Identity(long dimension)
        {
            string res = "";

            for (long row = 0; row < dimension; row++)
                for (long col = 0; col < dimension; col++)
                    res += row == col ? "1," : "0,";

            return new Matrix(res, dimension);
        }

        private static void IsValid(string operation, Matrix matrix1, Matrix matrix2 = null)
        {
            switch (operation)
            {
                case "sum":
                case "sub":
                    if (matrix1.cols != matrix2.cols || matrix1.rows != matrix2.rows)
                        throw new FormatException("Both matrices must have the same number of rows and cols.");
                    break;

                case "mult":
                    if (matrix1.rows != matrix2.cols || matrix1.cols != matrix2.rows)
                        throw new FormatException("Matrix 1 cols must be equal to Matrix 2 rows and vice versa.");
                    break;

                case "det":
                case "inv":
                    if (matrix1.rows != matrix1.cols)
                        throw new FormatException("Determinants can be calculated only with square matrices.");
                    break;

                case "div":
                    if (matrix2.Determinant() == 0)
                        throw new FormatException("No unique solution!");
                    break;
            }
        }

        // First element = bool indicating if it is or not valid
        // Second element = reason why it isn't
        public static object[] IsValid(string[] matrix, long rows)
        {
            if (matrix.Length % rows != 0) return new object[] { false, "Matrix length and row number aren't multiples" };

            for (long i = 1; i <= matrix.Length; i++)
                for (long j = 1; j <= i; j++)
                    if (i % j == 0) return new object[] { true };

            return new object[] { false, "A matrix with a prime number length isn't a regular matrix and therefore not valid here" };
        }

        public override bool Equals(object obj = null)
        {
            return Equals(obj as Matrix);
        }

        public bool Equals(Matrix matrix)
        {
            return this.ToString() == matrix.ToString();
        }

        public override int GetHashCode()
        {
            return this.matrix.GetHashCode();
        }

        public static bool operator ==(Matrix matrix1, Matrix matrix2)
        {
            return matrix1.ToString() == matrix2.ToString();
        }

        public static bool operator !=(Matrix matrix1, Matrix matrix2)
        {
            return matrix1.ToString() != matrix2.ToString();
        }

        public override string ToString()
        {
            /*
                Format:

                ⎡                  ⎤
                ⎢  1   2   3   4   ⎢
                ⎢  5   6   7   8   ⎢
                ⎢  9   10  11  12  ⎢
                ⎢  13  14  15  16  ⎢
                ⎣                  ⎦


                Got UNICODE bracket-like characters from https://unicode-search.net/unicode-namesearch.pl?term=bracket
            */

            int rowLength;
            string res = "",
                padding = "".PadLeft(this.style["identation"], ' ');

            for (long row = 0; row < this.rows; row++)
            {
                res += "⎢ ";

                for (long col = 0; col < this.cols; col++)
                    res += Math.Abs(this.matrix[row, col]) == this.matrix[row, col] ?
                        $"{padding}{this.matrix[row, col].ToString().PadRight(this.style["biggestLength"], ' ')}{padding}" :
                        $"{padding}-{Math.Abs(this.matrix[row, col]).ToString().PadRight(this.style["biggestLength"] - 1, ' ')}{padding}"; ;

                res += " ⎥\n";
            }

            rowLength = res.Split("\n")[0].Length - 2;
            res = $"⎡{"".PadLeft(rowLength, ' ')}⎤\n{res}⎣{"".PadLeft(rowLength, ' ')}⎦";
            return res;
        }
    }
}
