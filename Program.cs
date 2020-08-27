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
                Console.Title = "MatrixMath";
                Console.Write("Howdy!\nWhat matrix operation would you like to perform?\n\n0). Visually represent a matrix\n1). Matrix addition\n2). Matrix subtraction\n3). Matrix multiplication\n4). Matrix division\n5). Matrix determinant\n6). Transpose a matrix\n7). Negate a matrix\n8). Check for symmetry\n9). Check for anti-symmetry\n10). Get identity matrix\n\n> Your choice: ");
                Console.WriteLine($"\n{Program.Caller(Console.ReadLine())}");
            }
            catch (Exception exc)
            {
                Console.Title = "MatrixMath - ERROR";
                Console.WriteLine($"\n\nERROR!\n{exc.GetType()}: {exc.Message}");
            }
        }

        private static dynamic Caller(string input)
        {
            Matrix matrix1;
            Matrix matrix2;

            switch (input)
            {
                case "0":
                    Console.Title = "MatrixMath - Visually represent a matrix";
                    matrix1 = Program.GetMatrix();

                    return matrix1;

                case "1":
                    Console.Title = "Matrix addition";
                    matrix1 = Program.GetMatrix(1);
                    matrix2 = Program.GetMatrix(2);

                    return Matrix.Sum(matrix1, matrix2);


                case "2":
                    Console.Title = "Matrix subtraction";
                    matrix1 = Program.GetMatrix(1);
                    matrix2 = Program.GetMatrix(2);

                    return Matrix.Subtract(matrix1, matrix2);


                case "3":
                    Console.Title = "Matrix multiplication";
                    matrix1 = Program.GetMatrix(1);
                    matrix2 = Program.GetMatrix(2);

                    return Matrix.Multiply(matrix1, matrix2);


                case "4":
                    Console.Title = "Matrix division";

                    return null;


                case "5":
                    Console.Title = "Matrix determinant";
                    matrix1 = Program.GetMatrix();

                    return matrix1.Determinant();


                case "6":
                Console.Title = "Transpose a matrix";
                    matrix1 = Program.GetMatrix();

                    return matrix1.Transpose();


                case "7":
                Console.Title = "Negate a matrix";
                    matrix1 = Program.GetMatrix();

                    return matrix1.Negate();


                case "8":
                    Console.Title = "Check for symmetry";
                    matrix1 = Program.GetMatrix();

                    return matrix1.IsSymmetric() ? "Given matrix is symmetric" : "Given matrix isn't symmetric";


                case "9":
                Console.Title = "Check for anti-symmetry";
                    matrix1 = Program.GetMatrix();

                    return matrix1.IsAntiSymmetric() ? "Given matrix is anti-symmetric" : "Given matrix isn't anti-symmetric";


                case "10":
                    Console.Title = "Get identity matrix";
                    Console.Write("Input number of rows of identity matrix\n> Your choice: ");

                    return Matrix.Identity(Convert.ToInt64(Console.ReadLine()));


                default:
                    throw new FormatException("Invalid operation selected.");
            }
        }

        private static Matrix GetMatrix(int num = 0)
        {
            string matrix;
            long rows;
            Regex regex = new Regex(@"\d+(,\d+)*(\.\d*)?");
            object[] isValid;

            Console.Write($"Input matrix{(num > 0 ? $" {num}" : "")} using a comma as separator (e.g 1,2,3,4,5,6)\n> Your choice: ");
            matrix = Console.ReadLine();
            Console.Write($"Input number of rows of matrix{(num > 0 ? $" {num}" : "")}:\n> Your choice: ");
            rows = Convert.ToInt64(Console.ReadLine());

            if (regex.Matches(matrix).Count < 1)
                throw new FormatException($"Expecting matrix to be in 'N1,N2,N3,N4,Nn' format. Got '{matrix}'");

            if (rows < 1)
                throw new IndexOutOfRangeException("Matrix cannot have 0 or less rows");

            isValid = Matrix.IsValid(matrix.Split(','), rows);

            if (!(bool)isValid[0])
                throw new FormatException((string)isValid[1]);

            return new Matrix(matrix, rows, false);
        }
    }
}
