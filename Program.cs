using System;
using System.Text.RegularExpressions;

namespace MatrixMath
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.Write("Howdy!\nWhat mathematical operation would you like to perform?\n\n1). Matrix addition\n2). Matrix subtraction\n3). Matrix multiplication\n4). Matrix division\n5). Transpose a matrix\n6). Negate a matrix\n7). Check for symmetry\n8). Check for anti-symmetry\n\n> Your choice: ");
                Console.WriteLine(Program.Caller(Console.ReadLine()));
            }
            catch (Exception exc)
            {
                Console.WriteLine($"\n\nERROR!\n{exc.GetType()}: {exc.Message}");
            }
        }

        private static dynamic Caller(string input)
        {
            Matrix matrix1;
            Matrix matrix2;

            switch (input)
            {
                case "1":
                    matrix1 = Program.GetMatrix(1);
                    matrix2 = Program.GetMatrix(2);

                    return Matrix.Sum(matrix1, matrix2);


                case "2":
                    matrix1 = Program.GetMatrix(1);
                    matrix2 = Program.GetMatrix(2);

                    return Matrix.Subtract(matrix1, matrix2);


                case "3":
                    matrix1 = Program.GetMatrix(1);
                    matrix2 = Program.GetMatrix(2);

                    return Matrix.Multiply(matrix1, matrix2);


                case "5":
                    matrix1 = Program.GetMatrix();

                    return Matrix.Transpose(matrix1);


                case "6":
                    matrix1 = Program.GetMatrix();

                    return Matrix.Negate(matrix1);


                case "7":
                    matrix1 = Program.GetMatrix();

                    return Matrix.IsSymmetric(matrix1) ? "Given matrix is symmetric" : "Given matrix isn't symmetric";


                case "8":
                    matrix1 = Program.GetMatrix();

                    return Matrix.IsAntiSymmetric(matrix1) ? "Given matrix is anti-symmetric" : "Given matrix isn't anti-symmetric";


                default:
                    return -1;
            }
        }

        private static Matrix GetMatrix(int num = 0)
        {
            string matrix;
            long rows;
            Regex regex = new Regex(@"\d+(,\d+)*(\.\d*)?");
            string[] matrixArray;
            object[] isValid;

            Console.Write($"Input matrix{(num > 0 ? $" {num}" : "")} using a comma as separator (e.g 1,2,3,4,5,6)\n> Your choice: ");
            matrix = Console.ReadLine();
            Console.Write($"> Input number of rows of matrix{(num > 0 ? $" {num}" : "")}: ");
            rows = Convert.ToInt64(Console.ReadLine());

            if (regex.Matches(matrix).Count < 1)
                throw new FormatException($"Expecting matrix to be in 'N1,N2,N3,N4,Nn' format. Got '{matrix}'");

            if (rows < 1)
                throw new IndexOutOfRangeException("Matrix cannot have 0 or less rows");

            matrixArray = matrix.Split(',');
            isValid = Matrix.IsValid(matrixArray, rows);

            if (!(bool)isValid[0])
                throw new FormatException((string)isValid[1]);

            return new Matrix(Matrix.Convert(matrixArray), rows);
        }
    }
}
