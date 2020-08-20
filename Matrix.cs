using System.Collections.Generic;
using System.Linq;
using System;

public class Matrix
{
    private List<long[]> matrix = new List<long[]>();
    private long rows;
    private long cols;
    /*
        Naming it "identation" is just a joke.
        It actually means the number of spaces in between the matrix members (horzontally speaking)
    */
    private Dictionary<string, int> style = new Dictionary<string, int>() { ["identation"] = 2, ["biggestLength"] = 0, ["smallestLength"] = 0 };

    public Matrix(long[] matrix, long rows)
    {
        long[] orderedMatrix = matrix.OrderByDescending(n => n).ToArray();
        this.style["biggestLength"] = orderedMatrix.First().ToString().Length;
        this.style["smallestLength"] = orderedMatrix.Last().ToString().Length;

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

    // private static bool IsValid(string operation, Matrix matrix)
    // {
    //     switch (operation)
    //     {
    //         case "sum":
    //             break;
    //     }

    //     return false;
    // }

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

    private string CalculateSpaces(long n)
    {
        string res = "";

        for (int i = 0; i < this.style["identation"]; i++)
        {
            res += " ";
        }

        return res;
    }

    public override string ToString()
    {
        string res = "\n";
        foreach (long[] row in this.matrix)
        {
            res += "| ";

            foreach (long n in row)
                res += n.ToString();
                // space count = biggest - smallest -> both center and right

            res += " |\n";
        }

        // Removing extra linebreak
        return res.Substring(0, res.Length - 1);
    }
}
