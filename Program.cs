using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace MatrixMath
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string matrix;
                long rows;

                Console.Write("Input matrix using a comma as separator (e.g 1,2,3,4,5)\n> Your choice: ");
                matrix = Console.ReadLine();
                Console.Write("> Input number of rows of that matrix: ");
                rows = Convert.ToInt64(Console.ReadLine());

                if (new Regex(@"\d+(,\d+)*(\.\d*)?").Matches(matrix).Count < 1)
                    throw new FormatException($"Expecting matrix to be in 'N1,N2,N3,N4,Nn' format. Got '{matrix}'");

                if (rows < 1)
                    throw new IndexOutOfRangeException("Matrix cannot have 0 or less rows");

                string[] matrixArray = matrix.Split(',');
                object[] isValid = Matrix.IsValid(matrixArray, rows);
                if(!(bool)isValid[0])
                    throw new FormatException((string)isValid[1]);

                Console.WriteLine(new Matrix(Matrix.Convert(matrixArray), rows));
            }
            catch (Exception exc)
            {
                Console.Write("Either you did something wrong or I did :(\n\nPress 'd' to read the excpetion or anything else to exit: ");
                if (Console.ReadLine() == "d")
                    Console.WriteLine($"{exc.GetType()}: {exc.Message}");

                Environment.Exit(-1);
            }
        }
    }
}
