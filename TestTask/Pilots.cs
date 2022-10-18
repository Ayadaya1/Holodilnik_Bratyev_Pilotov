using System.Globalization;
using System.Collections.Generic;
using System.Drawing;

namespace TestTask
{
    internal class Pilots
    {

        public static bool Check(int[,] array)
        {

            int a = array[0, 0];
            foreach (int i in array)
            {
                if (i != a)
                    return true;
            }
            return false;
        }
        public static void Fill(int[,] array, int n)
        {

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    array[i, j] = 1;
                }
            }
            Random rand = new Random();
            int count = 60 + rand.Next() % 15;
            for (int i = 0; i < count; i++)
            {
                int a = rand.Next() % n + 1;
                int b = rand.Next() % n + 1;
                Switch(array, a, b, n);
            }
            if (!Check(array))
                Fill(array, n);
        }
        public static void Solve(int[,] array, int n, List<int[]> matrix)
        {
            if (matrix.Count == 0)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (array[i, j] == 0)
                        {
                            int[] ar = new int[2];
                            ar[0] = i + 1;
                            ar[1] = j + 1;
                            matrix.Add(ar);
                        }
                    }
                }
            }
            else
            { 
                Switch(array, matrix[0][0], matrix[0][1], n);
                matrix.RemoveAt(0);
            }
        }
        public static void Switch(int[,] array, int a, int b, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i + 1 == a || (j + 1 == b && i + 1 != a))
                    {
                        if (array[i, j] == 0)
                            array[i, j] = 1;
                        else
                            array[i, j] = 0;
                    }
                }
            }
        }
    }
}